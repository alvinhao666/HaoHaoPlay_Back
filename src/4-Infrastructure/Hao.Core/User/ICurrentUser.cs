namespace Hao.Core
{
    public interface ICurrentUser
    {
        long? Id { get; set; }

        string Name { get; set; }
    }
}
