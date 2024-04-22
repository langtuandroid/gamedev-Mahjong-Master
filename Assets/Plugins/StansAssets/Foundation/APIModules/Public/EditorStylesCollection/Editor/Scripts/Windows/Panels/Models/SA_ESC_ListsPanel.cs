using System.Collections.Generic;
using UnityEditor;

namespace SA.Foundation.EditorStylesCollection
{

    public class SA_ESC_ListsPanel : SA_ESC_Panel
    {
        
        public SA_ESC_ListsPanel(EditorWindow window) : base(window) {
            m_elements = new List<SA_ESC_UIExampleElement>();

            SA_ESC_UIExampleElement rotorzList = new SA_ESC_UIExampleElement(
                elementName: "Rotorz Reorderable List",
                scriptName: "SA_ESC_ListsPreviewExample.cs",
                lineNumber: 13,
                callback: () =>
                {
                    SA_ESC_ListsPreviewExample.RotorzReorderableListExample();
                }
            );
            m_elements.Add(rotorzList);



            SA_ESC_UIExampleElement saList = new SA_ESC_UIExampleElement(
              elementName: "Stan's Assets Reorderable List",
              scriptName: "SA_ESC_ListsPreviewExample.cs",
              lineNumber: 33,
              callback: () => {
                    SA_ESC_ListsPreviewExample.SAReorderableListExample();
              }
          );
            m_elements.Add(saList);
          
        }
    }
}
