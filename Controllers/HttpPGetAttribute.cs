
namespace MinhaAPI.Controllers
{
    internal class HttpPGetAttribute : Attribute
    {
        private string v;

        public HttpPGetAttribute(string v)
        {
            this.v = v;
        }
    }
}