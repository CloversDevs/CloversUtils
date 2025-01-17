using UnityEditor;

namespace Clovers.Tools.Editor
{
    [CustomPropertyDrawer(typeof(ObservableStringReference))]
    public class ObservableStringReferenceDrawer : ObservableFieldReferenceDrawer<string>
    {
        //
    }
}