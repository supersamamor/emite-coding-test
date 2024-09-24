using Emite.Common.Data;
using Emite.Common.Identity.Abstractions;
using Emite.CCM.Core.CCM;
using Microsoft.EntityFrameworkCore;

namespace Emite.CCM.Infrastructure.Data;

public class ApplicationContext : AuditableDbContext<ApplicationContext>
{
    private readonly IAuthenticatedUser _authenticatedUser;

    public ApplicationContext(DbContextOptions<ApplicationContext> options,
                              IAuthenticatedUser authenticatedUser) : base(options, authenticatedUser)
    {
        _authenticatedUser = authenticatedUser;
    }
    public DbSet<ReportState> Report { get; set; } = default!;  
    public DbSet<ReportQueryFilterState> ReportQueryFilter { get; set; } = default!;
    public DbSet<ReportRoleAssignmentState> ReportRoleAssignment { get; set; } = default!;
	public DbSet<ReportAIIntegrationState> ReportAIIntegration { get; set; } = default!;
	public DbSet<UploadProcessorState> UploadProcessor { get; set; } = default!;
	public DbSet<ApprovalState> Approval { get; set; } = default!;
	public DbSet<ApproverSetupState> ApproverSetup { get; set; } = default!;
	public DbSet<ApproverAssignmentState> ApproverAssignment { get; set; } = default!;
	public DbSet<ApprovalRecordState> ApprovalRecord { get; set; } = default!;
 
    public DbSet<AgentState> Agent { get; set; } = default!;
	public DbSet<CustomerState> Customer { get; set; } = default!;
	public DbSet<CallState> Call { get; set; } = default!;
	public DbSet<TicketState> Ticket { get; set; } = default!;
	
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        foreach (var property in modelBuilder.Model.GetEntityTypes()
                                                   .SelectMany(t => t.GetProperties())
                                                   .Where(p => p.ClrType == typeof(decimal)
                                                               || p.ClrType == typeof(decimal?)))
        {
            property.SetColumnType("decimal(18,6)");
        }
		foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var property in entityType.GetProperties()
                                               .Where(p => p.Name.Equals("CreatedBy", StringComparison.OrdinalIgnoreCase)
                                               || p.Name.Equals("LastModifiedBy", StringComparison.OrdinalIgnoreCase)
                                               || p.Name.Equals("Entity", StringComparison.OrdinalIgnoreCase)
                                               || p.Name.Equals("LastModifiedDate", StringComparison.OrdinalIgnoreCase)
                                               || p.Name.Equals("Id", StringComparison.OrdinalIgnoreCase)))
            {
                if (!property.Name.Equals("LastModifiedDate", StringComparison.OrdinalIgnoreCase))
                {
                    property.SetMaxLength(36);
                }
                entityType.AddIndex(property);
            }
        }
        #region Disable Cascade Delete
        var cascadeFKs = modelBuilder.Model.GetEntityTypes()
        .SelectMany(t => t.GetForeignKeys())
        .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);
        foreach (var fk in cascadeFKs)
        {
            fk.DeleteBehavior = DeleteBehavior.Restrict;
        }
        #endregion
        modelBuilder.Entity<Audit>().Property(e => e.PrimaryKey).HasMaxLength(120);
        modelBuilder.Entity<Audit>().HasIndex(p => p.PrimaryKey);
		modelBuilder.Entity<Audit>().HasIndex(p => p.TraceId);
        modelBuilder.Entity<Audit>().HasIndex(p => p.DateTime);
		modelBuilder.Entity<UploadProcessorState>().HasQueryFilter(e => _authenticatedUser.Entity == Core.Constants.Entities.Default.ToUpper() || e.Entity == _authenticatedUser.Entity);      
   
        modelBuilder.Entity<AgentState>().Property(e => e.Email).HasMaxLength(100);
		modelBuilder.Entity<AgentState>().Property(e => e.Name).HasMaxLength(100);
		modelBuilder.Entity<AgentState>().Property(e => e.PhoneExtension).HasMaxLength(50);
		modelBuilder.Entity<AgentState>().Property(e => e.Status).HasMaxLength(50);
		modelBuilder.Entity<CustomerState>().Property(e => e.Email).HasMaxLength(100);
		modelBuilder.Entity<CustomerState>().Property(e => e.Name).HasMaxLength(100);
		modelBuilder.Entity<CustomerState>().Property(e => e.PhoneNumber).HasMaxLength(20);
		modelBuilder.Entity<CallState>().Property(e => e.Status).HasMaxLength(50);
		modelBuilder.Entity<TicketState>().Property(e => e.Priority).HasMaxLength(50);
		modelBuilder.Entity<TicketState>().Property(e => e.Status).HasMaxLength(50);
		
        modelBuilder.Entity<AgentState>().HasMany(t => t.CallList).WithOne(l => l.Agent).HasForeignKey(t => t.AgentId);
		modelBuilder.Entity<CustomerState>().HasMany(t => t.CallList).WithOne(l => l.Customer).HasForeignKey(t => t.CustomerId);
		modelBuilder.Entity<AgentState>().HasMany(t => t.TicketList).WithOne(l => l.Agent).HasForeignKey(t => t.AgentId);
		modelBuilder.Entity<CustomerState>().HasMany(t => t.TicketList).WithOne(l => l.Customer).HasForeignKey(t => t.CustomerId);
		
		modelBuilder.Entity<ApprovalRecordState>().HasQueryFilter(e => _authenticatedUser.Entity == Core.Constants.Entities.Default.ToUpper() || e.Entity == _authenticatedUser.Entity);
		modelBuilder.Entity<ApprovalState>().HasQueryFilter(e => _authenticatedUser.Entity == Core.Constants.Entities.Default.ToUpper() || e.Entity == _authenticatedUser.Entity);
		modelBuilder.Entity<ApproverSetupState>().HasQueryFilter(e => _authenticatedUser.Entity == Core.Constants.Entities.Default.ToUpper() || e.Entity == _authenticatedUser.Entity);
		modelBuilder.Entity<ApproverAssignmentState>().HasQueryFilter(e => _authenticatedUser.Entity == Core.Constants.Entities.Default.ToUpper() || e.Entity == _authenticatedUser.Entity);
		modelBuilder.Entity<ApprovalRecordState>().HasIndex(l => l.DataId);
		modelBuilder.Entity<ApprovalRecordState>().Property(e => e.DataId).HasMaxLength(36);
		modelBuilder.Entity<ApprovalRecordState>().Property(e => e.ApproverSetupId).HasMaxLength(36);
		modelBuilder.Entity<ApprovalRecordState>().HasIndex(l => l.ApproverSetupId);
		modelBuilder.Entity<ApprovalRecordState>().HasIndex(l => l.Status);
		modelBuilder.Entity<ApprovalRecordState>().Property(e => e.Status).HasMaxLength(450);
		modelBuilder.Entity<ApprovalState>().HasIndex(l => l.ApproverUserId);
		modelBuilder.Entity<ApprovalState>().HasIndex(l => l.Status);
		modelBuilder.Entity<ApprovalState>().HasIndex(l => l.EmailSendingStatus);
		modelBuilder.Entity<ApprovalState>().Property(e => e.ApproverUserId).HasMaxLength(36);
		modelBuilder.Entity<ApprovalState>().Property(e => e.Status).HasMaxLength(450);
		modelBuilder.Entity<ApprovalState>().Property(e => e.EmailSendingStatus).HasMaxLength(450);
		modelBuilder.Entity<ApproverSetupState>().Property(e => e.TableName).HasMaxLength(450);
		modelBuilder.Entity<ApproverSetupState>().Property(e => e.ApprovalType).HasMaxLength(450);
		modelBuilder.Entity<ApproverSetupState>().Property(e => e.EmailSubject).HasMaxLength(450);
		modelBuilder.Entity<ApproverSetupState>().Property(e => e.WorkflowName).HasMaxLength(450);
		modelBuilder.Entity<ApproverSetupState>().HasIndex(e => new { e.WorkflowName, e.ApprovalSetupType, e.TableName, e.Entity }).IsUnique();
		modelBuilder.Entity<ApproverAssignmentState>().Property(e => e.ApproverUserId).HasMaxLength(36);
		modelBuilder.Entity<ApproverAssignmentState>().Property(e => e.ApproverRoleId).HasMaxLength(36);
		modelBuilder.Entity<ApproverAssignmentState>().HasIndex(e => new { e.ApproverSetupId, e.ApproverUserId, e.ApproverRoleId }).IsUnique();
		modelBuilder.Entity<UploadProcessorState>().Property(e => e.FileType).HasMaxLength(20);
        modelBuilder.Entity<UploadProcessorState>().Property(e => e.Path).HasMaxLength(450);
        modelBuilder.Entity<UploadProcessorState>().Property(e => e.Status).HasMaxLength(20);
        modelBuilder.Entity<UploadProcessorState>().Property(e => e.Module).HasMaxLength(50);
        modelBuilder.Entity<UploadProcessorState>().Property(e => e.UploadType).HasMaxLength(50);      
        base.OnModelCreating(modelBuilder);
    }
}
