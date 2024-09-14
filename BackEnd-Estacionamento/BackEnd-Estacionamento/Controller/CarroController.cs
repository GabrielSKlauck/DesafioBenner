using BackEnd_Estacionamento.Contracts.Repository;
using BackEnd_Estacionamento.DTO;
using BackEnd_Estacionamento.Entity;
using BackEnd_Estacionamento.Repository;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd_Estacionamento.Controller
{
    [ApiController]
    [Route("ControleCarro")]
    public class CarroController:ControllerBase
    {
        private readonly ICarroRepository _carroRepository; 

        public CarroController(ICarroRepository carroRepository)
        {
            _carroRepository = carroRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetTodos()
        {
            
            return Ok(await _carroRepository.GetTodos());
        }

        [HttpPost("{placa}")]
        public async Task<IActionResult> Adicionar(string placa)
        {
            await _carroRepository.Adicionar(placa);
            return Ok();    
        }

        [HttpPut]
        public async Task<IActionResult> Finalizar(string placa)
        {
            await _carroRepository.Finalizar(placa);
            return Ok();
        }
    }
}
