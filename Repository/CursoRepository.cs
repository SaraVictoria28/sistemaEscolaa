using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using Escola.Models;
using Escola.Models.Data;
using SistemaEscola.Data;

namespace Escola.Data
{
    public class CursoRepository
    {
        public void Inserir(Curso curso)
        {
            using (IDbConnection conexao = Db.GetConnection())
            {
                conexao.Open();
                var cmd = conexao.CreateCommand();
                cmd.CommandText = "INSERT INTO Curso (Nome, CargaHoraria) VALUES (@nome, @cargaHoraria); SELECT SCOPE_IDENTITY();";
                cmd.AddParameter("@nome", curso.Nome);
                cmd.AddParameter("@cargaHoraria", curso.CargaHoraria);

                var idGerado = cmd.ExecuteScalar();
                curso.Id = Convert.ToInt32(idGerado);
            }
        }

        public Curso ObterPorId(int id)
        {
            using (IDbConnection conexao = Db.GetConnection())
            {
                conexao.Open();
                var cmd = conexao.CreateCommand();
                cmd.CommandText = "SELECT IdCurso, Nome, CargaHoraria FROM Curso WHERE IdCurso = @id";
                cmd.AddParameter("@id", id);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Curso
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("IdCurso")),
                            Nome = reader.GetString(reader.GetOrdinal("Nome")),
                            CargaHoraria = reader.GetInt32(reader.GetOrdinal("CargaHoraria"))
                        };
                    }
                }
            }
            return null;
        }

        public List<Curso> ListarTodos()
        {
            var cursos = new List<Curso>();
            using (IDbConnection conexao = Db.GetConnection())
            {
                conexao.Open();
                var cmd = conexao.CreateCommand();
                cmd.CommandText = "SELECT IdCurso, Nome, CargaHoraria FROM Curso";

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        cursos.Add(new Curso
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("IdCurso")),
                            Nome = reader.GetString(reader.GetOrdinal("Nome")),
                            CargaHoraria = reader.GetInt32(reader.GetOrdinal("CargaHoraria"))
                        });
                    }
                }
            }
            return cursos;
        }
    }
}