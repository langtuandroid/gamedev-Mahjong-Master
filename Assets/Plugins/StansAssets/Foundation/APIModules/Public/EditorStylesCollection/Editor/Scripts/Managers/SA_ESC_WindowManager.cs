using UnityEngine;
using UnityEditor;

namespace SA.Foundation.EditorStylesCollection
{
    public static class SA_ESC_WindowManager
    {

        public static void ShowPropertiesWindow() {
            Window.minSize = new Vector2(1000f, 500f);
            Window.maxSize = new Vector2(Window.minSize.x, Window.maxSize.y);
            Window.position = new Rect(new Vector2(100f, 100f), Window.minSize);
            Window.Show();
        }

        public static SA_ESC_PropWindow Window {
            get {
                return EditorWindow.GetWindow<SA_ESC_PropWindow>(true, "Properties Window");
            }
        }
    }
}