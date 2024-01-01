using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TelegramBot.Models
{  
    public class Nasa
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Explanation { get; set; }
        public string Url { get; set; }
        public string Date { get; set; }

        private readonly HttpClient _httpClient = new HttpClient();
        private readonly string _apiKey = "TSLCHxDtNa5s4U9BW0iQA8G2ynOqiT9uT9VdvGjf";


        public async Task<string> GetAstronomyPictureAsync()
        {
            string apiUrl = "https://api.nasa.gov/planetary/apod";

            string requestUrl = $"{apiUrl}?date={GetRandomDateString()}&api_key={_apiKey}";

            try
            {
                using (HttpResponseMessage response = await _httpClient.GetAsync(requestUrl))
                {
                    response.EnsureSuccessStatusCode();

                    var json = await response.Content.ReadAsStringAsync();
                    var nasaData = JsonConvert.DeserializeObject<Nasa>(json);

                    UpdateProperties(nasaData);

                    return nasaData.Url;
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error making API request: {ex.Message}");
                return null;
            }
        }

        private void UpdateProperties(Nasa newData)
        {
            Title = newData.Title;
            Explanation = newData.Explanation;
            Url = newData.Url;
            Date = newData.Date;
        }

        private static string GetRandomDateString()
        {
            Random random = new Random();
            int year = random.Next(2000, 2023);
            int month = random.Next(1, 13);
            int maxDaysInMonth = DateTime.DaysInMonth(year, month);
            int day = random.Next(1, maxDaysInMonth + 1);

            DateTime randomDate = new DateTime(year, month, day);
            return randomDate.ToString("yyyy-MM-dd");
        }
    }
}