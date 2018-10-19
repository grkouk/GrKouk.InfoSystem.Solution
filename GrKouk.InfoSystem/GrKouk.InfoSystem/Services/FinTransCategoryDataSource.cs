using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using GrKouk.InfoSystem.Domain.Shared;
using GrKouk.InfoSystem.Models;
using Newtonsoft.Json;
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace GrKouk.InfoSystem.Services
{
    public class FinTransCategoryDataSource : IDataStore<FinTransCategory, FinTransCategory, FinTransCategory>
    {
        private static ISettings AppSettings => CrossSettings.Current;
        public static string WebApiBaseAddress
        {
            get => AppSettings.GetValueOrDefault(nameof(WebApiBaseAddress), "http://api.villakoukoudis.com/api");
            set => AppSettings.AddOrUpdateValue(nameof(WebApiBaseAddress), value);
        }

        private string BaseUrl = WebApiBaseAddress + "/FinTransCategories";
        public async Task<IEnumerable<FinTransCategory>> GetItemsAsync()
        {
            var httpClient = new HttpClient();

            try
            {
                var uri = new Uri(BaseUrl);

                var response = await httpClient.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var jsonContent = await response.Content.ReadAsStringAsync();
                    var facilitiesList = JsonConvert.DeserializeObject<List<FinTransCategory>>(jsonContent);
                    return facilitiesList;

                }

                return null;

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
                //throw;
            }
        }

        public Task<IEnumerable<FinTransCategory>> GetItemsInPeriodAsync(DateTime fromDate, DateTime toDate)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<SearchListItem>> GetItemsSearchListAsync()
        {
            throw new NotImplementedException();
        }

        public  Task<bool> AddItemAsync(FinTransCategory item)
        {
            throw new NotImplementedException();
        }

        public Task<FinTransCategory> AddItemAsync2(FinTransCategory item)
        {
            throw new NotImplementedException();
        }

        public Task<FinTransCategory> ModifyItemAsync(int id, FinTransCategory item)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteItemAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
