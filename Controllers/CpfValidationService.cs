// Services/CpfValidationService.cs

using System.Threading.Tasks;

namespace MinhaAPI.Services
{
    public class CpfValidationService : ICpfValidationService
    {
        // Em um cenário real, você injetaria o IHttpClientFactory
        // private readonly HttpClient _httpClient;
        // public CpfValidationService(IHttpClientFactory httpClientFactory)
        // {
        //     _httpClient = httpClientFactory.CreateClient("CpfApi");
        // }

        public async Task<bool> IsCpfValidAsync(string cpf)
        {
            // --- LÓGICA DE SIMULAÇÃO ---
            // Em um projeto real, aqui você faria a chamada HTTP para a API externa.
            // Ex: var response = await _httpClient.GetAsync($"https://api.validador.com/cpf/{cpf}");
            // return response.IsSuccessStatusCode;

            if (string.IsNullOrWhiteSpace(cpf) || cpf.Length != 11 || !cpf.All(char.IsDigit))
            {
                return await Task.FromResult(false);
            }

            // Simplesmente para simular uma falha: CPFs terminados em "00" são inválidos.
            if (cpf.EndsWith("00"))
            {
                return await Task.FromResult(false); // Simula um CPF inválido na API externa
            }

            return await Task.FromResult(true); // Simula um CPF válido
        }
    }
}