// Importa os namespaces necessários para o funcionamento do controller.
using ApiClientes.Data; // Contém o DbContext para interação com o banco de dados.
using ApiClientes.Models; // Contém as classes de modelo, como 'Cliente'.
using ApiClientes.Services; // Contém serviços de lógica de negócio, como 'ClienteService'.
using Microsoft.AspNetCore.Mvc; // Fornece classes para criar APIs web, como [ApiController] e ControllerBase.
using Microsoft.EntityFrameworkCore; // Fornece funcionalidades do Entity Framework Core, como ToListAsync e Include.
using System.Linq; // Adicionado para usar .Any()

namespace ApiClientes.Controllers
{
    /// <summary>
    /// Define o endpoint base para este controller como "api/Cliente".
    /// Ex: https://localhost:7020/api/Cliente
    /// </summary>
    [Route("api/[controller]")]
    [ApiController] // Indica que esta classe é um controller de API, habilitando comportamentos específicos.
    public class ClienteController : ControllerBase
    {
        // Campo privado para armazenar a instância do DbContext, usado para consultar o banco de dados.
        private readonly AppDbContext _contextFromDb;

        // Campo privado para armazenar a instância do serviço de cliente, usado para lógicas de negócio.
        private readonly ClienteService _clienteService;

        /// <summary>
        /// Construtor do controller, responsável por receber as dependências via injeção de dependência.
        /// </summary>
        public ClienteController(AppDbContext contextFromDb, ClienteService clienteService)
        {
            _contextFromDb = contextFromDb;
            _clienteService = clienteService;
        }

        /// <summary>
        /// Endpoint para obter todos os clientes (GET /api/Cliente).
        /// Retorna uma lista simplificada de clientes.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var clientes = await _contextFromDb.Clientes
                .Select(cliente => new
                {
                    cliente.Id,
                    cliente.Nome,
                    cliente.Email,
                    cliente.Telefone,
                    cliente.DataNascimento
                })
                .ToListAsync();

            return Ok(clientes);
        }

        /// <summary>
        /// Endpoint para obter um cliente específico pelo seu ID (GET /api/Cliente/{id}).
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            Cliente? cliente = await _contextFromDb.Clientes
                .Include(cliente => cliente.Enderecos)
                .FirstOrDefaultAsync(cliente => cliente.Id == id);

            if (cliente == null)
            {
                return NotFound($"Cliente com o id {id} não foi encontrado");
            }

            return Ok(cliente);
        }

        /// <summary>
        /// Endpoint para criar um novo cliente (POST /api/Cliente).
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Cliente cliente)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!await _clienteService.ValidarCpfAsync(cliente.Cpf))
            {
                // Retornar BadRequest é geralmente melhor que lançar uma exceção para erros de validação.
                return BadRequest("CPF inválido ou já cadastrado.");
            }

            _contextFromDb.Clientes.Add(cliente);
            await _contextFromDb.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = cliente.Id }, cliente);
        }

        /// <summary>
        /// Endpoint para atualizar um cliente existente (PUT /api/Cliente/{id}).
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] Cliente cliente)
        {
            if (id != cliente.Id)
            {
                return BadRequest("O ID do cliente na URL não corresponde ao ID no corpo da solicitação.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // --- AJUSTE IMPORTANTE AQUI ---
            // Busca o cliente existente no banco, incluindo seus endereços atuais para podermos gerenciá-los.
            var existingCliente = await _contextFromDb.Clientes
                .Include(c => c.Enderecos)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (existingCliente == null)
            {
                return NotFound($"Cliente com o id {id} não foi encontrado");
            }

            // Copia os valores das propriedades simples (Nome, Cpf, Email, etc.) do objeto recebido para o objeto do banco.
            // Isso evita a necessidade de atribuir cada propriedade manualmente.
            _contextFromDb.Entry(existingCliente).CurrentValues.SetValues(cliente);

            // Gerenciamento correto da coleção de Endereços:
            // 1. Remove todos os endereços antigos que estavam associados a este cliente.
            _contextFromDb.Enderecos.RemoveRange(existingCliente.Enderecos);

            // 2. Adiciona os novos endereços que vieram na requisição.
            if (cliente.Enderecos != null && cliente.Enderecos.Any())
            {
                // Para cada novo endereço, garantimos que ele será inserido como um novo registro.
                foreach (var endereco in cliente.Enderecos)
                {
                    endereco.Id = 0; // Zera o ID para o EF entender que é um novo endereço.
                    endereco.ClienteId = existingCliente.Id; // Garante a associação correta.
                }
                // Adiciona a nova lista de endereços ao contexto.
                await _contextFromDb.Enderecos.AddRangeAsync(cliente.Enderecos);
            }

            // Define a data de atualização para o momento atual.
            existingCliente.DataUltimaAtualizacao = DateTime.UtcNow;

            // Salva todas as alterações (dados do cliente, remoção e adição de endereços) no banco.
            await _contextFromDb.SaveChangesAsync();

            return Ok(existingCliente);
        }

        /// <summary>
        /// Endpoint para deletar um cliente (DELETE /api/Cliente/{id}).
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            Cliente? cliente = await _contextFromDb.Clientes.FindAsync(id);

            if (cliente == null)
            {
                return NotFound($"Cliente com o id {id} não foi encontrado");
            }

            _contextFromDb.Clientes.Remove(cliente);
            await _contextFromDb.SaveChangesAsync();

            return Ok("Cliente deletado com sucesso!");
        }
    }
}
