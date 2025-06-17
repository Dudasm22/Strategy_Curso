
using Strategy_Curso.Models;
using System;
using Microsoft.Data.SqlClient;
using Strategy_Curso_MVC.Controllers;
using System.Collections.Generic;
using System.Linq;
using System.Data;

namespace Strategy_Curso.Strategy
{
    public class SQLServerStrategy : IDataBaseStrategy
    {

        private readonly string _connectionString;

        // O construtor agora recebe a connection string via Injeção de Dependência
        public SQLServerStrategy(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void InserirCurso(Curso curso)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                Console.WriteLine("[SQLServer] Conexão aberta.");

                string sql = "INSERT INTO Cursos (nome, cargaHoraria, descricao, publicoAlvo, valor) VALUES (@nome, @cargaHoraria, @descricao, @publicoAlvo, @valor)";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@nome", curso.nome);
                    command.Parameters.AddWithValue("@cargaHoraria", curso.cargaHoraria);
                    command.Parameters.AddWithValue("@descricao", curso.descricao);
                    command.Parameters.AddWithValue("@publicoAlvo", curso.publicoAlvo.ToString()); // Enum para string
                    command.Parameters.AddWithValue("@valor", curso.valor);

                    command.ExecuteNonQuery();
                    Console.WriteLine($"[SQLServer] Curso '{curso.nome}' inserido com sucesso no DB.");
                }
            }
        }

        public List<Curso> VisualizarCursos()
        {
            List<Curso> cursos = new List<Curso>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                Console.WriteLine("[SQLServer] Conexão aberta para visualização.");

                string sql = "SELECT id, nome, cargaHoraria, descricao, publicoAlvo, valor FROM Cursos";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cursos.Add(new Curso
                            {
                                id = reader.GetInt32("id"),
                                nome = reader.GetString("nome"),
                                cargaHoraria = reader.GetInt32("cargaHoraria"),
                                descricao = reader.GetString("descricao"),
                                publicoAlvo = (PublicoAlvoEnum)Enum.Parse(typeof(PublicoAlvoEnum), reader.GetString("publicoAlvo")),
                                valor = reader.GetDouble("valor")
                            });
                        }
                    }
                }
            }
            Console.WriteLine($"[SQLServer] {cursos.Count} cursos visualizados do DB.");
            return cursos;
        }
    }
}