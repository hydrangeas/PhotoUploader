using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace CVPTest.ViewModels
{
    public class JobDataViewModel
    {
        [Required]
        public string Name { get; set; }
        public string FilePath { get; set; }
        public IFormFile File { get; set; }
    }
}