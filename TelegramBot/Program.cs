using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Telegram.Bot;
using TelegramBot.Models;
using Microsoft.EntityFrameworkCore;
using System.Net;

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
        throw new NotImplementedException();
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
            /*
            if (message.Text.ToLower() == "/offensivemes")
            {
                /*
                InsultingData ins = new();
                ins = await Insult.GetInsultingObjectAsync("ru");
                using (ApplicationContext db = new ApplicationContext())
                {
                    // добавляем в бд
                    db.Users.Add(ins);
                    db.SaveChanges();
                    Console.WriteLine("Объекты успешно сохранены");

                    // получаем объекты из бд и выводим на консоль
                    var users = db.Users.ToList();
                    Console.WriteLine("Список объектов:");
                    foreach (InsultingData u in users)
                    {
                        Console.WriteLine($"{u.number} \n {u.language} \n {u.insult} \n {u.created} \n {u.shown} \n {u.createdby} \n {u.active} \n {u.comment}");
                    }
                }
                
                await botClient.SendTextMessageAsync(message.Chat.Id, $"{ins.insult}");
                return;
                
                string messa = await Insult.GetInsultMessageAsync("ru");
                await botClient.SendTextMessageAsync(message.Chat.Id, $"{messa}");
                return;
            }
            */
            if (message.Text.ToLower() == "/nasapicture")
            {
                // Вызываем метод через экземпляр класса
                Nasa obj = new Nasa();
                string url = await obj.GetAstronomyPictureAsync();

                Message messa = await botClient.SendPhotoAsync(message.Chat.Id, InputFile.FromUri(url), caption: $"<b>Фото дня в {obj.date}</b>. ", parseMode: ParseMode.Html, cancellationToken: cancellationToken);
                return;
            }
        }
    }
   

  
}

public class ApplicationContext : DbContext
{
    public DbSet<InsultingData> Users => Set<InsultingData>();
    public ApplicationContext() => Database.EnsureCreated();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=helloapp.db");
    }
}