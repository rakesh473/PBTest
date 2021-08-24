using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PBSqlite.Models
{
    [Table("User")]
    public class User
    {
        [Required] public int Id { get; set; }

        [Key] [Required] public string UserName { get; set; }

        public string Password { get; set; }
    }
}