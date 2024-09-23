using Microsoft.AspNetCore.Mvc.Rendering;
using Emite.CCM.Infrastructure.Data;
using Emite.CCM.Core.CCM;
using Emite.Common.Data;
using Emite.CCM.Web.Areas.Admin.Queries.Users;
using MediatR;
using Emite.CCM.Web.Areas.Admin.Queries.Roles;
using Emite.CCM.Application.Features.CCM.Report.Queries;
using System.Globalization;
using System.Reflection;

namespace Emite.CCM.Web.Service
{
    public class DropdownServices
    {
        private readonly ApplicationContext _context;
        private readonly IMediator _mediaTr;

        public DropdownServices(ApplicationContext context, IMediator mediaTr)
        {
            _context = context;
            _mediaTr = mediaTr;
        }
        public IEnumerable<SelectListItem> GetDropdownFromConstants<T>()
        {
            var fieldInfos = typeof(T).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
            var selectListItems = new List<SelectListItem>();
            foreach (var fi in fieldInfos)
            {
                // Check if the field is a constant, not init only, and of type string
                if (fi.IsLiteral && !fi.IsInitOnly && fi.FieldType == typeof(string))
                {
                    var value = fi.GetValue(null)?.ToString();
                    if (value != null) // Ensure the value is not null
                    {
                        selectListItems.Add(new SelectListItem { Text = value, Value = value });
                    }
                }
            }
            return selectListItems.OrderBy(l => l.Value);
        }
        public async Task<IEnumerable<SelectListItem>> GetRoleList()
        {
            var query = new GetRolesQuery()
            {
                PageSize = -1
            };
            return (await _mediaTr.Send(query)).Data.Select(l => new SelectListItem { Value = l.Name, Text = l.Name });
        }
        public IEnumerable<SelectListItem> QueryTypeList()
        {
            IList<SelectListItem> items = new List<SelectListItem>
            {
                //new SelectListItem { Text = Core.Constants.QueryType.QueryBuilder, Value = Core.Constants.QueryType.QueryBuilder, },
                new SelectListItem { Text = Core.Constants.QueryType.TSql, Value = Core.Constants.QueryType.TSql, }
            };
            return items;
        }
        public IEnumerable<SelectListItem> ReportChartTypeList()
        {
            IList<SelectListItem> items = new List<SelectListItem>
            {
                new SelectListItem { Text = Core.Constants.ReportChartType.HorizontalBar, Value = Core.Constants.ReportChartType.HorizontalBar, },
                new SelectListItem { Text = Core.Constants.ReportChartType.Pie, Value = Core.Constants.ReportChartType.Pie, },
                new SelectListItem { Text = Core.Constants.ReportChartType.Table, Value = Core.Constants.ReportChartType.Table, },
            };
            return items;
        }
        public IEnumerable<SelectListItem> DataTypeList()
        {
            IList<SelectListItem> items = new List<SelectListItem>
            {
                new SelectListItem { Text = Core.Constants.DataTypes.CustomDropdown, Value = Core.Constants.DataTypes.CustomDropdown, },
                new SelectListItem { Text = Core.Constants.DataTypes.Date, Value = Core.Constants.DataTypes.Date, },
                new SelectListItem { Text = Core.Constants.DataTypes.DropdownFromTable, Value = Core.Constants.DataTypes.DropdownFromTable, },
                new SelectListItem { Text = Core.Constants.DataTypes.Months, Value = Core.Constants.DataTypes.Months, },
                new SelectListItem { Text = Core.Constants.DataTypes.Years, Value = Core.Constants.DataTypes.Years, },
            };
            return items;
        }
        public IEnumerable<SelectListItem> GetDropdownFromCsv(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return Enumerable.Empty<SelectListItem>();
            }
            return value.Split(',')
                         .Select(option => new SelectListItem { Text = option.Trim(), Value = option.Trim() })
                         .ToList();
        }
        public IEnumerable<SelectListItem> GetYearsList(int yearsPrevious, int yearsAdvance)
        {
            List<SelectListItem> yearsList = new();
            int currentYear = DateTime.Now.Year;
            int startYear = currentYear - yearsPrevious;
            int endYear = currentYear + yearsAdvance;
            for (int year = startYear; year <= endYear; year++)
            {
                SelectListItem listItem = new()
                {
                    Text = year.ToString(),
                    Value = year.ToString(),
                };
                yearsList.Add(listItem);
            }
            return yearsList;
        }
        public IEnumerable<SelectListItem> GetMonthsList()
        {
            List<SelectListItem> monthsList = new();
            // Loop through the months and create SelectListItem objects for each month
            for (int month = 1; month <= 12; month++)
            {
                string monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month);
                SelectListItem listItem = new()
                {
                    Text = monthName,
                    Value = month.ToString() // Month number as the 'Value'
                };
                monthsList.Add(listItem);
            }
            return monthsList;
        }
        public async Task<IEnumerable<SelectListItem>> GetDropdownFromTableKeyValue(string tableKeyValue, string? filter)
        {
            var dropdownValues = await _mediaTr.Send(new GetDropdownValuesQuery(tableKeyValue, filter));
            List<SelectListItem> selectListItems = new();
            foreach (var item in dropdownValues)
            {
                string? key = item.ContainsKey("Key") ? item["Key"] : "";
                string? value = item.ContainsKey("Value") ? item["Value"] : "";
                selectListItems.Add(new SelectListItem
                {
                    Text = value,
                    Value = key
                });
            }
            return selectListItems;
        }
        public async Task<IList<Dictionary<string, string>>> GetReportList()
        {
            return await _mediaTr.Send(new GetReportListQuery());
        }
        public SelectList GetAgentList(string? id)
        {
            return _context.GetSingle<AgentState>(e => e.Id == id, new()).Result.Match(
                Some: e => new SelectList(new List<SelectListItem> { new() { Value = e.Id, Text = e.Id } }, "Value", "Text", e.Id),
                None: () => new SelectList(new List<SelectListItem>(), "Value", "Text")
            );
        }
        public SelectList GetCustomerList(string? id)
        {
            return _context.GetSingle<CustomerState>(e => e.Id == id, new()).Result.Match(
                Some: e => new SelectList(new List<SelectListItem> { new() { Value = e.Id, Text = e.Id } }, "Value", "Text", e.Id),
                None: () => new SelectList(new List<SelectListItem>(), "Value", "Text")
            );
        }

        public async Task<IEnumerable<SelectListItem>> GetUserList(string currentSelectedApprover, IList<string> allSelectedApprovers)
        {
            return (await _mediaTr.Send(new GetApproversQuery(currentSelectedApprover, allSelectedApprovers) { PageSize = -1 })).Data.Select(l => new SelectListItem { Value = l.Id, Text = l.Name });
        }
        public async Task<IEnumerable<SelectListItem>> GetRoleApproverList(string currentSelectedApprover, IList<string> allSelectedApprovers)
        {
            return (await _mediaTr.Send(new GetApproverRolesQuery(currentSelectedApprover, allSelectedApprovers) { PageSize = -1 })).Data.Select(l => new SelectListItem { Value = l.Id, Text = l.Name });
        }
    }
}
