// Controllers/ClienteController.cs

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MinhaAPI.Data;
using MinhaAPI.Models;
using MinhaAPI.Services; // Importar o namespace do serviço
using System.Linq;
using System.Threading.Tasks;

namespace MinhaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        private readonly ICpfValidationService _cpfValidationService;

        public ClienteController(AppDbContext appContext, ICpfValidationService cpfValidationService)
        {
            _appDbContext = appContext;
            _cpfValidationService = cpfValidationService;
        }

        // POST: api/Cliente -> CreateCliente
        [HttpPost]
        public async Task<IActionResult> CreateCliente([FromBody] Cliente cliente)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Validação 1: Verificar se CPF ou Email já existem
            var clienteExistente = await _appDbContext.Clientes
                .AnyAsync(c => c.Cpf == cliente.Cpf || c.Email == cliente.Email);

            if (clienteExistente)
            {
                return Conflict("Já existe um cliente com este CPF ou Email.");
            }

            // Validação 2: Integrar com a API externa para validar o CPF
            var isCpfValid = await _cpfValidationService.IsCpfValidAsync(cliente.Cpf);
            if (!isCpfValid)
            {
                return BadRequest("O CPF informado é inválido.");
            }

            // Define valores padrão do servidor
            cliente.DataCadastro = DateTime.UtcNow;
            cliente.Ativo = true;

            await _appDbContext.Clientes.AddAsync(cliente);
            await _appDbContext.SaveChangesAsync();

            // Retorna 201 Created com a rota para o novo recurso
            return CreatedAtAction(nameof(GetClienteById), new { id = cliente.Id }, cliente);
        }

        // GET: api/Cliente/{id} -> GetClienteById
        [HttpGet("{id}")]
        public async Task<IActionResult> GetClienteById(int id)
        {
            var cliente = await _appDbContext.Clientes
                .Include(c => c.Enderecos)
                .AsNoTracking() // Melhora a performance para consultas
                .FirstOrDefaultAsync(c => c.Id == id);

            if (cliente == null)
            {
                return NotFound("Cliente não encontrado.");
            }

            return Ok(cliente);
        }

        // GET: api/Cliente -> GetAllClientes
        [HttpGet]
        public async Task<IActionResult> GetAllClientes()
        {
            var clientes = await _appDbContext.Clientes
                .Where(c => c.Ativo)
                .Select(c => new
                {
                    c.Id,
                    c.Nome,
                    c.Email,
                    c.Telefone,
                    c.DataNascimento
                })
                .AsNoTracking()
                .ToListAsync();

            return Ok(clientes);
        }

        // PUT: api/Cliente/{id} -> UpdateCliente
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCliente(int id, [FromBody] Cliente clienteAtualizado)
        {
            if (id != clienteAtualizado.Id)
            {
                return BadRequest("O ID da rota deve ser o mesmo do corpo da requisição.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var clienteExistente = await _appDbContext.Clientes.FindAsync(id);

            if (clienteExistente == null)
            {
                return NotFound("Cliente não encontrado.");
            }

            // Previne que o cliente seja reativado ou desativado indevidamente por esta rota
            if (!clienteExistente.Ativo)
            {
                return BadRequest("Não é possível atualizar um cliente inativo.");
            }

            // Atualiza as propriedades do objeto que o EF está rastreando
            _appDbContext.Entry(clienteExistente).CurrentValues.SetValues(clienteAtualizado);

            try
            {
                await _appDbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Adicionar tratamento para concorrência se necessário
                throw;
            }

            return NoContent(); // Retorno padrão para PUT bem-sucedido (204)
        }

        // DELETE: api/Cliente/{id} -> DeleteCliente
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCliente(int id)
        {
            var cliente = await _appDbContext.Clientes.FindAsync(id);

            if (cliente == null)
            {
                return NotFound("Cliente não encontrado.");
            }

            // --- Implementação de Soft Delete (Recomendado) ---
            cliente.Ativo = false;
            await _appDbContext.SaveChangesAsync();

            /*
            // --- Implementação de Hard Delete (Deleta permanentemente) ---
            // _appDbContext.Clientes.Remove(cliente);
            // await _appDbContext.SaveChangesAsync();
            */

            return NoContent(); // Retorno padrão para DELETE bem-sucedido (204)
        }
    }
}

//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using MinhaAPI.Data;
//using MinhaAPI.Models;

//namespace MinhaAPI.Controllers
//{
//        [Route("api/[controller]")]
//        [ApiController]
//        public class ClienteController : ControllerBase 
//        {
//            private readonly AppDbContext _appDbContext;

//            public ClienteController(AppDbContext appContext) {

//                _appDbContext = appContext;

//            }

//        [HttpGet]
//        public async Task<IActionResult> GetAll() {

//            var clientes = await _appDbContext.Clientes
//                .Include(cliente => cliente.Enderecos)
//                .Select(cliente => new {

//                    Id = cliente.Id,
//                    Nome = cliente.Nome,
//                    Email = cliente.Email,
//                    Enderecos = cliente.Enderecos.Select(endereco => new {
//                        Id = endereco.Id,
//                        Logradouro = endereco.Logradouro,
//                        Bairro = endereco.Bairro,
//                        Cidade = endereco.Cidade

//                    }).ToList()

//                }).ToListAsync();
//            return Ok(clientes);


//        }

//        [HttpGet("{id}")]
//        public async Task<IActionResult> GetById([FromRoute]int id)
//        {

//            var clientes = await _appDbContext.Clientes
//                .Include(cliente => cliente.Enderecos)
//                .FirstOrDefaultAsync(cliente => cliente.Id == id);
//            return Ok(clientes);

//        }

//        [HttpPost]
//        public async Task<ActionResult> Create([FromBody] Cliente cliente) 
//        {
//            if (!ModelState.IsValid)
//            {
//                return BadRequest(ModelState);
//            }

//            _appDbContext.Clientes.Add(cliente);
//            await _appDbContext.SaveChangesAsync();
//            return Ok("Cliente criado com sucesso!");

//        }

//    }
//}

////Add-Migration RelacionamentoEntidades

