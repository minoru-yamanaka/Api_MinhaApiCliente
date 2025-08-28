// Importa os namespaces necessários para os serviços que serão configurados.
using ApiClientes.Data; // Necessário para referenciar o AppDbContext.
using ApiClientes.Services; // Necessário para referenciar o ClienteService.
using Microsoft.EntityFrameworkCore; // Necessário para o UseSqlServer.

namespace ApiClientes
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Cria um construtor de aplicação web (WebApplicationBuilder).
            // É aqui que a configuração da aplicação começa.
            var builder = WebApplication.CreateBuilder(args);

            // --- SEÇÃO DE CONFIGURAÇÃO DE SERVIÇOS ---
            // Adiciona os serviços que a aplicação irá utilizar ao contêiner de injeção de dependência.

            // 1. Configura o Entity Framework Core e o DbContext.
            builder.Services.AddDbContext<AppDbContext>(options =>
                // Especifica que o provedor de banco de dados é o SQL Server.
                // Pega a string de conexão chamada "DefaultConnection" do arquivo appsettings.json.
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // 2. Adiciona os serviços necessários para os Controllers da API funcionarem.
            builder.Services.AddControllers();

            // 3. Configura o Swagger/OpenAPI para a documentação da API.
            // A linha abaixo é necessária para que o Swagger possa explorar os endpoints da API.
            builder.Services.AddEndpointsApiExplorer();
            // Adiciona o gerador do Swagger, que cria o arquivo swagger.json com a definição da API.
            builder.Services.AddSwaggerGen();

            // 4. Configura o HttpClient para ser injetado no ClienteService.
            // Esta é a forma recomendada de usar HttpClient, pois gerencia o ciclo de vida e o pooling de conexões.
            builder.Services.AddHttpClient<ClienteService>();

            // Constrói a aplicação web com todos os serviços configurados.
            var app = builder.Build();

            // --- SEÇÃO DE CONFIGURAÇÃO DO PIPELINE HTTP ---
            // Define a ordem em que as requisições HTTP serão processadas (middleware).

            // 1. Configura o pipeline para o ambiente de desenvolvimento.
            if (app.Environment.IsDevelopment())
            {
                // Habilita o middleware do Swagger para gerar o arquivo swagger.json.
                app.UseSwagger();
                // Habilita o middleware do Swagger UI, que serve a página de documentação interativa.
                app.UseSwaggerUI();
            }

            // 2. Redireciona automaticamente todas as requisições HTTP para HTTPS, por segurança.
            app.UseHttpsRedirection();

            // 3. Habilita o middleware de autorização.
            // Essencial para endpoints que usam o atributo [Authorize].
            app.UseAuthorization();

            // 4. Mapeia as rotas definidas nos seus controllers.
            // É esta linha que conecta as URLs (ex: /api/Cliente) aos seus métodos de action.
            app.MapControllers();

            // 5. Inicia a aplicação e a faz começar a ouvir por requisições HTTP.
            app.Run();
        }
    }
}
