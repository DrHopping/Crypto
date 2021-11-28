using System;

namespace CasinoRoyale.Models
{
    public class Account
    {
        public int Id { get; set; }
        public long Money { get; set; }
        public DateTime DeletionTime { get; set; }
    }
}