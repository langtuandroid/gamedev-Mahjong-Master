using UnityEngine;

namespace SA.Foundation.EditorStylesCollection
{

    public class SA_ESC_PropertiesSection
    {
        
        private readonly GUIContent m_content;
        private readonly SA_ESC_IPropertiesPanel m_panel;

        public SA_ESC_PropertiesSection(string name, SA_ESC_IPropertiesPanel panel) {
            m_content = new GUIContent(name);
            m_panel = panel;
        }

        public SA_ESC_PropertiesSection(string name, Texture2D icon, SA_ESC_IPropertiesPanel panel) {
            m_content = new GUIContent(name, icon);
            m_panel = panel;
        }

        public void Draw() {
            GUILayout.Label(m_content.text, SA_ESC_PropWindow.Constants.sectionHeader, new GUILayoutOption[0]);

            m_panel.OnGUI();
        }

        public GUIContent Content {
            get {
                return m_content;
            }
        }

        public bool CanBeSelected {
            get {
                return m_panel.CanBeSelected;
            }
        }

    }
}