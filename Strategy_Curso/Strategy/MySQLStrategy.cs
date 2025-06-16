using Strategy_Curso.Models;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Strategy_Curso.Strategy
{
    public class MySQLStrategy : IDataBaseStrategy
    {
        private static List<Curso> cursos = new List<Curso>();
        public static int nextId = 1;

        public void InserirCurso(Curso curso)
        {
            curso.id = nextId++;
            cursos.Add(curso);
            Console.WriteLine($"[MySQL] Curso '{curso.nome}' inserido com sucesso!");
        }
        public List<Curso> VisualizarCursos()
        {
            Console.WriteLine("[MySQL] Listando cursos:");
            if (cursos.Count == 0)
            {
                Console.WriteLine("Nenhum curso cadastrado.");
               // return new List<Curso>();
            }
            return cursos;

        }
    }
}