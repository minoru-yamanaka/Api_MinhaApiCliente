using ApiClientes.Data;                 // Importa o contexto de dados (AppDbContext) da aplicação
using Microsoft.AspNetCore.Http;        // Fornece tipos para manipulação de requisições e respostas HTTP
using Microsoft.AspNetCore.Mvc;         // Necessário para criar Controllers e usar atributos do ASP.NET Core MVC
using Microsoft.EntityFrameworkCore;    // Biblioteca do Entity Framework Core para operações com banco de dados

namespace ApiClientes.Controllers
{
    // Define a rota base para esse controller como "api/Endereco"
    [Route("api/[controller]")]
    [ApiController] // Informa ao ASP.NET Core que essa classe é um Controller de API
    public class EnderecoController : ControllerBase
    {
        private readonly AppDbContext _contextDb; // Contexto do banco de dados injetado via construtor

        // Construtor que recebe o AppDbContext pelo mecanismo de Injeção de Dependência
        public EnderecoController(AppDbContext contextDb)
        {
            _contextDb = contextDb;
        }

        // ========================
        // MÉTODOS DA API (CRUD)
        // ========================

        // POST api/Endereco
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Endereco endereco)
        {
            // Valida se o modelo enviado no corpo da requisição está correto
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Retorna erro 400 se inválido
            }

            // Adiciona o endereço ao contexto e salva no banco
            _contextDb.Enderecos.Add(endereco);
            await _contextDb.SaveChangesAsync();

            // Retorna 201 Created, incluindo no header a rota do recurso criado
            return CreatedAtAction(nameof(GetById), new { id = endereco.Id }, endereco);
        }

        // GET api/Endereco
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            // Busca todos os endereços do banco
            var enderecos = await _contextDb.Enderecos.ToListAsync();
            return Ok(enderecos); // Retorna 200 OK com a lista
        }

        // GET api/Endereco/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            // Busca o endereço pelo ID (chave primária)
            Endereco? endereco = await _contextDb.Enderecos.FindAsync(id);

            if (endereco == null)
            {
                // Retorna 404 se não encontrado
                return NotFound($"Endereço com o id {id} não foi encontrado");
            }
            return Ok(endereco); // Retorna 200 OK com o endereço
        }

        // PUT api/Endereco/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] Endereco endereco)
        {
            // Valida se o ID passado na rota corresponde ao ID do objeto enviado
            if (id != endereco.Id)
            {
                return BadRequest("O ID enviado não corresponde ao ID do endereço no Banco de Dados");
            }

            // Valida o modelo
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Marca a entidade como modificada para o EF Core atualizar no banco
            _contextDb.Entry(endereco).State = EntityState.Modified;
            await _contextDb.SaveChangesAsync();

            return Ok("Endereço atualizado com sucesso!");
        }

        // DELETE api/Endereco/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            // Busca o endereço pelo ID
            Endereco? endereco = await _contextDb.Enderecos.FindAsync(id);

            if (endereco == null)
            {
                return NotFound($"Endereço com o id {id} não foi encontrado");
            }

            // Remove o endereço do contexto e salva no banco
            _contextDb.Enderecos.Remove(endereco);
            await _contextDb.SaveChangesAsync();

            return Ok("Endereço deletado com sucesso!");
        }

    }
}

// -> Esse EnderecoController cobre todo o CRUD de endereços.

//O POST cria um novo endereço e retorna 201 Created.

//O GET lista todos ou busca um específico.

//O PUT atualiza (checando se o ID bate).

//O DELETE remove um endereço.
