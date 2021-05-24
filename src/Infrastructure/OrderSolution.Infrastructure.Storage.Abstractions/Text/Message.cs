using System;
using System.Threading.Tasks;

namespace OrderSolution.Infrastructure.Storage.Abstractions.Text
{
    public abstract class Message
    {
        protected Message(string content)
        {
            this.Content = content;
        }

        public string Content { get; protected set; }

        public abstract T To<T>();

        public abstract Task ConfirmAsync();
    }
}
