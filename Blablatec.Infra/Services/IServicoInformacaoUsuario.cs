namespace Blablatec.Infra.Services
{
    public interface IServiceInformationUser
    {
        public string IdUsuario { get; set; }
        public string Ra { get; set; }
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public string Token { get; set; }
    }
}