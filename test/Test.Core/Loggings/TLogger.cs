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
            ILogger logger = new Logger();
            logger.Log(new LogItem
            {
                Level = LogLevel.Debug,
            });
            Assert.AreEqual(1, logger.View().Count());
        }

        [TestMethod]
        public void FilterLevel()
        {
            ILogger logger = new Logger().UseLevelFilter(LogLevel.Information);
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
            Assert.AreEqual(2, logger.View().Count());
        }

        [TestMethod]
        public void Parent()
        {
            ILogger logger = new Logger();
            ILogger child = new Logger(logger).UseLevelFilter(LogLevel.Warning);
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
            Assert.AreEqual(1, child.View().Count());
            Assert.AreEqual(2, logger.View().Count());
        }

        [TestMethod]
        public void Scope()
        {
            Logger logger = new Logger();
            LoggerScope scope = logger.CreateScope("scope", LogLevel.Information);
            scope.Information("info");
            scope.Debug("debug");
            scope.Warning("warning");
            scope.Error("error");
            scope.Error(new Exception());
            scope.Fatal("fatal");
            scope.Fatal(new Exception());
            LogItem item = logger.View().FirstOrDefault();
            Assert.IsNotNull(item);
            Assert.AreEqual(LogLevel.Information, item.Level);
            Assert.AreEqual("/scope", item.Scope);
            StringAssert.Contains(item.Content, "info");

            LoggerScope subScope = scope.CreateScope("subScope", LogLevel.Debug);
            Assert.AreEqual("/scope/subScope", subScope.Name);
        }
    }
}
