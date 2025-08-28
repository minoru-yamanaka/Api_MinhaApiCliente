// Importa os namespaces necess�rios para os servi�os que ser�o configurados.
using ApiClientes.Data; // Necess�rio para referenciar o AppDbContext.
using ApiClientes.Services; // Necess�rio para referenciar o ClienteService.
using Microsoft.EntityFrameworkCore; // Necess�rio para o UseSqlServer.

namespace ApiClientes
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Cria um construtor de aplica��o web (WebApplicationBuilder).
            // � aqui que a configura��o da aplica��o come�a.
            var builder = WebApplication.CreateBuilder(args);

            // --- SE��O DE CONFIGURA��O DE SERVI�OS ---
            // Adiciona os servi�os que a aplica��o ir� utilizar ao cont�iner de inje��o de depend�ncia.

            // 1. Configura o Entity Framework Core e o DbContext.
            builder.Services.AddDbContext<AppDbContext>(options =>
                // Especifica que o provedor de banco de dados � o SQL Server.
                // Pega a string de conex�o chamada "DefaultConnection" do arquivo appsettings.json.
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // 2. Adiciona os servi�os necess�rios para os Controllers da API funcionarem.
            builder.Services.AddControllers();

            // 3. Configura o Swagger/OpenAPI para a documenta��o da API.
            // A linha abaixo � necess�ria para que o Swagger possa explorar os endpoints da API.
            builder.Services.AddEndpointsApiExplorer();
            // Adiciona o gerador do Swagger, que cria o arquivo swagger.json com a defini��o da API.
            builder.Services.AddSwaggerGen();

            // 4. Configura o HttpClient para ser injetado no ClienteService.
            // Esta � a forma recomendada de usar HttpClient, pois gerencia o ciclo de vida e o pooling de conex�es.
            builder.Services.AddHttpClient<ClienteService>();

            // Constr�i a aplica��o web com todos os servi�os configurados.
            var app = builder.Build();

            // --- SE��O DE CONFIGURA��O DO PIPELINE HTTP ---
            // Define a ordem em que as requisi��es HTTP ser�o processadas (middleware).

            // 1. Configura o pipeline para o ambiente de desenvolvimento.
            if (app.Environment.IsDevelopment())
            {
                // Habilita o middleware do Swagger para gerar o arquivo swagger.json.
                app.UseSwagger();
                // Habilita o middleware do Swagger UI, que serve a p�gina de documenta��o interativa.
                app.UseSwaggerUI();
            }

            // 2. Redireciona automaticamente todas as requisi��es HTTP para HTTPS, por seguran�a.
            app.UseHttpsRedirection();

            // 3. Habilita o middleware de autoriza��o.
            // Essencial para endpoints que usam o atributo [Authorize].
            app.UseAuthorization();

            // 4. Mapeia as rotas definidas nos seus controllers.
            // � esta linha que conecta as URLs (ex: /api/Cliente) aos seus m�todos de action.
            app.MapControllers();

            // 5. Inicia a aplica��o e a faz come�ar a ouvir por requisi��es HTTP.
            app.Run();
        }
    }
}
