using System;
using System.Collections.Generic;
using System.Text;

namespace FairfieldAllergy.Domain.Models
{
    public class AllergyPatient
    {
        public int pkid { get; set; }

        public string last_name { get; set; }
        public string first_name { get; set; }
        public string middle_name { get; set; }
        public string goes_by { get; set; }
        public string social { get; set; }
        public DateTime birthday { get; set; }
        public string home_phone { get; set; }
        public string work_phone { get; set; }
        public string account { get; set; }
        public int doctor_id { get; set; }
        public string scan_id { get; set; }
        public float gender { get; set; }
        public int insurance_id { get; set; }
        public int si_proc_id { get; set; }
        public int mi_proc_id { get; set; }
        public char require_referral { get; set; }
        public int servicedoctor_id { get; set; }
        public string notes { get; set; }
        public string mailadd_id { get; set; }
        public string si_charge { get; set; }
        public string mi_charge { get; set; }
        public int co_pay { get; set; }
        public string inj_notes { get; set; }
        public int injection_office_id { get; set; }
        public int mixing_office_id { get; set; }
        public char has_alerts { get; set; }
        public DateTime next_alert_date { get; set; }
        public string wait_list_string { get; set; }
        public char nobillins { get; set; }
        public DateTime next_checkup { get; set; }
        public DateTime dt_initiated { get; set; }
        public string chart_num { get; set; }
        public string mixingnotes { get; set; }
        public int chargescheme_id { get; set; }
        public string EMailAddress { get; set; }
        public char SignInDisabled { get; set; }
        public string SignInDisabledReason { get; set; }
        public char PeakFlowRequired { get; set; }
        public int PeakFlowBest { get; set; }
        public int PeakFlowMinimum { get; set; }
        public DateTime PeakFlowBestDate { get; set; }
        public DateTime PeakFlowDueDate { get; set; }
        public int HealthScreenID { get; set; }
        public int InActive { get; set; }
        public string cell_phone { get; set; }
        public int hold_order { get; set; }
        public string passcode { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime LastModified { get; set; }
        public string LastModifiedBy { get; set; }
        public string RevisionNumber { get; set; }
        public char diagnoses_warning_given { get; set; }
    }
}
