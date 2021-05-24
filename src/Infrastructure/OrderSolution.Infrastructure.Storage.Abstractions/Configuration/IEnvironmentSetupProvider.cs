using System;
using System.Collections.Generic;
using System.Text;

namespace OrderSolution.Infrastructure.Storage.Abstractions.Configuration
{
    public interface IEnvironmentSetupProvider
    {
        void Apply();
    }
}
