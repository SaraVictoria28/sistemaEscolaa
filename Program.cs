﻿using Escola.Models.Data;
using Escola.Service;

namespace Escola
{
    class Program
    {
        static void Main(string[] args)
        {
            Db.ConnectionString = "Server=localhost\\SQLEXPRESS;Database=EscolaDb;Trusted_Connection=True;Encrypt=False;";

            var escolaService = new EscolaService();
            escolaService.Executar();
        }
    }
}