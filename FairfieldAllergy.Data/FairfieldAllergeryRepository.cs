using Business.Configuration;
using Dapper;
using FairfieldAllergy.Domain;
using FairfieldAllergy.Domain.Models;
using FairfieldAllergy.Domain.ViewModels;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace FairfieldAllergy.Data
{
    public class FairfieldAllergeryRepository
    {
        private string SqlForError { get; set; }
        private string Query { get; set; }

        public FairfieldAllergeryRepository()
        {
            Query = string.Empty;
            SqlForError = string.Empty;
        }

        public OperationResult AddFamilyAccount(FamilyAccount familyAccount)
        {
            OperationResult operationResult = new OperationResult();

            try
            {
                using (SqlConnection db = new SqlConnection(ConfigurationValues.FairfieldAllergyConnection))
                {
                    var returnValue = db.Query<int>("dt_add_family_account", new
                    {

                        @main_account = familyAccount.MainAccount,
                        @added_account = familyAccount.AddedAccount
                    },
                    commandType: CommandType.StoredProcedure).FirstOrDefault();

                    if (returnValue == 1)
                    {
                        operationResult.Success = true;
                        operationResult.ErrorMessage = "None";
                        return operationResult;
                    }
                    else
                    {
                        operationResult.Success = false;
                        operationResult.ErrorMessage = "Account Already Created";
                        return operationResult;
                    }
                }

            }
            catch (Exception er)
            {
                operationResult.Success = false;
                operationResult.ErrorMessage = er.ToString();
                return operationResult;
            }

            //try
            //{
            //    using (SqlConnection db = new SqlConnection(ConfigurationValues.FairfieldAllergyConnection))
            //    {
            //        var returnValue = db.Query<int>("dt_add_family_account", new
            //        {

            //            @main_account = familyAccount.MainAccount,
            //            @added_account = familyAccount.AddedAccount
            //        },
            //        commandType: CommandType.StoredProcedure).FirstOrDefault();
            //        //return Json(categories);
            //    }


            //    Query = "SELECT count(*) FROM [Appointment].[family_accounts]"
            //    + " where main_account = @main_account"
            //    + " and added_account = @added_account";

            //    using (SqlConnection db = new SqlConnection(ConfigurationValues.FairfieldAllergyConnection))
            //    {
            //        count = db.Query<int>(Query, new
            //        {
            //            @main_account = familyAccount.MainAccount,
            //            @added_account = familyAccount.AddedAccount

            //        }).Single();
            //        //   return allergyPatient;
            //    }
            //}
            //catch (Exception er)
            //{
            //    operationResult.Success = false;
            //    operationResult.ErrorMessage = "Account Already Created";
            //    return operationResult;

            //}

            //if (count < 1)
            //{

            //    using (IDbConnection db = new SqlConnection(ConfigurationValues.FairfieldAllergyConnection))
            //    {
            //        try
            //        {
            //            Query = "INSERT INTO [Appointment].[family_accounts]"
            //            + " ([main_account],[added_account],[date_added])"
            //            + " VALUES("
            //            + " @main_account, @added_account,@date_added"
            //            + " )";

            //            count = db.Execute(Query, new
            //            {
            //                @main_account = familyAccount.MainAccount,
            //                @added_account = familyAccount.AddedAccount,
            //                @date_added = DateTime.Now
            //            });

            //            operationResult.Success = true;
            //            operationResult.ErrorMessage = "None";
            //            return operationResult;
            //        }
            //        catch (Exception er)
            //        {
            //            operationResult.Success = false;
            //            operationResult.ErrorMessage = er.ToString();
            //            return operationResult;
            //        }
            //    }
            //}
            //else
            //{
            //    operationResult.Success = false;
            //    operationResult.ErrorMessage = "Account Already Created";
            //    return operationResult;

            //}
        }

        public void UpdateEmail(string id, string email)
        {
            try
            {
                using (SqlConnection db = new SqlConnection(ConfigurationValues.FairfieldAllergyConnection))
                {
                    var returnValue = db.Query<int>("dt_update_email_accountx", new
                    {
                        @id = id,
                        @email = email
                    },
                    commandType: CommandType.StoredProcedure).FirstOrDefault();
                }

            }
            catch (Exception er)
            {
                string s1 = er.ToString();
                //operationResult.Success = false;
                //operationResult.ErrorMessage = er.ToString();
                //return operationResult;
            }

            string userid = string.Empty;

            try
            {
                Query = "SELECT [UID]"
                    + " FROM[Appointment].[WEBID]"
                    + "  where [PersonID] = @PersonID";

                using (SqlConnection db = new SqlConnection(ConfigurationValues.FairfieldAllergyConnection))
                {
                    userid = db.Query<string>(Query, new
                    {
                        @PersonID = id
                    }).Single();
                    //   return allergyPatient;
                }
            }
            catch (Exception er)
            {
                //operationResult.Success = false;
                //operationResult.ErrorMessage = "Account Already Created";
                //return operationResult;
            }

            int count = 0;
            using (IDbConnection db = new SqlConnection(ConfigurationValues.FairfieldAllergyConnection))
            {
                try
                {
                    Query = "UPDATE [Appointment].[AspNetUsers]"
                    + " SET [Email] = @Email"
                    + " , [NormalizedEmail] = @NormalizedEmail"
                    + " , [EmailConfirmed] = @EmailConfirmed"
                    + " where [UserName] = @UserName";

                    count = db.Execute(Query, new
                    {
                        @Email = email,
                        @NormalizedEmail = email,
                        @EmailConfirmed = -1,
                        @UserName = userid
                    });

                    //operationResult.Success = true;
                    //operationResult.ErrorMessage = "None";
                    //return operationResult;
                }
                catch (Exception er)
                {
                    //operationResult.Success = false;
                    //operationResult.ErrorMessage = er.ToString();
                    //return operationResult;
                }
            }





            //using (IDbConnection db = new SqlConnection(ConfigurationValues.FairfieldAllergyConnection))
            //{
            //    try
            //    {
            //        Query = " update [Appointment].[Appointment].[Person]"
            //            + " set email = @email"
            //            + " where id = @id";

            //        rowsAffected = db.Execute(Query, new
            //        {
            //            @email = email,
            //            @id = id
            //        });
            //    }
            //    catch (Exception er)
            //    {
            //        string s1 = er.ToString();
            //    }
            //}
        }
        //Start here 
        public OperationResult GetFamilyAccount(int mainAccount)
        {
            OperationResult operationResult = new OperationResult();


            try
            {
                using (SqlConnection db = new SqlConnection(ConfigurationValues.FairfieldAllergyConnection))
                {
                    operationResult.Patient = db.Query<Patient>("dt_get_family_account", new
                    {
                        @main_account = mainAccount
                    },
                    commandType: CommandType.StoredProcedure).ToList();
                }

            }
            catch (Exception er)
            {
                string s1 = er.ToString();
                operationResult.Success = false;
                operationResult.ErrorMessage = er.ToString();
                return operationResult;
            }

            operationResult.Success = true;
            operationResult.ErrorMessage = "None";
            return operationResult;

            //try
            //{
            //    Query = " select C.Id as Id, LTRIM(RTRIM(C.firstname)) + ' ' + LTRIM(RTRIM(C.lastname)) as PatientName,"
            //    + " C.locationid as PatientLocation, B.ID as PatientWebId "
            //    + " FROM [Appointment].[Appointment].[family_accounts] A"
            //    + " inner join [Appointment].WEBID B"
            //    + " on A.added_account = B.ID"
            //    + " inner join [Appointment].Person C"
            //    + " on B.PersonID = C.ID"
            //    + " Where A.main_account = @ID";

            //    using (SqlConnection db = new SqlConnection(ConfigurationValues.FairfieldAllergyConnection))
            //    {
            //        operationResult.Patient = db.Query<Patient>(Query, new
            //        {
            //            @ID = mainAccount.ToString()
            //        }).ToList();
            //    }
            //}
            //catch (Exception er)
            //{
            //    // return allergyPatient;
            //}
            //return operationResult;
        }
        //Do not convert to Stored Procedure now
        public List<BroadcastMessage> GetBroadcastMessage(string userId, string location)
        {
            OperationResult operationResult = new OperationResult();
            List<BroadcastMessage> broadcastMessageHeard = new List<BroadcastMessage>();
            List<BroadcastMessage> broadcastMessage = new List<BroadcastMessage>();

            if (location == "1")
            {
                location = " and A.Norwalk = 'true'";
            }
            else if (location == "2")
            {
                location = " and A.Greenwich = 'true'";
            }
            else if (location == "3")
            {
                location = " and A.Stamford = 'true'";
            }
            else if (location == "5")
            {
                location = " and A.Ridgefield = 'true'";
            }

            try
            {
                Query = "SELECT A.Id  FROM [Appointment].[BroadcastMessage] A"
                + " inner join [Appointment].[BroadcastMessageRead] B"
                + " on B.BroadcastMessageId = A.Id"
                + " where B.UserId = @UserId"
                + location;

                using (SqlConnection db = new SqlConnection(ConfigurationValues.FairfieldAllergyConnection))
                {
                    broadcastMessageHeard = db.Query<BroadcastMessage>(Query, new
                    {
                        @UserId = userId

                    }).ToList();
                    //   return allergyPatient;
                }
            }
            catch (Exception er)
            {
                string s1 = er.ToString();
            }

            string listOfSeenBroadcastMessages = string.Empty;

            if (broadcastMessageHeard.Count > 0)
            {
                listOfSeenBroadcastMessages = "(";

                for (int i = 0; i < broadcastMessageHeard.Count; i++)
                {
                    listOfSeenBroadcastMessages = listOfSeenBroadcastMessages + broadcastMessageHeard[i].Id.ToString() + ",";
                }

                listOfSeenBroadcastMessages = listOfSeenBroadcastMessages.Substring(0, listOfSeenBroadcastMessages.Length - 1);

                listOfSeenBroadcastMessages = listOfSeenBroadcastMessages + ")";
            }
            else
            {
                listOfSeenBroadcastMessages = "(0)";
            }

            string s2 = string.Empty;
            try
            {
                Query = "SELECT A.Message, A.Id FROM [Appointment].[BroadcastMessage] A"
                + " where A.Id NOT IN " + listOfSeenBroadcastMessages
                + location;

                using (SqlConnection db = new SqlConnection(ConfigurationValues.FairfieldAllergyConnection))
                {
                    broadcastMessage = db.Query<BroadcastMessage>(Query).ToList();
                }
            }
            catch (Exception er)
            {
                string s1 = er.ToString();
                // return allergyPatient;
            }

            for (int j = 0; j < broadcastMessage.Count; j++)
            {
                UpdateViewedBroadcastMessage(userId, broadcastMessage[j].Id.ToString());

            }

            return broadcastMessage;
        }

        public void UpdateViewedBroadcastMessage(string userId, string broadcastMessageId)
        {

            int rowsAffected = 0;

            try
            {
                using (SqlConnection db = new SqlConnection(ConfigurationValues.FairfieldAllergyConnection))
                {
                    rowsAffected = db.Query<int>("dt_update_broadcast_message", new
                    {
                        @user_id = userId,
                        @broadcast_message_id = broadcastMessageId
                    },
                    commandType: CommandType.StoredProcedure).FirstOrDefault();
                }

            }
            catch (Exception er)
            {
                string s1 = er.ToString();
            }

            //int rowsAffected = 0;
            //using (IDbConnection db = new SqlConnection(ConfigurationValues.FairfieldAllergyConnection))
            //{
            //    try
            //    {
            //        Query = "INSERT INTO [Appointment].[BroadcastMessageRead]"
            //        + " ([BroadcastMessageId],[UserId],[DateShown])"
            //        + " VALUES("
            //        + " @BroadcastMessageId,@UserId,@DateShown)";

            //        rowsAffected = db.Execute(Query, new
            //        {

            //            @BroadcastMessageId = broadcastMessageId,
            //            @UserId = userId,
            //            @DateShown = DateTime.Now
            //        });
            //    }
            //    catch (Exception er)
            //    {
            //        string s1 = er.ToString();
            //    }
            //}
        }

        public OperationResult UpdateUserName(string oldUserName, string newUserName, string password)
        {
            OperationResult operationResult = new OperationResult();

            int rowsAffected = 0;

            try
            {
                using (SqlConnection db = new SqlConnection(ConfigurationValues.FairfieldAllergyConnection))
                {
                    rowsAffected = db.Query<int>("dt_update_user_name", new
                    {
                        @old_user_name = oldUserName,
                        @new_user_name = newUserName
                    },
                    commandType: CommandType.StoredProcedure).FirstOrDefault();
                }

            }
            catch (Exception er)
            {
                operationResult.Success = false;
                operationResult.ErrorMessage = er.ToString();
                return operationResult;

            }

            operationResult.Success = true;
            operationResult.ErrorMessage = "None";
            return operationResult;

            //OperationResult operationResult = new OperationResult();
            //int rowsAffected = 0;

            //using (IDbConnection db = new SqlConnection(ConfigurationValues.FairfieldAllergyConnection))
            //{
            //    try
            //    {
            //        Query = "update Appointment.WEBID"
            //           + " set UID = @NewUID"
            //           + " where UID = @OldUID";


            //        rowsAffected = db.Execute(Query, new
            //        {
            //            @OldUID = oldUserName,
            //            @NewUID = newUserName
            //        });

            //        operationResult.Success = true;
            //        operationResult.ErrorMessage = "Success";
            //        return operationResult;
            //    }
            //    catch (Exception er)
            //    {
            //        operationResult.Success = false;
            //        operationResult.ErrorMessage = er.ToString();
            //        return operationResult;
            //    }
            //}
        }

        public bool CheckToSeeIfIdIsAlreadyUsed(string userName)
        {
            int count = 0;
            try
            {
                Query = "SELECT count(*) "
                    + " FROM[Appointment].[WEBID]"
                    + " where [UID] = @UID";

                using (SqlConnection db = new SqlConnection(ConfigurationValues.FairfieldAllergyConnection))
                {
                    count = db.Query<int>(Query, new {
                        @UID = userName

                    }).Single();
                }

                if (count > 0)
                {
                    return false;
                }
                else
                {
                    return true; 
                }
            }
            catch (Exception er)
            {
                string s1 = er.ToString();
                return false;
            }
        }

        public OperationResult GetCurrentId()
        {
            int currentId = 0;
            OperationResult operationResult = new OperationResult();

            try
            {
                using (SqlConnection db = new SqlConnection(ConfigurationValues.FairfieldAllergyConnection))
                {
                    currentId = db.Query<int>("dt_get_current_id",
                    commandType: CommandType.StoredProcedure).Single();
                }

                operationResult.CurrentId = currentId;
                operationResult.ErrorMessage = "None";
                return operationResult;
            }
            catch (Exception er)
            {
                operationResult.Success = false;
                operationResult.ErrorMessage = er.ToString();
                return operationResult;

            }


            //    int currentId = 0;
            //    OperationResult operationResult = new OperationResult();

            //    try
            //    {
            //        Query = "SELECT [current_pkid] FROM Appointment.[CurrentPkid]";

            //        using (SqlConnection db = new SqlConnection(ConfigurationValues.FairfieldAllergyConnection))
            //        {
            //            currentId = db.Query<int>(Query).Single();
            //            operationResult.CurrentId = currentId;
            //            operationResult.ErrorMessage = "None";
            //            return operationResult;
            //        }
            //    }
            //    catch (Exception er)
            //    {
            //        operationResult.Success = false;
            //        operationResult.ErrorMessage = er.ToString();
            //        return operationResult;
            //    }
        }

        public void UpdateUseridAndPassword(ChangeCredentialsViewModel model)
        {

            int rowsAffected = 0;

            using (IDbConnection db = new SqlConnection(ConfigurationValues.FairfieldAllergyConnection))
            {
                try
                {
                    Query = "UPDATE[Appointment].[WEBID]"
                        + " SET [UID] = @UID"
                        + ", [PWD] = @PWD"
                        + ",[has_changed_password] = 'T' "
                        + " where [PWD] = @oldPassword";

                    rowsAffected = db.Execute(Query, new
                    {

                        @UID = model.UserName,
                        @PWD = model.Password,
                        @oldPassword = model.OldPassword
                    });
                }
                catch (Exception er)
                {
                    string s1 = er.ToString();
                }
            }

            using (IDbConnection db = new SqlConnection(ConfigurationValues.FairfieldAllergyConnection))
            {
                try
                {
                    Query = "UPDATE[Appointment].[AspNetUsers]"
                        + " SET [UserName] = @UserName"
                        + " ,[NormalizedUserName] = @NormalizedUserName"
                        + " WHERE [UserName] = @OldUserName";

                    rowsAffected = db.Execute(Query, new
                    {
                        @UserName = model.UserName,
                        @NormalizedUserName = model.UserName,
                        @OldUserName = model.OldUserName
                    });
                }
                catch (Exception er)
                {
                    string s1 = er.ToString();
                }
            }
        }

        public OperationResult UpdateUserId(int userId)
        {

            int rowsAffected = 0;
            OperationResult operationResult = new OperationResult();

            try
            {
                using (SqlConnection db = new SqlConnection(ConfigurationValues.FairfieldAllergyConnection))
                {
                    rowsAffected = db.Query<int>("dt_update_user_id", new {
                        @user_id = userId
                    },
                    commandType: CommandType.StoredProcedure).FirstOrDefault();
                }

                operationResult.Success = true;
                operationResult.ErrorMessage = "None";
                return operationResult;
            }
            catch (Exception er)
            {
                operationResult.Success = false;
                operationResult.ErrorMessage = er.ToString();
                return operationResult;

            }

            //OperationResult operationResult = new OperationResult();
            //int rowsAffected = 0;

            //using (IDbConnection db = new SqlConnection(ConfigurationValues.FairfieldAllergyConnection))
            //{
            //    try
            //    {
            //        Query = "update Appointment.[CurrentPkid]"
            //           + " set current_pkid = @current_pkid";

            //        rowsAffected = db.Execute(Query, new
            //        {
            //            @current_pkid = userId
            //        });

            //        operationResult.Success = true;
            //        operationResult.ErrorMessage = "Success";
            //        return operationResult;
            //    }
            //    catch (Exception er)
            //    {
            //        operationResult.Success = false;
            //        operationResult.ErrorMessage = er.ToString();
            //        return operationResult;
            //    }
            //}
        }

        public void AddBroadcastMessage(BroadcastMessage broadcastMessage)
        {

            int rowsAffected = 0;
            OperationResult operationResult = new OperationResult();

            try
            {
                using (SqlConnection db = new SqlConnection(ConfigurationValues.FairfieldAllergyConnection))
                {
                    rowsAffected = db.Query<int>("dt_add_broadcast_message", new
                    {
                        @message = broadcastMessage.Message,
                        @norwalk = broadcastMessage.Norwalk,
                        @greenwich = broadcastMessage.Greenwich,
                        @stamford = broadcastMessage.Stamford,
                        @ridgefield = broadcastMessage.Ridgefield
                    },
                    commandType: CommandType.StoredProcedure).Single();
                }
            }
            catch (Exception er)
            {
                string s1 = er.ToString();
            }


            //OperationResult operationResult = new OperationResult();
            //int rowsAffected = 0;

            //using (IDbConnection db = new SqlConnection(ConfigurationValues.FairfieldAllergyConnection))
            //{
            //    try
            //    {

            //        Query = "INSERT INTO [Appointment].[BroadcastMessage] ([Message],[DateCreated]"
            //        + " ,[Norwalk],[Greenwich],[Stamford],[Ridgefield])"
            //        + " VALUES("
            //        + " @Message,@DateCreated,@Norwalk,@Greenwich,@Stamford,@Ridgefield"
            //        + ")";

            //        rowsAffected = db.Execute(Query, new
            //        {
            //            @Message = broadcastMessage.Message,
            //            @DateCreated = DateTime.Now,
            //            @Norwalk = broadcastMessage.Norwalk,
            //            @Greenwich = broadcastMessage.Greenwich,
            //            @Stamford = broadcastMessage.Stamford,
            //            @Ridgefield = broadcastMessage.Ridgefield

            //        });

            //        operationResult.Success = true;
            //        operationResult.ErrorMessage = "Success";
            //        //return operationResult;
            //    }
            //    catch (Exception er)
            //    {
            //        operationResult.Success = false;
            //        operationResult.ErrorMessage = er.ToString();
            //        //return operationResult;
            //    }
            //}
        }
        public void CreateNewUser(AllergyPatient allergyPatient, string userId, string password)
        {
            int newId = 0;
            OperationResult operationResult = new OperationResult();

            string sex = string.Empty;
            if (allergyPatient.gender == 1)
            {
                sex = "M";
            }
            else if (allergyPatient.gender == 2)
            {
                sex = "F";
            }

            try
            {
                var p = new DynamicParameters();
                p.Add("@first_name", allergyPatient.first_name.Trim());


                p.Add("@last_name", allergyPatient.last_name);


                string shortDateString = allergyPatient.birthday.ToShortDateString();
                if (shortDateString == "1/1/1900")
                {
                    p.Add("@dob", null);
                }
                else
                {
                    p.Add("@dob", allergyPatient.birthday);
                }

                p.Add("@sex", sex);
                p.Add("@home_phone", allergyPatient.home_phone);
                p.Add("@email", allergyPatient.EMailAddress);


                p.Add("@location_id", allergyPatient.injection_office_id);
                p.Add("@first_appointment", "N");
                p.Add("@venom_patient", "N");
                p.Add("@sendemail", "N");

                p.Add("@sendlate", "N");
                p.Add("@sendnone", "N");
                p.Add("@notification_id", "");
                p.Add("@patient_id", 0);
                p.Add("@pkid", allergyPatient.pkid);
                p.Add("@digi_account", allergyPatient.pkid);
                p.Add("@uid", userId);
                p.Add("@password", password);

                p.Add("@new_id", dbType: DbType.Int32, direction: ParameterDirection.Output);



                //var p = new DynamicParameters();
                //p.Add("VAR1", "John");
                //p.Add("VAR2", "McEnroe");
                //p.Add("BASEID", 1);
                //p.Add("NEWID", dbType: DbType.Int32, direction: ParameterDirection.Output);
                //connection.Query<int>("SP_MYTESTpROC", p, commandType: CommandType.StoredProcedure);
                //int newID = p.Get<int>("NEWID");


                //using (SqlConnection db = new SqlConnection(ConfigurationValues.FairfieldAllergyConnection))
                //{
                //    newId = db.Query<int>("dt_create_new_user", new
                //    {
                //        @first_name = allergyPatient.first_name,
                //        @last_name = allergyPatient.last_name,
                //        @dob = allergyPatient.birthday,
                //        @sex = sex,
                //        @home_phone = allergyPatient.home_phone,
                //        @email = allergyPatient.EMailAddress,
                //        @location_id = allergyPatient.injection_office_id,
                //        @first_appointment = "N",
                //        @venom_patient = "N",
                //        @sendemail = "N",
                //        @sendlate = "N",
                //        @sendnone = "N",
                //        @notification_id = "",
                //        @patient_id = 0,


                //        @pkid = allergyPatient.pkid,
                //        @digi_account = allergyPatient.pkid,
                //        @uid = userId,
                //        @password = password,
                //    },
                //    commandType: CommandType.StoredProcedure).FirstOrDefault();
                //}


                using (SqlConnection db = new SqlConnection(ConfigurationValues.FairfieldAllergyConnection))
                {
                    db.Query<int>("dt_create_new_user", p, commandType: CommandType.StoredProcedure);
                    newId = p.Get<int>("@new_id");
                }//,
                //    commandType: CommandType.StoredProcedure).FirstOrDefault();
                //}
            }
            catch (Exception er)
            {
                string s1 = er.ToString();
            }

            //OperationResult operationResult = new OperationResult();
            //int newId = 0;

            //string sex = string.Empty;
            //if (allergyPatient.gender == 1)
            //{
            //    sex = "M";
            //}
            //else if (allergyPatient.gender == 2)
            //{
            //    sex = "F";
            //}

            //using (IDbConnection db = new SqlConnection(ConfigurationValues.FairfieldAllergyConnection))
            //{
            //    try
            //    {
            //        Query = "DECLARE @InsertedRows AS TABLE(Id int);"
            //            + "INSERT INTO Appointment.[Person]([FirstName],[LastName],[DOB],[Sex],[HPhone]"
            //            + " ,[EMail],[LocationID],[first_appointment],[venom_patient],[sendemail],[sendlate]"
            //            + " ,[sendnone],[NotificationID],[patientid]) OUTPUT Inserted.Id INTO @InsertedRows VALUES("
            //            + " @FirstName,@LastName,@DOB,@Sex,@HPhone,@EMail,@LocationID,@first_appointment,@venom_patient,"
            //            + " @sendemail,@sendlate,@sendnone,@NotificationID,@patientid)"
            //            + " SELECT Id FROM @InsertedRows";

            //        newId = db.Query<int>(Query, new
            //        {
            //            @FirstName = allergyPatient.first_name,
            //            @LastName = allergyPatient.last_name,
            //            @DOB = allergyPatient.birthday,
            //            @Sex = sex,
            //            @HPhone = allergyPatient.home_phone,
            //            @EMail = allergyPatient.EMailAddress,
            //            @LocationID = allergyPatient.injection_office_id,
            //            @first_appointment = "N",
            //            @venom_patient = "N",
            //            @sendemail = "N",
            //            @sendlate = "N",
            //            @sendnone = "N",
            //            @NotificationID = "",
            //            @patientid = 0
            //        }).Single();
            //        //var id = connection.Query<int>(sql, new { Stuff = mystuff}).Single();

            //        operationResult.Success = true;
            //        operationResult.ErrorMessage = "Success";
            //        //return operationResult;
            //    }
            //    catch (Exception er)
            //    {
            //        operationResult.Success = false;
            //        operationResult.ErrorMessage = er.ToString();
            //        //return operationResult;
            //    }
            //}

            //using (IDbConnection db = new SqlConnection(ConfigurationValues.FairfieldAllergyConnection))
            //{
            //    try
            //    {

            //        Query = "INSERT INTO Appointment.[WEBID]([PersonID],[pkid],[digi_account],[UID],[PWD],[HINTTYPE],[HINTVALUE])"
            //            + " VALUES("
            //            + " @PersonID,@pkid, @digi_account, @UID,@PWD,@HINTTYPE,@HINTVALUE)";


            //        newId = db.Execute(Query, new
            //        {
            //            @PersonID = newId,
            //            @pkid = allergyPatient.pkid,
            //            @digi_account = allergyPatient.pkid,
            //            @UID = userId,
            //            @PWD = password,
            //            @HINTTYPE = "None",
            //            @HINTVALUE = "None"
            //        });
            //        //var id = connection.Query<int>(sql, new { Stuff = mystuff}).Single();

            //        operationResult.Success = true;
            //        operationResult.ErrorMessage = "Success";
            //        //return operationResult;
            //    }
            //    catch (Exception er)
            //    {
            //        operationResult.Success = false;
            //        operationResult.ErrorMessage = er.ToString();
            //        //return operationResult;
            //    }
            //}
        }


        public OperationResult GetUserIdAndPassword(string userId)
        {
            OperationResult operationResult = new OperationResult();
            UserInformation userInformation = new UserInformation();
            try
            {
                Query = "SELECT [UID],[PWD]"
                    + " FROM [Appointment].[WEBID]"
                    + " where [UID] = @UID";

                using (SqlConnection db = new SqlConnection(ConfigurationValues.FairfieldAllergyConnection))
                {
                    userInformation = db.Query<UserInformation>(Query, new
                    {
                        @UID = userId

                    }).Single();
                }
            }
            catch (Exception er)
            {
                string s1 = er.ToString();
                operationResult.UserInformation = userInformation;
            }
            operationResult.UserInformation = userInformation;

            return operationResult;
        }


        public void UpdateUsersWithDataFromRoach()
        {
            List<AllergyPatient> allergyPatient = new List<AllergyPatient>();

            try
            {
                Query = "SELECT [pkid],[last_name],[first_name],[middle_name],[goes_by],[social],[birthday],[home_phone],[work_phone],[account]"
                    + " ,[doctor_id],[scan_id],[gender],[insurance_id],[si_proc_id],[mi_proc_id],[servicedoctor_id]"
                    + ",[notes],[mailadd_id],[si_charge],[mi_charge],[co_pay],[inj_notes],[injection_office_id],[mixing_office_id]"
                    + " ,[next_alert_date],[wait_list_string],[next_checkup],[dt_initiated],[chart_num],[mixingnotes],[chargescheme_id]"
                    + " ,[EMailAddress],[SignInDisabledReason],[PeakFlowBest],[PeakFlowMinimum],[PeakFlowBestDate]"
                    + " ,[PeakFlowDueDate],[HealthScreenID],[InActive],[cell_phone],[hold_order],[passcode],[Created],[CreatedBy],[LastModified]"
                    + " ,[LastModifiedBy],[RevisionNumber]"
                    + " FROM [PATIENT_INFO]";

                using (SqlConnection db = new SqlConnection("Server = 172.16.1.13; Database = RoschIT; Uid = walden; Pwd = Db#rd!927!Mz;"))
                {
                    allergyPatient = db.Query<AllergyPatient>(Query).ToList();
                }
            }
            catch (Exception er)
            {
                string s1 = er.ToString();
            }

            Update update = new Update();

            for (int i = 0; i < allergyPatient.Count; i++)
            {

                Console.WriteLine(DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + " Processing record " + i.ToString() + " of " + allergyPatient.Count.ToString());

                try
                {
                    Query = "SELECT [PersonID],[UID],[user_id] as UserId"
                        + " FROM [Appointment].[WEBID]"
                        + " where [pkid] = @id";

                    using (SqlConnection db = new SqlConnection(ConfigurationValues.FairfieldAllergyConnection))
                    {
                        update = db.Query<Update>(Query, new {
                            @id = allergyPatient[i].pkid

                        }).Single();
                    }
                }
                catch (Exception er)
                {
                    string s1 = er.ToString();
                }

                int rowsAffected = 0;

                string sex = string.Empty;

                if (allergyPatient[i].gender == 1)
                {
                    sex = "M";
                }
                else
                {
                    sex = "F";
                }


                try
                {
                    using (SqlConnection db = new SqlConnection(ConfigurationValues.FairfieldAllergyConnection))
                    {
                        Query = "UPDATE [Appointment].[Person]"
                        + "SET [FirstName] = @FirstName"
                        + " ,[LastName] = @LastName"
                        + " ,[DOB] = @DOB"
                        + " ,[Sex] = @Sex"
                        + " ,[HPhone] = @HPhone"
                        + " ,[EMail] = @EMail"
                        + " ,[LocationID] = @LocationID"
                        + " WHERE id = @id";

                        rowsAffected = db.Execute(Query, new
                        {
                            @FirstName = allergyPatient[i].first_name,
                            @LastName = allergyPatient[i].last_name,
                            @DOB = allergyPatient[i].birthday,
                            @Sex = sex,
                            @HPhone = allergyPatient[i].home_phone,
                            @EMail = allergyPatient[i].EMailAddress,
                            @LocationID = allergyPatient[i].injection_office_id,
                            @id = update.PersonID
                        });
                    }
                }
                catch (Exception er)
                {
                    string s1 = er.ToString();
                }


                try
                {
                    using (SqlConnection db = new SqlConnection(ConfigurationValues.FairfieldAllergyConnection))
                    {

                        Query = "UPDATE [Appointment].[AspNetUsers]"
                            + " SET [City] = @City"
                            + " ,[FirstName] = @FirstName"
                            + " ,[LastName] = @LastName"
                            + " ,[DateOfBirth] = @DateOfBirth"
                            + " ,[Sex] = @Sex"
                            + " ,[HomePhone] = @HomePhone" 
                            +" ,[Location] = @Location"
                            + " where Id = @Id";

                        rowsAffected = db.Execute(Query, new
                        {
                            @City = "None",
                            @FirstName = allergyPatient[i].first_name,
                            @LastName = allergyPatient[i].last_name,
                            @DateOfBirth = allergyPatient[i].birthday,
                            @Sex = sex,
                            @HomePhone = allergyPatient[i].home_phone,
                            @Location = allergyPatient[i].injection_office_id,
                            @Id = update.UserId

                        });
                    }
                }
                catch (Exception er)
                {
                    string s1 = er.ToString();
                }
            }
        }


        public List<AllergyPatient> GetListOfNewUsers(string currentId)
        {
            List<AllergyPatient> allergyPatient = new List<AllergyPatient>();

            try
            {
                Query = "SELECT [pkid],[last_name],[first_name],[middle_name],[goes_by],[social],[birthday],[home_phone],[work_phone],[account]"
                    + " ,[doctor_id],[scan_id],[gender],[insurance_id],[si_proc_id],[mi_proc_id],[servicedoctor_id]"
                    + ",[notes],[mailadd_id],[si_charge],[mi_charge],[co_pay],[inj_notes],[injection_office_id],[mixing_office_id]"
                    + " ,[next_alert_date],[wait_list_string],[next_checkup],[dt_initiated],[chart_num],[mixingnotes],[chargescheme_id]"
                    + " ,[EMailAddress],[SignInDisabledReason],[PeakFlowBest],[PeakFlowMinimum],[PeakFlowBestDate]"
                    + " ,[PeakFlowDueDate],[HealthScreenID],[InActive],[cell_phone],[hold_order],[passcode],[Created],[CreatedBy],[LastModified]"
                    + " ,[LastModifiedBy],[RevisionNumber]"
                    + " FROM [PATIENT_INFO]"
                    //+ " where pkid > @pkid;"
                    + " where pkid = 42";
                    //+ " where pkid = 4";
                    //+ " where pkid = 11";

                using (SqlConnection db = new SqlConnection("Server = 172.16.1.13; Database = RoschIT; Uid = walden; Pwd = Db#rd!927!Mz;"))
                {
                    allergyPatient = db.Query<AllergyPatient>(Query, new
                    {
                        @pkid = currentId

                    }).ToList();
                    return allergyPatient;
                }
            }
            catch (Exception er)
            {
                string s1 = er.ToString();
                return allergyPatient;
            }
        }

        public OperationResult UpdateNumberOfSlots(int slotId, int slotNumber)
        {
            int rowsAffected = 0;
            OperationResult operationResult = new OperationResult();

            try
            {
                using (SqlConnection db = new SqlConnection(ConfigurationValues.FairfieldAllergyConnection))
                {
                    rowsAffected = db.Query<int>("dt_update_number_of_slots", new
                    {
                        @slot_id = slotId,
                        @slot_number = slotNumber
                    },
                    commandType: CommandType.StoredProcedure).FirstOrDefault();
                }

                operationResult.Success = true;
                operationResult.ErrorMessage = "Success";
                return operationResult;
            }
            catch (Exception er)
            {
                operationResult.Success = false;
                operationResult.ErrorMessage = er.ToString();
                return operationResult;
            }


            //OperationResult operationResult = new OperationResult();
            //int rowsAffected = 0;

            //using (IDbConnection db = new SqlConnection(ConfigurationValues.FairfieldAllergyConnection))
            //{
            //    try
            //    {
            //        Query = "update Appointment.appointmentslots"
            //           + " set NumberSlots = @NumberSlots"
            //           + " where SlotId = @SlotId";


            //        rowsAffected = db.Execute(Query, new
            //        {
            //            @NumberSlots = slotNumber,
            //            @SlotId = slotId
            //        });

            //        operationResult.Success = true;
            //        operationResult.ErrorMessage = "Success";
            //        return operationResult;
            //    }
            //    catch (Exception er)
            //    {
            //        operationResult.Success = false;
            //        operationResult.ErrorMessage = er.ToString();
            //        return operationResult;
            //    }
            //}
        }

        public OperationResult GetPatientIdAndPassword(int patientId)
        {
            OperationResult operationResult = new OperationResult();

            try
            {
                using (SqlConnection db = new SqlConnection(ConfigurationValues.FairfieldAllergyConnection))
                {
                    operationResult.PatientCredentials = db.Query<Patient>("dt_get_patient_id_and_password", new
                    {
                        @patient_id = patientId
                    },
                    commandType: CommandType.StoredProcedure).Single();
                }

                operationResult.Success = true;
                operationResult.ErrorMessage = "Success";
                return operationResult;
            }
            catch (Exception er)
            {
                operationResult.Success = false;
                operationResult.ErrorMessage = er.ToString();
                return operationResult;
            }

            //OperationResult operationResult = new OperationResult();

            //try
            //{
            //    Query = " select Uid as PatientUserId, Pwd as PatientPassword from Appointment.WEBID"
            //    + " where personid = @Personid";

            //    using (SqlConnection db = new SqlConnection(ConfigurationValues.FairfieldAllergyConnection))
            //    {
            //        operationResult.PatientCredentials = db.Query<Patient>(Query, new
            //        {
            //            @Personid = patientId
            //        }).Single();
            //        operationResult.Success = true;
            //        operationResult.ErrorMessage = "None";
            //        return operationResult;
            //    }
            //}
            //catch (Exception er)
            //{
            //    operationResult.Success = false;
            //    operationResult.ErrorMessage = er.ToString();
            //    return operationResult;
            //}
        }

        public OperationResult GetPatients()
        {

            OperationResult operationResult = new OperationResult();

            try
            {
                using (SqlConnection db = new SqlConnection(ConfigurationValues.FairfieldAllergyConnection))
                {
                    operationResult.Patient = db.Query<Patient>("dt_get_patients",
                    commandType: CommandType.StoredProcedure).ToList();
                }

                operationResult.Success = true;
                operationResult.ErrorMessage = "Success";
                return operationResult;
            }
            catch (Exception er)
            {
                operationResult.Success = false;
                operationResult.ErrorMessage = er.ToString();
                return operationResult;
            }


            //OperationResult operationResult = new OperationResult();
            //List<Patient> patients = new List<Patient>();

            //try
            //{
            //    Query = " select B.Id as Id, LTRIM(RTRIM(B.firstname)) + ' ' + LTRIM(RTRIM(B.lastname)) as PatientName,"
            //    + " B.locationid as PatientLocation, A.ID as PatientWebId "
            //    + " from Appointment.WEBID A"
            //    + " INNER JOIN Appointment.Person B"
            //    + " ON A.PersonID = B.ID"
            //    + " where B.locationid not in (-1)"
            //    + " and B.lastname not like '%zz%'"
            //    + " and B.id NOT IN (3337,3500,3696,3703,3772,3943,3973,4405,5132,5398,5517,5685,5689,5855,6112)"
            //    + " order by B.lastname";

            //    using (SqlConnection db = new SqlConnection(ConfigurationValues.FairfieldAllergyConnection))
            //    {
            //        operationResult.Patient = db.Query<Patient>(Query).ToList();
            //        operationResult.Success = true;
            //        operationResult.ErrorMessage = "None";
            //        return operationResult;
            //    }
            //}
            //catch (Exception er)
            //{
            //    operationResult.Success = false;
            //    operationResult.ErrorMessage = er.ToString();
            //    return operationResult;
            //}
        }

        public OperationResult GetPatients(string filter)
        {
            OperationResult operationResult = new OperationResult();

            try
            {
                using (SqlConnection db = new SqlConnection(ConfigurationValues.FairfieldAllergyConnection))
                {
                    operationResult.Patient = db.Query<Patient>("dt_get_patients_with_variable", new
                    {
                        @filter = filter
                    },
                    commandType: CommandType.StoredProcedure).ToList();
                }

                operationResult.Success = true;
                operationResult.ErrorMessage = "Success";
                return operationResult;
            }
            catch (Exception er)
            {
                operationResult.Success = false;
                operationResult.ErrorMessage = er.ToString();
                return operationResult;
            }

            //OperationResult operationResult = new OperationResult();
            //List<Patient> patients = new List<Patient>();

            //try
            //{
            //    Query = " select B.Id as Id, LTRIM(RTRIM(B.firstname)) + ' ' + LTRIM(RTRIM(B.lastname)) as PatientName,"
            //    + " B.locationid as PatientLocation, A.ID as PatientWebId "
            //    + " from Appointment.WEBID A"
            //    + " INNER JOIN Appointment.Person B"
            //    + " ON A.PersonID = B.ID"
            //    + " where B.locationid not in (-1)"
            //    + " and B.lastname not like '%zz%'"
            //    + " and B.id NOT IN (3337,3500,3696,3703,3772,3943,3973,4405,5132,5398,5517,5685,5689,5855,6112)"
            //    + " and LTRIM(RTRIM(B.firstname)) + ' ' + LTRIM(RTRIM(B.lastname)) LIKE '%" + filter + "%'"
            //    + " order by B.lastname";

            //    using (SqlConnection db = new SqlConnection(ConfigurationValues.FairfieldAllergyConnection))
            //    {
            //        operationResult.Patient = db.Query<Patient>(Query).ToList();
            //        operationResult.Success = true;
            //        operationResult.ErrorMessage = "None";
            //        return operationResult;
            //    }
            //}
            //catch (Exception er)
            //{
            //    operationResult.Success = false;
            //    operationResult.ErrorMessage = er.ToString();
            //    return operationResult;
            //}
        }

        public OperationResult GetPatientAllergyAppointments(string date, string location)
        {
            List<AllergyPatientAppointments> allergyPatientAppointmentsList = new List<AllergyPatientAppointments>();

            OperationResult operationResult = new OperationResult();

            try
            {
                using (SqlConnection db = new SqlConnection(ConfigurationValues.FairfieldAllergyConnection))
                {

                    allergyPatientAppointmentsList = db.Query<AllergyPatientAppointments>("dt_get_patient_allergy_appointments", new
                    {
                        @date = date,
                        @location = location

                    },
                    commandType: CommandType.StoredProcedure).ToList();
                }

                operationResult.Success = true;
                operationResult.ErrorMessage = "Success";
                return operationResult;
            }
            catch (Exception er)
            {
                operationResult.Success = false;
                operationResult.ErrorMessage = er.ToString();
                return operationResult;
            }


            //OperationResult operationResult = new OperationResult();

            //List<AllergyPatientAppointments> allergyPatientAppointmentsList = new List<AllergyPatientAppointments>();

            //try
            //{
            //    Query = " SELECT Appointment.ID as Id, person.FirstName as FirstName, person.LastName as LastName,"
            //        + " CONVERT(varchar, [ApptDay], 101) as DateOfAppointment,CONVERT(varchar(15), CAST([ApptTime] AS TIME), 100) as TimeOfAppointment,"
            //        + " CONVERT(varchar, person.DOB, 101) as DateOfBirth, '0' as Account "
            //        + " FROM Appointment.Appointment"
            //        + " inner join webid"
            //        + " on webid.ID = appointment.UserID"
            //        + " inner join person"
            //        + " on person.ID = webid.PersonID"
            //        + " where [ApptDay] = @ApptDay"
            //        + " and person.locationid = @locationId"
            //        + " order by Appointment.ApptDay, Appointment.ApptTime";

            //    using (SqlConnection db = new SqlConnection(ConfigurationValues.FairfieldAllergyConnection))
            //    {
            //        allergyPatientAppointmentsList = db.Query<AllergyPatientAppointments>(Query, new
            //        {
            //            @ApptDay = date,
            //            @locationId = location
            //        }).ToList();

            //        if (allergyPatientAppointmentsList.Count > 0)
            //        {

            //            operationResult.AllergyPatientAppointments = allergyPatientAppointmentsList;
            //            operationResult.Success = true;
            //            operationResult.ErrorMessage = "None";
            //        }
            //        else
            //        {
            //            operationResult.AllergyPatientAppointments = allergyPatientAppointmentsList;
            //            operationResult.Success = false;
            //            operationResult.ErrorMessage = "None";

            //        }
            //    }
            //}
            //catch (Exception er)
            //{
            //    operationResult.Success = false;
            //    operationResult.ErrorMessage = er.ToString();

            //}
            //return operationResult;
        }

        public OperationResult GetPatientAllergyAppointmentSlots(string date, string location)
        {
            List<AllergyPatientAppointments> allergyPatientAppointmentsList = new List<AllergyPatientAppointments>();

            OperationResult operationResult = new OperationResult();

            try
            {
                using (SqlConnection db = new SqlConnection(ConfigurationValues.FairfieldAllergyConnection))
                {

                    allergyPatientAppointmentsList = db.Query<AllergyPatientAppointments>("dt_get_patient_allergy_appointment_slots", new
                    {
                        @date = date,
                        @location = location

                    },
                    commandType: CommandType.StoredProcedure).ToList();
                }

                operationResult.Success = true;
                operationResult.ErrorMessage = "Success";
                return operationResult;
            }
            catch (Exception er)
            {
                operationResult.Success = false;
                operationResult.ErrorMessage = er.ToString();
                return operationResult;
            }


            //OperationResult operationResult = new OperationResult();

            //List<AllergyPatientAppointments> allergyPatientAppointmentsList = new List<AllergyPatientAppointments>();

            //try
            //{
            //    Query = " SELECT SlotId ,LocationID ,AppointmentTypeID,NumberSlots ,CONVERT(varchar, SlotDate, 101) as SlotDate,"
            //        //+ " CAST(SlotTime AS TIME), 100) as SlotTime,"
            //        + " CONVERT(varchar(15), CAST([SlotTime] AS TIME), 100),"
            //        + " Block"
            //        + " FROM Appointment.[AppointmentSlots]"
            //        + " where SlotDate = @SlotDate"
            //        + " AND LocationID = @LocationId"
            //        + " Order by SlotDate, SlotTime";

            //    using (SqlConnection db = new SqlConnection(ConfigurationValues.FairfieldAllergyConnection))
            //    {
            //        allergyPatientAppointmentsList = db.Query<AllergyPatientAppointments>(Query, new
            //        {
            //            @SlotDate = date,
            //            @LocationId = location
            //        }).ToList();

            //        operationResult.AllergyPatientAppointments = allergyPatientAppointmentsList;
            //        operationResult.Success = true;
            //        operationResult.ErrorMessage = "None";
            //        return operationResult;
            //    }
            //}
            //catch (Exception er)
            //{
            //    //SerilogLogError.LogErrorToFile(er.ToString());
            //}
            //return operationResult;
        }

        public OperationResult GetListOfAppointment(string date, string location)
        {
            List<AllergyPatientAppointments> allergyPatientAppointmentsList = new List<AllergyPatientAppointments>();
            List<OpenAppointment> appointments = new List<OpenAppointment>();
            OpenAppointment appointment = new OpenAppointment();

            OperationResult operationResult = new OperationResult();

            try
            {
                using (SqlConnection db = new SqlConnection(ConfigurationValues.FairfieldAllergyConnection))
                {

                    allergyPatientAppointmentsList = db.Query<AllergyPatientAppointments>("dt_get_list_of_appointments", new
                    {
                        @date = date,
                        @location = location

                    },
                    commandType: CommandType.StoredProcedure).ToList();
                }


                for (int i = 0; i < allergyPatientAppointmentsList.Count; i++)
                {
                    appointment = new OpenAppointment();
                    appointment.AppointmentDescription = allergyPatientAppointmentsList[i].SlotTime.ToShortTimeString()
                                        + " on " + allergyPatientAppointmentsList[i].SlotDate.ToShortDateString().Replace("/", "-");
                    //                    //+ dr.GetDateTime(4).ToShortDateString().Replace("/", "-")
                    //                    //;;+ " at "
                    //                    //;;+ dr.GetDateTime(5).ToShortTimeString();
                    appointment.SlotID = int.Parse(allergyPatientAppointmentsList[i].SlotId);
                    appointments.Add(appointment);
                }

                operationResult.Appointments = appointments;
                operationResult.Success = true;
                operationResult.ErrorMessage = "Success";
                return operationResult;
            }
            catch (Exception er)
            {
                operationResult.Success = false;
                operationResult.ErrorMessage = er.ToString();
                return operationResult;
            }

            //OperationResult operationResult = new OperationResult();
            //List<AppointmentSlots> appointmentSlots = new List<AppointmentSlots>();
            //List<OpenAppointment> appointments = new List<OpenAppointment>();
            //OpenAppointment appointment = new OpenAppointment();

            //try
            //{
            //    using (SqlConnection cn = new SqlConnection(ConfigurationValues.FairfieldAllergyConnection))
            //    {
            //        cn.Open();
            //        using (SqlCommand cm = cn.CreateCommand())
            //        {
            //            //cm.CommandText = "Select  A.SlotId,A.LocationID,A.AppointmentTypeID,A.NumberSlots,A.SlotDate,A.SlotTime from appointmentslots A"
            //            cm.CommandText = "select A.SlotId as SlotId,A.LocationID,A.AppointmentTypeID,A.NumberSlots,A.SlotDate,A.SlotTime,"
            //                + " A.Block,A.AddId,A.num_slots_wi,A.num_slots_ol "
            //                + " from Appointment.appointmentslots A"
            //                + " where A.slotdate = @SlotDate"
            //                + " and A.numberslots > (select count(ID) from appointment B"
            //                + " where B.apptday = @SlotDate"
            //                + " and B.slotnumber = A.slotid"
            //                + " and B.location = @Location)"
            //                + " AND A.Block = 'N'"
            //                + " and A.locationid = @Location"
            //                + " order by A.slotid";

            //            cm.CommandType = System.Data.CommandType.Text;
            //            cm.Parameters.AddWithValue("@SlotDate", date);
            //            cm.Parameters.AddWithValue("@Location", location);
            //            using (SqlDataReader dr = cm.ExecuteReader())
            //            {
            //                while (dr.Read())
            //                {
            //                    //8:00 am on July 2, 2020 is available 
            //                    appointment = new OpenAppointment();
            //                    appointment.AppointmentDescription = dr.GetDateTime(5).ToShortTimeString()
            //                    + " on " + dr.GetDateTime(4).ToShortDateString().Replace("/", "-");
            //                    //+ " is available ";
            //                    //+ dr.GetDateTime(4).ToShortDateString().Replace("/", "-")
            //                    //;;+ " at "
            //                    //;;+ dr.GetDateTime(5).ToShortTimeString();
            //                    appointment.SlotID = int.Parse(dr["SlotId"].ToString());
            //                    appointments.Add(appointment);
            //                }
            //            }

            //        }
            //    }
            //}
            //catch (Exception er)
            //{
            //    operationResult.Success = false;
            //    operationResult.ErrorMessage = er.ToString();
            //    return operationResult;
            //}

            //operationResult.Success = true;
            //operationResult.ErrorMessage = "None";

            //operationResult.Appointments = appointments;
            //return operationResult;
        }

        public OperationResult GetAppointmentSlots(string date, string location)
        {
            List<AppointmentSlots> appointmentSlots = new List<AppointmentSlots>();

            OperationResult operationResult = new OperationResult();

            try
            {
                using (SqlConnection db = new SqlConnection(ConfigurationValues.FairfieldAllergyConnection))
                {

                    appointmentSlots = db.Query<AppointmentSlots>("dt_get_appointment_slots", new
                    {
                        @date = date,
                        @location = location

                    },
                    commandType: CommandType.StoredProcedure).ToList();
                }

                operationResult.Success = true;
                operationResult.AppointmentSlots = appointmentSlots;
                operationResult.ErrorMessage = "None";
                return operationResult;

            }
            catch (Exception er)
            {
                operationResult.Success = false;
                operationResult.ErrorMessage = er.ToString();
                return operationResult;
            }

            //OperationResult operationResult = new OperationResult();
            //List<AppointmentSlots> appointmentSlots = new List<AppointmentSlots>();

            //try
            //{
            //    //CONVERT(varchar, [ApptDay], 101) as AppointmentDate,CONVERT(varchar(15), CAST([ApptTime] AS TIME), 100) as AppointmentTime,"
            //    //cm.CommandText = "Select  A.SlotId,A.LocationID,A.AppointmentTypeID,A.NumberSlots,A.SlotDate,A.SlotTime from appointmentslots A"
            //    Query = " select A.SlotId as SlotId,A.LocationID,A.AppointmentTypeID,A.NumberSlots,"
            //        + " CONVERT(varchar, [SlotDate], 101) as SlotDate,"
            //        + " CONVERT(varchar(15), CAST([SlotTime] AS TIME), 100) as SlotTime,"
            //        + " -1 as NewSlotNumber,"
            //        + " A.Block,A.AddId,A.num_slots_wi,A.num_slots_ol "
            //        + " from Appointment.appointmentslots A"
            //        + " where A.slotdate = @SlotDate"
            //        //+ " and A.numberslots > (select count(ID) from appointment B"
            //        //+ " where B.apptday = @SlotDate"
            //        //+ " and B.slotnumber = A.slotid"
            //        //+ " and B.location = @Location)"
            //        + " AND A.Block = 'N'"
            //        + " and A.locationid = @Location"
            //        + " order by A.slotid";


            //    using (SqlConnection db = new SqlConnection(ConfigurationValues.FairfieldAllergyConnection))
            //    {
            //        appointmentSlots = db.Query<AppointmentSlots>(Query, new
            //        {
            //            @Location = location,
            //            @SlotDate = date

            //        }).ToList();

            //        operationResult.Success = true;
            //        operationResult.AppointmentSlots = appointmentSlots;
            //        operationResult.ErrorMessage = "None";
            //        return operationResult;
            //    }
            //}
            //catch (Exception er)
            //{
            //    //SerilogLogError.LogErrorToFile(er.ToString());
            //    operationResult.Success = false;
            //    operationResult.ErrorMessage = er.ToString();
            //    return operationResult;
            //}
        }

        public OperationResult DeleteAppointment(string appointmentID)
        {

            int rowsAffected = 0;

            OperationResult operationResult = new OperationResult();

            try
            {
                using (SqlConnection db = new SqlConnection(ConfigurationValues.FairfieldAllergyConnection))
                {

                    rowsAffected = db.Query<int>("dt_delete_appointment", new
                    {
                        @id = appointmentID
                    },
                    commandType: CommandType.StoredProcedure).FirstOrDefault();
                }

                operationResult.Success = true;
                operationResult.ErrorMessage = "None";
                return operationResult;

            }
            catch (Exception er)
            {
                operationResult.Success = false;
                operationResult.ErrorMessage = er.ToString();
                return operationResult;
            }


            //OperationResult operationResult = new OperationResult();
            //int rowsAffected = 0;

            //using (IDbConnection db = new SqlConnection(ConfigurationValues.FairfieldAllergyConnection))
            //{
            //    try
            //    {
            //        Query = "DELETE FROM Appointment.Appointment"
            //            + " where id = @id";
            //        rowsAffected = db.Execute(Query, new
            //        {
            //            @id = appointmentID,
            //        });
            //        operationResult.Success = true;
            //        operationResult.ErrorMessage = "None";
            //        return operationResult;
            //    }
            //    catch (Exception er)
            //    {
            //        operationResult.Success = false;
            //        operationResult.ErrorMessage = er.ToString();
            //        return operationResult;
            //    }
            //}
        }

        public UserInformation GetUserInformation(string userName)
        {
            UserInformation userInformation = new UserInformation();

            try
            {
                using (SqlConnection db = new SqlConnection(ConfigurationValues.FairfieldAllergyConnection))
                {

                    userInformation = db.Query<UserInformation>("dt_get_user_information", new
                    {
                        @user_name = userName
                    },
                    commandType: CommandType.StoredProcedure).Single();
                }
            }
            catch (Exception er)
            {
                string s1 = er.ToString();
            }

            return userInformation;

            //UserInformation userInformation = new UserInformation();

            //try
            //{
            //    Query = " SELECT WEBID.ID as UserId,[AspNetUsers].[Id],[AspNetUsers].[UserName],[AspNetUsers].[Email],"
            //        + " [AspNetUsers].[PhoneNumber],[AspNetUsers].[City],[AspNetUsers].[FirstName],[AspNetUsers].[LastName],"
            //        + " [AspNetUsers].[DateOfBirth],[AspNetUsers].[Sex],[AspNetUsers].[HomePhone],[AspNetUsers].[location]"
            //        + " FROM Appointment.[AspNetUsers]"
            //        + " inner join Appointment.WEBID"
            //        + " on WEBID.UID = [AspNetUsers].UserName"
            //        + " where [UserName] = @UserName";

            //    using (SqlConnection db = new SqlConnection(ConfigurationValues.FairfieldAllergyConnection))
            //    {
            //        userInformation = db.Query<UserInformation>(Query, new
            //        {
            //            @UserName = userName

            //        }).Single();

            //        //operationResult.Success = true;
            //        //operationResult.ErrorMessage = "None";
            //        //return operationResult;
            //    }
            //}
            //catch (Exception er)
            //{
            //    //SerilogLogError.LogErrorToFile(er.ToString());
            //    //operationResult.Success = false;
            //    //operationResult.ErrorMessage = er.ToString();
            //    //return operationResult;
            //}
            //return userInformation;
        }

        public OperationResult UpdatePatientLocation(string patientId, string locationId)
        {

            int rowsAffected = 0;
            OperationResult operationResult = new OperationResult();

            try
            {
                using (SqlConnection db = new SqlConnection(ConfigurationValues.FairfieldAllergyConnection))
                {
                    rowsAffected = db.Query<int>("dt_update_patient_location", new
                    {
                        @id = patientId,
                        @location_id = locationId

                    },
                    commandType: CommandType.StoredProcedure).FirstOrDefault();
                }

                operationResult.Success = true;
                operationResult.ErrorMessage = "Success";
                return operationResult;

            }
            catch (Exception er)
            {
                operationResult.Success = false;
                operationResult.ErrorMessage = er.ToString();
                return operationResult;
            }


            //OperationResult operationResult = new OperationResult();
            //        int rowsAffected = 0;

            //        using (IDbConnection db = new SqlConnection(ConfigurationValues.FairfieldAllergyConnection))
            //        {
            //            try
            //            {
            //                Query = "update Appointment.Person"
            //                   + " set LocationId = @locationId"
            //                   + " where Id = @Id";


            //                rowsAffected = db.Execute(Query, new
            //                {
            //                    @Id = patientId,
            //                    @locationId = locationId
            //                });

            //                operationResult.Success = true;
            //                operationResult.ErrorMessage = "Success";
            //                return operationResult;
            //            }
            //            catch (Exception er)
            //            {
            //                operationResult.Success = false;
            //                operationResult.ErrorMessage = er.ToString();
            //                return operationResult;
            //            }
            //        }
        }


        public OperationResult AddAppointment(string date, string time, string location, string userID, string slotID)
        {
            int rowsAffected = 0;
            OperationResult operationResult = new OperationResult();

            try
            {
                using (SqlConnection db = new SqlConnection(ConfigurationValues.FairfieldAllergyConnection))
                {

                    rowsAffected = db.Query<int>("dt_add_appointment", new
                    {
                        @date = date,
                        @time = time,
                        @location = location,
                        @user_id = userID,
                        @slot_id = slotID
                    },
                    commandType: CommandType.StoredProcedure).FirstOrDefault();
                }

                operationResult.Success = true;
                operationResult.ErrorMessage = "Success";
                return operationResult;

            }
            catch (Exception er)
            {
                operationResult.Success = false;
                operationResult.ErrorMessage = er.ToString();
                return operationResult;
            }


            //OperationResult operationResult = new OperationResult();
            //try
            //{
            //    using (SqlConnection cn = new SqlConnection(ConfigurationValues.FairfieldAllergyConnection))
            //    {
            //        cn.Open();
            //        using (SqlCommand cm = cn.CreateCommand())
            //        {
            //            cm.CommandText = "INSERT INTO Appointment.Appointment(UserID,ApptDay,ApptTime,Location,SlotNumber)VALUES("
            //                + "@UserID,@ApptDay,@ApptTime,@Location,@SlotNumbersID)";
            //            cm.CommandType = System.Data.CommandType.Text;
            //            cm.Parameters.AddWithValue("@UserID", userID);
            //            cm.Parameters.AddWithValue("@ApptDay", date);
            //            cm.Parameters.AddWithValue("@ApptTime", date + " " + time);
            //            cm.Parameters.AddWithValue("@Location", location);
            //            cm.Parameters.AddWithValue("@SlotNumbersID", slotID);
            //            cm.ExecuteNonQuery();
            //            operationResult.Success = true;
            //            operationResult.ErrorMessage = "None";
            //            return operationResult;
            //        }
            //    }
            //}
            //catch (Exception er)
            //{
            //    operationResult.Success = false;
            //    operationResult.ErrorMessage = er.ToString();
            //    return operationResult;
            //}
        }

        public List<Appointment> GetScheduledAppointmentList(string userID)
        {
            List<Appointment> appointments = new List<Appointment>();
            Appointment appointment = new Appointment();

            try
            {
                using (SqlConnection db = new SqlConnection(ConfigurationValues.FairfieldAllergyConnection))
                {
                    appointments = db.Query<Appointment>("dt_get_scheduled_appointment_list", new
                    {
                        @user_id = userID
                    },
                    commandType: CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception er)
            {
                string s1 = er.ToString();
            }

            return appointments;


            //List<AppointmentSlots> appointmentSlots = new List<AppointmentSlots>();
            //List<Appointment> appointments = new List<Appointment>();
            //Appointment appointment = new Appointment();

            //try
            //{
            //    using (SqlConnection cn = new SqlConnection(ConfigurationValues.FairfieldAllergyConnection))
            //    {
            //        cn.Open();
            //        using (SqlCommand cm = cn.CreateCommand())
            //        {

            //            cm.CommandText = "SELECT [ID],[UserID],[ApptDay],[ApptTime],[ApptointType],[Location],[SlotNumber]"
            //                + " FROM Appointment.[Appointment]"
            //                + " where UserID = @UserID"
            //                + "  order by apptday desc";

            //            cm.CommandType = System.Data.CommandType.Text;
            //            cm.Parameters.AddWithValue("@UserID", userID);
            //            using (SqlDataReader dr = cm.ExecuteReader())
            //            {
            //                while (dr.Read())
            //                {
            //                    appointment = new Appointment();
            //                    appointment.AppointmentDescription = dr["ID"].ToString() + "~" + "Cancel Appt on "
            //                        + dr.GetDateTime(2).ToShortDateString()
            //                        + " at "
            //                        + dr.GetDateTime(3).ToShortTimeString();

            //                    appointments.Add(appointment);
            //                }
            //            }
            //            return appointments;
            //        }
            //    }
            //}
            //catch (Exception er)
            //{
            //    return appointments;
            //}
        }

        public OperationResult GetExistingAppointments(string userID)
        {
            List<Appointment> appointments = new List<Appointment>();
            Appointment appointment = new Appointment();

            OperationResult operationResult = new OperationResult();

            try
            {
                using (SqlConnection db = new SqlConnection(ConfigurationValues.FairfieldAllergyConnection))
                {
                    appointments = db.Query<Appointment>("dt_get_existing_appointments", new
                    {
                        @user_id = userID
                    },
                    commandType: CommandType.StoredProcedure).ToList();
                }

                for (int i = 0; i < appointments.Count; i++)
                {
                    appointments[i].AppointmentDescription = appointments[i].AppointmentDate
                                + " at "
                                + appointments[i].AppointmentTime;

                }

                operationResult.ExistingAppointments = appointments;
                operationResult.Success = true;
                operationResult.ErrorMessage = "None";
                return operationResult;

            }
            catch (Exception er)
            {
                operationResult.ExistingAppointments = appointments;
                operationResult.Success = false;
                operationResult.ErrorMessage = er.ToString();
                return operationResult;

            }


            //List<AppointmentSlots> appointmentSlots = new List<AppointmentSlots>();
            //List<Appointment> appointments = new List<Appointment>();
            //Appointment appointment = new Appointment();
            //OperationResult operationResult = new OperationResult();

            //try
            //{
            //    Query = " SELECT [ID] as Id ,[UserID],"
            //        + " CONVERT(varchar, [ApptDay], 101) as AppointmentDate,CONVERT(varchar(15), CAST([ApptTime] AS TIME), 100) as AppointmentTime,"
            //        + " [ApptointType],[Location],[SlotNumber]"
            //        + " FROM Appointment.[Appointment]"
            //        + " where UserID = @UserID"
            //        + "  order by apptday desc";

            //    using (SqlConnection db = new SqlConnection(ConfigurationValues.FairfieldAllergyConnection))
            //    {
            //        appointments = db.Query<Appointment>(Query, new
            //        {
            //            @UserID = userID
            //        }).ToList();

            //        for (int i = 0; i < appointments.Count; i++)
            //        {
            //            appointments[i].AppointmentDescription = appointments[i].AppointmentDate
            //                        + " at "
            //                        + appointments[i].AppointmentTime;

            //        }

            //        operationResult.ExistingAppointments = appointments;
            //        operationResult.Success = true;
            //        operationResult.ErrorMessage = "None";
            //        return operationResult;
            //    }
            //}
            //catch (Exception er)
            //{
            //    operationResult.ExistingAppointments = appointments;
            //    operationResult.Success = false;
            //    operationResult.ErrorMessage = er.ToString();
            //    return operationResult;
            //}
        }
        public OperationResult GetCurrentAppointments(string userID)
        {
            List<Appointment> appointments = new List<Appointment>();
            Appointment appointment = new Appointment();

            OperationResult operationResult = new OperationResult();

            try
            {
                using (SqlConnection db = new SqlConnection(ConfigurationValues.FairfieldAllergyConnection))
                {
                    appointments = db.Query<Appointment>("dt_get_current_appointments", new
                    {
                        @user_id = userID,
                        @todays_date = DateTime.Now.AddDays(-1)
                    },
                    commandType: CommandType.StoredProcedure).ToList();
                }

                for (int i = 0; i < appointments.Count; i++)
                {
                    appointments[i].AppointmentDescription = "Cancel Appt on "
                                + appointments[i].AppointmentDate
                                + " at "
                                + appointments[i].AppointmentTime;

                }

                operationResult.ExistingAppointments = appointments;
                operationResult.Success = true;
                operationResult.ErrorMessage = "None";
                return operationResult;

            }
            catch (Exception er)
            {
                operationResult.ExistingAppointments = appointments;
                operationResult.Success = false;
                operationResult.ErrorMessage = er.ToString();
                return operationResult;

            }

            //List<AppointmentSlots> appointmentSlots = new List<AppointmentSlots>();
            //List<Appointment> appointments = new List<Appointment>();
            //Appointment appointment = new Appointment();
            //OperationResult operationResult = new OperationResult();

            //try
            //{
            //    Query = " SELECT [ID] as Id ,[UserID],"
            //        + " CONVERT(varchar, [ApptDay], 101) as AppointmentDate,CONVERT(varchar(15), CAST([ApptTime] AS TIME), 100) as AppointmentTime,"
            //        + " [ApptointType],[Location],[SlotNumber]"
            //        + " FROM Appointment.[Appointment]"
            //        + " where UserID = @UserID"
            //        + " and [ApptDay] > @TodaysDate"
            //        + "  order by apptday desc";

            //    using (SqlConnection db = new SqlConnection(ConfigurationValues.FairfieldAllergyConnection))
            //    {
            //        appointments = db.Query<Appointment>(Query, new
            //        {
            //            @UserID = userID,
            //            @TodaysDate = DateTime.Now.AddDays(-1)
            //        }).ToList();

            //        for (int i = 0; i < appointments.Count; i++)
            //        {
            //            appointments[i].AppointmentDescription = "Cancel Appt on "
            //                        + appointments[i].AppointmentDate
            //                        + " at "
            //                        + appointments[i].AppointmentTime;

            //        }

            //        operationResult.ExistingAppointments = appointments;
            //        operationResult.Success = true;
            //        operationResult.ErrorMessage = "None";
            //        return operationResult;
            //    }
            //}
            //catch (Exception er)
            //{
            //    operationResult.ExistingAppointments = appointments;
            //    operationResult.Success = false;
            //    operationResult.ErrorMessage = er.ToString();
            //    return operationResult;
            //}
        }

        public Patient GetPatientNameForEmail(string patientID)
        {

            Patient patients = new Patient();

            try
            {
                using (SqlConnection db = new SqlConnection(ConfigurationValues.FairfieldAllergyConnection))
                {
                    patients = db.Query<Patient>("dt_get_patient_name_from_email", new
                    {
                        @patient_id = patientID
                    },
                    commandType: CommandType.StoredProcedure).Single();
                }

            }
            catch (Exception er)
            {
                string s1 = er.ToString();
            }

            return patients;

            //OperationResult operationResult = new OperationResult();
            //Patient patients = new Patient();
            //try
            //{
            //    Query = " select A.UID as PatientUserId, A.PWD as PatientPassword, B.Id as Id, LTRIM(RTRIM(B.firstname)) + ' ' + LTRIM(RTRIM(B.lastname)) as PatientName,"
            //    + " B.locationid as PatientLocation, A.ID as PatientWebId "
            //    + " from Appointment.Appointment.WEBID A"
            //    + " INNER JOIN Appointment.Person B"
            //    + " ON A.PersonID = B.ID"
            //    + " where B.Id = @Id";

            //    using (SqlConnection db = new SqlConnection(ConfigurationValues.FairfieldAllergyConnection))
            //    {
            //        patients = db.Query<Patient>(Query, new
            //        {
            //            @Id = patientID
            //        }).Single();
            //        operationResult.Success = true;
            //        operationResult.ErrorMessage = "None";

            //    }
            //}
            //catch (Exception er)
            //{
            //    string s1 = er.ToString();
            //}

            ////operationResult.Patient

            //return patients;
        }

        public void UpdateWebIdTableWithGuidFromAspNetUsersTable(string userID)
        {
            OperationResult operationResult = new OperationResult();
            Patient patients = new Patient();
            string id = string.Empty;

            try
            {
                Query = " SELECT[Id]"
                + " FROM[Appointment].[AspNetUsers]"
                + " where [UserName] = @UserName";

                using (SqlConnection db = new SqlConnection(ConfigurationValues.FairfieldAllergyConnection))
                {
                    id = db.Query<string>(Query, new
                    {
                        @UserName = userID
                    }).Single();
                    operationResult.Success = true;
                    operationResult.ErrorMessage = "None";
                }
            }
            catch (Exception er)
            {
                string s1 = er.ToString();
            }

            int rowsAffected = 0;

            using (IDbConnection db = new SqlConnection(ConfigurationValues.FairfieldAllergyConnection))
            {
                try
                {
                    Query = "UPDATE[Appointment].[WEBID]"
                        + " SET [user_id] = @user_id"
                        + " ,[has_changed_password] = 'F'"
                        + " WHERE [UID] = @UID";

                    rowsAffected = db.Execute(Query, new
                    {
                        @user_id = id,
                        @UID = userID
                    });

                }
                catch (Exception er)
                {
                    string s1 = er.ToString();
                }
            }
        }
        public bool HasUserChangedIdAndPassword(string userID)
        {
            string changed = string.Empty;

            try
            {

                Query = " SELECT [has_changed_password]"
                    + " FROM[Appointment].[WEBID]"
                    + " where [UID] = @UID";

                using (SqlConnection db = new SqlConnection(ConfigurationValues.FairfieldAllergyConnection))
                {
                    changed = db.Query<string>(Query, new
                    {
                        @UID = userID
                    }).Single();
                }
            }
            catch (Exception er)
            {
                string s1 = er.ToString();
                return false;
            }
            if (changed == "T")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public string GetUseridFromEmail(string email )
        {

            string userId = string.Empty;

            try
            {
                Query = " SELECT [UserName]"
                    + " FROM[Appointment].[Appointment].[AspNetUsers]"
                    + " where [Email] = @Email";

                using (SqlConnection db = new SqlConnection(ConfigurationValues.FairfieldAllergyConnection))
                {
                    userId = db.Query<string>(Query, new
                    {
                        @Email = email
                    }).Single();
                }
            }
            catch (Exception er)
            {
                string s1 = er.ToString();
                return userId;
            }

            return userId;
        }
    }
}
