using System.Collections.Generic;
using UnityEditor;

namespace SA.Foundation.EditorStylesCollection
{

    public class SA_ESC_SlidersPanel : SA_ESC_Panel
    {

        public SA_ESC_SlidersPanel(EditorWindow window) : base(window) {
            m_elements = new List<SA_ESC_UIExampleElement>();

            SA_ESC_UIExampleElement simpleSlider = new SA_ESC_UIExampleElement(
                elementName: "Simple Slider",
                scriptName: "SA_ESC_ElementsPreviewExample.cs",
                lineNumber: 94,
                callback: () =>
                {
                    SA_ESC_ElementsPreviewExample.CreateSimpleSlider();
                }
            );
            m_elements.Add(simpleSlider);


            SA_ESC_UIExampleElement rangeSlider = new SA_ESC_UIExampleElement(
                elementName: "Range Slider",
                scriptName: "SA_ESC_ElementsPreviewExample.cs",
                lineNumber: 110,
                callback: () =>
                {
                    SA_ESC_ElementsPreviewExample.CreateRangeSlider();
                }
            );
            m_elements.Add(rangeSlider);
        }
    }
}
