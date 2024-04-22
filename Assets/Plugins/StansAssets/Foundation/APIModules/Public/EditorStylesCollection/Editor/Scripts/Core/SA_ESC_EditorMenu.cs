using UnityEditor;
using SA.Foundation.Config;

namespace SA.Foundation.EditorStylesCollection
{
    public static class SA_ESC_EditorMenu
    {
        [MenuItem(SA_Config.EDITOR_FOUNDATION_LIB_MENU_ROOT + "Editor Styles Collection", false, SA_Config.FOUNDATION_MENU_INDEX)]
        public static void ShowPropertiesWindow() {
            SA_ESC_WindowManager.ShowPropertiesWindow();
        }
    }
}