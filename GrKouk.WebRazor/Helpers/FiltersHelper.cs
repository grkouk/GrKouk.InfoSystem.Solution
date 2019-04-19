using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GrKouk.InfoSystem.Definitions;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GrKouk.WebRazor.Helpers
{
    public static class FiltersHelper
    {
        public static List<SelectListItem> GetPageSizeFiltersSelectList()
        {
            List<SelectListItem> filtersSelectList = new List<SelectListItem>
            {
                new SelectListItem() {Value = "1", Text = "1"},
                new SelectListItem() {Value = "5", Text = "5"},
                new SelectListItem() {Value = "10", Text = "10"},
                new SelectListItem() {Value = "20", Text = "20"},
                new SelectListItem() {Value = "50", Text = "50"},
                new SelectListItem() {Value = "100", Text = "100"}

            };
            return filtersSelectList;
        }
        public static List<SelectListItem> GetWarehouseItemNaturesList()
        {
            var materialNatures = Enum.GetValues(typeof(WarehouseItemNatureEnum))
                .Cast<WarehouseItemNatureEnum>()
                .Select(c => new SelectListItem()
                {
                    Value = ((int)c).ToString(),
                    Text = c.GetDescription()
                }).ToList();
            //Αλλαγή του στοιχείου 0 από απροσδιόριστο σε {Ολές οι φύσεις είδους}
            materialNatures[0].Text = "{All Natures}";
            return materialNatures;
        }
        public static List<SelectListItem> GetSeriesPayoffWayList()
        {
            var materialNatures = Enum.GetValues(typeof(WarehouseItemNatureEnum))
                .Cast<WarehouseItemNatureEnum>()
                .Select(c => new SelectListItem()
                {
                    Value = ((int)c).ToString(),
                    Text = c.GetDescription()
                }).ToList();
            //Αλλαγή του στοιχείου 0 από απροσδιόριστο σε {Ολές οι φύσεις είδους}
            materialNatures[0].Text = "{All }";
            return materialNatures;
        }
    }
}
