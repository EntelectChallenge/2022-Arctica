namespace Logger.Models
{
    public class LoggerConfig
    {
        public string RunnerUrl { get; set; }
        public string RunnerPort { get; set; }
        public string LogDirectory { get; set; }
        public string GameStateLogFileName { get; set; }
        public string GameStateStaticLogFileName { get; set; }
        public string GameExceptionLogFileName { get; set; }
        public bool CondencedLoggingToggle { get; set; }
    }
}