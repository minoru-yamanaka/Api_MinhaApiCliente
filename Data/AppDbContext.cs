using ApiClientes.Models;          // Importa as classes de modelo (Cliente e Endereco)
using Microsoft.EntityFrameworkCore; // Necessário para usar o Entity Framework Core

namespace ApiClientes.Data
{
    // Classe responsável por gerenciar a conexão e o mapeamento com o banco de dados
    public class AppDbContext : DbContext
    {
        // Construtor recebe as opções do contexto (string de conexão, provedores etc.)
        // Esse padrão permite que o contexto seja configurado via Injeção de Dependência no Program.cs
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Cria uma tabela "Clientes" no banco a partir do modelo Cliente
        public DbSet<Cliente> Clientes { get; set; }

        // Cria uma tabela "Enderecos" no banco a partir do modelo Endereco
        public DbSet<Endereco> Enderecos { get; set; }
    }
}

//-> Em resumo:

//O AppDbContext herda de DbContext e define as tabelas que existirão no banco.

//DbSet<Cliente> e DbSet<Endereco> representam as coleções que o EF Core vai mapear para tabelas.

//O construtor com DbContextOptions é o que permite configurar SQL Server, migrations, e a injeção no Program.cs.