using System;
using System.ComponentModel.DataAnnotations;

namespace EndlessNetworkBot.DB
{
    public abstract class Entity
    {
        [Key]
        public int Id { get; set; }
    }
}
