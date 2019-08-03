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
                builder.Raw = "--version";
                builder.WithFlag("ff", "-").WithFlag("O2", "-");
                builder.WithOption("o", "a.out", "-").WithOption("cc", 1, "--").WithOption("cc", "a", "--").WithoutFlag("-ff").WithoutOption("--cc");
                Assert.AreEqual("gcc a.c -O2 -o a.out --version", builder.Resolve().Result);
            }
        }
    }
}
