// Services/ICpfValidationService.cs

using System.Threading.Tasks;

namespace MinhaAPI.Services
{
    public interface ICpfValidationService
    {
        Task<bool> IsCpfValidAsync(string cpf);
    }
}