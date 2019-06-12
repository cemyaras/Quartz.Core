using Quartz.Impl;
using System;
using System.Threading.Tasks;

namespace Quartz.Core
{
    class Program
    {
        static void Main(string[] args)
        {
            Job.InitializeJobs();

            Console.WriteLine("Tasks started!");

            Console.ReadLine();
        }
    }

    public static class Job
    {
        public static async void InitializeJobs()
        {
            var _schduler = await new StdSchedulerFactory().GetScheduler();
            await _schduler.Start();

            var userEmailsJob = JobBuilder.Create<SendUserEmailsJob>()
                .WithIdentity("SendUserEmailsJob")
                .Build();
            var userEmailsTrigger = TriggerBuilder.Create()
                .WithIdentity("SendUserEmailsCron")
                .StartNow()
                .WithCronSchedule("* * * ? * *")
                .Build();

            var result = await _schduler.ScheduleJob(userEmailsJob, userEmailsTrigger);
        }
    }

    public class SendUserEmailsJob: IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine(DateTime.Now.ToString("HH:mm:ss"));

            return Task.CompletedTask;
        }
    }
}
