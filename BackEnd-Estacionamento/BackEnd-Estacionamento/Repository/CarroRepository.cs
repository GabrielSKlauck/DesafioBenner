using BackEnd_Estacionamento.Contracts.Repository;
using BackEnd_Estacionamento.DTO;
using BackEnd_Estacionamento.Entity;
using BackEnd_Estacionamento.Infrastucture;
using Dapper;

namespace BackEnd_Estacionamento.Repository
{
    public class CarroRepository : Connection, ICarroRepository
    {
        public async Task Adicionar(CarroDTO carro)
        {
            string sql = @$"INSERT INTO CARRO(placa, preco) 
                            VALUES(@placa,{2})";
            await this.Execute(sql, carro);
        }

        public Task Finalizar(string placa)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<CarroEntity>> GetTodos()
        {
            string sql = "SELECT * FROM CARRO";
            return await GetConnection().QueryAsync<CarroEntity>(sql);
        }
    }
}
