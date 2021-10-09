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
    public class BlacklistCommand : BaseCommandModule
    {
        [Command("Blacklist")]
        [Description("Comando per scrivere un ban in blacklist.")]
        [RequireRoles(RoleCheckMode.Any, "Staff")]
        public async Task CommandAsync(CommandContext command)
        {
            #region Embeds

            DiscordEmbedBuilder userNameEmbed = new DiscordEmbedBuilder
            {
                Title = "Qual è il nome del player bannato? (il nome in gioco)",
                Description = "Rispondi in 60 secondi",
                Color = new DiscordColor("#CD0000"),
            };

            DiscordEmbedBuilder stafferNameEmbed = new DiscordEmbedBuilder
            {
                Title = "Qual è il nome dello staffer? (il nome in gioco)",
                Description = "Rispondi in 60 secondi",
                Color = new DiscordColor("#CD0000"),
            };

            DiscordEmbedBuilder durationEmbed = new DiscordEmbedBuilder
            {
                Title = "Quando dura il ban?",
                Description = "Rispondi in 60 secondi",
                Color = new DiscordColor("#CD0000"),
            };

            DiscordEmbedBuilder reasonEmbed = new DiscordEmbedBuilder
            {
                Title = "Perchè è stato bannato?",
                Description = "Specifica un motivo entro 60 secondi",
                Color = new DiscordColor("#CD0000"),
            };

            #endregion

            // Customizable values
            string reason;
            string duration;
            string user;
            string moderator;

            // default embed, it will be automatically updated
            DiscordEmbedBuilder embed = new DiscordEmbedBuilder()
            {
                Title = "Ban su Minecraft",
            };

            // the name of the user who has been banned
            #region User Name

            // ask for the name of the user who has been banned
            await command.Message.DeleteAsync();
            DiscordMessage askForUserName = await command.Channel.SendMessageAsync(userNameEmbed);

            var userName = await command.Client.GetInteractivity().WaitForMessageAsync(msg => msg.Channel == command.Channel
            && msg.Channel.Guild.GetMemberAsync(msg.Author.Id).Result == command.Member).ConfigureAwait(false);

            if (!userName.TimedOut)
            {
                // if moderator responded to the question the user name will be set as the embed title
                user = userName.Result.Content;
                await userName.Result.DeleteAsync();
                await askForUserName.DeleteAsync();
            }
            else
            {
                // if moderator didn't respond the command will be cancelled
                await command.Channel.SendMessageAsync(command.Member.Mention + ", Non hai scritto niente in 60 secondi. Comando Cancellato.");
                await askForUserName.DeleteAsync();

                return;
            }

            #endregion

            // the name of the staffer/moderator who banned the user
            #region Staffer Name

            // ask for the name of the moderator who banned the user
            DiscordMessage askForStafferName = await command.Channel.SendMessageAsync(stafferNameEmbed);

            var stafferName = await command.Client.GetInteractivity().WaitForMessageAsync(msg => msg.Channel == command.Channel
            && msg.Channel.Guild.GetMemberAsync(msg.Author.Id).Result == command.Member).ConfigureAwait(false);

            if (!stafferName.TimedOut)
            {
                // if moderator responded to the question the user name will be set as the embed title
                moderator = stafferName.Result.Content;
                await stafferName.Result.DeleteAsync();
                await askForStafferName.DeleteAsync();
            }
            else
            {
                // if moderator didn't respond the command will be cancelled
                await command.Channel.SendMessageAsync(command.Member.Mention + ", Non hai scritto niente in 60 secondi. Comando Cancellato.");
                await askForStafferName.DeleteAsync();

                return;
            }

            #endregion

            // customizable embed ban reason field/why was the player banned?
            #region Ban Reason

            // ask why the user has been banned
            DiscordMessage askForBanReason = await command.Channel.SendMessageAsync(reasonEmbed);

            var banReason = await command.Client.GetInteractivity().WaitForMessageAsync(msg => msg.Channel == command.Channel
            && msg.Channel.Guild.GetMemberAsync(msg.Author.Id).Result == command.Member).ConfigureAwait(false);

            if (!banReason.TimedOut)
            {
                // if moderator responded to the question the ban reason will be set and will be added to the embed later
                reason = banReason.Result.Content;
                await banReason.Result.DeleteAsync();
                await askForBanReason.DeleteAsync();
            }
            else
            {
                // if moderator didn't respond the command will be cancelled
                await command.Channel.SendMessageAsync(command.Member.Mention + ", Non hai scritto niente in 60 secondi. Comando Cancellato.");
                await askForBanReason.DeleteAsync();

                return;
            }

            #endregion

            // customizable embed ban duration field/for how long was the player banned?
            #region Ban Duration

            // ask for the name of the user who has been banned
            DiscordMessage askForBanDuration = await command.Channel.SendMessageAsync(durationEmbed);

            var banDuration = await command.Client.GetInteractivity().WaitForMessageAsync(msg => msg.Channel == command.Channel
            && msg.Channel.Guild.GetMemberAsync(msg.Author.Id).Result == command.Member).ConfigureAwait(false);

            if (!banDuration.TimedOut)
            {
                // if moderator responded with the ban duration, it will be set and added to the embed later on
                duration = banDuration.Result.Content;
                await banDuration.Result.DeleteAsync();
                await askForBanDuration.DeleteAsync();
            }
            else
            {
                // if moderator didn't respond the command will be cancelled
                await command.Channel.SendMessageAsync(command.Member.Mention + ", Non hai scritto niente in 60 secondi. Comando Cancellato.");
                await askForBanDuration.DeleteAsync();

                return;
            }

            #endregion

            embed.AddField("Utente Bannato", user, true);
            embed.AddField("Staffer", moderator, true);
            embed.AddField("Motivo del ban", reason, true);
            embed.AddField("Durata del ban", duration, true);

            await command.Guild.GetChannel(885121887368257539).SendMessageAsync(embed);
        }
    }
}
