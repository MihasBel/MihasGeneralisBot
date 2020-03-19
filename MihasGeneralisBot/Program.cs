using System;
using System.Threading;
using MihaZupan;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.ReplyMarkups;
using System.Text.RegularExpressions;


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


                botClient.OnCallbackQuery += Bot_OnCallbackQuery;
                botClient.OnMessage += Bot_OnMessage;
                botClient.StartReceiving();
                Thread.Sleep(10000);

            Console.ReadKey();
            
        }
        private static async void Bot_OnCallbackQuery(object sender, CallbackQueryEventArgs e)
        {
            string buttonText = e.CallbackQuery.Data;
            string name = $"{e.CallbackQuery.From.FirstName} {e.CallbackQuery.From.LastName}";
            Console.WriteLine($"{name} press the button {buttonText}");
            botClient.OnMessage -= Bot_OnMessage;
            botClient.OnMessage += Bot_OnMessageAfterButton;
            await botClient.SendTextMessageAsync(e.CallbackQuery.From.Id, @$"Now please enter the counter number and indications as shown in the example.  
                        Example:12345 045566.9"); 
        }

        private static async void Bot_OnMessageAfterButton(object sender, MessageEventArgs e)
        {
            var message = e?.Message?.Text;
            Regex regex = new Regex(@"^\d+\s\d+$");
            string text;
            if (regex.IsMatch(message))
            {

                Indicators indicators = new Indicators();
                string[] messageCut = message.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);                
                indicators.counterIndicators = messageCut[1];
                indicators.counterNumber = messageCut[0];
                DateTime dateTime = DateTime.Today;
                indicators.dateOfIndicate = dateTime.ToShortDateString();
                indicators.sendIndicators("D://indicators");
                text =
                    @$"You enter the counter number:'{messageCut[0]}' and indications:{messageCut[1]}";
                await botClient.SendTextMessageAsync(e.Message.Chat, text);
                botClient.OnMessage -= Bot_OnMessageAfterButton;
                botClient.OnMessage += Bot_OnMessage;
            } 
            else
            {
                text =
                    @$"The indicate does not match the format. Please enter the counter number and indications as shown in the example.  
                        example:12345 045566.9";
                await botClient.SendTextMessageAsync(e.Message.Chat, text);
            } 

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
                    text = @"You wont to start indicate?";
                    var inlineKeyboard = new InlineKeyboardMarkup(new[]
                    {
                        InlineKeyboardButton.WithCallbackData("start indicate"),

                    });
                    await botClient.SendTextMessageAsync(e.Message.Chat, text, replyMarkup: inlineKeyboard);
                    break;
                default:
                    text =
                        @"Please enter /start and follow the instructions";
                    await botClient.SendTextMessageAsync(e.Message.Chat, text);
                    break;

            }
        }
    }
}
