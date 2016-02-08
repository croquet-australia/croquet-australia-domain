using System;
using System.Linq;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace CroquetAustralia.Logging
{
    public static class LoggerConfiguration
    {
        public static void SetChainsawLevel(LoggerLevel minimumLevel)
        {
            SetTargetLevel(
                "Chainsaw",
                ToLogLevel(minimumLevel),
                targetName => new ChainsawTarget {Name = targetName, Address = "udp://127.0.0.1:7071"});
        }

        private static void SetTargetLevel(string targetName, LogLevel minimumLevel, Func<string, Target> targetFactory)
        {
            var config = LogManager.Configuration ?? new LoggingConfiguration();
            var target = config.FindTargetByName(targetName) ?? targetFactory(targetName);
            var loggingRule = config.LoggingRules.SingleOrDefault(rule => rule.Targets.Single().Name == targetName);

            if (loggingRule == null)
            {
                config.AddTarget(targetName, target);
            }
            else
            {
                config.LoggingRules.Remove(loggingRule);
            }

            config.LoggingRules.Add(new LoggingRule("*", minimumLevel, target));

            if (LogManager.Configuration == null)
            {
                LogManager.Configuration = config;
            }
            else
            {
                config.Reload();
            }
        }

        private static LogLevel ToLogLevel(LoggerLevel minimumLevel)
        {
            return LogLevel.FromOrdinal((int) minimumLevel);
        }
    }
}