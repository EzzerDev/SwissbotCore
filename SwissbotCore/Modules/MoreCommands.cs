﻿using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace SwissbotCore.Modules
{
    [DiscordCommandClass()]
    public class CommandClass : CommandModuleBase
    {
        [DiscordCommand("topic", commandHelp = "(PREFIX)topic", description = "Generates a random topic to keep the chat active")]
        public async Task Topic()
        {
            var typingChannel = Context.Channel;
            string[] topic = {"What's your favorite TV show?", "What are your hobby's?", "Are you working on anything exciting?", "What's your biggest fear?", "Who's your role model?", "What's your biggest regret?",
            "What's your dream job?", "Do you believe in aliens?", "what's the biggest thing you've hid from someone?", "What do you plan to do this weekend?", "What's something not many people know about you?",
            "What are you most passionate about?", "What makes you laugh out loud?", "What was your favorite thing to do as a kid?", "What is your favorite book?", "What is the worst present you have ever received and why",
            "What is the weirdest food combination you’ve ever tried?", "Who’s your favorite comedian?", "If you could be famous, would you want to? Why?", "Are you a cat person or a dog person?", "How did you hear about this sever",
            "Do you know a good joke, if so, what is it?"};
            Random rand = new Random();
            int index = rand.Next(topic.Length);
            await typingChannel.TriggerTypingAsync();
            await Context.Channel.SendMessageAsync(topic[index]);
        }

        [DiscordCommand("8ball", commandHelp = "(PREFIX)8ball <question>", description = "Replies to your question with the standard 8ball responses")]
        public async Task EightBall(params string[] args)
        {
            string qstn = string.Join(' ', args);
            var typingChannel = Context.Channel;
            if (string.IsNullOrEmpty(qstn))
            {
                await typingChannel.TriggerTypingAsync();
                await Context.Channel.SendMessageAsync("Please enter a parameter");
            }

            else
            {
                string[] answers = { "As I see it, yes.", "Ask again later.", "It is certain.", "It is decidedly so.", "Don't count on it.", "Better not tell you now.", "Concentrate and ask again.", " Cannot predict now.",
            "Most likely.", "My reply is no", "Yes.", "You may rely on it.", "Yes - definitely.", "Very doubtful.", "Without a doubt.", " My sources say no.", " Outlook not so good.", "Outlook good.", "Reply hazy, try again",
            "Signs point to yes"};

                Random rand = new Random();
                int index = rand.Next(answers.Length);
                await typingChannel.TriggerTypingAsync();
                await Context.Channel.SendMessageAsync(answers[index]);
            }
        }

        [DiscordCommand("roll", commandHelp = "(PREFIX)roll <input number>", description = "Replies with a random number between 0 and your input number")]
        public async Task Roll(int max_number)
        {
            var typingChannel = Context.Channel;
            Random r = new Random();
            int ans = r.Next(0, max_number);
            await typingChannel.TriggerTypingAsync();
            await Context.Channel.SendMessageAsync($"Number: {ans}");
        }
        public async Task<Embed> GetUserInfo(SocketUser user)
        {
            SocketGuildUser SGU = (SocketGuildUser)user;
              string nickState = "";
            if (SGU.Nickname != null)
                nickState = SGU.Nickname;
            EmbedBuilder eb = new EmbedBuilder()
            {
                Title = "I am a title!",
                Description = $"if you see this, somthings wrong",
            };
            eb.AddField($"Roles: [{SGU.Roles.Count}]", $"<@&{String.Join(separator: ">,\n <@&", values: SGU.Roles.Select(r => r.Id))}>");
            eb.AddField("ID:", $"{user.Id}")
                .AddField("Username", user)
                .AddField("Nickname?", nickState == "" ? "None" : nickState)
                .AddField("Status", user.Status)
                .AddField("Created at UTC", user.CreatedAt.UtcDateTime.ToString("r"))
                .AddField("Joined at UTC?", SGU.JoinedAt.HasValue ? SGU.JoinedAt.Value.UtcDateTime.ToString("r") : "No value :/")
                .WithAuthor(SGU)
                .WithColor(Color.DarkPurple)
                .WithTitle($"{user.Username}")
                .WithDescription($"Heres some stats for {user} <3")

            .WithCurrentTimestamp();

            return eb.Build();
        }
        [DiscordCommand("userinfo", commandHelp = "(PREFIX)userinfo <user>", description = "Shows information about a user such as when they joined")]
        //[Command("userinfo")]
        public async Task Userinfo(params string[] arg)
        {
            if (arg.Length == 0)
                await Context.Channel.SendMessageAsync("", false, await GetUserInfo(Context.Message.Author));
            else
                if(Context.Message.MentionedUsers.Any())
                    await Context.Channel.SendMessageAsync("", false, await GetUserInfo(Context.Message.MentionedUsers.First()));
        }
    }
}
