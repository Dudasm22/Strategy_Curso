using Strategy_Curso.Models;
using Strategy_Curso.Strategy;
using System.Collections.Generic;

namespace Strategy_Curso.Context
{
    public class CursoCRUDContext
    {
        private IDataBaseStrategy _dataBaseStrategy;

        public CursoCRUDContext(IDataBaseStrategy dataBaseStrategy)
        {
            _dataBaseStrategy = dataBaseStrategy;
        }

        public void InserirCurso(Curso curso)
        {
            _dataBaseStrategy.InserirCurso(curso);
        }

        public void SetStrategy(IDataBaseStrategy dataBaseStrategy)
        {
            _dataBaseStrategy = dataBaseStrategy;
        }

        public List<Curso> VisualizarCursos()
        {
            return _dataBaseStrategy.VisualizarCursos();
        }
    }
}
