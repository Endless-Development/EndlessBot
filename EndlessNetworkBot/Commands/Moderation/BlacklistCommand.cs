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
        [RequirePermissions(DSharpPlus.Permissions.ManageMessages)]
        public async Task Comando(CommandContext command)
        {
            string reason;
            string duration;

            DiscordEmbedBuilder embed = new DiscordEmbedBuilder()
            {
                
            };

            #region User Name

            // ask for the name of the user who has been banned
            await command.Message.DeleteAsync();
            DiscordMessage askForUserName = await command.Channel.SendMessageAsync("Qual è il nome del player è stato bannato?");

            var userName = await command.Client.GetInteractivity().WaitForMessageAsync(msg => msg.Channel == command.Channel).ConfigureAwait(false);

            if (!userName.TimedOut)
            {
                // if moderator responded to the question the user name will be set as the embed title
                embed.Title = userName.Result.Content;
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

            #region Ban Reason

            // ask why the user has been banned
            await command.Message.DeleteAsync();
            DiscordMessage askForBanReason = await command.Channel.SendMessageAsync("Per quale motivo è stato bannato?");

            var banReason = await command.Client.GetInteractivity().WaitForMessageAsync(msg => msg.Channel == command.Channel).ConfigureAwait(false);

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

            #region Ban Duration

            // ask for the name of the user who has been banned
            await command.Message.DeleteAsync();
            DiscordMessage askForBanDuration = await command.Channel.SendMessageAsync("Qual è il nome del player è stato bannato?");

            var banDuration = await command.Client.GetInteractivity().WaitForMessageAsync(msg => msg.Channel == command.Channel).ConfigureAwait(false);

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

            embed.AddField("Motivo del ban", reason, true);
            embed.AddField("Durata del ban", duration, true);

            await command.Channel.SendMessageAsync(embed);
        }
    }
}
