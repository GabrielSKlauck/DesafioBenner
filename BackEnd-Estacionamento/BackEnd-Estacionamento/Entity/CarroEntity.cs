namespace BackEnd_Estacionamento.Entity
{
    public class CarroEntity
    {
        public int Id { get; set; } 
        public DateTime chegada { get; set; }
        public DateTime saida { get; set; }
        public TimeOnly duracao { get; set; }
        public int tempoCobradoHora { get; set; }
        public double preco { get; set; }
        public double valorPagar { get; set; }
    }
}
