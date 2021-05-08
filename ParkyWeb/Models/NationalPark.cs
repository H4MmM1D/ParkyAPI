using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyWeb.Models
{
    public class NationalPark
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "نام اجباری می باشد.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "اینم اجباریه دیوث")]
        public string State { get; set; }
        public DateTime Created { get; set; }
        public byte[] Picture { get; set; }
        public DateTime Established { get; set; }
    }
}
