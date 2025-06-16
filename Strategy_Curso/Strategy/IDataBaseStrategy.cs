using Strategy_Curso.Models;
using System.Collections.Generic;

namespace Strategy_Curso.Strategy
{
    public interface IDataBaseStrategy
    {
        void InsertCurso(Curso curso);
        List<Curso> VisualizarCursos();
    }
}
