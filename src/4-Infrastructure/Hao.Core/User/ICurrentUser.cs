namespace Hao.Core
{
    public interface ICurrentUser
    {
        long Id { get; }

        string Name { get; }
    }
}
