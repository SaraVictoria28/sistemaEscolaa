using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Escola.Models
{
    public class Curso
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public int CargaHoraria { get; set; }

        public override string ToString() =>
             $"Id: {Id}, Nome: {Nome}, Carga Horária: {CargaHoraria}h";
    }
}