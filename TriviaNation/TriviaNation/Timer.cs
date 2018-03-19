using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrystalJobScheduler;

namespace TriviaNation
{
    class Timer
    {
        public void TimedMethod(TimerTestTable TTT)
        {
            JobScheduler jobScheduler = new JobScheduler(TimeSpan.FromMinutes(.25), new Action(() => {
                for (int i = 0; i <= TTT.RetrieveNumberOfRowsInTable(); i++)
                {
                    Console.WriteLine(TTT.RetrieveTableRow(TTT.TableName, i));
                }
            }));
            jobScheduler.Start(); // To Start up the Scheduler
            //jobScheduler.Stop(); // To Stop Scheduler from Running.
        }
    }
}
