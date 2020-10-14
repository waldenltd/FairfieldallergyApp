using Business.Configuration;
using FairfieldAllergy.Data;
using FairfieldAllergy.Domain.Models;
using FairfieldAllergy.Domain.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace CreatingIDAndPasswords.Service
{
    public class CreateIds
    {
        System.Timers.Timer timer = null;
        static string whyFaxWasNotSent = string.Empty;

        public void Start()
        {
            timer = new System.Timers.Timer(30000);
            timer.Elapsed += new ElapsedEventHandler(this.ServiceTimer_Tick);

            timer.Enabled = true;
            timer.Start();
        }

        public void Stop()
        {
            timer.Enabled = false;
        }

        private async void ServiceTimer_Tick(object sender, System.Timers.ElapsedEventArgs e)
        {
            timer.Stop();

            //1.  Get new records from Roach
            //2.  Iterate through the records and create id's and passwords
            //3.  Send email to user with information

            // Generate a random number  
            Random random = new Random();

            // Use other methods   
            RandomNumberGenerator generator = new RandomNumberGenerator();

            FairfieldAllergeryRepository fairfieldAllergeryRepository = new FairfieldAllergeryRepository();

            List<AllergyPatient> allergyPatient = new List<AllergyPatient>();


            ConfigurationValues.FairfieldAllergyConnection = System.Configuration.ConfigurationManager.AppSettings["fairfieldDatabaseConnection"];
            ConfigurationValues.RegisterUrl = System.Configuration.ConfigurationManager.AppSettings["registerUrl"];

            OperationResult getCurrentUserId = fairfieldAllergeryRepository.GetCurrentId();

            int currentId = getCurrentUserId.CurrentId;
            allergyPatient = fairfieldAllergeryRepository.GetListOfNewUsers(currentId.ToString());


            for (int i = 0; i < allergyPatient.Count; i++)
            {
                //i = 3695;

                //Console.WriteLine("Begin record: " + i.ToString() + " of " + allergyPatient.Count.ToString());
                try
                {
                    Console.WriteLine(DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + " Processing record " + i.ToString() + " of " + allergyPatient.Count.ToString());

                    if (allergyPatient[i].first_name.Length > 0 && allergyPatient[i].last_name.Length > 0)
                    {
                        if (allergyPatient[i].first_name.Length == 1)
                        {
                            allergyPatient[i].first_name = "None";
                        }

                        if (allergyPatient[i].last_name.Length == 1)
                        {
                            allergyPatient[i].last_name = "None";
                        }

                        string userID = generator.RandomString(10, false);

                        string password = generator.RandomPassword();

                        fairfieldAllergeryRepository.CreateNewUser(allergyPatient[i], userID, password);

                        string sex = string.Empty;
                        if (allergyPatient[i].gender == 1)
                        {
                            sex = "M";
                        }
                        else if (allergyPatient[i].gender == 2)
                        {
                            sex = "F";
                        }

                        RegisterViewModel registerViewModel = new RegisterViewModel();

                        registerViewModel.Email = "None";

                        registerViewModel.FirstName = allergyPatient[i].first_name;
                        registerViewModel.LastName = allergyPatient[i].last_name;
                        if (string.IsNullOrEmpty(allergyPatient[i].home_phone))
                        {
                            registerViewModel.HomePhone = "0000000000";
                        }
                        else
                        {
                            registerViewModel.HomePhone = allergyPatient[i].home_phone;
                        }

                        if (sex.Length < 1)
                        {
                            registerViewModel.Sex = "U";
                        }
                        else
                        {
                            registerViewModel.Sex = sex;
                        }


                        if (string.IsNullOrEmpty(allergyPatient[i].birthday.ToShortDateString()))
                        {
                            registerViewModel.DateOfBirth = " ";
                        }
                        else
                        {
                            registerViewModel.DateOfBirth = allergyPatient[i].birthday.ToShortDateString();
                        }


                        //registerViewModel.DateOfBirth = allergyPatient[i].birthday.ToShortDateString();

                        registerViewModel.UserName = userID;
                        registerViewModel.Password = password;
                        registerViewModel.ConfirmPassword = password;

                        var json = JsonConvert.SerializeObject(registerViewModel);
                        var data = new StringContent(json, Encoding.UTF8, "application/json");

                        var url = ConfigurationValues.RegisterUrl;
                        using var client = new HttpClient();

                        var response = await client.PostAsync(url, data);

                        string jsonResult = response.Content.ReadAsStringAsync().Result;

                        var result = JsonConvert.DeserializeObject<Status>(jsonResult);

                        Console.WriteLine(result);

                        currentId = allergyPatient[i].pkid;

                        if (result.status == "Failure")
                        {
                            string s2 = string.Empty;
                        }


                        fairfieldAllergeryRepository.UpdateWebIdTableWithGuidFromAspNetUsersTable(userID);
                    }
                    Console.WriteLine("Done with record: " + i.ToString() + " of " + allergyPatient.Count.ToString());

                }
                catch (Exception er)
                {
                    Console.WriteLine(er.ToString());
                }
                //Need to get GUID in AspNetUsers table and put it into WEBID
            }

            if (allergyPatient.Count > 0)
            {
                fairfieldAllergeryRepository.UpdateUserId(currentId++);
            }
            else
            {
                Console.WriteLine("No records to process");
            }

            fairfieldAllergeryRepository.UpdateUsersWithDataFromRoach();

            //SQL Access Info
            //ID: walden
            //PW: Db#rd!927!Mz
            //DB Name: RoschIT
            //172.16.1.13

            //Thread.Sleep(3600000);

            timer.Start();
        }

        public class RandomNumberGenerator
        {
            // Generate a random number between two numbers    
            public int RandomNumber(int min, int max)
            {
                Random random = new Random();
                return random.Next(min, max);
            }

            // Generate a random string with a given size and case.   
            // If second parameter is true, the return string is lowercase  
            public string RandomString(int size, bool lowerCase)
            {
                StringBuilder builder = new StringBuilder();
                Random random = new Random();
                char ch;
                for (int i = 0; i < size; i++)
                {
                    ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                    builder.Append(ch);
                }
                if (lowerCase)
                    return builder.ToString().ToLower();
                return builder.ToString();
            }

            // Generate a random password of a given length (optional)  
            public string RandomPassword(int size = 0)
            {
                StringBuilder builder = new StringBuilder();
                builder.Append(RandomString(4, true));
                builder.Append(RandomNumber(1000, 9999));
                builder.Append(RandomString(2, false));
                return builder.ToString();
            }
        }

    }//end of class 
}

