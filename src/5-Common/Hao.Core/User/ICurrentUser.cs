namespace Hao.Core
{
    public interface ICurrentUser
    {
        long? UserId { get; set; }

        string UserName { get; set; }
    }
}
