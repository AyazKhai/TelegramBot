using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace TelegramBot.Models
{
    public class InsultingData
    {
        public string? number { get; set; }
        public string? language { get; set; }
        public string? insult { get; set; }
        public string? created { get; set; }
        public string? shown { get; set; }
        public string? createdby { get; set; }
        public string? active { get; set; }
        public string? comment { get; set; }
    }
    public class Insult
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        public static async Task<InsultingData> GetInsultingObjectAsync(string lang)
        {
            try
            {
                var url = $"https://evilinsult.com/generate_insult.php?lang={lang}&type=json";

                using (var response = await _httpClient.GetAsync(url))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        return JsonConvert.DeserializeObject<InsultingData>(json);
                    }
                    else
                    {
                        return null;
                    }
                }
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

        public static async Task<string> GetInsultMessageAsync(string lang)
        {
            try
            {
                var url = $"https://evilinsult.com/generate_insult.php?lang={lang}&type=json";

                using (var response = await _httpClient.GetAsync(url))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        var messageData = JsonConvert.DeserializeObject<InsultingData>(json);
                        return messageData.insult;
                    }
                    else
                    {
                        return $"Error getting data. Status code: {response.StatusCode}";
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                return $"HTTP request failed: {ex.Message}";
            }
            catch (JsonException ex)
            {
                return $"JSON deserialization failed: {ex.Message}";
            }
        }
    }
}
