using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyCensus.Utils
{
    public static class LogException
    {
        //MyCensus.Utils
        //Task.Factory.StartNew(() => PopYourPage()).LogExceptions();
        public static void LogExceptions(this Task task)
        {
            task.ContinueWith(t =>
            {
                var aggException = t.Exception.Flatten();
                foreach (var exception in aggException.InnerExceptions)
                {
                    System.Diagnostics.Debug.WriteLine("LOG EXCEPTION catch: " + exception);
                }
            },
            TaskContinuationOptions.OnlyOnFaulted);
        }
    }
}
