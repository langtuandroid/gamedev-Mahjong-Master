using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace SA.Foundation.Tests
{
    [CustomEditor(typeof(SA_TestsManager))]
    public class SA_TestManagerInspector : UnityEditor.Editor
    {

        private UnityEditor.Editor m_condifEditor;

        private void OnEnable() {
            UpdateConfigEditor();
        }

        public override void OnInspectorGUI() {

            EditorGUI.BeginChangeCheck();
            DrawDefaultInspector();

            if(m_condifEditor != null) {
                m_condifEditor.OnInspectorGUI();
            } else {
                EditorGUILayout.HelpBox("No test configuration assigned", MessageType.Warning);
            }
           

            if (EditorGUI.EndChangeCheck()) {
                UpdateConfigEditor();
            }



        }

        private void UpdateConfigEditor() {
            SA_TestsManager manager = (SA_TestsManager)target;
            m_condifEditor = UnityEditor.Editor.CreateEditor(manager.Config);
        }
    }
}