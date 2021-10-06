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
using DSharpPlus;
using DSharpPlus.CommandsNext.Builders;
using DSharpPlus.Entities;
using DSharpPlus.Exceptions;
using System.Threading.Tasks;

namespace EndlessNetworkBot.Commands.Moderation
{
    public class KickCommand : BaseCommandModule
    {
        [Command("Kick")]
        [Description("Espelle un utente dal server.")]
        [RequireRoles(RoleCheckMode.Any, "Staff")]
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
                    Title = Utente.Mention + " è stato kickato da " + command.Member.Mention,
                    Description = "Nessun motivo specificato."
                };
                DiscordEmbedBuilder blacklist = new DiscordEmbedBuilder
                {
                    Title = "Kick su Discord",
                    Description = Utente.Mention + " è stato kickato da " + command.Member.Mention,
                    Color = new DiscordColor("#7289da")
                };

                if (motivoFinale != null) embed.Description = "Motivo: " + motivoFinale;

                await Utente.RemoveAsync(motivoFinale);
                await command.Message.DeleteAsync();
                await command.Channel.SendMessageAsync(embed);
                await command.Guild.GetChannel(885121887368257539).SendMessageAsync(blacklist);
            }
            catch (NotFoundException)
            {
                DiscordEmbedBuilder embed = new DiscordEmbedBuilder
                {
                    Color = new DiscordColor("#CD0000"),
                    Description = "Utente non trovato",
                };

                await command.Channel.SendMessageAsync(embed);
            }
            catch (BadRequestException)
            {
                DiscordEmbedBuilder embed = new DiscordEmbedBuilder
                {
                    Color = new DiscordColor("#CD0000"),
                    Description = "Impossibile kickare l'utente (BadRequestException)",
                };

                await command.Channel.SendMessageAsync(embed);
            }
            catch (ServerErrorException)
            {
                DiscordEmbedBuilder embed = new DiscordEmbedBuilder
                {
                    Color = new DiscordColor("#CD0000"),
                    Description = "Errore interno del server, impossibile kickare l'utente",
                };

                await command.Channel.SendMessageAsync(embed);
            }
            catch (UnauthorizedException)
            {
                DiscordEmbedBuilder embed = new DiscordEmbedBuilder
                {
                    Color = new DiscordColor("#CD0000"),
                    Description = "Non ho i permessi per kickare quell'utente",
                };

                await command.Channel.SendMessageAsync(embed);
            }
        }
    }
}
