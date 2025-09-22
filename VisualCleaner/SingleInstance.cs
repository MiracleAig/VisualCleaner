using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace VisualCleaner
{
    internal class SingleInstance
    {
        private const string MutexName = "Global\\KeyboardCleaningMode.TrayApp.v1";
        private const string ExitEventName = "Global\\KeyboardCleaningMode.TrayApp.v1.ExitEvent";
        private static Mutex? mutex;


        public static bool AquireMutex()
        {
            try
            {
                mutex = new Mutex(
                    initiallyOwned: true, 
                    name: MutexName, 
                    createdNew: out bool createdNew
                );
                return createdNew;
            }
            catch
            {
                // If mutex creation fails, allow startup to proceed
                return true;
            }
        }

        public static void RequestExistingInstanceExit()
        {
            try
            {
                using var ewh = EventWaitHandle.OpenExisting(ExitEventName);
                ewh.Set();
            }
            catch
            {
                // If event open or set fails, ignore
            }
        }
    }
}
