namespace Emite.CCM.API;

public static class Permission
{
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
	
}