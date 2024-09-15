using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata;

namespace TaskManager.Models
{
    public class Roles
    {
        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]      
        public int Id { get; set; }
        public string RoleName { get; set; }
    }

 


}
