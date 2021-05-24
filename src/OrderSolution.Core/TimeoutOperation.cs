using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OrderSolution.Core
{
    public struct TimeoutOperation<T>
    {
        private readonly Task<T> task;
        private readonly CancellationTokenSource cancellationToken;

        public TimeoutOperation(TimeSpan timeout, Task<T> task, CancellationTokenSource cancellationToken)
        {
            Timeout = timeout;
            this.task = task;
            this.cancellationToken = cancellationToken;
        }

        public TimeSpan Timeout { get; }

        public async Task<T> Wait()
        {
            var timeoutTask = Task.Delay((int)Timeout.TotalMilliseconds);
            await Task.WhenAny(task, timeoutTask);
            if (task.IsCompleted)
                return task.Result;
            this.cancellationToken.Cancel();
            await task;
            throw new Exception("The read message received a timeout");
        }
    }
}
