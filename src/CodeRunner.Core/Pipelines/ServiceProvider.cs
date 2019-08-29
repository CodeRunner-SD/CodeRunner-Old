using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CodeRunner.Pipelines
{
    public class ServiceProvider
    {
        private readonly Dictionary<Type, Dictionary<string, ServiceItem>> pools = new Dictionary<Type, Dictionary<string, ServiceItem>>();

        public Task<ServiceScope> CreateScope(string name) => Task.FromResult(new ServiceScope(name, pools));
    }
}
