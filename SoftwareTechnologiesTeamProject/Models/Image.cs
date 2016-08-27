using System;

namespace SoftwareTechnologiesTeamProject.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Image
    {
        public Image()
        {
            this.UploadedDate = DateTime.Now;
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string ImagePath { get; set; }
            
        public DateTime UploadedDate  { get; set; }
    }
}