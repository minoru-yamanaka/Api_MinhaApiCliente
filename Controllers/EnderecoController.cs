using ApiClientes.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiClientes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnderecoController : ControllerBase {
        private readonly AppDbContext _contextDb;

        public EnderecoController(AppDbContext contextDb)
        {
            _contextDb = contextDb;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Endereco endereco)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _contextDb.Enderecos.Add(endereco);
            await _contextDb.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = endereco.Id }, endereco);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var enderecos = await _contextDb.Enderecos.ToListAsync();
            return Ok(enderecos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            Endereco? endereco = await _contextDb.Enderecos.FindAsync(id);
            if (endereco == null)
            {
                return NotFound($"Endereço com o id {id} não foi encontrado");
            }
            return Ok(endereco);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] Endereco endereco)
        {
            if (id != endereco.Id)
            {
                return BadRequest("O ID enviado não corresponde ao ID do endereço no Banco de Dados");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _contextDb.Entry(endereco).State = EntityState.Modified;
            await _contextDb.SaveChangesAsync();
            return Ok("Endereço atualizado com sucesso!");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            Endereco? endereco = await _contextDb.Enderecos.FindAsync(id);
            if (endereco == null)
            {
                return NotFound($"Endereço com o id {id} não foi encontrado");
            }
            _contextDb.Enderecos.Remove(endereco);
            await _contextDb.SaveChangesAsync();
            return Ok("Endereço deletado com sucesso!");
        }

    }
}
