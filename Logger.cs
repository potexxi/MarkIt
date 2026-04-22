using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;

namespace MarkIt
{
    public static class Logger
    {
        public static Serilog.ILogger logger { get; set; }



        public static void Init()
        {
            LoggerConfiguration config = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.File("markit-log-.txt", rollingInterval: RollingInterval.Day, retainedFileCountLimit: 5);
            logger = config.CreateLogger();
        }
    }
}
