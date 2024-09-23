using Emite.Common.Services.Shared.Interfaces;
using Emite.Common.Services.Shared.Models.Mail;
using Emite.CCM.Core.CCM;
using Emite.CCM.Infrastructure.Data;
using Emite.CCM.ExcelProcessor.Services;
using LanguageExt;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Quartz;
using Emite.CCM.ExcelProcessor.CustomTemplate;
using Emite.CCM.ExcelProcessor.CustomProcessor;

namespace Emite.CCM.Scheduler.Jobs
{
    [DisallowConcurrentExecution]
    public class BatchUploadJob : IJob
    {
        private readonly ApplicationContext _context;
		private readonly ILogger<BatchUploadJob> _logger;
		private readonly string? _uploadPath;
		private readonly IMailService _emailSender;
		private readonly ExcelService _excelService;
        private readonly IdentityContext _identityContext;
        public BatchUploadJob(ApplicationContext context, ILogger<BatchUploadJob> logger, IConfiguration configuration, IMailService emailSender, ExcelService excelService, IdentityContext identityContext)
		{
			_context = context;
			_logger = logger;
			_uploadPath = configuration.GetValue<string>("UsersUpload:UploadFilesPath");
			_emailSender = emailSender;
			_excelService = excelService;
            _identityContext = identityContext;

        }
        public async Task Execute(IJobExecutionContext context)
        {
            await ProcessBatchUploadAsync();
        }
        private async Task ProcessBatchUploadAsync()
        {
            var uploadProcessorList = await _context.UploadProcessor.Where(l => l.Status == Core.Constants.FileUploadStatus.Pending).IgnoreQueryFilters().AsNoTracking()
                .OrderBy(l => l.CreatedDate).ToListAsync();
            var exceptionFilePath = "";
            foreach (var item in uploadProcessorList)
            {
                try
                {
                    //Tag Start Date/Time
                    item.SetStart();
                    _context.Update(item);
                    await _context.UpdateBatchRecordAsync(item.CreatedBy, item);
                    //Start Processing
                    exceptionFilePath = await ValidateBatchUpload(item.Module, item.Path, item.CreatedBy!);
                    if (string.IsNullOrEmpty(exceptionFilePath))
                    {
                        item.SetDone();
                    }
                    else
                    {
                        item.SetFailed(exceptionFilePath, "Error from the file.");
                    }
                    _context.Update(item);
                    await _context.UpdateBatchRecordAsync(item.CreatedBy, item);
                }
                catch (Exception ex)
                {
					_context.DetachAllTrackedEntities();
                    _logger.LogError(ex, @"ProcessBatchUploadAsync Error Message : {Message} / StackTrace : {StackTrace}", ex.Message, ex.StackTrace);
                    item.SetFailed("", ex.Message);
                    _context.Update(item);
                    await _context.UpdateBatchRecordAsync(item.CreatedBy, item);
                }
                try
                {
                    if (!string.IsNullOrEmpty(exceptionFilePath))
                    {
                        var email = (await _identityContext.Users.Where(l => l.Id == item.CreatedBy).AsNoTracking().FirstOrDefaultAsync())!.Email!;
                        await SendValidatedBatchUploadFile(email, item.Module, exceptionFilePath);
                    }
                }
                catch (Exception ex)
                {               
                    _logger.LogError(ex, @"ProcessBatchUploadAsync Error Message : {Message} / StackTrace : {StackTrace}", ex.Message, ex.StackTrace);
                }              
            }
        }
		private async Task<string?> ValidateBatchUpload(string module, string path, string processedByUserId)
        {
            string? exceptionFilePath = null;   
            switch (module)
            {
                case nameof(AgentState):
					var agentImportResult = await _excelService.ImportAsync<AgentState>(path);
					if (agentImportResult.IsSuccess)
					{
						await _context.AddRangeAsync(agentImportResult.SuccessRecords);
					}
					else
					{
						exceptionFilePath = ExcelService.UpdateExistingExcelValidationResult<AgentState>(agentImportResult.FailedRecords, _uploadPath + "/BatchUploadErrors", path);
					}
					break;
				case nameof(CustomerState):
					var customerImportResult = await _excelService.ImportAsync<CustomerState>(path);
					if (customerImportResult.IsSuccess)
					{
						await _context.AddRangeAsync(customerImportResult.SuccessRecords);
					}
					else
					{
						exceptionFilePath = ExcelService.UpdateExistingExcelValidationResult<CustomerState>(customerImportResult.FailedRecords, _uploadPath + "/BatchUploadErrors", path);
					}
					break;
				case nameof(CallState):
					var callImportResult = await _excelService.ImportAsync<CallState>(path);
					if (callImportResult.IsSuccess)
					{
						await _context.AddRangeAsync(callImportResult.SuccessRecords);
					}
					else
					{
						exceptionFilePath = ExcelService.UpdateExistingExcelValidationResult<CallState>(callImportResult.FailedRecords, _uploadPath + "/BatchUploadErrors", path);
					}
					break;
				case nameof(TicketState):
					var ticketImportResult = await _excelService.ImportAsync<TicketState>(path);
					if (ticketImportResult.IsSuccess)
					{
						await _context.AddRangeAsync(ticketImportResult.SuccessRecords);
					}
					else
					{
						exceptionFilePath = ExcelService.UpdateExistingExcelValidationResult<TicketState>(ticketImportResult.FailedRecords, _uploadPath + "/BatchUploadErrors", path);
					}
					break;
				
				case nameof(SampleTableState): //Sample Only For Custom Processing
					var unitImportResult = await _excelService.ImportAsync<SampleTableState>(path);
					if (unitImportResult.IsSuccess)
					{
						await SampleTableCustomProcessor.Process(_context, unitImportResult.SuccessRecords, processedByUserId);
					}
					else
					{
						exceptionFilePath = ExcelService.UpdateExistingExcelValidationResult<SampleTableState>(unitImportResult.FailedRecords, _uploadPath + "\\BatchUploadErrors", path);
					}
					break;
                default: break;
            }           
            return exceptionFilePath;
        }
        
        public async Task SendValidatedBatchUploadFile(string email, string module, string exceptionFilePath)
        {
            string wordToRemove = "State";
            if (module.EndsWith(wordToRemove))
            {
                module = module[..^wordToRemove.Length];
            }
            string subject = $"Batch Upload - " + module;
            string message = GenerateEmailBody();
            var emailRequest = new MailRequest()
            {
                Subject = subject,
                Body = message,
                Attachments = new List<string>() { exceptionFilePath },
                To = email
            };           
            await _emailSender.SendAsync(emailRequest);
        }
        private static string GenerateEmailBody()
        {
            string str = "<span style='font-size:10pt; font-family:Arial;'> ";
            str += "Your uploaded file has been failed on processing. Please see attached file for the validation remarks.";
            str += "<br />";           
            str += "</span> ";
            return str;
        }
    }
}
