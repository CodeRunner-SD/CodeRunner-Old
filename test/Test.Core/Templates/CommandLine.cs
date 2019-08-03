using CodeRunner.Templates;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.Core.Templates
{
    [TestClass]
    public class CommandLine
    {
        [TestMethod]
        public void Basic()
        {
            {
                CommandLineTemplate builder = new CommandLineTemplate();
                builder.Commands.Add("gcc");
                builder.Arguments.Add("a.c");
                builder.WithFlag("ff", "-").WithFlag("O2", "-");
                builder.WithOption("o", "a.out", "-").WithOption("cc", 1, "--").WithoutFlag("-ff").WithoutOption("--cc");
                Assert.AreEqual("gcc a.c -O2 -o a.out", builder.Resolve().Result);
            }
        }
    }
}
