using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManager.Models
{
    public class TaskItems
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TaskId { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }


        [ForeignKey("Username")]
        public Users CreatedBy { get; set; }
        
        [ForeignKey("Username")]
        public string AssignedTo {  get; set; }

        public int StatusId;
        [ForeignKey("StatusId")]
        public virtual Status ?Status { get; set; }

       

    }
}
