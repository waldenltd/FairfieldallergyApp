using Business.Configuration;
using CreatingIDAndPasswords.Service;
using FairfieldAllergy.Data;
using FairfieldAllergy.Domain.Models;
using FairfieldAllergy.Domain.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace CreatingIDAndPasswords
{
    class Program
    {
        static void Main(string[] args)
        {
            HostFactory.Run(hostConfigurator =>
            {
                hostConfigurator.Service<CreateIds>(serviceConfigurator =>
                {
                    serviceConfigurator.ConstructUsing(() => new CreateIds());
                    serviceConfigurator.WhenStarted(createIds => createIds.Start());
                    serviceConfigurator.WhenStopped(createIds => createIds.Stop());
                });

                hostConfigurator.RunAsLocalSystem();
                hostConfigurator.SetDisplayName("Walden-Automate Sending Notes");
                hostConfigurator.SetDescription("Walden-Automate Sending Notes To Providers");
                hostConfigurator.SetServiceName("Walden-AutomateSendingNotes");
            });
        }
    }
}