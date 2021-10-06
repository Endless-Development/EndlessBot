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
using System.Collections.Generic;
using System.Text;
using DSharpPlus.Entities;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Entities;
using DSharpPlus.CommandsNext.Converters;

namespace EndlessNetworkBot
{
    // custom help formatter for the bot
    public class HelpFormatter : BaseHelpFormatter
    {
        DiscordEmbedBuilder embed = new DiscordEmbedBuilder();
        StringBuilder stringBuilder = new StringBuilder();

        // main embed
        public HelpFormatter(CommandContext ctx) : base(ctx)
        {
            embed.Color = new DiscordColor("#CD0000");
            embed.Title = "Endless Network";
            embed.Description = "Lista dei comandi del server.";
        }
        
        public override CommandHelpMessage Build()
        {
            return new CommandHelpMessage(embed: embed);
        }

        // single command
        public override BaseHelpFormatter WithCommand(Command command)
        {
            // embed
            embed.Title = "Comando \"" + command.Name + "\" - Help";
            embed.Description = command.Description;
            stringBuilder.AppendLine("Comando: " + command.Name + command.Description);

            // command arguments things
            string argomenti = "```";
            int index = 0;

            // adds a comma between every arguments of the comma
            foreach (CommandOverload overl in command.Overloads)
            {

                foreach (CommandArgument arg in overl.Arguments)
                {

                    index++;
                    if (overl.Arguments.Count == 1 || overl.Arguments.Count == index)
                    {
                        argomenti += arg.Description + ". ";
                    }
                    else
                    {
                        argomenti += arg.Description + ", ";
                    }

                }

            }

            argomenti += "```";

            if (argomenti == "``````") argomenti = "Nessun argomento richiesto";

            // finally adds the arguments to the embed
            embed.AddField("Argomenti del Comando", argomenti);
            stringBuilder.AppendLine("Argomenti del Comando" + argomenti);

            return this;
        }

        // all commands 
        public override BaseHelpFormatter WithSubcommands(IEnumerable<Command> subcommands)
        {
            // embed things
            embed.Description = "Usa !help <comando> per le informazioni specifiche di un comando.";
            string comandi = "```";
            
            // adds a comma between every command
            foreach (Command command in subcommands)
            {
                if (command.Name.ToLower() == "help") continue;

                char[] commandChars = command.Name.ToCharArray();
                string commandName = null;
                bool first = true;
                foreach (char c in commandChars)
                {

                    if (c == commandChars[0] && first)
                    {
                        first = false;
                        char[] chars = c.ToString().ToUpper().ToCharArray();
                        commandName += chars[0].ToString();
                    }
                    else
                    {
                        commandName += c.ToString();
                    }

                }
                if (commandName != null) comandi = comandi + commandName + ", ";

            }

            comandi += "Help.";
            comandi += "```";

            // finally adds the command list to the embed
            embed.AddField("Comandi del Bot:", comandi);

            return this;
        }
    }
}
