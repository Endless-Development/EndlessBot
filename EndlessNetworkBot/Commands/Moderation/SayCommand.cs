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

namespace EndlessNetworkBot.Commands.Moderation
{
    public class SayCommand : BaseCommandModule
    {
        [Command("Say")]
        [Description("Ripete quello che è stato scritto dall'utente")]
        [RequirePermissions(DSharpPlus.Permissions.ManageMessages)]
        public async Task Comando(CommandContext command, [Description("Testo da ripetere")] params string[] Testo)
        {
            if (Testo.Length == 0)
            {
                DiscordEmbedBuilder embed = new DiscordEmbedBuilder
                {
                    Color = new DiscordColor(Bot.instance.GetConfig().Result.Command_BadColor),
                    Description = "Scrivi anche un testo da ripetere",
                };

                await command.RespondAsync(embed);
                return;
            }
            string messaggio = null;
            foreach (string arg in Testo)
            {
                messaggio = messaggio + arg + " ";
            }
            await command.Channel.SendMessageAsync(messaggio);
            await command.Message.DeleteAsync();
        }
    }
}
