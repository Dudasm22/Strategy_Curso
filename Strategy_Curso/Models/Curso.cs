namespace Strategy_Curso.Models
{
    public class Curso
    {
        public int id { get; set; }
        public string nome { get; set; }
        public int cargaHoraria { get; set; }
        public string descricao { get; set; }
        public PublicoAlvoEnum publicoAlvo { get; set; }
        public double valor { get; set; }



        public Curso() { }

        public Curso(int id, string nome, int cargaHoraria, PublicoAlvoEnum publicoAlvo, double valor, string descricao)
        {
            this.id = id;
            this.nome = nome;
            this.cargaHoraria = cargaHoraria;
            this.publicoAlvo = publicoAlvo;
            this.valor = valor;
            this.descricao = descricao;
        }

        public override string ToString()
        {
            return $"Curso: {nome}, Carga Horária: {cargaHoraria} horas, Público Alvo: {publicoAlvo}, Valor: R${valor}, Descrição: {descricao}";
        }

    }
         public enum PublicoAlvoEnum
    {
        Infantil,
        Adolescente,
        Adulto,
        JovemAdulto,
        Todos
    }

}
