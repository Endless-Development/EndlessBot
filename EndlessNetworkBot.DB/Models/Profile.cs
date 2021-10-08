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

using EndlessNetworkBot.DB;
using EndlessNetworkBot.DB.Models.Ranks;
using System.Collections.Generic;

namespace EndlessNetworkBot.DB.Models
{
    public class Profile : Entity
    {
        public ulong DiscordId { get; set; }
        public ulong GuildId { get; set; }
        public int Xp { get; set; }
        public int Level => Xp / 100;

        public List<ProfileRank> Ranks { get; set; } = new List<ProfileRank>();
    }
}
