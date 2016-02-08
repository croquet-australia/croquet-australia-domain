using NLog;

namespace CroquetAustralia.Logging
{
    public class LoggerFactory
    {
        public static Logger GetLogger<T>()
        {
            return new Logger(LogManager.GetLogger(typeof(T).FullName));
        }
    }
}