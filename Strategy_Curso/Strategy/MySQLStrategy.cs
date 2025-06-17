using Strategy_Curso.Models;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Linq;
using System;

namespace Strategy_Curso.Strategy
{
    public class MySQLStrategy : IDataBaseStrategy
    {
        private readonly string _connectionString;

        public MySQLStrategy(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void InserirCurso(Curso curso)
        {
            // O using garante que a conexão e o comando sejam fechados e descartados
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                try
                {
                    connection.Open(); // Abre a conexão com o banco de dados
                    Console.WriteLine("[MySQL] Conexão aberta para inserção.");

                    // Query SQL para inserir um novo curso
                    // O ID é auto-incrementado pelo banco de dados, então não o passamos aqui
                    string sql = "INSERT INTO Cursos (nome, cargaHoraria, descricao, publicoAlvo, valor) VALUES (@nome, @cargaHoraria, @descricao, @publicoAlvo, @valor)";

                    using (MySqlCommand command = new MySqlCommand(sql, connection))
                    {
                        // Adiciona os parâmetros para evitar SQL Injection
                        command.Parameters.AddWithValue("@nome", curso.nome);
                        command.Parameters.AddWithValue("@cargaHoraria", curso.cargaHoraria);
                        command.Parameters.AddWithValue("@descricao", curso.descricao);
                        command.Parameters.AddWithValue("@publicoAlvo", curso.publicoAlvo.ToString()); // Converte o Enum para string
                        command.Parameters.AddWithValue("@valor", curso.valor);

                        command.ExecuteNonQuery(); // Executa a query de inserção
                        Console.WriteLine($"[MySQL] Curso '{curso.nome}' inserido com sucesso no BANCO DE DADOS.");
                    }
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine($"[MySQL] Erro ao inserir curso: {ex.Message}");
                    // Você pode relançar a exceção ou logar de forma mais robusta
                    throw;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[MySQL] Erro inesperado ao inserir curso: {ex.Message}");
                    throw;
                }
            }
        }

        public List<Curso> VisualizarCursos()
        {
            List<Curso> cursosDoBanco = new List<Curso>(); // Lista para armazenar os cursos lidos do DB

            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                try
                {
                    connection.Open(); // Abre a conexão
                    Console.WriteLine("[MySQL] Conexão aberta para visualização.");

                    // Query SQL para selecionar todos os cursos
                    string sql = "SELECT id, nome, cargaHoraria, descricao, publicoAlvo, valor FROM Cursos";

                    using (MySqlCommand command = new MySqlCommand(sql, connection))
                    {
                        using (MySqlDataReader reader = command.ExecuteReader()) // Executa a query e obtém um leitor de dados
                        {
                            while (reader.Read()) // Itera sobre cada linha retornada
                            {
                                // Mapeia os dados do reader para um objeto Curso
                                cursosDoBanco.Add(new Curso
                                {
                                    id = reader.GetInt32("id"), // Lê o ID da coluna 'id'
                                    nome = reader.GetString("nome"),
                                    cargaHoraria = reader.GetInt32("cargaHoraria"),
                                    // Certifique-se de que o Enum.Parse seja robusto a valores inválidos do DB
                                    publicoAlvo = (PublicoAlvoEnum)Enum.Parse(typeof(PublicoAlvoEnum), reader.GetString("publicoAlvo"), true), // 'true' para ignorar case
                                    valor = reader.GetDouble("valor"),
                                    descricao = reader.GetString("descricao")
                                });
                            }
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine($"[MySQL] Erro ao visualizar cursos: {ex.Message}");
                    throw;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[MySQL] Erro inesperado ao visualizar cursos: {ex.Message}");
                    throw;
                }
            }
            Console.WriteLine($"[MySQL] {cursosDoBanco.Count} cursos visualizados do BANCO DE DADOS.");
            return cursosDoBanco;
        }
    }
}