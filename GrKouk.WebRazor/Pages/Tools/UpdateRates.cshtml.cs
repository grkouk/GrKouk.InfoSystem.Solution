using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GrKouk.InfoSystem.Domain.Shared;
using GrKouk.WebApi.Data;
using Newtonsoft.Json;

namespace GrKouk.WebRazor.Pages.Tools
{
    public class UpdateRatesModel : PageModel
    {
        private readonly GrKouk.WebApi.Data.ApiDbContext _context;

        public UpdateRatesModel(GrKouk.WebApi.Data.ApiDbContext context)
        {
            _context = context;
        }

        public IList<ExchangeRate> ExchangeRate { get;set; }

        public async Task OnGetAsync()
        {
            string BaseUrl = "https://api.exchangeratesapi.io/latest?symbols=USD,BGN";
            var httpClient = new HttpClient();

            try
            {
                var uri = new Uri(BaseUrl);

                var response = await httpClient.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var jsonContent = await response.Content.ReadAsStringAsync();
                    var rateResponse = JsonConvert.DeserializeObject<RateResponse>(jsonContent);
                    if (rateResponse !=null)
                    {
                        foreach (var rateItem in rateResponse.Rates)
                        {
                            int currencyId;
                            var currencyCode = rateItem.Key;
                            var cur = await _context.Currencies.FirstOrDefaultAsync(p => p.Code == currencyCode);
                            if (cur != null)
                            {
                                currencyId = cur.Id;
                                var rateToAdd = new ExchangeRate
                                {
                                    CurrencyId = currencyId,
                                    ClosingDate = rateResponse.Date,
                                    Rate = (decimal) rateItem.Value
                                };
                               await _context.ExchangeRates.AddAsync(rateToAdd);
                            }
                           
                        }

                        await _context.SaveChangesAsync();
                        
                    }

                }

                

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
               
                //throw;
            }

        }
    }

    public class RateResponse
    {
        [JsonProperty("base")]
        public string BaseCurrecy { get; set; }

        [JsonProperty("date")]
        public DateTime Date { get; set; }

        [JsonProperty("rates")]
        public Dictionary<string, double> Rates { get; set; }
    }
}
