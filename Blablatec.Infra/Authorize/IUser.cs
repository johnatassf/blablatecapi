namespace Blablatec.Infra.Authorize
{
    public interface IUser
    {
        string Ra { get; set; }
        string Password { get; set; }
    }
}