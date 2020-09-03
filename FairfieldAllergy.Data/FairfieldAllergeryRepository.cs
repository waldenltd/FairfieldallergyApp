using Business.Configuration;
using Dapper;
using FairfieldAllergy.Domain;
using FairfieldAllergy.Domain.Models;
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
            int count = 0;

            try
            {
                Query = "SELECT count(*) FROM [Appointment].[family_accounts]"
                + " where main_account = @main_account"
                + " and added_account = @added_account";

                using (SqlConnection db = new SqlConnection(ConfigurationValues.FairfieldAllergyConnection))
                {
                    count = db.Query<int>(Query, new
                    {
                        @main_account = familyAccount.MainAccount,
                        @added_account = familyAccount.AddedAccount

                    }).Single();
                    //   return allergyPatient;
                }
            }
            catch (Exception er)
            {
                // return allergyPatient;
            }

            if (count < 1)
            {

                using (IDbConnection db = new SqlConnection(ConfigurationValues.FairfieldAllergyConnection))
                {
                    try
                    {
                        Query = "INSERT INTO [Appointment].[family_accounts]"
                        + " ([main_account],[added_account],[date_added])"
                        + " VALUES("
                        + " @main_account, @added_account,@date_added"
                        + " )";

                        count = db.Execute(Query, new
                        {
                            @main_account = familyAccount.MainAccount,
                            @added_account = familyAccount.AddedAccount,
                            @date_added = DateTime.Now
                        });

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
                }
            }
            else
            {
                operationResult.Success = false;
                operationResult.ErrorMessage = "Account Already Created";
                return operationResult;

            }
        }

        public void UpdateEmail(string id, string email)
        {

            int rowsAffected = 0;

            using (IDbConnection db = new SqlConnection(ConfigurationValues.FairfieldAllergyConnection))
            {
                try
                {
                    Query = " update [Appointment].[Appointment].[Person]"
                        + " set email = @email"
                        + " where id = @id";

                    rowsAffected = db.Execute(Query, new
                    {
                        @email = email,
                        @id = id
                    });
                }
                catch (Exception er)
                {
                    string s1 = er.ToString();
                }
            }
        }

        public OperationResult GetFamilyAccount(int mainAccount)
        {
            OperationResult operationResult = new OperationResult();

            try
            {
                Query = " select C.Id as Id, LTRIM(RTRIM(C.firstname)) + ' ' + LTRIM(RTRIM(C.lastname)) as PatientName,"
                + " C.locationid as PatientLocation, B.ID as PatientWebId "
                + " FROM [Appointment].[Appointment].[family_accounts] A"
                + " inner join [Appointment].WEBID B"
                + " on A.added_account = B.ID"
                + " inner join [Appointment].Person C"
                + " on B.PersonID = C.ID"
                + " Where A.main_account = @ID";

                using (SqlConnection db = new SqlConnection(ConfigurationValues.FairfieldAllergyConnection))
                {
                    operationResult.Patient = db.Query<Patient>(Query, new
                    {
                        @ID = mainAccount.ToString()
                    }).ToList();
                }
            }
            catch (Exception er)
            {
                // return allergyPatient;
            }
            return operationResult;
        }

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
                // return allergyPatient;
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
            using (IDbConnection db = new SqlConnection(ConfigurationValues.FairfieldAllergyConnection))
            {
                try
                {
                    Query = "INSERT INTO [Appointment].[BroadcastMessageRead]"
                    + " ([BroadcastMessageId],[UserId],[DateShown])"
                    + " VALUES("
                    + " @BroadcastMessageId,@UserId,@DateShown)";

                    rowsAffected = db.Execute(Query, new
                    {

                        @BroadcastMessageId = broadcastMessageId,
                        @UserId = userId,
                        @DateShown = DateTime.Now
                    });
                }
                catch (Exception er)
                {
                    string s1 = er.ToString();
                }
            }
        }

        public OperationResult UpdateUserName(string oldUserName, string newUserName, string password)
        {
            OperationResult operationResult = new OperationResult();
            int rowsAffected = 0;

            using (IDbConnection db = new SqlConnection(ConfigurationValues.FairfieldAllergyConnection))
            {
                try
                {
                    Query = "update Appointment.WEBID"
                       + " set UID = @NewUID"
                       + " where UID = @OldUID";


                    rowsAffected = db.Execute(Query, new
                    {
                        @OldUID = oldUserName,
                        @NewUID = newUserName
                    });

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
            }
        }

        public OperationResult GetCurrentId()
        {
            int currentId = 0;
            OperationResult operationResult = new OperationResult();

            try
            {
                Query = "SELECT [current_pkid] FROM Appointment.[CurrentPkid]";

                using (SqlConnection db = new SqlConnection(ConfigurationValues.FairfieldAllergyConnection))
                {
                    currentId = db.Query<int>(Query).Single();
                    operationResult.CurrentId = currentId;
                    operationResult.ErrorMessage = "None";
                    return operationResult;
                }
            }
            catch (Exception er)
            {
                operationResult.Success = false;
                operationResult.ErrorMessage = er.ToString();
                return operationResult;
            }
        }

        public OperationResult UpdateUserId(int userId)
        {
            OperationResult operationResult = new OperationResult();
            int rowsAffected = 0;

            using (IDbConnection db = new SqlConnection(ConfigurationValues.FairfieldAllergyConnection))
            {
                try
                {
                    Query = "update Appointment.[CurrentPkid]"
                       + " set current_pkid = @current_pkid";

                    rowsAffected = db.Execute(Query, new
                    {
                        @current_pkid = userId
                    });

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
            }
        }

        public void AddBroadcastMessage(BroadcastMessage broadcastMessage)
        {

            OperationResult operationResult = new OperationResult();
            int rowsAffected = 0;

            using (IDbConnection db = new SqlConnection(ConfigurationValues.FairfieldAllergyConnection))
            {
                try
                {

                    Query = "INSERT INTO [Appointment].[BroadcastMessage] ([Message],[DateCreated]"
                    + " ,[Norwalk],[Greenwich],[Stamford],[Ridgefield])"
                    + " VALUES("
                    + " @Message,@DateCreated,@Norwalk,@Greenwich,@Stamford,@Ridgefield"
                    + ")";

                    rowsAffected = db.Execute(Query, new
                    {
                        @Message = broadcastMessage.Message,
                        @DateCreated = DateTime.Now,
                        @Norwalk = broadcastMessage.Norwalk,
                        @Greenwich = broadcastMessage.Greenwich,
                        @Stamford = broadcastMessage.Stamford,
                        @Ridgefield = broadcastMessage.Ridgefield

                    });

                    operationResult.Success = true;
                    operationResult.ErrorMessage = "Success";
                    //return operationResult;
                }
                catch (Exception er)
                {
                    operationResult.Success = false;
                    operationResult.ErrorMessage = er.ToString();
                    //return operationResult;
                }
            }
        }
        public void CreateNewUser(AllergyPatient allergyPatient, string userId, string password)
        {

            OperationResult operationResult = new OperationResult();
            int newId = 0;

            string sex = string.Empty;
            if (allergyPatient.gender == 1)
            {
                sex = "M";
            }
            else if (allergyPatient.gender == 2)
            {
                sex = "F";
            }

            using (IDbConnection db = new SqlConnection(ConfigurationValues.FairfieldAllergyConnection))
            {
                try
                {
                    Query = "DECLARE @InsertedRows AS TABLE(Id int);"
                        + "INSERT INTO Appointment.[Person]([FirstName],[LastName],[DOB],[Sex],[HPhone]"
                        + " ,[EMail],[LocationID],[first_appointment],[venom_patient],[sendemail],[sendlate]"
                        + " ,[sendnone],[NotificationID],[patientid]) OUTPUT Inserted.Id INTO @InsertedRows VALUES("
                        + " @FirstName,@LastName,@DOB,@Sex,@HPhone,@EMail,@LocationID,@first_appointment,@venom_patient,"
                        + " @sendemail,@sendlate,@sendnone,@NotificationID,@patientid)"
                        + " SELECT Id FROM @InsertedRows";

                    newId = db.Query<int>(Query, new
                    {
                        @FirstName = allergyPatient.first_name,
                        @LastName = allergyPatient.last_name,
                        @DOB = allergyPatient.birthday,
                        @Sex = sex,
                        @HPhone = allergyPatient.home_phone,
                        @EMail = allergyPatient.EMailAddress,
                        @LocationID = allergyPatient.injection_office_id,
                        @first_appointment = "N",
                        @venom_patient = "N",
                        @sendemail = "N",
                        @sendlate = "N",
                        @sendnone = "N",
                        @NotificationID = "",
                        @patientid = 0
                    }).Single();
                    //var id = connection.Query<int>(sql, new { Stuff = mystuff}).Single();

                    operationResult.Success = true;
                    operationResult.ErrorMessage = "Success";
                    //return operationResult;
                }
                catch (Exception er)
                {
                    operationResult.Success = false;
                    operationResult.ErrorMessage = er.ToString();
                    //return operationResult;
                }
            }

            using (IDbConnection db = new SqlConnection(ConfigurationValues.FairfieldAllergyConnection))
            {
                try
                {

                    Query = "INSERT INTO Appointment.[WEBID]([PersonID],[pkid],[digi_account],[UID],[PWD],[HINTTYPE],[HINTVALUE])"
                        + " VALUES("
                        + " @PersonID,@pkid, @digi_account, @UID,@PWD,@HINTTYPE,@HINTVALUE)";


                    newId = db.Execute(Query, new
                    {

                        @PersonID = newId,
                        @pkid = allergyPatient.pkid,
                        @digi_account = allergyPatient.pkid,
                        @UID = userId,
                        @PWD = password,
                        @HINTTYPE = "None",
                        @HINTVALUE = "None"
                    });
                    //var id = connection.Query<int>(sql, new { Stuff = mystuff}).Single();

                    operationResult.Success = true;
                    operationResult.ErrorMessage = "Success";
                    //return operationResult;
                }
                catch (Exception er)
                {
                    operationResult.Success = false;
                    operationResult.ErrorMessage = er.ToString();
                    //return operationResult;
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
                    + " where pkid > @pkid;";

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
                return allergyPatient;
            }
        }

        public OperationResult UpdateNumberOfSlots(int slotId, int slotNumber)
        {
            OperationResult operationResult = new OperationResult();
            int rowsAffected = 0;

            using (IDbConnection db = new SqlConnection(ConfigurationValues.FairfieldAllergyConnection))
            {
                try
                {
                    Query = "update Appointment.appointmentslots"
                       + " set NumberSlots = @NumberSlots"
                       + " where SlotId = @SlotId";


                    rowsAffected = db.Execute(Query, new
                    {
                        @NumberSlots = slotNumber,
                        @SlotId = slotId
                    });

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
            }
        }

        public OperationResult GetPatientIdAndPassword(string patientId)
        {
            OperationResult operationResult = new OperationResult();

            try
            {
                Query = " select Uid as PatientUserId, Pwd as PatientPassword from Appointment.WEBID"
                + " where personid = @Personid";

                using (SqlConnection db = new SqlConnection(ConfigurationValues.FairfieldAllergyConnection))
                {
                    operationResult.PatientCredentials = db.Query<Patient>(Query, new
                    {
                        @Personid = patientId
                    }).Single();
                    operationResult.Success = true;
                    operationResult.ErrorMessage = "None";
                    return operationResult;
                }
            }
            catch (Exception er)
            {
                operationResult.Success = false;
                operationResult.ErrorMessage = er.ToString();
                return operationResult;
            }
        }

        public OperationResult GetPatients()
        {
            OperationResult operationResult = new OperationResult();
            List<Patient> patients = new List<Patient>();

            try
            {
                Query = " select B.Id as Id, LTRIM(RTRIM(B.firstname)) + ' ' + LTRIM(RTRIM(B.lastname)) as PatientName,"
                + " B.locationid as PatientLocation, A.ID as PatientWebId "
                + " from Appointment.WEBID A"
                + " INNER JOIN Appointment.Person B"
                + " ON A.PersonID = B.ID"
                + " where B.locationid not in (-1)"
                + " and B.lastname not like '%zz%'"
                + " and B.id NOT IN (3337,3500,3696,3703,3772,3943,3973,4405,5132,5398,5517,5685,5689,5855,6112)"
                + " order by B.lastname";

                using (SqlConnection db = new SqlConnection(ConfigurationValues.FairfieldAllergyConnection))
                {
                    operationResult.Patient = db.Query<Patient>(Query).ToList();
                    operationResult.Success = true;
                    operationResult.ErrorMessage = "None";
                    return operationResult;
                }
            }
            catch (Exception er)
            {
                operationResult.Success = false;
                operationResult.ErrorMessage = er.ToString();
                return operationResult;
            }
        }

        public OperationResult GetPatients(string filter)
        {
            OperationResult operationResult = new OperationResult();
            List<Patient> patients = new List<Patient>();

            try
            {
                Query = " select B.Id as Id, LTRIM(RTRIM(B.firstname)) + ' ' + LTRIM(RTRIM(B.lastname)) as PatientName,"
                + " B.locationid as PatientLocation, A.ID as PatientWebId "
                + " from Appointment.WEBID A"
                + " INNER JOIN Appointment.Person B"
                + " ON A.PersonID = B.ID"
                + " where B.locationid not in (-1)"
                + " and B.lastname not like '%zz%'"
                + " and B.id NOT IN (3337,3500,3696,3703,3772,3943,3973,4405,5132,5398,5517,5685,5689,5855,6112)"
                + " and LTRIM(RTRIM(B.firstname)) + ' ' + LTRIM(RTRIM(B.lastname)) LIKE '%" + filter + "%'"
                + " order by B.lastname";

                using (SqlConnection db = new SqlConnection(ConfigurationValues.FairfieldAllergyConnection))
                {
                    operationResult.Patient = db.Query<Patient>(Query).ToList();
                    operationResult.Success = true;
                    operationResult.ErrorMessage = "None";
                    return operationResult;
                }
            }
            catch (Exception er)
            {
                operationResult.Success = false;
                operationResult.ErrorMessage = er.ToString();
                return operationResult;
            }
        }

        public OperationResult GetPatientAllergyAppointments(string date, string location)
        {
            OperationResult operationResult = new OperationResult();

            List<AllergyPatientAppointments> allergyPatientAppointmentsList = new List<AllergyPatientAppointments>();

            try
            {


                //CONVERT(varchar(15), CAST([ApptTime] AS TIME), 100)
                Query = " SELECT Appointment.ID as Id, person.FirstName as FirstName, person.LastName as LastName,"
                    //+ " CONVERT(varchar, [ApptDay], 101) as DateOfAppointment,CONVERT(varchar, [ApptTime], 108) as TimeOfAppointment,"
                    + " CONVERT(varchar, [ApptDay], 101) as DateOfAppointment,CONVERT(varchar(15), CAST([ApptTime] AS TIME), 100) as TimeOfAppointment,"
                    + " CONVERT(varchar, person.DOB, 101) as DateOfBirth, '0' as Account "
                    + " FROM Appointment.Appointment"
                    + " inner join webid"
                    + " on webid.ID = appointment.UserID"
                    + " inner join person"
                    + " on person.ID = webid.PersonID"
                    + " where [ApptDay] = @ApptDay"
                    + " and person.locationid = @locationId"
                    + " order by Appointment.ApptDay, Appointment.ApptTime";

                using (SqlConnection db = new SqlConnection(ConfigurationValues.FairfieldAllergyConnection))
                {
                    allergyPatientAppointmentsList = db.Query<AllergyPatientAppointments>(Query, new
                    {
                        @ApptDay = date,
                        @locationId = location
                    }).ToList();

                    if (allergyPatientAppointmentsList.Count > 0)
                    {

                        operationResult.AllergyPatientAppointments = allergyPatientAppointmentsList;
                        operationResult.Success = true;
                        operationResult.ErrorMessage = "None";
                    }
                    else
                    {
                        operationResult.AllergyPatientAppointments = allergyPatientAppointmentsList;
                        operationResult.Success = false;
                        operationResult.ErrorMessage = "None";

                    }
                }
            }
            catch (Exception er)
            {
                operationResult.Success = false;
                operationResult.ErrorMessage = er.ToString();

            }
            return operationResult;
        }

        public OperationResult GetPatientAllergyAppointmentSlots(string date, string location)
        {
            OperationResult operationResult = new OperationResult();

            List<AllergyPatientAppointments> allergyPatientAppointmentsList = new List<AllergyPatientAppointments>();

            try
            {
                Query = " SELECT SlotId ,LocationID ,AppointmentTypeID,NumberSlots ,CONVERT(varchar, SlotDate, 101) as SlotDate,"
                    //+ " CAST(SlotTime AS TIME), 100) as SlotTime,"
                    + " CONVERT(varchar(15), CAST([SlotTime] AS TIME), 100),"
                    + " Block"
                    + " FROM Appointment.[AppointmentSlots]"
                    + " where SlotDate = @SlotDate"
                    + " AND LocationID = @LocationId"
                    + " Order by SlotDate, SlotTime";

                using (SqlConnection db = new SqlConnection(ConfigurationValues.FairfieldAllergyConnection))
                {
                    allergyPatientAppointmentsList = db.Query<AllergyPatientAppointments>(Query, new
                    {
                        @SlotDate = date,
                        @LocationId = location
                    }).ToList();

                    operationResult.AllergyPatientAppointments = allergyPatientAppointmentsList;
                    operationResult.Success = true;
                    operationResult.ErrorMessage = "None";
                    return operationResult;
                }
            }
            catch (Exception er)
            {
                //SerilogLogError.LogErrorToFile(er.ToString());
            }
            return operationResult;
        }

        public OperationResult GetListOfAppointment(string date, string location)
        {
            OperationResult operationResult = new OperationResult();
            List<AppointmentSlots> appointmentSlots = new List<AppointmentSlots>();
            List<OpenAppointment> appointments = new List<OpenAppointment>();
            OpenAppointment appointment = new OpenAppointment();

            try
            {
                using (SqlConnection cn = new SqlConnection(ConfigurationValues.FairfieldAllergyConnection))
                {
                    cn.Open();
                    using (SqlCommand cm = cn.CreateCommand())
                    {
                        //cm.CommandText = "Select  A.SlotId,A.LocationID,A.AppointmentTypeID,A.NumberSlots,A.SlotDate,A.SlotTime from appointmentslots A"
                        cm.CommandText = "select A.SlotId as SlotId,A.LocationID,A.AppointmentTypeID,A.NumberSlots,A.SlotDate,A.SlotTime,"
                            + " A.Block,A.AddId,A.num_slots_wi,A.num_slots_ol "
                            + " from Appointment.appointmentslots A"
                            + " where A.slotdate = @SlotDate"
                            + " and A.numberslots > (select count(ID) from appointment B"
                            + " where B.apptday = @SlotDate"
                            + " and B.slotnumber = A.slotid"
                            + " and B.location = @Location)"
                            + " AND A.Block = 'N'"
                            + " and A.locationid = @Location"
                            + " order by A.slotid";

                        cm.CommandType = System.Data.CommandType.Text;
                        cm.Parameters.AddWithValue("@SlotDate", date);
                        cm.Parameters.AddWithValue("@Location", location);
                        using (SqlDataReader dr = cm.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                //8:00 am on July 2, 2020 is available 
                                appointment = new OpenAppointment();
                                appointment.AppointmentDescription = dr.GetDateTime(5).ToShortTimeString()
                                + " on " + dr.GetDateTime(4).ToShortDateString().Replace("/", "-")
                                + " is available ";
                                //+ dr.GetDateTime(4).ToShortDateString().Replace("/", "-")
                                //;;+ " at "
                                //;;+ dr.GetDateTime(5).ToShortTimeString();
                                appointment.SlotID = int.Parse(dr["SlotId"].ToString());
                                appointments.Add(appointment);
                            }
                        }

                    }
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

            operationResult.Appointments = appointments;
            return operationResult;

        }

        public OperationResult GetAppointmentSlots(string date, string location)
        {
            OperationResult operationResult = new OperationResult();
            List<AppointmentSlots> appointmentSlots = new List<AppointmentSlots>();

            try
            {
                //CONVERT(varchar, [ApptDay], 101) as AppointmentDate,CONVERT(varchar(15), CAST([ApptTime] AS TIME), 100) as AppointmentTime,"
                //cm.CommandText = "Select  A.SlotId,A.LocationID,A.AppointmentTypeID,A.NumberSlots,A.SlotDate,A.SlotTime from appointmentslots A"
                Query = " select A.SlotId as SlotId,A.LocationID,A.AppointmentTypeID,A.NumberSlots,"
                    + " CONVERT(varchar, [SlotDate], 101) as SlotDate,"
                    + " CONVERT(varchar(15), CAST([SlotTime] AS TIME), 100) as SlotTime,"
                    + " -1 as NewSlotNumber,"
                    + " A.Block,A.AddId,A.num_slots_wi,A.num_slots_ol "
                    + " from Appointment.appointmentslots A"
                    + " where A.slotdate = @SlotDate"
                    //+ " and A.numberslots > (select count(ID) from appointment B"
                    //+ " where B.apptday = @SlotDate"
                    //+ " and B.slotnumber = A.slotid"
                    //+ " and B.location = @Location)"
                    + " AND A.Block = 'N'"
                    + " and A.locationid = @Location"
                    + " order by A.slotid";


                using (SqlConnection db = new SqlConnection(ConfigurationValues.FairfieldAllergyConnection))
                {
                    appointmentSlots = db.Query<AppointmentSlots>(Query, new
                    {
                        @Location = location,
                        @SlotDate = date

                    }).ToList();

                    operationResult.Success = true;
                    operationResult.AppointmentSlots = appointmentSlots;
                    operationResult.ErrorMessage = "None";
                    return operationResult;
                }
            }
            catch (Exception er)
            {
                //SerilogLogError.LogErrorToFile(er.ToString());
                operationResult.Success = false;
                operationResult.ErrorMessage = er.ToString();
                return operationResult;
            }
        }

        public OperationResult DeleteAppointment(string appointmentID)
        {
            OperationResult operationResult = new OperationResult();
            int rowsAffected = 0;

            using (IDbConnection db = new SqlConnection(ConfigurationValues.FairfieldAllergyConnection))
            {
                try
                {
                    Query = "DELETE FROM Appointment.Appointment"
                        + " where id = @id";
                    rowsAffected = db.Execute(Query, new
                    {
                        @id = appointmentID,
                    });
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
            }
        }

        public UserInformation GetUserInformation(string userName)
        {
            UserInformation userInformation = new UserInformation();

            try
            {
                Query = " SELECT WEBID.ID as UserId,[AspNetUsers].[Id],[AspNetUsers].[UserName],[AspNetUsers].[Email],"
                    + " [AspNetUsers].[PhoneNumber],[AspNetUsers].[City],[AspNetUsers].[FirstName],[AspNetUsers].[LastName],"
                    + " [AspNetUsers].[DateOfBirth],[AspNetUsers].[Sex],[AspNetUsers].[HomePhone],[AspNetUsers].[location]"
                    + " FROM Appointment.[AspNetUsers]"
                    + " inner join Appointment.WEBID"
                    + " on WEBID.UID = [AspNetUsers].UserName"
                    + " where [UserName] = @UserName";

                using (SqlConnection db = new SqlConnection(ConfigurationValues.FairfieldAllergyConnection))
                {
                    userInformation = db.Query<UserInformation>(Query, new
                    {
                        @UserName = userName

                    }).Single();

                    //operationResult.Success = true;
                    //operationResult.ErrorMessage = "None";
                    //return operationResult;
                }
            }
            catch (Exception er)
            {
                //SerilogLogError.LogErrorToFile(er.ToString());
                //operationResult.Success = false;
                //operationResult.ErrorMessage = er.ToString();
                //return operationResult;
            }
            return userInformation;
        }

        public OperationResult UpdatePatientLocation(string patientId, string locationId)
        {
            OperationResult operationResult = new OperationResult();
            int rowsAffected = 0;

            using (IDbConnection db = new SqlConnection(ConfigurationValues.FairfieldAllergyConnection))
            {
                try
                {
                    Query = "update Appointment.Person"
                       + " set LocationId = @locationId"
                       + " where Id = @Id";


                    rowsAffected = db.Execute(Query, new
                    {
                        @Id = patientId,
                        @locationId = locationId
                    });

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
            }
        }


        public OperationResult AddAppointment(string date, string time, string location, string userID, string slotID)
        {

            OperationResult operationResult = new OperationResult();
            try
            {
                using (SqlConnection cn = new SqlConnection(ConfigurationValues.FairfieldAllergyConnection))
                {
                    cn.Open();
                    using (SqlCommand cm = cn.CreateCommand())
                    {
                        cm.CommandText = "INSERT INTO Appointment.Appointment(UserID,ApptDay,ApptTime,Location,SlotNumber)VALUES("
                            + "@UserID,@ApptDay,@ApptTime,@Location,@SlotNumbersID)";
                        cm.CommandType = System.Data.CommandType.Text;
                        cm.Parameters.AddWithValue("@UserID", userID);
                        cm.Parameters.AddWithValue("@ApptDay", date);
                        cm.Parameters.AddWithValue("@ApptTime", date + " " + time);
                        cm.Parameters.AddWithValue("@Location", location);
                        cm.Parameters.AddWithValue("@SlotNumbersID", slotID);
                        cm.ExecuteNonQuery();
                        operationResult.Success = true;
                        operationResult.ErrorMessage = "None";
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
        }

        public List<Appointment> GetScheduledAppointmentList(string userID)
        {
            List<AppointmentSlots> appointmentSlots = new List<AppointmentSlots>();
            List<Appointment> appointments = new List<Appointment>();
            Appointment appointment = new Appointment();

            try
            {
                using (SqlConnection cn = new SqlConnection(ConfigurationValues.FairfieldAllergyConnection))
                {
                    cn.Open();
                    using (SqlCommand cm = cn.CreateCommand())
                    {

                        cm.CommandText = "SELECT [ID],[UserID],[ApptDay],[ApptTime],[ApptointType],[Location],[SlotNumber]"
                            + " FROM Appointment.[Appointment]"
                            + " where UserID = @UserID"
                            + "  order by apptday desc";

                        cm.CommandType = System.Data.CommandType.Text;
                        cm.Parameters.AddWithValue("@UserID", userID);
                        using (SqlDataReader dr = cm.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                appointment = new Appointment();
                                appointment.AppointmentDescription = dr["ID"].ToString() + "~" + "Cancel Appt on "
                                    + dr.GetDateTime(2).ToShortDateString()
                                    + " at "
                                    + dr.GetDateTime(3).ToShortTimeString();

                                appointments.Add(appointment);
                            }
                        }
                        return appointments;
                    }
                }
            }
            catch (Exception er)
            {
                return appointments;
            }
        }

        public OperationResult GetExistingAppointments(string userID)
        {
            List<AppointmentSlots> appointmentSlots = new List<AppointmentSlots>();
            List<Appointment> appointments = new List<Appointment>();
            Appointment appointment = new Appointment();
            OperationResult operationResult = new OperationResult();

            try
            {
                Query = " SELECT [ID] as Id ,[UserID],"
                    + " CONVERT(varchar, [ApptDay], 101) as AppointmentDate,CONVERT(varchar(15), CAST([ApptTime] AS TIME), 100) as AppointmentTime,"
                    + " [ApptointType],[Location],[SlotNumber]"
                    + " FROM Appointment.[Appointment]"
                    + " where UserID = @UserID"
                    + "  order by apptday desc";

                using (SqlConnection db = new SqlConnection(ConfigurationValues.FairfieldAllergyConnection))
                {
                    appointments = db.Query<Appointment>(Query, new
                    {
                        @UserID = userID
                    }).ToList();

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
            }
            catch (Exception er)
            {
                operationResult.ExistingAppointments = appointments;
                operationResult.Success = false;
                operationResult.ErrorMessage = er.ToString();
                return operationResult;
            }
        }
        public OperationResult GetCurrentAppointments(string userID)
        {
            List<AppointmentSlots> appointmentSlots = new List<AppointmentSlots>();
            List<Appointment> appointments = new List<Appointment>();
            Appointment appointment = new Appointment();
            OperationResult operationResult = new OperationResult();

            try
            {
                Query = " SELECT [ID] as Id ,[UserID],"
                    + " CONVERT(varchar, [ApptDay], 101) as AppointmentDate,CONVERT(varchar(15), CAST([ApptTime] AS TIME), 100) as AppointmentTime,"
                    + " [ApptointType],[Location],[SlotNumber]"
                    + " FROM Appointment.[Appointment]"
                    + " where UserID = @UserID"
                    + " and [ApptDay] > @TodaysDate"
                    + "  order by apptday desc";

                using (SqlConnection db = new SqlConnection(ConfigurationValues.FairfieldAllergyConnection))
                {
                    appointments = db.Query<Appointment>(Query, new
                    {
                        @UserID = userID,
                        @TodaysDate = DateTime.Now.AddDays(-1)
                    }).ToList();

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
            }
            catch (Exception er)
            {
                operationResult.ExistingAppointments = appointments;
                operationResult.Success = false;
                operationResult.ErrorMessage = er.ToString();
                return operationResult;
            }
        }

        public Patient GetPatientNameForEmail(string patientID)
        {
            OperationResult operationResult = new OperationResult();
            Patient patients = new Patient();
            try
            {
                Query = " select A.UID as PatientUserId, A.PWD as PatientPassword, B.Id as Id, LTRIM(RTRIM(B.firstname)) + ' ' + LTRIM(RTRIM(B.lastname)) as PatientName,"
                + " B.locationid as PatientLocation, A.ID as PatientWebId "
                + " from Appointment.Appointment.WEBID A"
                + " INNER JOIN Appointment.Person B"
                + " ON A.PersonID = B.ID"
                + " where B.Id = @Id";

                using (SqlConnection db = new SqlConnection(ConfigurationValues.FairfieldAllergyConnection))
                {
                    patients = db.Query<Patient>(Query, new
                    {
                        @Id = patientID
                    }).Single();
                    operationResult.Success = true;
                    operationResult.ErrorMessage = "None";

                }
            }
            catch (Exception er)
            {
                string s1 = er.ToString();
            }

            //operationResult.Patient

            return patients;
        }
    }
}
