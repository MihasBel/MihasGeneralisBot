using System;
using System.Threading;
using MihaZupan;
using Telegram.Bot;
using Telegram.Bot.Args;


namespace MihasGeneralisBot
{
    class Program
    {
        private static ITelegramBotClient botClient;
        static void Main(string[] args)
        {
            var proxy = new HttpToSocks5Proxy("213.136.89.190", 38816);
            proxy.ResolveHostnamesLocally = true;
            botClient = new TelegramBotClient("1083411004:AAFGzhy_5CIJkpnyT1TcHjRsOKY2472TEgc", proxy) ;
            var me = botClient.GetMeAsync().Result;
            Console.WriteLine(
              $"Hello, World! I am user {me.Id} and my name is {me.FirstName}."
            );
            

            
                botClient.OnMessage += Bot_OnMessage;
                botClient.StartReceiving();
                Thread.Sleep(10000);

            Console.ReadKey();
            
        }
        private static async void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            var message = e?.Message?.Text;
            if (message == null)
                return;
            
                Console.WriteLine($"Received a text message '{message}' in chat {e.Message.Chat.Id}.");
            Indicators indicators = new Indicators();
            
            string[] messageCut = message.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            string text;
            switch (messageCut[0])
            {
                case "/start":
                    text =
                        @"Для начала внесения показателей счетчика введите номер счетчика и показания, как в примере, без скобок  
                        пример:indicators (номер счетчика) (показания)";
                    await botClient.SendTextMessageAsync(e.Message.Chat, text);
                    break;
                case "indicators":
                    indicators.counterIndicators = messageCut[2];
                    indicators.counterNumber = messageCut[1];
                    DateTime dateTime = DateTime.Today;
                    indicators.dateOfIndicate = dateTime.ToShortDateString();
                    indicators.sendIndicators();
                    text =
                        @$"вы ввели номер счетчика:'{messageCut[1]}' и показания:{messageCut[2]}";
                    await botClient.SendTextMessageAsync(e.Message.Chat, text);
                    
                    break;
                default:
                    text =
                        @"Пожалуйста введите /start и следуете инструкции";
                    await botClient.SendTextMessageAsync(e.Message.Chat, text);
                    break;

            }
        }
    }
}
