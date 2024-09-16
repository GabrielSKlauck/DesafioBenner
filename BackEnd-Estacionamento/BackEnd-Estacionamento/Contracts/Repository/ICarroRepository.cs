using BackEnd_Estacionamento.DTO;
using BackEnd_Estacionamento.Entity;

namespace BackEnd_Estacionamento.Contracts.Repository
{
    public interface ICarroRepository
    {
        Task Adicionar(string placa);
        Task<IEnumerable<CarroEntity>> GetTodos();
        Task Finalizar(string placa);
        Task AdicionarEspecifico(CarroDetailDTO carro);
        Task<IEnumerable<CarroEntity>> GetCarro(string placa);
    }
}
