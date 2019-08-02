using CodeRunner.Commands;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Test.Core.Commands
{
    [TestClass]
    public class BuilderTest
    {
        [TestMethod]
        public void Basic()
        {
            {
                CommandLineBuilder builder = new CommandLineBuilder();
                builder.Command.Add("gcc");
                builder.Flags.Add("ff");
                builder.Raw = "-O2";
                builder.Arguments.Add("a.c");
                builder.AddOption("o", "a.out");
                Assert.AreEqual("gcc a.c --ff -o a.out -O2", builder.ToString());
            }

        }
    }
}
