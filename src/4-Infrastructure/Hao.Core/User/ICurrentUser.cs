namespace Hao.Core
{
    public interface ICurrentUser
    {
        public long Id { get; }

        public string Name { get; }

        public int RoleLevel { get; }

        public string Jti { get; }
    }
}
