using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreditStatistics
{
    internal class PortCheckerService
    {
        // Define event arguments

        private cManagedPCs ManagedPCs;
        private Task aTask;

        public PortCheckerService(ref  cManagedPCs rManagedPCs)
        {
            ManagedPCs = rManagedPCs;
        }

        public class StatusEventArgs : EventArgs
        {
            public string PCname { get; }
            public bool HasSSH { get; }
            public bool HasBoinc { get; }

            public StatusEventArgs(string ComputerID, bool hasBoinc,bool hasSSH)
            {
                PCname = ComputerID;
                HasBoinc = hasBoinc;
                HasSSH = hasSSH;
            }
        }

        // Declare the event
        public event EventHandler<StatusEventArgs> StatusChecked;

        // Helper to raise the event
        protected virtual void OnStatusChecked(string PCname, bool hasBoinc, bool hasSSH)
        {
            StatusChecked?.Invoke(this, new StatusEventArgs(PCname, hasBoinc, hasSSH));
        }

        public void CheckOnlineStatus()
        {
            foreach (cHostInfo hi in ManagedPCs.LocalSystems)
            {
                string IPaddress = hi.IPaddress;
                aTask = Task.Run(async () =>
                {
                    hi.HasBOINC = await globals.PortChecker.IsPortOpenAsync(IPaddress, 31416);
                });
                aTask.Wait();

                aTask = Task.Run(async () =>
                {
                    if (hi.IPaddress != "127.0.0.1")
                        hi.HasSSH = await globals.PortChecker.IsPortOpenAsync(hi.IPaddress, 44);
                    else hi.HasSSH = true;
                });
                aTask.Wait();

                OnStatusChecked(hi.ComputerID, hi.HasBOINC, hi.HasSSH);
            }
        }
    }


}
