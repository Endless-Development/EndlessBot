// THIS CODE IS PART OF ENDLESS NETWORK BOT (made by Kheeto)
// License:
/* 
 * Copyright (c) 2021 Kheeto

Permission is hereby granted, free of charge, to any person obtaining a copy of this software 
and associated documentation files (the "Software"), to deal in the Software without restriction, 
including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, 
and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, 
subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial 
portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND 
NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR 
OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN 
CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;

namespace EndlessNetworkBot.Commands.Moderation
{
    public class AggiungiCommand : BaseCommandModule
    {
        Bot bot; // this will be set when the class will be instantiated, and will be used to get the voice channels

        public AggiungiCommand(Bot bot)
        {
            this.bot = bot;
        }

        [Command("Aggiungi")]
        [Description("Comando per aggiungere un membro al vocale custom.")]
        public async Task CommandAsync(CommandContext command, DiscordMember member)
        {
            #region Embeds

            DiscordEmbedBuilder addedEmbed = new DiscordEmbedBuilder
            {
                Title = "Fatto",
                Description = member.Mention + " è stato aggiunto al tuo vocale custom",
                Color = new DiscordColor("#00FF00"),
            };

            DiscordEmbedBuilder notFoundEmbed = new DiscordEmbedBuilder
            {
                Title = "Errore",
                Description = "Non ho trovato quell'utente, probabilmente la menzione non è valida.",
                Color = new DiscordColor("#CD0000"),
            };
            notFoundEmbed.AddField("Uso corretto del comando", "!aggiungi <menzione>");

            DiscordEmbedBuilder notInChannelEmbed = new DiscordEmbedBuilder
            {
                Title = "Non sei in un vocale",
                Description = "Prima devi creare un vocale custom.",
                Color = new DiscordColor("#CD0000"),
            };

            DiscordEmbedBuilder ownerNotFoundEmbed = new DiscordEmbedBuilder
            {
                Title = "Creatore del vocale non trovato",
                Description = "E' un bug del bot.",
                Color = new DiscordColor("#CD0000"),
            };

            DiscordEmbedBuilder notChannelOwnerEmbed = new DiscordEmbedBuilder
            {
                Title = "Non sei il creatore del vocale",
                Description = "Solo il creatore del vocale può aggiungere delle persone.",
                Color = new DiscordColor("#CD0000"),
            };

            DiscordEmbedBuilder inPublicChannelEmbed = new DiscordEmbedBuilder
            {
                Title = "Il vocale in cui sei non è custom",
                Description = "Per usare questo comando devi essere in un vocale custom.",
                Color = new DiscordColor("#CD0000"),
            };

            #endregion

            // if the user is not in a voice channel, he will get an error embed
            if (command.Member.VoiceState == null || command.Member.VoiceState.Channel == null)
            {
                await command.Message.RespondAsync(notInChannelEmbed);
                return;
            }

            // if the user is in a default voice channel, not a custom one, he will get an error embed
            if (!bot.customChannels.ContainsKey(command.Member.VoiceState.Channel.Id))
            {
                await command.Message.RespondAsync(inPublicChannelEmbed);
                return;
            }

            // if the user is not the owner of the custom voice channel, he will get an error
            DiscordMember channelOwner;
            if (!bot.customChannels.TryGetValue(command.Member.VoiceState.Channel.Id, out channelOwner))
            {
                await command.Message.RespondAsync(ownerNotFoundEmbed);
                return;
            }
            
            // if the member who is executing the command is not the channel owner, he will get an error
            if(channelOwner != command.Member)
            {
                await command.Message.RespondAsync(notChannelOwnerEmbed);
                return;
            }
            
            // get the custom voice channel
            DiscordChannel channel = command.Guild.GetChannel(command.Member.VoiceState.Channel.Id);
        }
    }
}
