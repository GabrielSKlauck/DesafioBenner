using BackEnd_Estacionamento.Contracts.Repository;
using BackEnd_Estacionamento.DTO;
using BackEnd_Estacionamento.Entity;
using BackEnd_Estacionamento.Infrastucture;
using Dapper;
using System.Collections.Generic;
using System.Numerics;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BackEnd_Estacionamento.Repository
{
    public class CarroRepository : Connection, ICarroRepository
    {
        public async Task Adicionar(string placa)
        {
            string sql = $"SELECT * FROM CARRO WHERE PLACA LIKE '{placa}' AND VALORPAGAR IS NULL";
            CarroEntity carro = null;
            try
            {
               carro = await GetConnection().QueryFirstAsync<CarroEntity>(sql, new { placa });
            }
            catch (Exception e)
            {}
            if (carro == null)
            {
                sql = @$"INSERT INTO CARRO(placa, preco) 
                            VALUES('{placa}',{2})";
                await this.Execute(sql, new { placa });
            }        
        }

        public async Task AdicionarEspecifico(CarroDetailDTO carro)
        {

            string sql = $"SELECT * FROM CARRO WHERE PLACA LIKE '{carro.placa}' AND VALORPAGAR IS NULL";
            CarroEntity carroVer = null;
            try
            {
                carroVer = await GetConnection().QueryFirstAsync<CarroEntity>(sql, new { carro.placa });
            }
            catch (Exception e)
            { }
            if (carroVer == null)
            {
                sql = @$"INSERT INTO CARRO(placa, chegada, saida, preco)
                            VALUES(@placa, @chegada, @saida, { 2})";
                await this.Execute(sql, carro);
            }
        
        }

        public async Task Finalizar(string placa)
        {
            string sql = $"SELECT * FROM CARRO WHERE placa LIKE '{placa}' AND VALORPAGAR IS NULL";
            CarroEntity carro = (CarroEntity) await GetConnection().QueryFirstAsync<CarroEntity>(sql, new {placa});
            int idCarro = carro.id;
            DateTime testagemData = new DateTime(00001, 01,01,00,00,00);

            DateTime horaAtual;
            string horaSql;
            if (DateTime.Compare(carro.saida, testagemData) == 0)
            {
                horaAtual = DateTime.Now;
                horaSql = System.String.Format("{0:yyyy-MM-dd HH:mm:ss}", horaAtual);
            }
            else
            {
                horaAtual = carro.saida;
                horaSql = System.String.Format("{0:yyyy-MM-dd HH:mm:ss}", horaAtual);
            }           
            
            double totalPagar = 0;
          
            TimeSpan tempoPermanecido = horaAtual - carro.chegada; 
            int horaCobrada = tempoPermanecido.Hours;


            

            if (tempoPermanecido.Hours >= 1)
            {
                if(tempoPermanecido.Minutes > 10)
                {                   
                    totalPagar = (tempoPermanecido.Hours+1) * 2;
                    horaCobrada++;
                    sql = @$"UPDATE CARRO SET SAIDA = '{horaSql}',
                                           DURACAO = '{tempoPermanecido}',
                                           TEMPOCOBRADOHORA = {horaCobrada},
                                           PRECO = 2,
                                           VALORPAGAR = {totalPagar}
                                           WHERE ID = {idCarro}";
                    await this.Execute(sql, new { placa });
                }
                else
                {
                    totalPagar = tempoPermanecido.Hours * 2;
                    sql = @$"UPDATE CARRO SET SAIDA = '{horaSql}',
                                           DURACAO = '{tempoPermanecido}',
                                           TEMPOCOBRADOHORA = {horaCobrada},
                                           PRECO = 2,
                                           VALORPAGAR = {totalPagar}
                                           WHERE ID = {idCarro}";
                    await this.Execute(sql, new { placa });
                }
               
            }else if (tempoPermanecido.TotalMinutes <= 30)
            {
                horaCobrada = 1;
                totalPagar = 1;
                sql = @$"UPDATE CARRO SET SAIDA = '{horaSql}',
                                           DURACAO = '{tempoPermanecido}',
                                           TEMPOCOBRADOHORA = {horaCobrada},
                                           PRECO = 2,
                                           VALORPAGAR = {totalPagar}
                                           WHERE ID = {idCarro}";
                await this.Execute(sql, new { placa });
            }
            else
            {
                totalPagar = 2;
                sql = @$"UPDATE CARRO SET SAIDA = '{horaSql}',
                                           DURACAO = '{tempoPermanecido}',
                                           TEMPOCOBRADOHORA = {horaCobrada},
                                           PRECO = 2,
                                           VALORPAGAR = {totalPagar}
                                           WHERE ID = {idCarro}";
                await this.Execute(sql, new { placa });
            }
        }

        public async Task<CarroEntity> GetCarro(string placa)
        {
            string sql = $"SELECT * FROM CARRO WHERE PLACA LIKE '{placa}'";
            return await GetConnection().QueryFirstAsync<CarroEntity>(sql, new { placa });
        }

        public async Task<IEnumerable<CarroEntity>> GetTodos()
        {
            string sql = "SELECT * FROM CARRO ORDER BY DURACAO";
            return await GetConnection().QueryAsync<CarroEntity>(sql);
        }
    }
}
