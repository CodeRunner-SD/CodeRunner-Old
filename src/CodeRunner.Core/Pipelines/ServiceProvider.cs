using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CodeRunner.Pipelines
{
    public class ServiceProvider
    {
        readonly Dictionary<Type, Dictionary<string, ServiceItem>> pools = new Dictionary<Type, Dictionary<string, ServiceItem>>();

        public Task<ServiceScope> CreateScope(string name)
        {
            return Task.FromResult(new ServiceScope(name, pools));
        }
    }
}
