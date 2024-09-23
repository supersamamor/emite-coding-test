using System.Reflection;
namespace Emite.CCM.Web;

public static class Permission
{
    public static IEnumerable<string> GenerateAllPermissions()
	{
		var permissions = new List<string>();
		// Get all nested classes in the Permissions class
		var nestedClasses = typeof(Permission).GetNestedTypes();
		foreach (var nestedClass in nestedClasses)
		{
			// Get all public static string fields in the nested class
			var permissionsInClass = nestedClass.GetFields(BindingFlags.Public | BindingFlags.Static)
				.Where(f => f.FieldType == typeof(string))
				.Select(f => f.GetValue(null)!.ToString());

			permissions.AddRange(permissionsInClass!);
		}
		return permissions.OrderBy(l=>l);
	}
	public static IEnumerable<string> GeneratePermissionsForModule(string module)
	{
		var permissions = new List<string>();
		// Get the nested class for the specified module
		var moduleType = typeof(Permission).GetNestedType(module);
		if (moduleType != null)
		{
			// Get all public static string fields in the module class
			var modulePermissions = moduleType.GetFields(BindingFlags.Public | BindingFlags.Static)
				.Where(f => f.FieldType == typeof(string))
				.Select(f => f.GetValue(null)!.ToString());
			permissions.AddRange(modulePermissions!);
		}     
		return permissions.OrderBy(l => l);
	}

    public static class Admin
    {
        public const string View = "Permission.Admin.View";
        public const string Create = "Permission.Admin.Create";
        public const string Edit = "Permission.Admin.Edit";
        public const string Delete = "Permission.Admin.Delete";
    }

    public static class Entities
    {
        public const string View = "Permission.Entities.View";
        public const string Create = "Permission.Entities.Create";
        public const string Edit = "Permission.Entities.Edit";
        public const string Delete = "Permission.Entities.Delete";
    }

    public static class Roles
    {
        public const string View = "Permission.Roles.View";
        public const string Create = "Permission.Roles.Create";
        public const string Edit = "Permission.Roles.Edit";
        public const string Delete = "Permission.Roles.Delete";
    }

    public static class Users
    {
        public const string View = "Permission.Users.View";
        public const string Create = "Permission.Users.Create";
        public const string Edit = "Permission.Users.Edit";
        public const string Delete = "Permission.Users.Delete";
    }

    public static class Apis
    {
        public const string View = "Permission.Apis.View";
        public const string Create = "Permission.Apis.Create";
        public const string Edit = "Permission.Apis.Edit";
        public const string Delete = "Permission.Apis.Delete";
    }

    public static class Applications
    {
        public const string View = "Permission.Applications.View";
        public const string Create = "Permission.Applications.Create";
        public const string Edit = "Permission.Applications.Edit";
        public const string Delete = "Permission.Applications.Delete";
    }

    public static class AuditTrail
    {
        public const string View = "Permission.AuditTrail.View";
        public const string Create = "Permission.AuditTrail.Create";
        public const string Edit = "Permission.AuditTrail.Edit";
        public const string Delete = "Permission.AuditTrail.Delete";
    }
	public static class Report
    {
        public const string View = "Permission.Report.View";
		public const string AIDrivenDataAnalysisAndInsights = "Permission.Report.AIDrivenDataAnalysisAndInsights";
    }
    public static class ReportSetup
    {
        public const string View = "Permission.ReportSetup.View";
        public const string Create = "Permission.ReportSetup.Create";
        public const string Edit = "Permission.ReportSetup.Edit";
        public const string Delete = "Permission.ReportSetup.Delete";
        public const string Approve = "Permission.ReportSetup.Approve";
    }
    public static class Agent
	{
		public const string View = "Permission.Agent.View";
		public const string Create = "Permission.Agent.Create";
		public const string Edit = "Permission.Agent.Edit";
		public const string Delete = "Permission.Agent.Delete";
		public const string Upload = "Permission.Agent.Upload";
		public const string History = "Permission.Agent.History";
        public const string UpdateStatus = "Permission.Agent.UpdateStatus";
    }
	public static class Customer
	{
		public const string View = "Permission.Customer.View";
		public const string Create = "Permission.Customer.Create";
		public const string Edit = "Permission.Customer.Edit";
		public const string Delete = "Permission.Customer.Delete";
		public const string Upload = "Permission.Customer.Upload";
		public const string History = "Permission.Customer.History";
	}
	public static class Call
	{
		public const string View = "Permission.Call.View";
		public const string Create = "Permission.Call.Create";
		public const string Edit = "Permission.Call.Edit";
		public const string Delete = "Permission.Call.Delete";
		public const string Upload = "Permission.Call.Upload";
		public const string History = "Permission.Call.History";
	}
	public static class Ticket
	{
		public const string View = "Permission.Ticket.View";
		public const string Create = "Permission.Ticket.Create";
		public const string Edit = "Permission.Ticket.Edit";
		public const string Delete = "Permission.Ticket.Delete";
		public const string Upload = "Permission.Ticket.Upload";
		public const string History = "Permission.Ticket.History";
		public const string Approve = "Permission.Ticket.Approve";
	}
	
	public static class ApproverSetup
	{
		public const string Create = "Permission.ApproverSetup.Create";
		public const string View = "Permission.ApproverSetup.View";
		public const string Edit = "Permission.ApproverSetup.Edit";
		public const string PendingApprovals = "Permission.ApproverSetup.PendingApprovals";
	}
}
