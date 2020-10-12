namespace Hao.Dependency
{
    /// <summary>
    /// 每次都是一个新实例
    /// 依赖注入接口，表示该接口的实现类将自动注册到IoC容器中
    /// 为了统一管理 IoC 相关的代码，并避免在底层类库中到处引用 Autofac 这个第三方组件，定义了一个专门用于管理需要依赖注入的接口与实现类的空接口 
    /// 这个接口没有任何方法，不会对系统的业务逻辑造成污染，所有需要进行依赖注入的接口，都要继承这个空接口，例如：
    /// Autofac 是支持批量子类注册的，有了 IDependency 这个基接口，我们只需要 Global 中很简单的几行代码，就可以完成整个系统的依赖注入匹配
    /// </summary>
    public interface ITransientDependency
    {
    }
}
