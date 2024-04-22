using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

using SA.Foundation.Editor;
using Rotorz.ReorderableList;


namespace SA.Foundation.EditorStylesCollection
{

    public static class SA_ESC_ListsPreviewExample
    {
        private static List<string> s_someList = new List<string>();


        public static void RotorzReorderableListExample() {


            ReorderableListGUI.Title("String's list");
            ReorderableListGUI.ListField<string>(s_someList, 
                (Rect position, string item) => {
                    //draw item
                    return EditorGUI.TextField(position, item);
                }, 
                () => {
                    //draw empty
                    EditorGUILayout.LabelField("Empty Item");
                } );
        }


        public static void SAReorderableListExample() {
            SA_EditorGUILayout.ReorderablList(s_someList,
                (string item) => {
                    //draw item name (Required)
                    return item;
                },
                (string item) => {
                    //draw item content (Optional)
                    EditorGUILayout.LabelField("Item Body");
                    item = EditorGUILayout.TextField(item);
                    EditorGUILayout.LabelField("Item Body");
                },
                () => {
                    //draw item add button (Optional)
                    s_someList.Add("New Item");
                }
             );
        }


       
    }
}
