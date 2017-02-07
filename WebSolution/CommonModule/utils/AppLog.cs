using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using log4net.Config;
using System.IO;
using System.Security.Permissions;
using System.Globalization;

namespace CommonModule
{
    public enum LOGLEVELTYPE : int
    {
        DEBUG = 0,
        INFO = 1,
        WARN = 2,
        ERROR = 3,
        FATAL = 4,
        TRACE = 10,
    }
    public class AppLog
    {
        private log4net.ILog log = null;
        private void SetLogFile(string appName,string fileName)
        {
            log = log4net.LogManager.GetLogger(appName);
            XmlConfigurator.ConfigureAndWatch(new System.IO.FileInfo(fileName));
        }

        public void writeLog(LOGLEVELTYPE logLevel, string userBy, string moduleName, string logTxt, string apiName, object startTime = null)
        {

            //lock (this)  // log4net thread safe, no need for manual locking
            {
                string logString;
                if (startTime != null)
                {
                    DateTime startT = (DateTime)startTime;
                    TimeSpan elapsedSpan = new TimeSpan(DateTime.Now.Ticks - startT.Ticks);
                    logString = string.Format("[{0}] [{1}] [{2},{3}],TimeSpan:{4}", userBy, moduleName, logTxt, apiName, elapsedSpan.TotalSeconds);
                }
                else
                {
                    logString = string.Format("[{0}] [{1}] [{2},{3}]", userBy, moduleName, logTxt, apiName);
                }

                switch (logLevel)
                {
                    case LOGLEVELTYPE.DEBUG:
                        log.Debug(logString);
                        break;
                    case LOGLEVELTYPE.INFO:
                        log.Info(logString);
                        break;
                    case LOGLEVELTYPE.WARN:
                        log.Warn(logString);
                        break;
                    case LOGLEVELTYPE.ERROR:
                        log.Error(logString);
                        break;
                    case LOGLEVELTYPE.FATAL:
                        log.Fatal(logString);
                        break;
                    case LOGLEVELTYPE.TRACE:
                        Console.WriteLine(DateTime.Now + " >>" + logString);
                        break;
                }
            }
        }

    }
}
