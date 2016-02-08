using System;
using NLog;

namespace CroquetAustralia.Logging
{
    public class Logger
    {
        private readonly ILogger _logger;

        internal Logger(ILogger logger)
        {
            _logger = logger;
        }

        public bool IsTraceEnabled => _logger.IsTraceEnabled;
        public bool IsDebugEnabled => _logger.IsDebugEnabled;
        public bool IsInformationEnabled => _logger.IsInfoEnabled;
        public bool IsWarningEnabled => _logger.IsWarnEnabled;
        public bool IsErrorEnabled => _logger.IsErrorEnabled;
        public bool IsFatalEnabled => _logger.IsFatalEnabled;

        public void Trace(string format, params object[] args)
        {
            _logger.Trace(format, args);
        }

        public void Trace(Exception exception, string format, params object[] args)
        {
            _logger.Trace(exception, format, args);
        }

        public void Debug(string format, params object[] args)
        {
            _logger.Debug(format, args);
        }

        public void Debug(Exception exception, string format, params object[] args)
        {
            _logger.Debug(exception, format, args);
        }

        public void Information(string format, params object[] args)
        {
            _logger.Info(format, args);
        }

        public void Information(Exception exception, string format, params object[] args)
        {
            _logger.Info(exception, format, args);
        }

        public void Warning(string format, params object[] args)
        {
            _logger.Warn(format, args);
        }

        public void Warning(Exception exception, string format, params object[] args)
        {
            _logger.Warn(exception, format, args);
        }

        public void Error(string format, params object[] args)
        {
            _logger.Error(format, args);
        }

        public void Error(Exception exception, string format, params object[] args)
        {
            _logger.Error(exception, format, args);
        }

        public void Fatal(string format, params object[] args)
        {
            _logger.Fatal(format, args);
        }

        public void Fatal(Exception exception, string format, params object[] args)
        {
            _logger.Fatal(exception, format, args);
        }
    }
}