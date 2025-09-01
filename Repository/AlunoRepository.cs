using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using Escola.Models;
using Escola.Models.Data;
using Escola.Models.Enums;
using SistemaEscola.Data;

namespace Escola.Data
{
    public class AlunoRepository
    {
        public void Inserir(Aluno aluno)
        {
            using (IDbConnection conexao = Db.GetConnection())
            {
                conexao.Open();
                var cmd = conexao.CreateCommand();
                cmd.CommandText = "INSERT INTO Aluno (Nome, Idade) VALUES (@nome, @idade); SELECT SCOPE_IDENTITY();";
                cmd.AddParameter("@nome", aluno.Nome);
                cmd.AddParameter("@idade", aluno.Idade);

                var idGerado = cmd.ExecuteScalar();
                aluno.Id = Convert.ToInt32(idGerado);
            }
        }

        public Aluno ObterPorId(int id)
        {
            using (IDbConnection conexao = Db.GetConnection())
            {
                conexao.Open();
                var cmd = conexao.CreateCommand();
                cmd.CommandText = "SELECT IdAluno, Nome, Idade FROM Aluno WHERE IdAluno = @id";
                cmd.AddParameter("@id", id);
                
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Aluno
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("IdAluno")),
                            Nome = reader.GetString(reader.GetOrdinal("Nome")),
                            Idade = reader.GetString(reader.GetOrdinal("Idade"))
                        };
                    }
                }
            }
            return null;
        }

        public List<Aluno> ListarTodos()
        {
            var alunos = new List<Aluno>();
            using (IDbConnection conexao = Db.GetConnection())
            {
                conexao.Open();
                var cmd = conexao.CreateCommand();
                cmd.CommandText = "SELECT IdAluno, Nome, Idade FROM Aluno";

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        alunos.Add(new Aluno
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("IdAluno")),
                            Nome = reader.GetString(reader.GetOrdinal("Nome")),
                            Idade = reader.GetString(reader.GetOrdinal("Idade"))
                        });
                    }
                }
            }
            return alunos;
        }
    }
}