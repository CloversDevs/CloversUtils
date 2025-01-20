using UnityEditor;

namespace Clovers.Tools.Editor
{
    [CustomPropertyDrawer(typeof(ObservableBoolReference))]
    public class ObservableBoolReferenceDrawer : ObservableFieldReferenceDrawer<bool>
    {
        //
    }
}