using BackEnd_Estacionamento.Contracts.Repository;
using BackEnd_Estacionamento.DTO;
using BackEnd_Estacionamento.Entity;
using BackEnd_Estacionamento.Infrastucture;
using Dapper;

namespace BackEnd_Estacionamento.Repository
{
    public class CarroRepository : Connection, ICarroRepository
    {
        public async Task Adicionar(string placa)
        {
            string sql = @$"INSERT INTO CARRO(placa, preco) 
                            VALUES('{placa}',{2})";
            await this.Execute(sql, new { placa});
        }

        public async Task Finalizar(string placa)
        {
            string sql = $"SELECT * FROM CARRO WHERE placa LIKE '{placa}'";
            CarroEntity carro = (CarroEntity) await GetConnection().QueryFirstAsync<CarroEntity>(sql, new {placa});
            DateTime horaAtual = DateTime.Now;


            TimeSpan tempoPermanecido = horaAtual - carro.chegada;

            if (tempoPermanecido.TotalMinutes <= 30)
            {
                sql = @$"UPDATE CARROS SET ";
            }
            
        }

        public async Task<IEnumerable<CarroEntity>> GetTodos()
        {
            string sql = "SELECT * FROM CARRO";
            return await GetConnection().QueryAsync<CarroEntity>(sql);
        }
    }
}
