namespace CasinoRoyale.Models
{
    public class Result
    {
        public Account Account { get; set; }
        public long RealNumber { get; set; }
        public bool IsVictory { get; set; }
        public string Message { get; set; }
    }
}