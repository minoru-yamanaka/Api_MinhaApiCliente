using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MinhaAPI.Data;
using MinhaAPI.Models;

namespace MinhaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // A rota será /api/Servico
    public class ServicoController : ControllerBase
    {
        private readonly AppDbContext _contextDb;

        public ServicoController(AppDbContext contextDb)
        {
            _contextDb = contextDb;
        }

        // GET: api/Servico
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Servico>>> GetAll()
        {
            // CORREÇÃO: Buscando da tabela de Serviços (Servico) e usando uma variável com nome correto.
            var servicos = await _contextDb.Servico.ToListAsync();
            

            return Ok(servicos);
        }

        // GET: api/Servico/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Servico>> GetById(int id)
        {
            // CORREÇÃO: Buscando um 'Servico' específico.
            var servico = await _contextDb.Servico.FindAsync(id);

            if (servico == null)
            {
                return NotFound("Serviço não encontrado.");
            }

            return Ok(servico);
        }

        // POST: api/Servico
        [HttpPost]
        public async Task<ActionResult<Servico>> Create([FromBody] Servico servico)
        {
            // CORREÇÃO: O método agora recebe um objeto do tipo 'Servico'.
            _contextDb.Servico.Add(servico);
            await _contextDb.SaveChangesAsync();

            // MELHORIA: Retornando 201 Created com o objeto criado.
            return CreatedAtAction(nameof(GetById), new { id = servico.Id }, servico);
        }

        // PUT: api/Servico/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Servico servico)
        {
            // CORREÇÃO: O método agora recebe um objeto do tipo 'Servico'.
            if (id != servico.Id)
            {
                return BadRequest("O ID da rota não corresponde ao ID do serviço.");
            }

            _contextDb.Entry(servico).State = EntityState.Modified;

            try
            {
                await _contextDb.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // CORREÇÃO: Verificando a existência na tabela de Serviços.
                if (!_contextDb.Servico.Any(e => e.Id == id))
                {
                    return NotFound("Serviço não encontrado para atualização.");
                }
                else
                {
                    throw;
                }
            }

            // MELHORIA: Retornando 204 No Content.
            return NoContent();
        }

        // DELETE: api/Servico/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            // CORREÇÃO: Buscando e removendo um 'Servico'.
            var servico = await _contextDb.Servico.FindAsync(id);
            if (servico == null)
            {
                return NotFound("Serviço não encontrado para exclusão.");
            }

            _contextDb.Servico.Remove(servico);
            await _contextDb.SaveChangesAsync();

            // MELHORIA: Retornando 204 No Content.
            return NoContent();
        }
    }
}