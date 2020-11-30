using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

namespace Gardient.SQL2Email
{
    class Program
    {
        static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.EventLog("Application", restrictedToMinimumLevel: LogEventLevel.Information)
                .WriteTo.Console(theme: SystemConsoleTheme.Colored)
                .ReadFrom.AppSettings()
                .CreateLogger();

            Log.Information("SQL2Email started");

            EmailSender.SendEmails(DBExecuter.GetData());

            Log.Information("SQL2Email finished");
        }
    }
}
