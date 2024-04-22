using UnityEditor;
using UnityEngine;
using SA.Foundation.Editor;

namespace SA.Foundation.EditorStylesCollection
{

    public static class SA_ESC_ElementsPreviewExample
    {

        public static void CreateHelpBoxError() {
            EditorGUILayout.HelpBox("Error", MessageType.Error);
        }

        public static void CreateHelpBoxInfo() {
            EditorGUILayout.HelpBox("Info", MessageType.Info);
        }

        public static void CreateHelpBoxNone() {
            EditorGUILayout.HelpBox("None", MessageType.None);
        }

        public static void CreateHelpBoxWarning() {
            EditorGUILayout.HelpBox("Warning", MessageType.Warning);
        }

        public static void CreateHelpBoxWithText() {
            GUILayout.Box("Type your text", GUI.skin.box);
        }

        public static void CreatePRInsertion() {
            SA_EditorGUILayout.HorizontalLinePR();
        }

        public static void CreateInsertion() {
            SA_EditorGUILayout.HorizontalLine();
        }

        public static void CreateIndentLevel() {
            EditorGUI.indentLevel++;
            EditorGUILayout.LabelField("Indent level 1");

            EditorGUI.indentLevel++;
            EditorGUILayout.LabelField("Indent level 2");

            EditorGUI.indentLevel--;
            EditorGUI.indentLevel--;
        }

        public static void CreatePlusMinusButtons() {
            GUILayout.BeginHorizontal();
            {
				Texture texture = SA_EditorAssets.GetTextureAtPath("Plugins/StansAssets/Support2018/APIModules/EditorStylesCollection/Editor/Resources/button_plus.png");
                GUIContent content = new GUIContent(texture);
                if (GUILayout.Button(content, GUILayout.MaxWidth(24), GUILayout.MaxHeight(24))) { }


				Texture texture1 = SA_EditorAssets.GetTextureAtPath("Plugins/StansAssets/Support2018/APIModules/EditorStylesCollection/Editor/Resources/button_minus.png");
                GUIContent content1 = new GUIContent(texture1);
                if (GUILayout.Button(content1, GUILayout.MaxWidth(24), GUILayout.MaxHeight(24))) { }
            }
            GUILayout.EndHorizontal();
        }

        public static void CreateGroupButtons() {
            GUILayout.Space(10f);
            GUILayout.BeginHorizontal();
            {
                GUILayout.Space(10f);

                if (GUILayout.Button("Left", EditorStyles.miniButtonLeft)) { }
                if (GUILayout.Button("Middle", EditorStyles.miniButtonMid)) { }
                if (GUILayout.Button("Right", EditorStyles.miniButtonRight)) { }

                GUILayout.Space(10f);
            }
            GUILayout.EndHorizontal();
        }

        public static void CreateEnumsButtons() {
            GUILayout.Button("Upper", EditorStyles.miniButton, GUILayout.MinWidth(100f), GUILayout.MaxWidth(100f));
            GUILayout.Space(-3f);
            GUILayout.Button("Middle", EditorStyles.miniButton, GUILayout.MinWidth(100f), GUILayout.MaxWidth(100f));
            GUILayout.Space(-3f);
            GUILayout.Button("Bottom", EditorStyles.miniButton, GUILayout.MinWidth(100f), GUILayout.MaxWidth(100f));
        }

        private static float s_ssCurrentValue = 40f;

        private static float s_ssMinLimit = 0f;
        private static float s_ssMaxLimit = 100f;

        public static void CreateSimpleSlider() {
            GUILayout.Space(10f);
            GUILayout.BeginHorizontal();
            {
                s_ssCurrentValue = GUILayout.HorizontalSlider(s_ssCurrentValue, s_ssMinLimit, s_ssMaxLimit);
                GUILayout.TextField(s_ssCurrentValue.ToString(), GUILayout.MinWidth(50f), GUILayout.MaxWidth(50f));
            }
            GUILayout.EndHorizontal();
        }

        private static float s_rsMinLimit = -10f;
        private static float s_rsMaxLimit = 10f;

        private static float s_rsMinCurrentValue = -3f;
        private static float s_rsMaxCurrentValue = 3f;

        public static void CreateRangeSlider() {
            GUILayout.Space(10f);
            GUILayout.BeginHorizontal();
            {
                EditorGUILayout.MinMaxSlider(ref s_rsMinCurrentValue, ref s_rsMaxCurrentValue, s_rsMinLimit, s_rsMaxLimit);
                GUILayout.TextField(s_rsMinCurrentValue.ToString(), GUILayout.MinWidth(50f), GUILayout.MaxWidth(50f));
                GUILayout.TextField(s_rsMaxCurrentValue.ToString(), GUILayout.MinWidth(50f), GUILayout.MaxWidth(50f));
            }
            GUILayout.EndHorizontal();
        }

        private static bool show;
        private static Vector3 vctr;

        public static void CreateSimpleFoldout() {
            show = EditorGUILayout.Foldout(show, "Show example");
            if (show) {
                vctr = EditorGUILayout.Vector3Field("", vctr);
            }
        }

        public static void CreateLabelWithIcon() {
			Texture texture = SA_EditorAssets.GetTextureAtPath("Plugins/StansAssets/Support2018/APIModules/EditorStylesCollection/Editor/Resources/button_plus.png");
            GUIContent content = new GUIContent("GameObject", texture);
            GUILayout.Label(content, GUILayout.MaxWidth(128f), GUILayout.MaxHeight(24f));
        }

		public static void CreateThinInsertion() {
			SA_EditorGUILayout.HorizontalLineThin();
		}
    }
}
