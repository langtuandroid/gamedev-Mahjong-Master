using System.Collections.Generic;
using UnityEditor;

namespace SA.Foundation.EditorStylesCollection
{

    public class SA_ESC_InsertionPanel : SA_ESC_Panel
    {
        
        public SA_ESC_InsertionPanel(EditorWindow window) : base(window) {
            m_elements = new List<SA_ESC_UIExampleElement>();


            SA_ESC_UIExampleElement standartInsertion = new SA_ESC_UIExampleElement(
               elementName: "Standart Insertion",
               scriptName: "SA_ESC_ElementsPreviewExample.cs",
               lineNumber: 36,
               callback: () => {
                   SA_ESC_ElementsPreviewExample.CreateInsertion();
               }
           );
            m_elements.Add(standartInsertion);

            SA_ESC_UIExampleElement prInsertion = new SA_ESC_UIExampleElement(
                elementName: "PR Insertion",
                scriptName: "SA_ESC_ElementsPreviewExample.cs",
                lineNumber: 32,
                callback: () =>
                {
                    SA_ESC_ElementsPreviewExample.CreatePRInsertion();
                }
            );
			m_elements.Add(prInsertion);

			SA_ESC_UIExampleElement thinInsertion = new SA_ESC_UIExampleElement(
				elementName: "Thin Insertion",
				scriptName: "SA_ESC_ElementsPreviewExample.cs",
				lineNumber: 137,
				callback: () =>
				{
					SA_ESC_ElementsPreviewExample.CreateThinInsertion();
				}
			);

			m_elements.Add(thinInsertion);

           
        }
    }
}
