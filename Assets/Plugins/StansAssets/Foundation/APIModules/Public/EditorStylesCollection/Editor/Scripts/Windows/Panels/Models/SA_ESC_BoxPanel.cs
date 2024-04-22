using System.Collections.Generic;
using UnityEditor;

namespace SA.Foundation.EditorStylesCollection
{

    public class SA_ESC_BoxPanel : SA_ESC_Panel
    {
        
        public SA_ESC_BoxPanel(EditorWindow window) : base(window) {
            m_elements = new List<SA_ESC_UIExampleElement>();

            SA_ESC_UIExampleElement helpBoxError = new SA_ESC_UIExampleElement(
                elementName: "HelpBox Error",
                scriptName: "SA_ESC_ElementsPreviewExample.cs",
                lineNumber: 12,
                callback: () =>
                {
                    SA_ESC_ElementsPreviewExample.CreateHelpBoxError();
                }
            );
            m_elements.Add(helpBoxError);


            SA_ESC_UIExampleElement helpBoxInfo = new SA_ESC_UIExampleElement(
                elementName: "HelpBox Info",
                scriptName: "SA_ESC_ElementsPreviewExample.cs",
                lineNumber: 16,
                callback: () =>
                {
                    SA_ESC_ElementsPreviewExample.CreateHelpBoxInfo();
                }
            );
            m_elements.Add(helpBoxInfo);


            SA_ESC_UIExampleElement helpBoxNone = new SA_ESC_UIExampleElement(
                elementName: "HelpBox None",
                scriptName: "SA_ESC_ElementsPreviewExample.cs",
                lineNumber: 20,
                callback: () =>
                {
                    SA_ESC_ElementsPreviewExample.CreateHelpBoxNone();
                }
            );
            m_elements.Add(helpBoxNone);


            SA_ESC_UIExampleElement helpBoxWarning = new SA_ESC_UIExampleElement(
                elementName: "HelpBox Warning",
                scriptName: "SA_ESC_ElementsPreviewExample.cs",
                lineNumber: 24,
                callback: () =>
                {
                    SA_ESC_ElementsPreviewExample.CreateHelpBoxWarning();
                }
            );
            m_elements.Add(helpBoxWarning);


            SA_ESC_UIExampleElement helpBoxWithText = new SA_ESC_UIExampleElement(
                elementName: "HelpBox with Text",
                scriptName: "SA_ESC_ElementsPreviewExample.cs",
                lineNumber: 28,
                callback: () =>
                {
                    SA_ESC_ElementsPreviewExample.CreateHelpBoxWithText();
                }
            );
            m_elements.Add(helpBoxWithText);
        }
    }
}
