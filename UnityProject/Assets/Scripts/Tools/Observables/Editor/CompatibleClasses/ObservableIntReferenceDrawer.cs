using UnityEditor;

namespace Clovers.Tools.Editor
{
    [CustomPropertyDrawer(typeof(ObservableIntReference))]
    public class ObservableIntReferenceDrawer : ObservableFieldReferenceDrawer<int>
    {
        //
    }
}