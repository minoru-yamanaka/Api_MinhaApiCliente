using System.Net.Http;          // Usado para fazer requisições HTTP
using System.Net.Http.Json;     // Extensões para lidar com JSON nas respostas/requisições

namespace ApiClientes.Services
{
    // Serviço responsável por validar CPF através de um endpoint externo
    public class ClienteService
    {
        private readonly HttpClient _httpClient; // Cliente HTTP usado para chamadas externas

        // Construtor recebe um HttpClient injetado (Dependency Injection)
        // Isso permite reutilizar instâncias e facilita testes/mocks
        public ClienteService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Método assíncrono que consulta API pública para verificar se o CPF é válido
        public async Task<bool> ValidarCpfAsync(string cpf)
        {
            // Monta a URL da API externa com o CPF
            var url = $"https://scpa-backend.saude.gov.br/public/scpa-usuario/validacao-cpf/{cpf}";

            // Faz a chamada GET
            var response = await _httpClient.GetAsync(url);

            // Caso a resposta não seja sucesso (200, 201 etc.), retorna false direto
            if (!response.IsSuccessStatusCode)
            {
                return false;
            }

            // Lê o conteúdo da resposta como string
            var responseString = await response.Content.ReadAsStringAsync();

            // Tenta converter o retorno para booleano (true/false)
            bool.TryParse(responseString, out bool isCpfValido);

            return isCpfValido;
        }
    }

    // Classe auxiliar que poderia ser usada caso a API retornasse JSON estruturado
    // Ex.: { "valido": true }
    public class ValidacaoCpfResponse
    {
        public bool Valido { get; set; }
    }
}
//-> O método assume que a API retorna "true" ou "false" em texto puro.

//Se o endpoint retornar JSON (ex.: { "valido": true }), a classe ValidacaoCpfResponse seria usada com ReadFromJsonAsync<ValidacaoCpfResponse>().

//O uso de HttpClient via DI evita problemas de socket exhaustion e segue boas práticas.

