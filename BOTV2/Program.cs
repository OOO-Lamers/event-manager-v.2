using Microsoft.VisualBasic;
using System;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Requests;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

var botClient = new TelegramBotClient("7463883649:AAHyYkO3t6Z3U-mNp6lQBL82fMeYWV7s2Dg");

using CancellationTokenSource cts = new();

ReceiverOptions receiverOptions = new()
{
    AllowedUpdates = Array.Empty<UpdateType>()
};

botClient.StartReceiving(
    updateHandler: HandleUpdateAsync,
    pollingErrorHandler: HandlePollingErrorAsync,
    receiverOptions: receiverOptions,
    cancellationToken: cts.Token
);

User me = await botClient.GetMeAsync();

Console.WriteLine($"Start listening for @{me.Username}");
Console.ReadLine();

cts.Cancel();

async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
{

    if (update.Message is not { } message)
        return;

    if (message.Text is not { } messageText)
        return;

    var chatId = message.Chat.Id;
    Console.WriteLine($"Received a '{messageText}' message in chat {chatId}.");
    var action = messageText.Split(' ')[0] switch
    {
        "/schedule" => SendSchedule(botClient, message, cancellationToken),
        "/register" => SendRegister(botClient, message, cancellationToken),
        "/comment" => SendComment(botClient, message, cancellationToken),
        "/study" => SendStudy(botClient, message, cancellationToken),
        _ => SendWELCOM(botClient, message, cancellationToken)

    };
        Message sentMessage = await action;
        

    //Console.WriteLine($"Received a '{messageText}' message in chat {chatId}.");
    /*Message sentMessage = await botClient.SendTextMessageAsync(
        chatId: chatId, 
        text: "Приветсвую вас, Это бот-помощник по вопросам активностей и коммуникации. Что вас интересует?", 
        cancellationToken: cancellationToken,
        replyToMessageId: update.Message.MessageId,
        replyMarkup: new InlineKeyboardMarkup(InlineKeyboardButton.WithUrl(text: "Check sendMessage method", url: "https://core.telegram.org/bots/api#sendmessage"))
        );*/
 //Приветсвую вас, Это бот-помощник по вопросам активностей и коммуникации. Что вас интересует? parseMode: ParseMode.MarkdownV2;
 //disableNotification: false;
 //replyToMessageId: update.Message.MessageId;
 //replyMarkup: new InlineKeyboardMarkup(InlineKeyboardButton.WithUrl(text: "Check sendMessage method", url: "https://core.telegram.org/bots/api#sendmessage"));
 //cancellationToken: cancellationToken)
 static async Task<Message> SendSchedule(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
        {
        const string usage = "Расписание";

            return await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
               text: "Хотите записться на трениг/обучение?" +
            "\n" +
            "На какое меропритяи вы бы хотели записаться:\n" +
            "\n" +
            "1.Тренинг на тему эмоциональный интеллект\n" +
            "16.10 в 18:00\n" +
            "\n" +
            "2.Обучение на тему 'SCRUM'\n" +
            "20.10 в 19:00\n\n" + 
            "Для регистрации в мероприятии напишите следуещее: \n" +
            "'/register Номер мероприятия'",
                replyMarkup: new ReplyKeyboardRemove(),
                cancellationToken: cancellationToken);
        }
    static async Task<Message> SendWELCOM(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
    {
        const string usage = "Приветсвую вас, Это бот-помощник по вопросам активностей и коммуникации. Что вас интересует?\n" +
                             "Вот что я умею:\n" +
                             "\n" +
                             "/register   \n" +
                             "Записаться на треининнг/обучение\n" +
                             "\n" +
                             "/schedule   \n" +
                             "Узнать расписание активностей\n" +
                             "\n" +
                             "/comment    \n" +
                             "Оставить свой комментарий и предложенние  по развитию корпоративной культуры\n" +
                             "\n" +
                             "/study      \n" +
                             "Предложить обучение/запросить тему для обучение\n";

        return await botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: usage,
            replyMarkup: new ReplyKeyboardRemove(),
            cancellationToken: cancellationToken);
    }
    static async Task<Message> SendRegister(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
    {
        var messagePart = message.Text.Split(' ');
        var TrainingID = "";
        if
            (messagePart.Length > 1)
            {
            TrainingID = message.Text.Split(' ')[1];
        }
        if (TrainingID == "")
        {
            return await botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: "Вы не передали номер события");
        }
            
        
        var Response = await botClient.SendTextMessageAsync(
            chatId: 437637599,
            text: "Пользователь записался на событие " + TrainingID);

        return await botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: "Благодарим за проявленный интерес к мероприятию, Вы были записаны.",
            cancellationToken: cancellationToken);
    }
    static async Task<Message> SendComment(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
    {

        return await botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: "Хотите оставить свое предложение по развитию корпоративный культуры?\n" +
            "Напишите, пожалуйста, что бы Вы хотели улучшить/предложть/изменить", 
            cancellationToken: cancellationToken
            );
            
            //    chatId: message.Chat.Id,
           //     text: "You said:\n" + messageText,
           //     cancellationToken: cancellationToken);




    }
    static async Task<Message> SendStudy(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
    {

        return await botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: "Хотите предложить обучение/запросить тему для обучения?",
            cancellationToken: cancellationToken
            );



    }
    Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        var ErrorMessage = exception switch
        {
            ApiRequestException apiRequestException
                => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => exception.ToString()
        };

        Console.WriteLine(ErrorMessage);
        return Task.CompletedTask;
    }
}

static object GetChatMenuButtonRequest()
{
    throw new NotImplementedException();

}

async Task HandlePollingErrorAsync(ITelegramBotClient client, Exception exception, CancellationToken token)
{
    throw new NotImplementedException();
}
