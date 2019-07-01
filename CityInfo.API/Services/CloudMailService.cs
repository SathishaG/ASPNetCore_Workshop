using System;
namespace CityInfo.API.Services
{
    public class CloudMailService : IMailService
    {
        private string mailTo = Startup.Configuration["Mail:To"];
        private string mailFrom = Startup.Configuration["Mail:From"];
        public void Send(string subject, string message)
        {
            Console.WriteLine($"Mail from {mailFrom} to {mailTo} with cloud");
            Console.WriteLine($"Mail Subject {subject} message {message}");
        }
    }
}