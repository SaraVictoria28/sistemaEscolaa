using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Escola.Models
{
    public class Matricula
    {
        public int Id { get; set; }
        public int IdAluno { get; set; }
        public int IdCurso { get; set; }
        public DateOnly DataMatricula { get; set; }

    }
}