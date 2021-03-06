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

using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Exceptions;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.VoiceNext;
using EndlessNetworkBot.Commands.Moderation;
using EndlessNetworkBot.DB;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace EndlessNetworkBot
{
    public class Bot
    {
        public Dictionary<ulong, DiscordMember> customChannels { get; private set; }

        public ServerContext context { get; private set; }

        public Bot(IServiceProvider sp)
        {
            StartAsync(sp);
        }

        public DiscordClient Client { get; private set; }
        public CommandsNextExtension Commands { get; private set; }
        public InteractivityExtension Interactivity { get; private set; }
        public VoiceNextExtension Voice { get; private set; }

        /// <summary>
        /// Start and setup the bot.
        /// </summary>
        public async Task StartAsync(IServiceProvider sp)
        {
            customChannels = new Dictionary<ulong, DiscordMember>();

            // loads main config and starts bot
            DiscordConfiguration discord = GetDiscordConfiguration();
            Client = new DiscordClient(discord);
            Client.Ready += OnReadyAsync; // gets called when the bot is ready
            Client.VoiceStateUpdated += OnVoiceStateUpdatedAsync; // gets called when someone join/quit/moves in a channel

            
            // loads commandsnext
            CommandsNextConfiguration commandsNext = GetCommandsNextConfiguration(sp);
            Commands = Client.UseCommandsNext(commandsNext);
            Commands.SetHelpFormatter<HelpFormatter>();
            RegisterCommands();
            Commands.CommandErrored += OnCommandErrored;

            // loads interactivity
            InteractivityConfiguration interactivity = GetInteractivityConfiguration();
            Client.UseInteractivity(interactivity);

            // loads voicenext
            VoiceNextConfiguration voiceNext = GetVoiceNextConfiguration();
            Client.UseVoiceNext(voiceNext);

            // finally connects the bot to discord
            await Client.ConnectAsync(null, UserStatus.Invisible).ConfigureAwait(false);

            await Task.Delay(-1);
        }

        #region Registering Commands

        /// <summary>
        /// Register the bot commands
        /// </summary>
        private void RegisterCommands()
        {
            Commands.RegisterCommands<KickCommand>();
            Commands.RegisterCommands<BanCommand>();
            Commands.RegisterCommands<SayCommand>();
            Commands.RegisterCommands<EmbedCommand>();
            Commands.RegisterCommands<BlacklistCommand>();
        }

        #endregion

        #region Ready Event

        /// <summary>
        /// Gets called when the bot is ready
        /// </summary>
        private async Task OnReadyAsync(DiscordClient client, ReadyEventArgs ev)
        {
            // sets custom activity
            DiscordActivity activity = new DiscordActivity
            {
                ActivityType = ActivityType.Playing,
                Name = GetConfig().Result.Bot_Status,
            };

            // updates bot activity
            await Client.UpdateStatusAsync(activity, UserStatus.DoNotDisturb);

            Console.WriteLine("Bot is now ready.");
        }

        #endregion

        #region VoiceStateUpdate Event

        /// <summary>
        /// Gets called when an user joins/leaves/moves in a voice channel
        /// Used to make custom voice channel
        /// </summary>
        private async Task OnVoiceStateUpdatedAsync(DiscordClient client, VoiceStateUpdateEventArgs ev)
        {
            DiscordMember member = await ev.Guild.GetMemberAsync(ev.User.Id);
            
            if(ev.After != null && ev.After.Channel != null) // if user joined a channel
            {
                // if an user join a certain voice channel, it will make a custom one for him
                if (ev.After.Channel.Id == 896424758722330704)
                {
                    #region Permissions
                    
                    DiscordOverwriteBuilder memberPermissions = new DiscordOverwriteBuilder(member)
                    {
                        Allowed = Permissions.All,
                    };
                    DiscordOverwriteBuilder usersPermissions = new DiscordOverwriteBuilder(ev.Guild.GetRole(885121885371793419))
                    {
                        Allowed = Permissions.AccessChannels | Permissions.Speak | Permissions.Stream,
                        Denied = Permissions.UseVoice,
                    };
                    DiscordOverwriteBuilder everyonePermissions = new DiscordOverwriteBuilder(ev.Guild.GetRole(885121885346623498))
                    {
                        Denied = Permissions.AccessChannels
                    };
                    List<DiscordOverwriteBuilder> permissions = new List<DiscordOverwriteBuilder>();
                    permissions.Add(memberPermissions);
                    permissions.Add(usersPermissions);
                    permissions.Add(everyonePermissions);

                    #endregion
                    
                    // creates a new voice channel
                    DiscordChannel custom = await ev.Guild.CreateChannelAsync(member.DisplayName, ChannelType.Voice,
                        ev.Guild.GetChannel(896404597843828756), " ", null, 1, permissions);
                    
                    // moves the user to that voice channel
                    await custom.PlaceMemberAsync(member);

                    // adds it to the dictionary so we can find it later
                    customChannels.Add(custom.Id, member);
                }
            }

            if (ev.Before != null && ev.Before.Channel != null) // it user left a channel or moved to another one
            {
                // if an user leaves a custom voice channel, and theres no one left, the bot will delete it
                if (ev.Before.Channel.Users.Count() == 0 && customChannels.ContainsKey(ev.Before.Channel.Id))
                {
                    // removes the channel from the dictionary
                    customChannels.Remove(ev.Before.Channel.Id);

                    // deletes the channel
                    await ev.Before.Channel.DeleteAsync();
                }
            }
        }

        #endregion

        #region CommandError Event

        /// <summary>
        /// Gets called when a command throws an error
        /// </summary>
        private async Task OnCommandErrored(CommandsNextExtension commands, CommandErrorEventArgs ev)
        {
            Console.WriteLine("ERROR IN COMMAND: " + ev.Command.Name);
            Console.WriteLine("----------------------------------------------");
            Console.WriteLine(ev.Exception);
        }

        #endregion

        #region Loading Configurations

        /// <returns>Returns the Discord Configuration</returns>
        private DiscordConfiguration GetDiscordConfiguration()
        {
            DiscordConfiguration config = new DiscordConfiguration
            {
                Token = GetConfig().Result.Token,
                TokenType = TokenType.Bot,
                AutoReconnect = true,
                MinimumLogLevel = Microsoft.Extensions.Logging.LogLevel.Debug,
                AlwaysCacheMembers = true,
                Proxy = null,
                GatewayCompressionLevel = GatewayCompressionLevel.Stream,
                MessageCacheSize = 2048,
                UseRelativeRatelimit = false,
                Intents = DiscordIntents.GuildMessages | DiscordIntents.GuildVoiceStates | DiscordIntents.Guilds
            };

            return config;
        }

        /// <returns>Returns the CommandsNext Configuration</returns>
        private CommandsNextConfiguration GetCommandsNextConfiguration(IServiceProvider sp)
        {
            CommandsNextConfiguration config = new CommandsNextConfiguration
            {
                StringPrefixes = new string[] { GetConfig().Result.Prefix },
                EnableDms = false,
                EnableDefaultHelp = true,
                EnableMentionPrefix = true,
                CaseSensitive = false,
                Services = sp,
            };

            return config;
        }

        /// <returns>Returns the Interactivity Configuration</returns>
        private InteractivityConfiguration GetInteractivityConfiguration()
        {
            PaginationEmojis paginationEmojis = new PaginationEmojis();
            paginationEmojis.Left = DiscordEmoji.FromName(Client, ":arrow_left:");
            paginationEmojis.Right = DiscordEmoji.FromName(Client, ":arrow_right:");
            paginationEmojis.SkipLeft = DiscordEmoji.FromName(Client, ":rewind:");
            paginationEmojis.SkipRight = DiscordEmoji.FromName(Client, ":fast_forward:");
            paginationEmojis.Stop = DiscordEmoji.FromName(Client, ":octagonal_sign:");

            InteractivityConfiguration config = new InteractivityConfiguration
            {
                PaginationBehaviour = DSharpPlus.Interactivity.Enums.PaginationBehaviour.WrapAround,
                PaginationDeletion = DSharpPlus.Interactivity.Enums.PaginationDeletion.DeleteEmojis,
                PaginationEmojis = paginationEmojis,
                PollBehaviour = DSharpPlus.Interactivity.Enums.PollBehaviour.DeleteEmojis,
                Timeout = new TimeSpan(0, 0, 60),
            };

            return config;
        }

        /// <returns>Returns the VoiceNext Configuration</returns>
        private VoiceNextConfiguration GetVoiceNextConfiguration()
        {
            VoiceNextConfiguration config = new VoiceNextConfiguration
            {
                AudioFormat = AudioFormat.Default,
                EnableIncoming = false,
                PacketQueueSize = 25,
            };

            return config;
        }

        #endregion

        #region Loading JSON Config

       public async Task<JSONConfig> GetConfig()
       {
            string config = null;

            FileStream fs = File.OpenRead("config.json");
            StreamReader sr = new StreamReader(fs, new UTF8Encoding(false));
            config = await sr.ReadToEndAsync().ConfigureAwait(false);

            JSONConfig json = JsonConvert.DeserializeObject<JSONConfig>(config);

            return json;

       }

        #endregion
    }

    public struct JSONConfig
    {
        [JsonProperty("token")] public string Token { get; private set; }
        [JsonProperty("prefix")] public string Prefix { get; private set; }
        [JsonProperty("bot_status")] public string Bot_Status { get; private set; }
    }

}
