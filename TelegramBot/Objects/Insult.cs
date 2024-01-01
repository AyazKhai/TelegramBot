using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;

namespace TelegramBot.Models
{
    public class InsultingData
    {
         public int Id { get; set; }
        public string? Number { get; set; }
        public string? Language { get; set; }
        public string? Insult { get; set; }
        public string? Created { get; set; }
        public string? Shown { get; set; }
        public string? CreatedBy { get; set; }
        public string? Active { get; set; }
        public string? Comment { get; set; }
    }

    public class Insult
    {
        private static readonly HttpClient HttpClient = new HttpClient();
        private const string InsultApiUrl = "https://evilinsult.com/generate_insult.php?type=json";

        public static async Task<InsultingData?> GetInsultingObjectAsync(string lang)
        {
            try
            {
                var url = $"{InsultApiUrl}&lang={lang}";

                using var response = await HttpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<InsultingData>(json);
                }

                Console.WriteLine($"Error getting data. Status code: {response.StatusCode}");
                return null;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HTTP request failed: {ex.Message}");
                return null;
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"JSON deserialization failed: {ex.Message}");
                return null;
            }
        }

        public static async Task<string?> GetInsultMessageAsync(string lang)
        {
            try
            {
                var url = $"{InsultApiUrl}&lang={lang}";

                using var response = await HttpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var messageData = JsonConvert.DeserializeObject<InsultingData>(json);
                    return messageData?.Insult;
                }

                Console.WriteLine($"Error getting data. Status code: {response.StatusCode}");
                return null;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HTTP request failed: {ex.Message}");
                return null;
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"JSON deserialization failed: {ex.Message}");
                return null;
            }
        }
    }
}