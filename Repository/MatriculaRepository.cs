using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using Escola.Models;
using Escola.Models.Data;
using SistemaEscola.Data;

namespace Escola.Data
{
    public class MatriculaRepository
    {
        public void Inserir(Matricula matricula)
        {
            using (IDbConnection conexao = Db.GetConnection())
            {
                conexao.Open();
                var cmd = conexao.CreateCommand();
                cmd.CommandText = "INSERT INTO Matricula (IdAluno, IdCurso, DataMatricula) VALUES (@idAluno, @idCurso, @dataMatricula); SELECT SCOPE_IDENTITY();";
                cmd.AddParameter("@idAluno", matricula.IdAluno);
                cmd.AddParameter("@idCurso", matricula.IdCurso);
                cmd.AddParameter("@dataMatricula", matricula.DataMatricula);

                var idGerado = cmd.ExecuteScalar();
                matricula.Id = Convert.ToInt32(idGerado);
            }
        }

        public List<Curso> ListarCursosPorAluno(int idAluno)
        {
            var cursos = new List<Curso>();
            using (IDbConnection conexao = Db.GetConnection())
            {
                conexao.Open();
                var cmd = conexao.CreateCommand();
                cmd.CommandText = @"
                    SELECT C.IdCurso, C.Nome, C.CargaHoraria 
                    FROM Curso C
                    INNER JOIN Matricula M ON C.IdCurso = M.IdCurso
                    WHERE M.IdAluno = @idAluno";
                cmd.AddParameter("@idAluno", idAluno);

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

        public List<Aluno> ListarAlunosPorCurso(int idCurso)
        {
            var alunos = new List<Aluno>();
            using (IDbConnection conexao = Db.GetConnection())
            {
                conexao.Open();
                var cmd = conexao.CreateCommand();
                cmd.CommandText = @"
                    SELECT A.IdAluno, A.Nome, A.Idade
                    FROM Aluno A
                    INNER JOIN Matricula M ON A.IdAluno = M.IdAluno
                    WHERE M.IdCurso = @idCurso";
                cmd.AddParameter("@idCurso", idCurso);

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