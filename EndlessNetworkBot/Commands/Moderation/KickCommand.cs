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

using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Exceptions;
using System.Threading.Tasks;

namespace EndlessNetworkBot.Commands.Moderation
{
    public class KickCommand : BaseCommandModule
    {
        [Command("Kick")]
        [Description("Espelle un utente dal server.")]
        [RequirePermissions(DSharpPlus.Permissions.KickMembers)]
        public async Task CommandAsync(CommandContext command, [Description("Utente da Kickare")] DiscordMember Utente, [Description("Motivo del Ban")] params string[] Motivo)
        {
            string motivoFinale = null;
            if (Motivo.Length > 0)
            {
                foreach (string arg in Motivo)
                {
                    motivoFinale = motivoFinale + arg + " ";
                }
            }
            try
            {
                DiscordEmbedBuilder embed = new DiscordEmbedBuilder
                {
                    Color = new DiscordColor(Bot.instance.GetConfig().Result.Command_GoodColor),
                    Title = Utente.Mention + " è stato kickato dal server",
                    Description = "Nessun motivo specificato."
                };

                if (motivoFinale != null) embed.Description = motivoFinale;

                await Utente.BanAsync(0, motivoFinale);
                await command.RespondAsync(embed);
            }
            catch (NotFoundException)
            {
                DiscordEmbedBuilder embed = new DiscordEmbedBuilder
                {
                    Color = new DiscordColor(Bot.instance.GetConfig().Result.Command_BadColor),
                    Description = "Utente non trovato",
                };

                await command.RespondAsync(embed);
            }
            catch (BadRequestException)
            {
                DiscordEmbedBuilder embed = new DiscordEmbedBuilder
                {
                    Color = new DiscordColor(Bot.instance.GetConfig().Result.Command_BadColor),
                    Description = "Impossibile kickare l'utente (BadRequestException)",
                };

                await command.RespondAsync(embed);
            }
            catch (ServerErrorException)
            {
                DiscordEmbedBuilder embed = new DiscordEmbedBuilder
                {
                    Color = new DiscordColor(Bot.instance.GetConfig().Result.Command_BadColor),
                    Description = "Errore interno del server, impossibile kickare l'utente",
                };

                await command.RespondAsync(embed);
            }
            catch (UnauthorizedException)
            {
                DiscordEmbedBuilder embed = new DiscordEmbedBuilder
                {
                    Color = new DiscordColor(Bot.instance.GetConfig().Result.Command_BadColor),
                    Description = "Non ho i permessi per kickare quell'utente",
                };

                await command.RespondAsync(embed);
            }
        }
    }
}
