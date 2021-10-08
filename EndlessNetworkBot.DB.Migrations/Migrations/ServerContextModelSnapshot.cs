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
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EndlessNetworkBot.DB.Migrations.Migrations
{
    [DbContext(typeof(ServerContext))]
    partial class ServerContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("EndlessNetworkBot.DB.Models.Rank", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int")
                    .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                b.Property<string>("Description")
                    .HasColumnType("nvarchar(max)");

                b.Property<string>("Name")
                    .HasColumnType("nvarchar(max)");

                b.Property<int>("Price")
                    .HasColumnType("int");

                b.HasKey("Id");

                b.ToTable("Ranks");
            });

            modelBuilder.Entity("EndlessNetworkBot.DB.Models.Ranks.ProfileRank", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int")
                    .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                b.Property<int>("RankId")
                    .HasColumnType("int");

                b.Property<int>("ProfileId")
                    .HasColumnType("int");

                b.HasKey("Id");

                b.HasIndex("RankId");

                b.HasIndex("ProfileId");

                b.ToTable("ProfileRanks");
            });

            modelBuilder.Entity("EndlessNetworkBot.DB.Models.Profile", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int")
                    .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                b.Property<decimal>("DiscordId")
                    .HasColumnType("decimal(20,0)");

                b.Property<decimal>("GuildId")
                    .HasColumnType("decimal(20,0)");

                b.Property<int>("Xp")
                    .HasColumnType("int");

                b.HasKey("Id");

                b.ToTable("Profiles");
            });

            modelBuilder.Entity("EndlessNetworkBot.DB.Models.Ranks.ProfileRank", b =>
            {
                b.HasOne("EndlessNetworkBot.DB.Models.Ranks.ProfileRank", "Rank")
                    .WithMany()
                    .HasForeignKey("RankID")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.HasOne("EndlessNetworkBot.DB.Models.Profile", "Profile")
                    .WithMany("Ranks")
                    .HasForeignKey("ProfileId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
            });
#pragma warning restore 612, 618
        }
    }
}
