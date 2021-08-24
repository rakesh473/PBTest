namespace PBSqlite.Models
{
    public class Player
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int BuyIns { get; set; } = 0;
        public double Close { get; set; } = 0;
        public double Total { get; set; } = -5;
    }
}