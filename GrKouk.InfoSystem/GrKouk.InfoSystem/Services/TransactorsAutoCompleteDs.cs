﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using GrKouk.InfoSystem.Domain.Shared;
using GrKouk.InfoSystem.Models;
using Newtonsoft.Json;
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace GrKouk.InfoSystem.Services
{
    public class TransactorsAutoCompleteDs:IAutoCompleteDataSource<Transactor>
    {
        private static ISettings AppSettings => CrossSettings.Current;

        public static string WebApiBaseAddress
        {
            get => AppSettings.GetValueOrDefault(nameof(WebApiBaseAddress), "http://api2.villakoukoudis.com/api");
            set => AppSettings.AddOrUpdateValue(nameof(WebApiBaseAddress), value);
        }
        private readonly string BaseUrl = WebApiBaseAddress + "/transactors";
      
        public async Task<IEnumerable<SearchListItem>> GetSearchListItemsAsync()
        {
            
            var httpClient = new HttpClient();
            try
            {
                var uri = new Uri(BaseUrl + "/transactorssearchlist");
               
                var response = await httpClient.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var jsonContent = await response.Content.ReadAsStringAsync();
                    var itemsSearchList = JsonConvert.DeserializeObject<List<SearchListItem>>(jsonContent);
                    return itemsSearchList;

                }
                
                var emptyItems = new List<SearchListItem>();
                return emptyItems;

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                //return null;
                throw(e);
            }
        }

        public async Task<IList<SearchListItem>> GetSearchListItemsLightAsync()
        {
            var httpClient = new HttpClient();
            try
            {
                var uri = new Uri(BaseUrl + "/transactorssearchlist");

                var response = await httpClient.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var jsonContent = await response.Content.ReadAsStringAsync();
                    var itemsSearchList = JsonConvert.DeserializeObject<IList<SearchListItem>>(jsonContent);
                    return itemsSearchList;

                }

                var emptyItems = new List<SearchListItem>();
                return emptyItems;

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                //return null;
                throw (e);
            }
        }
    }
}
