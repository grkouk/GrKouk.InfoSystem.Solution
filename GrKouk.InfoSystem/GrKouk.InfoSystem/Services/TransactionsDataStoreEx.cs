﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using GrKouk.InfoSystem.Dtos;
using GrKouk.InfoSystem.Models;
using Newtonsoft.Json;
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace GrKouk.InfoSystem.Services
{
    public class TransactionsDataStoreEx : ITransactionDataStore<FinDiaryTransactionDto, FinDiaryTransactionCreateDto, FinDiaryTransactionModifyDto>
    {
        private static ISettings AppSettings => CrossSettings.Current;

        public static string WebApiBaseAddress
        {
            get => AppSettings.GetValueOrDefault(nameof(WebApiBaseAddress), "http://api.villakoukoudis.com/api");
            set => AppSettings.AddOrUpdateValue(nameof(WebApiBaseAddress), value);
        }
        private readonly string BaseUrl = WebApiBaseAddress + "/transactions";

        public async Task<IEnumerable<FinDiaryTransactionDto>> GetItemsAsync()
        {
            var httpClient = new HttpClient();

            try
            {
                var uri = new Uri(BaseUrl);

                var response = await httpClient.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var jsonContent = await response.Content.ReadAsStringAsync();
                    var itemsList = JsonConvert.DeserializeObject<List<FinDiaryTransactionDto>>(jsonContent);
                    return itemsList;

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

        public async Task<IEnumerable<FinDiaryTransactionDto>> GetItemsInPeriodAsync(DateTime fromDate, DateTime toDate)
        {
            var httpClient = new HttpClient();

            try
            {
                var fDate = fromDate.ToString("yyyy-MM-dd");
                var tDate = toDate.ToString("yyyy-MM-dd") + " 11:59:00 PM";
                //var tDate = toDate.ToString();
                var uri = new Uri(BaseUrl + $"/transactionsinperiod?fromdate={fDate}&todate={tDate}");

                var response = await httpClient.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var jsonContent = await response.Content.ReadAsStringAsync();
                    var itemsList = JsonConvert.DeserializeObject<List<FinDiaryTransactionDto>>(jsonContent);
                    return itemsList;
                }
                else
                {
                    var e = new Exception(response.ToString());
                    throw e;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                // return null;
                throw e;
            }
        }

        public Task<IEnumerable<SearchListItem>> GetItemsSearchListAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> AddItemAsync(FinDiaryTransactionCreateDto item)
        {
            var httpClient = new HttpClient();
            var jsonItem = JsonConvert.SerializeObject(item);
            try
            {

                var uri = new Uri(BaseUrl);
                HttpContent httpContent = new StringContent(jsonItem);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                httpClient.DefaultRequestHeaders.Accept
                    .Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await httpClient.PostAsync(uri, httpContent);

                if (response.IsSuccessStatusCode)
                {
                    return await Task.FromResult(true);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return await Task.FromResult(false);
                //throw;
            }
            return await Task.FromResult(false);
        }

        public async Task<FinDiaryTransactionDto> AddItemAsync2(FinDiaryTransactionCreateDto item)
        {
            var httpClient = new HttpClient();
            var jsonItem = JsonConvert.SerializeObject(item);
            try
            {

                var uri = new Uri(BaseUrl);
                HttpContent httpContent = new StringContent(jsonItem);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                httpClient.DefaultRequestHeaders.Accept
                    .Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await httpClient.PostAsync(uri, httpContent);

                if (response.IsSuccessStatusCode)
                {
                    var jsonContent = await response.Content.ReadAsStringAsync();
                    var newItem = JsonConvert.DeserializeObject<FinDiaryTransactionDto>(jsonContent);
                    return newItem;
                }
                else
                {
                    var e = new Exception(response.ToString());
                    throw e;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                //return await Task.FromResult(null);
                throw e;
            }

        }

        public async Task<FinDiaryTransactionModifyDto> ModifyItemAsync(int id, FinDiaryTransactionModifyDto item)
        {
            var httpClient = new HttpClient();
            var jsonItem = JsonConvert.SerializeObject(item);
            try
            {

                var uri = new Uri(BaseUrl + $"/{id}");
                HttpContent httpContent = new StringContent(jsonItem);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                httpClient.DefaultRequestHeaders.Accept
                    .Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // var response1 = await httpClient.PostAsync(uri, httpContent);
                var response = await httpClient.PutAsync(uri, httpContent);

                if (response.IsSuccessStatusCode)
                {
                    var jsonContent = await response.Content.ReadAsStringAsync();
                    var newItem = JsonConvert.DeserializeObject<FinDiaryTransactionModifyDto>(jsonContent);
                    return newItem;
                }
                else
                {
                    var e = new Exception(response.ToString());
                    throw e;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                //return await Task.FromResult(null);
                throw e;
            }
        }

        public async Task<bool> DeleteItemAsync(int id)
        {
            var httpClient = new HttpClient();

            try
            {
                var uri = new Uri(BaseUrl + $"/{id}");

                httpClient.DefaultRequestHeaders.Accept
                    .Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = await httpClient.DeleteAsync(uri);
                if (response.IsSuccessStatusCode)
                {

                    return true;
                }
                else
                {
                    var e = new Exception(response.ToString());
                    throw e;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                //return await Task.FromResult(null);
                throw e;
            }
        }


        public async Task<SearchListItem> GetCategoryIdOfTransactorsLastTransactionAsync(int transactorId)
        {
            var list = await GetItemsAsync();

            var trDto = list.OrderByDescending(p => p.TransactionDate)
                       .FirstOrDefault(p => p.TransactorId == transactorId);
            if (trDto != null)
            {
                var cat = new SearchListItem { Key = trDto.FinTransCategoryId, Name = trDto.FinTransCategoryName };
                return cat;
            }

            return null;
        }

        public async Task<SearchListItem> GetCostCentreIdOfTransactorsLastTransactionAsync(int transactorId)
        {
            var list = await GetItemsAsync();

            var trDto = list.OrderByDescending(p => p.TransactionDate)
                .FirstOrDefault(p => p.TransactorId == transactorId);
            if (trDto != null)
            {
                var cat = new SearchListItem { Key = trDto.CostCentreId, Name = trDto.CostCentreName };
                return cat;
            }

            return null;
        }

        public async Task<SearchListItem> GetRevenueCentreIdOfTransactorsLastTransactionAsync(int transactorId)
        {
            var list = await GetItemsAsync();

            var trDto = list.OrderByDescending(p => p.TransactionDate)
                .FirstOrDefault(p => p.TransactorId == transactorId);
            if (trDto != null)
            {
                var cat = new SearchListItem { Key = trDto.RevenueCentreId, Name = trDto.RevenueCentreName };
                return cat;
            }

            return null;
        }
    }
}
