using System;
using System.Linq;
using Escola.Models;
using Escola.Data;
using SistemaEscola.Data;

namespace Escola.Service
{
    public class EscolaService // Classe principal do sistema 
    {
        private readonly AlunoRepository _alunoRepo;
        private readonly CursoRepository _cursoRepo;
        private readonly MatriculaRepository _matriculaRepo;

        public EscolaService() //Construtor
        {
            _alunoRepo = new AlunoRepository();
            _cursoRepo = new CursoRepository();
            _matriculaRepo = new MatriculaRepository();
        }

        public void Executar() //Loop principal 
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("--- Sistema de Gestão Escolar ---");
                Console.WriteLine("1 - Cadastrar Aluno");
                Console.WriteLine("2 - Cadastrar Curso");
                Console.WriteLine("3 - Matricular Aluno em Curso");
                Console.WriteLine("4 - Listar Cursos de um Aluno");
                Console.WriteLine("5 - Listar Alunos de um Curso");
                Console.WriteLine("6 - Sair");
                Console.Write("Escolha: ");

                var opcao = Console.ReadLine();

                try
                {
                    switch (opcao)
                    {
                        case "1": CadastrarNovoAluno(); break;
                        case "2": CadastrarNovoCurso(); break;
                        case "3": MatricularAluno(); break;
                        case "4": ListarCursosDoAluno(); break;
                        case "5": ListarAlunosDoCurso(); break;
                        case "6": return;
                        default: Console.WriteLine("Opção inválida!"); break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\nOcorreu um erro: {ex.Message}");
                }

                Console.WriteLine("\nPressione qualquer tecla para continuar...");
                Console.ReadKey();
            }
        }

        private void CadastrarNovoAluno() //Metodo para cadastro de novo aluno
        {
            Console.Clear();
            Console.WriteLine("--- Cadastro de Aluno ---");
            Console.Write("Nome: ");
            var nome = Console.ReadLine();

            Console.Write("Idade: ");
            var idade = Console.ReadLine();

            var novoAluno = new Aluno 
            {
                Nome = nome,
                Idade = idade
            };

            if (string.IsNullOrWhiteSpace(novoAluno.Nome)) 
            {
                Console.WriteLine("O nome do aluno não pode ser vazio.");
                return;
            }

            _alunoRepo.Inserir(novoAluno);
            Console.WriteLine($"Aluno '{novoAluno.Nome}' cadastrado com sucesso! ID: {novoAluno.Id}");
        }

        private void CadastrarNovoCurso() //MEtodo de cadastro de novo curso
        {
            Console.Clear();
            Console.WriteLine("--- Cadastro de Curso ---");
            Console.Write("Nome: ");
            var nome = Console.ReadLine();

            Console.Write("Carga Horária: ");
            if (!int.TryParse(Console.ReadLine(), out int cargaHoraria)) //Validacao de entrada
            {
                Console.WriteLine(" Carga horária inválida. Deve ser um número inteiro.");
                return;
            }

            var novoCurso = new Curso
            {
                Nome = nome,
                CargaHoraria = cargaHoraria
            };

            if (string.IsNullOrWhiteSpace(novoCurso.Nome)) 
            {
                Console.WriteLine("O nome do curso não pode ser vazio.");
                return;
            }
            if (novoCurso.CargaHoraria <= 0)
            {
                Console.WriteLine("A carga horária deve ser maior que zero.");
                return;
            }

            _cursoRepo.Inserir(novoCurso);
            Console.WriteLine($"Curso '{novoCurso.Nome}' cadastrado com sucesso! ID: {novoCurso.Id}");
        }

        private void MatricularAluno() //Metodo para matricular aluno em curso
        {
            Console.Clear();
            Console.WriteLine("--- Matricular Aluno em Curso ---");
            
            var alunos = _alunoRepo.ListarTodos();
            Console.WriteLine("\nAlunos disponíveis:");
            if (!alunos.Any()) Console.WriteLine("Nenhum aluno cadastrado.");
            foreach (var a in alunos) Console.WriteLine($"ID: {a.Id}, Nome: {a.Nome}");

            Console.Write("\nDigite o ID do Aluno: ");
            if (!int.TryParse(Console.ReadLine(), out int idAluno))
            {
                Console.WriteLine(" ID do aluno inválido.");
                return;
            }

            var cursos = _cursoRepo.ListarTodos();
            Console.WriteLine("\nCursos disponíveis:");
            if (!cursos.Any()) Console.WriteLine("Nenhum curso cadastrado.");
            foreach (var c in cursos) Console.WriteLine($"ID: {c.Id}, Nome: {c.Nome}");

            Console.Write("\nDigite o ID do Curso: ");
            if (!int.TryParse(Console.ReadLine(), out int idCurso))
            {
                Console.WriteLine(" ID do curso inválido.");
                return;
            }

            var aluno = _alunoRepo.ObterPorId(idAluno);
            if (aluno == null)
            {
                Console.WriteLine($"Aluno com ID {idAluno} não encontrado.");
                return;
            }

            var curso = _cursoRepo.ObterPorId(idCurso);
            if (curso == null)
            {
                Console.WriteLine($"Curso com ID {idCurso} não encontrado.");
                return;
            }

            var matricula = new Matricula
            {
                IdAluno = idAluno,
                IdCurso = idCurso,
                DataMatricula = DateOnly.FromDateTime(DateTime.Now)
            };

            _matriculaRepo.Inserir(matricula);
            Console.WriteLine("Matrícula realizada com sucesso!");
        }

        private void ListarCursosDoAluno() 
        {
            Console.Clear();
            Console.WriteLine("--- Listar Cursos de um Aluno ---");
            
            var alunos = _alunoRepo.ListarTodos();
            Console.WriteLine("\nAlunos cadastrados:");
            if (!alunos.Any()) Console.WriteLine("Nenhum aluno cadastrado.");
            foreach (var a in alunos) Console.WriteLine($"ID: {a.Id}, Nome: {a.Nome}");

            Console.Write("\nDigite o ID do Aluno: ");
            if (!int.TryParse(Console.ReadLine(), out int idAluno))
            {
                Console.WriteLine(" ID do aluno inválido.");
                return;
            }

            var cursos = _matriculaRepo.ListarCursosPorAluno(idAluno);
            if (!cursos.Any())
            {
                Console.WriteLine("Nenhum curso encontrado para este aluno.");
            }
            else
            {
                Console.WriteLine($"Cursos do Aluno ID {idAluno}:");
                foreach (var curso in cursos)
                {
                    Console.WriteLine(curso.ToString());
                }
            }
        }

        private void ListarAlunosDoCurso() 
        {
            Console.Clear();
            Console.WriteLine("--- Listar Alunos de um Curso ---");

            var cursos = _cursoRepo.ListarTodos();
            Console.WriteLine("\nCursos cadastrados:");
            if (!cursos.Any()) Console.WriteLine("Nenhum curso cadastrado.");
            foreach (var c in cursos) Console.WriteLine($"ID: {c.Id}, Nome: {c.Nome}");

            Console.Write("\nDigite o ID do Curso: ");
            if (!int.TryParse(Console.ReadLine(), out int idCurso))
            {
                Console.WriteLine(" ID do curso inválido.");
                return;
            }

            var alunos = _matriculaRepo.ListarAlunosPorCurso(idCurso);
            if (!alunos.Any())
            {
                Console.WriteLine("Nenhum aluno matriculado neste curso.");
            }
            else
            {
                Console.WriteLine($"Alunos do Curso ID {idCurso}:");
                foreach (var aluno in alunos)
                {
                    Console.WriteLine(aluno.ToString());
                }
            }
        }
    }
}