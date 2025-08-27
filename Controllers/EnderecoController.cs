using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using MinhaAPI.Data;
using MinhaAPI.Models;

namespace MinhaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EnderecoController : ControllerBase
    {

        private readonly AppDbContext _contextDb;

        public EnderecoController(AppDbContext contextDb)
        {

            _contextDb = contextDb;
        }

        [HttpPost]
        public async Task<ActionResult> EnderecoCreate([FromBody] Endereco endereco) {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _contextDb.Endereco.Add(endereco);
            await _contextDb.SaveChangesAsync();

            return Ok("Endereco criado com sucesso!");

        }

        [HttpDelete]
        public async Task<ActionResult> DeleteEndereco(int id) {
            Endereco? endereco = await _contextDb.Endereco.FindAsync(id);

            if (endereco == null)
            {
                return NotFound("Endereco mão encontrado");  
            }

            _contextDb.Endereco.Remove(endereco);
            await _contextDb.SaveChangesAsync();


            return Ok("Endereco mão encontrado");

        }
    }   
}
