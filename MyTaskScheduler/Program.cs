using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32.TaskScheduler;    //!

namespace MyTaskScheduler
{
    /// <summary>
    /// NuGet: Task Scheduler Managed Wrapper
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Programme start");
            //  -----------------------------------------------------

            InitializeSchduledTask();
            Console.WriteLine("Task has been scheduled");
            Console.WriteLine("Press any key to kill task");
            Console.ReadKey();
            DeleteScheduledTask();

            //  -----------------------------------------------------
            Console.WriteLine("Programme ended - press any key");
            Console.ReadKey();
        }

        private static void InitializeSchduledTask()
        {
            TimeSpan howOften = new TimeSpan(0, 0, 1, 0);
            TimeSpan howLong = new TimeSpan(0, 0, 3, 0);
            RepetitionPattern rp = new RepetitionPattern(howOften, howLong, stopAtDurationEnd: true);

            TimeTrigger tt = new TimeTrigger();
            tt.StartBoundary = DateTime.Now;
            tt.Repetition = rp;
            
            ShowMessageAction msg = new ShowMessageAction("Jeg er en ost","Hvad er du?");
            TaskDefinition td = TaskService.Instance.NewTask();
            td.RegistrationInfo.Author = "Holle TechNolle";
            td.Actions.Add(msg);
            td.Triggers.Add(tt);

            TaskService.Instance.RootFolder.RegisterTaskDefinition("MsgTask", td);

            tt.Enabled = true;
        }

        private static void DeleteScheduledTask()
        {
            TaskService.Instance.RootFolder.DeleteTask("MsgTask", exceptionOnNotExists: false);
        }

        static void HelloSailor()
        {
            Console.WriteLine("Hello, Sailor ...");
        }

        static void OriginaExampleCode()
        {
            return;
            // Create a new task definition for the local machine and assign properties
            TaskDefinition td = TaskService.Instance.NewTask();
            td.RegistrationInfo.Description = "Does something";
            
            // Add a trigger that, starting tomorrow, will fire every other week on Monday
            // and Saturday and repeat every 10 minutes for the following 11 hours
            WeeklyTrigger wt = new WeeklyTrigger();
            wt.StartBoundary = DateTime.Today.AddDays(1);
            wt.DaysOfWeek = DaysOfTheWeek.Monday | DaysOfTheWeek.Saturday;
            wt.WeeksInterval = 2;
            wt.Repetition.Duration = TimeSpan.FromHours(11);
            wt.Repetition.Interval = TimeSpan.FromMinutes(10);
            td.Triggers.Add(wt);

            // Create an action that will launch Notepad whenever the trigger fires
            td.Actions.Add("notepad.exe", "c:\\test.log");

            // Register the task in the root folder of the local machine
            TaskService.Instance.RootFolder.RegisterTaskDefinition("Test", td);
        }
    }
}
