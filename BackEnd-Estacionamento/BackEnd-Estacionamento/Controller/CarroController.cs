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

        public CarroController(CarroRepository carroRepository)
        {
            _carroRepository = carroRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetTodos()
        {
            await _carroRepository.GetTodos();
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Adicionar(CarroDTO carro)
        {
            await _carroRepository.Adicionar(carro);
            return Ok();    
        }
    }
}
