using Serilog;
using System;

namespace Gardient.SQL2Email
{
    class Program
    {
        static void Main(string[] args)
        {
            Serilog.Debugging.SelfLog.Enable(Console.Out);
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.AppSettings()
                .CreateLogger();

            Log.Information("SQL2Email started");

            EmailSender.SendEmails(DBExecuter.GetData());

            Log.Information("SQL2Email finished");
        }
    }
}
