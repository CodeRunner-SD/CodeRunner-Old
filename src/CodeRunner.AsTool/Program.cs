using System;
using System.Threading.Tasks;

namespace CodeRunner.AsTool
{
    class Program
    {
        static Task<int> Main(string[] args)
        {
            return CodeRunner.Program.Main(args);
        }
    }
}
