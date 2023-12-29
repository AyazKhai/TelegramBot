using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBot.Models
{
    public class AstoData
    {
        public static string Title { get; set; }
        public string Explanation { get; set; }
        public string Url { get; set; }

    }
    public class Nasa
    {
        public string date = GetRandomDateString();
        public async Task<string> GetAstronomyPictureAsync()
        {

            string apiKey = "TSLCHxDtNa5s4U9BW0iQA8G2ynOqiT9uT9VdvGjf";
            string apiUrl = "https://api.nasa.gov/planetary/apod";

            string requestUrl = $"{apiUrl}?date={date}&api_key={apiKey}";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(requestUrl);
                    response.EnsureSuccessStatusCode();

                    var json = await response.Content.ReadAsStringAsync();
                    var MessageData = JsonConvert.DeserializeObject<AstoData>(json);
                    string url = MessageData.Url;
                    return url;
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine($"Ошибка при запросе к API: {ex.Message}");
                    return null;
                }
            }
        }
        public static string GetRandomDateString()
        {
            Random random = new Random();

            // Получаем случайный год между 1900 и 2100
            int year = random.Next(2000, 2023);

            // Получаем случайный месяц от 1 до 12
            int month = random.Next(1, 13);

            // Получаем максимальное количество дней в текущем месяце
            int maxDaysInMonth = DateTime.DaysInMonth(year, month);

            // Получаем случайный день от 1 до максимального количества дней в месяце
            int day = random.Next(1, maxDaysInMonth + 1);

            // Создаем и возвращаем строку с форматированной датой без времени
            DateTime randomDate = new DateTime(year, month, day);
            string formattedDate = randomDate.ToString("yyyy-MM-dd");
            return formattedDate;
        }





    }
}
