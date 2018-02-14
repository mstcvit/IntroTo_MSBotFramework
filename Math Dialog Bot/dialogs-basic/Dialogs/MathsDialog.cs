﻿using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace dialogs_basic.Dialogs
{
    [Serializable]
    public class MathsDialog : IDialog<object>
    {
        // Bot Framework manages automatically persists per conversation data
        protected int number1 { get; set; }
        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedStart);

            return Task.CompletedTask;
        }
        public async Task MessageReceivedStart(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            await context.PostAsync("do you want to add, square root, or squared?");

            context.Wait(MessageReceivedOperationChoice);
        }

        public async Task MessageReceivedOperationChoice(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var message = await argument;

            if (message.Text.ToLower().Equals("add", StringComparison.InvariantCultureIgnoreCase))
            {
                await context.PostAsync("Provide number one:");
                context.Wait(MessageReceivedAddNumber1);
            }
            else if (message.Text.ToLower().Equals("square root", StringComparison.InvariantCultureIgnoreCase))
            {
                await context.PostAsync("Provide one number:");
                context.Wait(MessageReceivedSquareRoot);
            }
            else if (message.Text.ToLower().Equals("squared", StringComparison.InvariantCultureIgnoreCase))
            {
                await context.PostAsync("Provide one number:");
                context.Wait(MessageReceivedSquared);
            }
            else
            {
                context.Wait(MessageReceivedStart);
            }
        }

        public async Task MessageReceivedAddNumber1(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var numbers = await argument;
            // number one is persisted between messages automatically by bot framework dialog
            this.number1 = int.Parse(numbers.Text);
            await context.PostAsync("Provide number two:");

            context.Wait(MessageReceivedAddNumber2);
        }

        public async Task MessageReceivedAddNumber2(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var numbers = await argument;
            var number2 = int.Parse(numbers.Text);
            await context.PostAsync($"{this.number1} + {number2} is = {this.number1 + number2}");

            context.Wait(MessageReceivedStart);
        }

        public async Task MessageReceivedSquareRoot(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var number = await argument;
            var num = double.Parse(number.Text);

            await context.PostAsync($"square root of {num} is {Math.Sqrt(num)}");

            context.Wait(MessageReceivedStart);
        }

        public async Task MessageReceivedSquared(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var activity = await argument;
            var num = double.Parse(activity.Text);

            await context.PostAsync($"{num} squared is {num * num}");

            context.Wait(MessageReceivedStart);
        }
    }
}