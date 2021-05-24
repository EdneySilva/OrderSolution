using System;
using System.Collections.Generic;
using System.Text;

namespace OrderSolution.Infrastructure.Storage.Azure.Environment
{
    public class AzureConnectionOptions
    {
        public string Queue { get; set; }

        public string ConnectionString { get; set; }

        public int? RequestWaitInterval { get; set; }
    }
}
