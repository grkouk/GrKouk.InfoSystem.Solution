using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GrKouk.WebRazor.Helpers
{
    public class DateFilterDates
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string FilterName { get; set; }

    }
    public static class DateFilter
    {
        public static List<SelectListItem> GetDateFiltersSelectList()
        {
            List<SelectListItem> datePeriods = new List<SelectListItem>
            {
                new SelectListItem() {Value = "CURMONTH", Text = "Τρέχων Μήνας"},
                new SelectListItem() {Value = "30DAYS", Text = "30 Ημέρες"},
                new SelectListItem() {Value = "60DAYS", Text = "60 Ημέρες"},
                new SelectListItem() {Value = "90DAYS", Text = "90 Ημέρες"},
                new SelectListItem() {Value = "360DAYS", Text = "360 Ημέρες"},
                new SelectListItem() {Value = "PREMONTH", Text = "Προηγ.Μήνας"},
                new SelectListItem() {Value = "CURYEAR", Text = "Τρέχων Ετος"},
                new SelectListItem() {Value = "LASTYEAR", Text = "Προηγ. Ετος"},
                new SelectListItem() {Value = "ALL", Text = "{All}"}
            };
            return datePeriods;
        }

        public static DateFilterDates GetDateFilterDates(string dateFilterName)
        {

            DateFilterDates dateFilter = new DateFilterDates();
            dateFilter.FilterName = dateFilterName;

            switch (dateFilterName)
            {
                case "CURMONTH":
                    dateFilter.FromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    dateFilter.ToDate = DateTime.Now;
                    break;
                case "PREMONTH":
                    var m = DateTime.Now.Month;
                    var y = DateTime.Now.Year;
                    if (m>1)
                    {
                        m = m - 1;

                    }
                    else
                    {
                        m = 12;
                        y = y - 1;
                    }
                    dateFilter.FromDate = new DateTime(y, m, 1);
                    dateFilter.ToDate = new DateTime(y, m, DateTime.DaysInMonth(y, m));
                    break;
                case "30DAYS":
                    dateFilter.ToDate = DateTime.Now;
                    dateFilter.FromDate = dateFilter.ToDate.AddDays(-30);
                    break;
                case "60DAYS":
                    dateFilter.ToDate = DateTime.Now;
                    dateFilter.FromDate = dateFilter.ToDate.AddDays(-60);
                    break;
                case "90DAYS":
                    dateFilter.ToDate = DateTime.Now;
                    dateFilter.FromDate = dateFilter.ToDate.AddDays(-90);
                    break;
                case "360DAYS":
                    dateFilter.ToDate = DateTime.Now;
                    dateFilter.FromDate = dateFilter.ToDate.AddDays(-360);
                    break;
                case "CURYEAR":
                    dateFilter.FromDate = new DateTime(DateTime.Now.Year, 1, 1);
                    dateFilter.ToDate = DateTime.Now;
                    break;
                case "LASTYEAR":
                    var ty = DateTime.Now.Year;
                    ty = ty - 1;
                    dateFilter.FromDate = new DateTime(ty, 1, 1);
                    dateFilter.ToDate = new DateTime(ty, 12, DateTime.DaysInMonth(ty, 12));
                    break;
                case "ALL":
                    dateFilter.FromDate = new DateTime(1966, 8, 1);
                    dateFilter.ToDate =  DateTime.MaxValue;
                    break;
                default:
                    dateFilter.FromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    dateFilter.ToDate = DateTime.Now;
                    dateFilter.FilterName = "CURMONTH";
                    break;

            }
            return dateFilter;
        }
    }
}
