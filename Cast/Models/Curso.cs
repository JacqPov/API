using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cast.Models
{
    public class Curso
    {
        [Key]
        public int Id { get; set; }
        public string Descricao { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataTermino { get; set; }
        public int QtdAlunos { get; set; }
        public virtual Categoria Categoria { get; set; }
    }
}
