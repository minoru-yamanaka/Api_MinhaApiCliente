using System.Net.Http;
using System.Net.Http.Json;

namespace ApiClientes.Services
{
    public class ClienteService {
        private readonly HttpClient _httpClient;
        public ClienteService(HttpClient httpClient)  // Injetar HttpClient via DI
        {
            _httpClient = httpClient;
        }

        public async Task<bool> ValidarCpfAsync(string cpf)
        {
            var url = $"https://scpa-backend.saude.gov.br/public/scpa-usuario/validacao-cpf/{cpf}";
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                return false;  
            }

            var responseString = await response.Content.ReadAsStringAsync();
            bool.TryParse(responseString, out bool isCpfValido);

            return isCpfValido;
        }

    }
    public class ValidacaoCpfResponse
    {
        public bool Valido { get; set; }
       
    }
}
