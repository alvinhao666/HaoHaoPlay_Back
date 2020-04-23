namespace Hao.Core
{
    public interface ICurrentUser
    {
        public long Id { get; }

        public string Name { get; }

        public string Jti { get; }
    }
}
