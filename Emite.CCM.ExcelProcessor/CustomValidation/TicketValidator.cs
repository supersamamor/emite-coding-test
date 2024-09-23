using Emite.CCM.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Emite.CCM.Core.CCM;
using Emite.CCM.ExcelProcessor.Models;
using Emite.CCM.ExcelProcessor.Helper;


namespace Emite.CCM.ExcelProcessor.CustomValidation
{
    public static class TicketValidator
    {
        public static async Task<Dictionary<string, object?>>  ValidatePerRecordAsync(ApplicationContext context, Dictionary<string, object?> rowValue)
        {
			string errorValidation = "";
            var agentId = rowValue[nameof(TicketState.AgentId)]?.ToString();
			var agent = await context.Agent.Where(l => l.Id == agentId).AsNoTracking().IgnoreQueryFilters().FirstOrDefaultAsync();
			if(agent == null) {
				errorValidation += $"Agent Id is not valid.; ";
			}
			else
			{
				rowValue[nameof(TicketState.AgentId)] = agent?.Id;
			}
			
			var customerId = rowValue[nameof(TicketState.CustomerId)]?.ToString();
			var customer = await context.Customer.Where(l => l.Id == customerId).AsNoTracking().IgnoreQueryFilters().FirstOrDefaultAsync();
			if(customer == null) {
				errorValidation += $"Customer Id is not valid.; ";
			}
			else
			{
				rowValue[nameof(TicketState.CustomerId)] = customer?.Id;
			}
			if (!string.IsNullOrEmpty(customerId))
			{
				var customerIdMaxLength = 50;
				if (customerId.Length > customerIdMaxLength)
				{
					errorValidation += $"Customer Id should be less than {customerIdMaxLength} characters.;";
				}
			}
			
			var priority = rowValue[nameof(TicketState.Priority)]?.ToString();
			if (!string.IsNullOrEmpty(priority))
			{
				var priorityMaxLength = 50;
				if (priority.Length > priorityMaxLength)
				{
					errorValidation += $"Priority should be less than {priorityMaxLength} characters.;";
				}
			}
			
			var status = rowValue[nameof(TicketState.Status)]?.ToString();
			if (!string.IsNullOrEmpty(status))
			{
				var statusMaxLength = 50;
				if (status.Length > statusMaxLength)
				{
					errorValidation += $"Status should be less than {statusMaxLength} characters.;";
				}
			}
			
			
			if (!string.IsNullOrEmpty(errorValidation))
			{
				throw new Exception(errorValidation);
			}
            return rowValue;
        }
			
		public static Dictionary<string, HashSet<int>> DuplicateValidation(List<ExcelRecord> records)
		{
			List<string> listOfKeys = new()
			{
																																								
			};
			return listOfKeys.Count > 0 ? DictionaryHelper.FindDuplicateRowNumbersPerKey(records, listOfKeys) : new Dictionary<string, HashSet<int>>();
		}
    }
}
