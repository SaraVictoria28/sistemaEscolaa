using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Escola.Models.Enums;

namespace Escola.Models
{
    public class Aluno
    {
        public int Id { get; set; }
        public string Nome { get; set; }
       public string Idade { get; set; }


        public override string ToString() =>
            $"Id: {Id}, Nome: {Nome}, Idade: {Idade}";
    }
}