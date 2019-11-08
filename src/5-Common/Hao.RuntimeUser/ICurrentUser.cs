namespace Hao.RuntimeUser
{
    public interface ICurrentUser
    {
        string UserId { get; set; }

        string UserName { get; set; }
    }
}
