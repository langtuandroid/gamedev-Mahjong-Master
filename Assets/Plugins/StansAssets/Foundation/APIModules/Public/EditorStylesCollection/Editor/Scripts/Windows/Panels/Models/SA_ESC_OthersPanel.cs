using System.Collections.Generic;
using UnityEditor;

namespace SA.Foundation.EditorStylesCollection
{

    public class SA_ESC_OthersPanel : SA_ESC_Panel
    {
        
        public SA_ESC_OthersPanel(EditorWindow window) : base(window) {
            m_elements = new List<SA_ESC_UIExampleElement>();

            SA_ESC_UIExampleElement indentLevel = new SA_ESC_UIExampleElement(
                elementName: "Indent Level",
                scriptName: "SA_ESC_ElementsPreviewExample.cs",
                lineNumber: 40,
                callback: () =>
                {
                    SA_ESC_ElementsPreviewExample.CreateIndentLevel();
                }
            );
            m_elements.Add(indentLevel);


            SA_ESC_UIExampleElement plusMinusButtons = new SA_ESC_UIExampleElement(
                elementName: "Plus Minus Buttons",
                scriptName: "SA_ESC_ElementsPreviewExample.cs",
                lineNumber: 51,
                callback: () =>
                {
                    SA_ESC_ElementsPreviewExample.CreatePlusMinusButtons();
                }
            );
            m_elements.Add(plusMinusButtons);


            SA_ESC_UIExampleElement groupButtons = new SA_ESC_UIExampleElement(
                elementName: "Group Buttons",
                scriptName: "SA_ESC_ElementsPreviewExample.cs",
                lineNumber: 66,
                callback: () =>
                {
                    SA_ESC_ElementsPreviewExample.CreateGroupButtons();
                }
            );
            m_elements.Add(groupButtons);


            SA_ESC_UIExampleElement enumButtons = new SA_ESC_UIExampleElement(
                elementName: "Enum Buttons",
                scriptName: "SA_ESC_ElementsPreviewExample.cs",
                lineNumber: 81,
                callback: () =>
                {
                    SA_ESC_ElementsPreviewExample.CreateEnumsButtons();
                }
            );
            m_elements.Add(enumButtons);


            SA_ESC_UIExampleElement simpleFoldout = new SA_ESC_UIExampleElement(
                elementName: "Simple Foldout",
                scriptName: "SA_ESC_ElementsPreviewExample.cs",
                lineNumber: 124,
                callback: () =>
                {
                    SA_ESC_ElementsPreviewExample.CreateSimpleFoldout();
                }
            );
            m_elements.Add(simpleFoldout);

            SA_ESC_UIExampleElement labelIcon = new SA_ESC_UIExampleElement(
                elementName: "Label with Icon",
                scriptName: "SA_ESC_ElementsPreviewExample.cs",
                lineNumber: 131,
                callback: () =>
                {
                    SA_ESC_ElementsPreviewExample.CreateLabelWithIcon();
                }
            );
            m_elements.Add(labelIcon);
        }
    }
}
