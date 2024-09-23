using Emite.CCM.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Emite.CCM.Core.CCM;
using Emite.CCM.ExcelProcessor.Models;
using Emite.CCM.ExcelProcessor.Helper;


namespace Emite.CCM.ExcelProcessor.CustomValidation
{
    public static class AgentValidator
    {
        public static async Task<Dictionary<string, object?>>  ValidatePerRecordAsync(ApplicationContext context, Dictionary<string, object?> rowValue)
        {
			string errorValidation = "";
            var email = rowValue[nameof(AgentState.Email)]?.ToString();
			if (!string.IsNullOrEmpty(email))
			{
				var emailMaxLength = 100;
				if (email.Length > emailMaxLength)
				{
					errorValidation += $"Email should be less than {emailMaxLength} characters.;";
				}
			}
			var name = rowValue[nameof(AgentState.Name)]?.ToString();
			if (!string.IsNullOrEmpty(name))
			{
				var nameMaxLength = 100;
				if (name.Length > nameMaxLength)
				{
					errorValidation += $"Name should be less than {nameMaxLength} characters.;";
				}
			}
			var phoneExtension = rowValue[nameof(AgentState.PhoneExtension)]?.ToString();
			if (!string.IsNullOrEmpty(phoneExtension))
			{
				var phoneExtensionMaxLength = 50;
				if (phoneExtension.Length > phoneExtensionMaxLength)
				{
					errorValidation += $"Phone Extension should be less than {phoneExtensionMaxLength} characters.;";
				}
			}
			var status = rowValue[nameof(AgentState.Status)]?.ToString();
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
