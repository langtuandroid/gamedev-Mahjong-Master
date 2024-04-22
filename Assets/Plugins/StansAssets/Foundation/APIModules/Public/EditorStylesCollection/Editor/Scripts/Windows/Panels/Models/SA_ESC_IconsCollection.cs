using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SA.Foundation.EditorStylesCollection
{

    public class SA_ESC_IconsCollection : SA_ESC_Panel
    {
        
        public SA_ESC_IconsCollection(EditorWindow window) : base(window) {
            m_elements = new List<SA_ESC_UIExampleElement>();


            SA_ESC_UIExampleElement standartInsertion = new SA_ESC_UIExampleElement(
               elementName: "Unity Icons",
               scriptName: "SA_ESC_ElementsPreviewExample.cs",
               lineNumber: 36,
               callback: () => {

                    EditorGUILayout.LabelField(new GUIContent("Folder Icon", EditorGUIUtility.FindTexture("Folder Icon")));
                    EditorGUILayout.LabelField(new GUIContent("AudioSource Icon", EditorGUIUtility.FindTexture("AudioSource Icon"))); 
                    EditorGUILayout.LabelField(new GUIContent("Camera Icon", EditorGUIUtility.FindTexture("Camera Icon")));
                    EditorGUILayout.LabelField(new GUIContent("Windzone Icon", EditorGUIUtility.FindTexture("Windzone Icon")));
                    EditorGUILayout.LabelField(new GUIContent("GameObject Icon", EditorGUIUtility.FindTexture("GameObject Icon")));

                   EditorGUILayout.LabelField(new GUIContent("Console Error Icon", EditorGUIUtility.FindTexture("d_console.erroricon.sml")));
                   EditorGUILayout.LabelField(new GUIContent("Console Warn Icon", EditorGUIUtility.FindTexture("d_console.warnicon.sml")));
                   EditorGUILayout.LabelField(new GUIContent("Console Info Icon", EditorGUIUtility.FindTexture("d_console.infoicon.sml")));

               }
           );
            m_elements.Add(standartInsertion);

           

           
        }
    }
}
