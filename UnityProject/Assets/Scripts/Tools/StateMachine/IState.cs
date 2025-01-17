namespace Clovers.Tools
{
    public interface IState<in T>
    {
        void OnEnter(T context);
        void OnUpdate(T context);
        void OnExit(T context);
    }
}