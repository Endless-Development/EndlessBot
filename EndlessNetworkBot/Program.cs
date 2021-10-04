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

using System.Threading.Tasks;

namespace EndlessNetworkBot
{
    class Program
    {
        private static Bot bot;

        static void Main(string[] args)
        {
            // starts the bot
            bot = new Bot();
            StartBotAsync(bot).GetAwaiter().GetResult();
        }

        #region StartBotAsync task

        /// <summary>
        /// Asynchronous task that starts a bot
        /// </summary>
        /// <param name="bot">Which bot?</param>
        /// <returns>Returns the completed task state</returns>
        private static async Task<Task> StartBotAsync(Bot bot)
        {
            // starts the provided bot
            await bot.StartAsync();

            Bot.instance = bot;

            return Task.CompletedTask;
        }

        #endregion
    }
}
