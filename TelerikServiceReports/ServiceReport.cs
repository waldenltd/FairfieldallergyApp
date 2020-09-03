namespace TelerikServiceReports
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;

	/// <summary>
	/// Summary description for ServiceReport
	/// </summary>
    public partial class ServiceReport : Telerik.Reporting.Report
	{
		public ServiceReport(string totalBillable, string balanceFromPreviousMonth,
			string hoursPurchasedThisMonth, string balancedRemaining, string month)
		{
			InitializeComponent();

			this.txtTotalBillable.Value = totalBillable;
			this.txtBalanceFromPreviousMonth.Value = balanceFromPreviousMonth;
			this.txtHoursPurchasedThisMonth.Value = hoursPurchasedThisMonth;
			this.txtBalancedRemaining.Value = balancedRemaining;
			this.txtDate.Value = month;

		}
	}
}