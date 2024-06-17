﻿using Microsoft.VisualBasic;
using System;
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
                text: usage,
                replyMarkup: new ReplyKeyboardRemove(),
                cancellationToken: cancellationToken);
        }
    static async Task<Message> SendWELCOM(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
    {
        const string usage = "Приветсвую вас, Это бот-помощник по вопросам активностей и коммуникации. Что вас интересует?\n" +
                             "Вот что я умею:\n" +
                             "/      - send custom keyboard\n" +
                             "/      - remove custom keyboard\n" +
                             "/      - send a photo\n" +
                             "/      - request location or contact\n" +
                             "/      - send keyboard with Inline Query";

        return await botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: usage,
            replyMarkup: new ReplyKeyboardRemove(),
            cancellationToken: cancellationToken);
    }
    static async Task<Message> SendRegister(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
    { 

        return await botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: "Приветсвую вас, Это бот-помощник по вопросам активностей и коммуникации. Что вас интересует?",
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
