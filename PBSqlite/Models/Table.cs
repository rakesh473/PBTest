using System.ComponentModel.DataAnnotations;

namespace PBSqlite.Models
{
    public class Table
    {
        [Required] public int Id { get; set; }

        [Required] public string TableName { get; set; }

        public string PlayersData { get; set; }
        public int TotalBuyIns { get; set; } = 0;
        public double TotalGain { get; set; } = 0;
        public double TotalLoss { get; set; } = 0;
        public double TotalTable { get; set; } = 0;
    }
}