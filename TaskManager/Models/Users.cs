using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManager.Models
{
    public class Users
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Username { get; set; }

        public string Password { get; set; }

        [ForeignKey("RoleName")]
        public virtual Roles Role {  get; set; }
        

    }
}
