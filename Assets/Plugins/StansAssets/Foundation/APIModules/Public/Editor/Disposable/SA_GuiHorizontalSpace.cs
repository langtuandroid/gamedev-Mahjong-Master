using UnityEngine;
using UnityEditor;
using System;


namespace SA.Foundation.Editor
{
    public class SA_GuiHorizontalSpace : IDisposable
    {

        private int m_indentLevel;

        public SA_GuiHorizontalSpace(int space) {
            m_indentLevel = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(space);
            EditorGUILayout.BeginVertical();
        }

        public void Dispose() {
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();

            EditorGUI.indentLevel = m_indentLevel;
        }
    }
}
 

            