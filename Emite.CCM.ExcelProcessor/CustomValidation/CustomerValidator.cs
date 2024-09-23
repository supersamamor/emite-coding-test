using Emite.CCM.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Emite.CCM.Core.CCM;
using Emite.CCM.ExcelProcessor.Models;
using Emite.CCM.ExcelProcessor.Helper;


namespace Emite.CCM.ExcelProcessor.CustomValidation
{
    public static class CustomerValidator
    {
        public static async Task<Dictionary<string, object?>>  ValidatePerRecordAsync(ApplicationContext context, Dictionary<string, object?> rowValue)
        {
			string errorValidation = "";
            var email = rowValue[nameof(CustomerState.Email)]?.ToString();
			if (!string.IsNullOrEmpty(email))
			{
				var emailMaxLength = 100;
				if (email.Length > emailMaxLength)
				{
					errorValidation += $"Email should be less than {emailMaxLength} characters.;";
				}
			}
			
			var name = rowValue[nameof(CustomerState.Name)]?.ToString();
			if (!string.IsNullOrEmpty(name))
			{
				var nameMaxLength = 100;
				if (name.Length > nameMaxLength)
				{
					errorValidation += $"Name should be less than {nameMaxLength} characters.;";
				}
			}
			var phoneNumber = rowValue[nameof(CustomerState.PhoneNumber)]?.ToString();
			if (!string.IsNullOrEmpty(phoneNumber))
			{
				var phoneNumberMaxLength = 20;
				if (phoneNumber.Length > phoneNumberMaxLength)
				{
					errorValidation += $"Phone Number should be less than {phoneNumberMaxLength} characters.;";
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
