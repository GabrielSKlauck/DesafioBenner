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
            string sql = @$"INSERT INTO CARRO(placa, preco, saida) 
                            VALUES('{placa}',{2}, null)";
            await this.Execute(sql, new { placa});
        }

        public async Task Finalizar(string placa)
        {
            string sql = $"SELECT * FROM CARRO WHERE placa LIKE '{placa}'";
            CarroEntity carro = (CarroEntity) await GetConnection().QueryFirstAsync<CarroEntity>(sql, new {placa});

            DateTime horaAtual = DateTime.Now;
            string aux = horaAtual.ToString("yyyy-MM-dd HH:mm:ss");
            horaAtual = DateTime.Parse(aux).ToUniversalTime();
            
            double totalPagar = 0;
          
            TimeSpan tempoPermanecido = horaAtual - carro.chegada; 
            int horaCobrada = tempoPermanecido.Hours;

            if (tempoPermanecido.Hours >= 1)
            {
                if(tempoPermanecido.Minutes > 10)
                {                   
                    totalPagar = (tempoPermanecido.Hours+1) * 2;
                    horaCobrada++;
                    sql = @$"UPDATE CARRO SET SAIDA = '{horaAtual}',
                                           DURACAO = '{tempoPermanecido}',
                                           TEMPOCOBRADOHORA = {horaCobrada},
                                           PRECO = 2,
                                           VALORPAGAR = {totalPagar}
                                           WHERE PLACA LIKE '{placa}'";
                    await this.Execute(sql, new { placa });
                }
                else
                {
                    totalPagar = tempoPermanecido.Hours * 2;
                    sql = @$"UPDATE CARRO SET SAIDA = '{horaAtual}',
                                           DURACAO = '{tempoPermanecido}',
                                           TEMPOCOBRADOHORA = {horaCobrada},
                                           PRECO = 2,
                                           VALORPAGAR = {totalPagar}
                                           WHERE PLACA LIKE '{placa}'";
                    await this.Execute(sql, new { placa });
                }
               
            }else if (tempoPermanecido.TotalMinutes <= 30)
            {
                horaCobrada = 1;
                totalPagar = 1;
                sql = @$"UPDATE CARRO SET SAIDA = '{horaAtual}',
                                           DURACAO = '{tempoPermanecido}',
                                           TEMPOCOBRADOHORA = {horaCobrada},
                                           PRECO = 2,
                                           VALORPAGAR = {totalPagar}
                                           WHERE PLACA LIKE '{placa}'";
                await this.Execute(sql, new { placa });
            }
            else
            {
                totalPagar = 2;
                sql = @$"UPDATE CARRO SET SAIDA = '{horaAtual}',
                                           DURACAO = '{tempoPermanecido}',
                                           TEMPOCOBRADOHORA = {horaCobrada},
                                           PRECO = 2,
                                           VALORPAGAR = {totalPagar}
                                           WHERE PLACA LIKE '{placa}'";
                await this.Execute(sql, new { placa });
            }
        }

        public async Task<IEnumerable<CarroEntity>> GetTodos()
        {
            string sql = "SELECT * FROM CARRO ORDER BY DURACAO";
            return await GetConnection().QueryAsync<CarroEntity>(sql);
        }
    }
}
