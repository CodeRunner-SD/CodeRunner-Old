using CodeRunner.Templates;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace Test.Core.Templates
{
    [TestClass]
    public class TCommandLineTemplate
    {
        [TestMethod]
        public async Task Basic()
        {
            CommandLineTemplate builder = new CommandLineTemplate();
            builder.Commands.Add("gcc");
            builder.Arguments.Add("a.c");
            builder.Raw = "--version";
            _ = builder.WithFlag("ff", "-")
                .WithFlag("O2", "-")
                .WithOption("o", "a.out", "-")
                .WithOption("cc", 1, "--")
                .WithOption("cc", "a", "--")
                .WithoutFlag("-ff")
                .WithoutOption("--cc");
            Assert.AreEqual(0, builder.GetVariables().Count);
            Assert.AreEqual("gcc a.c -O2 -o a.out --version", string.Join(' ', await builder.Resolve(new ResolveContext())));
        }
    }
}
