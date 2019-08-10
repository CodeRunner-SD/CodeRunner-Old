using CodeRunner.Loggings;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Test.Core.Loggings
{
    [TestClass]
    public class TLogger
    {
        [TestMethod]
        public void Basic()
        {
            Logger logger = new Logger("logger", LogLevel.Debug);
            logger.Log(new LogItem
            {
                Level = LogLevel.Debug,
            });
            Assert.AreEqual(1, logger.GetAll().Length);
        }

        [TestMethod]
        public void FilterLevel()
        {
            Logger logger = new Logger("logger", LogLevel.Information);
            logger.Log(new LogItem
            {
                Level = LogLevel.Debug,
            });
            logger.Log(new LogItem
            {
                Level = LogLevel.Information,
            });
            logger.Log(new LogItem
            {
                Level = LogLevel.Warning,
            });
            Assert.AreEqual(2, logger.GetAll().Length);
        }

        [TestMethod]
        public void Parent()
        {
            Logger logger = new Logger("logger", LogLevel.Information);
            Logger child = logger.CreateLogger("child", LogLevel.Warning);
            child.Log(new LogItem
            {
                Level = LogLevel.Warning
            });
            child.Log(new LogItem
            {
                Level = LogLevel.Information
            });
            logger.Log(new LogItem
            {
                Level = LogLevel.Information
            });
            Assert.AreEqual(1, child.GetAll().Length);
            Assert.AreEqual(2, logger.GetAll().Length);
        }

        [TestMethod]
        public void Scope()
        {
            Logger logger = new Logger("logger", LogLevel.Information);
            LogScope scope = logger.CreateScope("scope");
            scope.Information("info");
            scope.Debug("debug");
            scope.Warning("warning");
            scope.Error("error");
            scope.Error(new Exception());
            scope.Fatal("fatal");
            scope.Fatal(new Exception());
            LogItem item = logger.GetAll().FirstOrDefault();
            Assert.IsNotNull(item);
            Assert.AreEqual(LogLevel.Information, item.Level);
            Assert.AreEqual("logger/scope", item.Scope);
            Assert.AreEqual("info", item.Content);

            Logger subLogger = scope.CreateLogger("subLogger", LogLevel.Debug);
            Assert.AreEqual("logger/scope/subLogger", subLogger.Name);

            LogScope subScope = scope.CreateScope("subScope");
            Assert.AreEqual("logger/scope/subScope", subScope.Name);
        }
    }
}
