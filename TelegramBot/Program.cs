using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Telegram.Bot;
using TelegramBot.Models;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text;
using System.Web;
using TelegramBot;
using Microsoft.Extensions.Configuration;

public class Program
{

    static void Main(string[] args)
    {
        var botClient = new TelegramBotClient("6438897987:AAGaho7IoA3P3KDHoqgyBWVCV-nKuxo43gc");
        botClient.StartReceiving(Update, Error);

       
        Console.ReadKey();
    }

    private static Task Error(ITelegramBotClient client, Exception exception, CancellationToken token)
    {
        Console.WriteLine($"Error: {exception.Message}");
        return Task.CompletedTask;
    }


    public static async Task Update(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        bool randomnumSwitcher = false;
        var message = update.Message;
        if (message != null)
        {
            if (message.Text.ToLower() == "/start")
            {
                await botClient.SendTextMessageAsync(message.Chat.Id, $"<b>Приветствую вас!!!</b> ", parseMode: ParseMode.Html);
                return;
            }
            
            if (message.Text.ToLower() == "/offensivemes")
            {

                InsultingData ins = await Insult.GetInsultingObjectAsync("ru");

                await Console.Out.WriteLineAsync(ins.Insult);
                await botClient.SendTextMessageAsync(message.Chat.Id, $"{ins.Insult}");
                return;
                
               
            }
            
            if (message.Text.ToLower().StartsWith("/nasapicture"))
            {
                // Вызываем метод через экземпляр класса
                Nasa obj = new Nasa();
                string url = await obj.GetAstronomyPictureAsync();

                var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();

                var options = optionsBuilder.UseSqlite("Data Source=nasadb.db").Options;

                using (ApplicationContext db = new ApplicationContext(options))
                {
                    db.NasaDB.Add(obj);
                    db.SaveChanges();
                    Console.WriteLine($"сохранено{obj.Title}\n");
                }

                await Console.Out.WriteLineAsync(obj.Title);
                await Console.Out.WriteLineAsync(obj.Explanation);
                await Console.Out.WriteLineAsync("///");
                await Console.Out.WriteLineAsync(Translator.Translatesentence(obj.Title,"ru"));
                await Console.Out.WriteLineAsync(Translator.TranslateText(obj.Explanation,"ru"));
                await Console.Out.WriteLineAsync();

                

                try
                {
                    Message messa = await botClient.SendPhotoAsync(
                    message.Chat.Id, InputFile.FromUri(url),
                    caption: $"<b>Фото дня NASA \"{obj.Title}\"({Translator.TranslateText(obj.Title, "ru")}) в {obj.Date}</b>",
                    parseMode: ParseMode.Html,
                    cancellationToken: cancellationToken);
                    await botClient.SendTextMessageAsync(message.Chat.Id, $"{Translator.TranslateText(obj.Explanation, "ru")} ", parseMode: ParseMode.Html);
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("message caption is too long"))
                    {
                        // Если ошибка связана с длинной подписью, разбиваем сообщение на более короткие части
                        int maxCaptionLength = 4096; // Максимальная длина подписи в Telegram
                        var captionChunks = SplitCaption(obj.Explanation, maxCaptionLength);

                        foreach (var chunk in captionChunks)
                        {
                            await botClient.SendTextMessageAsync(message.Chat.Id, chunk);
                        }
                    }
                    else
                    {
                        // Если это другая ошибка, выводим её
                        Console.WriteLine($"Error: {ex.Message}");
                        await botClient.SendTextMessageAsync(message.Chat.Id, $"{ex.Message}");
                    }
                    static string[] SplitCaption(string caption, int maxLength)
                    {
                        // Разбиваем текст на части, учитывая максимальную длину
                        int index = 0;
                        var chunks = new System.Collections.Generic.List<string>();

                        while (index < caption.Length)
                        {
                            int length = Math.Min(maxLength, caption.Length - index);
                            chunks.Add(caption.Substring(index, length));
                            index += length;
                        }

                        return chunks.ToArray();
                    }
                }
                

                return;
            }
        }
    }


}

public class ApplicationContext : DbContext
{
    public DbSet<User> Users { get; set; } = null!;
    public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
    {
        Database.EnsureCreated();
    }

    public DbSet<Nasa> NasaDB { get; set; } = null!;
    public DbSet<InsultingData> InsultDB { get; set; } = null;
}

public class User
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public int Age { get; set; }
}