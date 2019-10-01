using CodeRunner.Extensions.Helpers.Rendering;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.CommandLine.Rendering;

namespace Test.Extensions.Helpers.Rendering
{
    [TestClass]
    public class TConsoleIO
    {
        [TestMethod]
        public void OutputStyle()
        {
            ITerminal terminal = new TestTerminal();
            terminal.OutputEmphasize("");
            terminal.OutputBlink("");
            terminal.OutputBold("");
            terminal.OutputStandout("");
            terminal.OutputUnderline("");
        }

        [TestMethod]
        public void OutputColor()
        {
            ITerminal terminal = new TestTerminal();
            terminal.OutputErrorLine("");
            terminal.OutputWarningLine("");
            terminal.OutputInformationLine("");
            terminal.OutputFatalLine("");
            terminal.OutputDebugLine("");
        }

        [TestMethod]
        public void OutputTable()
        {
            ITerminal terminal = new TestTerminal();
            terminal.OutputTable(new string[] { "a", "b", "c" }, new OutputTableColumnStringView<string>(x => x, "Header"));
        }
    }
}
