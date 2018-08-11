using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace PetClinic.Models
{
    public class Procedure
    {
        public int Id { get; set; }

        [ForeignKey("Animal")]
        public int AnimalId { get; set; }
        [Required]
        public Animal Animal { get; set; }

        [ForeignKey("Vet")]
        public int VetId { get; set; }
        [Required]
        public Vet Vet { get; set; }

        [Required]
        public DateTime DateTime { get; set; }

        [NotMapped]
        public decimal Cost => this.ProcedureAnimalAids.Sum(p => p.AnimalAid.Price);

        public ICollection<ProcedureAnimalAid> ProcedureAnimalAids { get; set; } = new List<ProcedureAnimalAid>();
    }
}
