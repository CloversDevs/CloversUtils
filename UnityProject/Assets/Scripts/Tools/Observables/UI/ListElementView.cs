using Clovers.Tools;

namespace Clovers.Tools
{
    public abstract class ListElementView<T> : ToolboxMonoBehaviour
    {
        public void Set(T value) => _elementModel.Value = value;
        protected IReadOnlyObservable<T> ElementModel => _elementModel;
        private readonly Observable<T> _elementModel = new();

        protected override void OnReady()
        {
            OnCleanup += _elementModel.Track(OnValueChange);
        }
        
        protected abstract void OnValueChange(T value);
    }
}