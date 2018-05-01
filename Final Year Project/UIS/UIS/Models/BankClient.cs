using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;

namespace UIS.Models
{
    public class BankClient
    {
        private string baseUrl = "http://localhost:8024/api/";
        public IEnumerable<TempBankModel> findAll()
        {
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(baseUrl);
                client.DefaultRequestHeaders.Accept.Add(

                    new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.GetAsync("default1/1302004").Result;
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsAsync<IEnumerable<TempBankModel>>().Result;

                }

                else
                {
                    return null;
                }


            }
            catch
            {
                return null;
            }
        }

    }
}