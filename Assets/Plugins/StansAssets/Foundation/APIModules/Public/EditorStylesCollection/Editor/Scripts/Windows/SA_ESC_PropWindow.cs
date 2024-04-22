using System.Collections.Generic;
using SA.Foundation.Utility;
using UnityEditor;
using UnityEngine;

namespace SA.Foundation.EditorStylesCollection
{

    public class SA_ESC_PropWindow : EditorWindow
    {
        private Vector2 m_sectionScrollPos;
        private List<SA_ESC_PropertiesSection> m_sections;
        private static SA_ESC_PropertiesContants m_constants = null;

        private int SelectedTabIndex = 0;

        private void OnEnable() {
            m_sections = new List<SA_ESC_PropertiesSection>();
            m_sections.Add(new SA_ESC_PropertiesSection("HelpBox", new SA_ESC_BoxPanel(this)));
            m_sections.Add(new SA_ESC_PropertiesSection("Insertion", new SA_ESC_InsertionPanel(this)));

            m_sections.Add(new SA_ESC_PropertiesSection("Sliders", new SA_ESC_SlidersPanel(this)));
            m_sections.Add(new SA_ESC_PropertiesSection("Lists", new SA_ESC_ListsPanel(this)));
            m_sections.Add(new SA_ESC_PropertiesSection("Others", new SA_ESC_OthersPanel(this)));
            m_sections.Add(new SA_ESC_PropertiesSection("Icons", new SA_ESC_IconsCollection(this)));



        }

        void OnGUI() {
            GUI.changed = false;
            EditorGUIUtility.labelWidth = 200f;

            GUILayout.BeginHorizontal(new GUILayoutOption[0]);

            m_sectionScrollPos = GUILayout.BeginScrollView(this.m_sectionScrollPos, Constants.sectionScrollView, new GUILayoutOption[] { GUILayout.Width(250f) });

            for (int i = 0; i < m_sections.Count; i++) {
                var section = m_sections[i];

                Rect rect = GUILayoutUtility.GetRect(section.Content, Constants.sectionElement, new GUILayoutOption[] { GUILayout.ExpandWidth(true) });

                if (section == SelectedSection && Event.current.type == EventType.Repaint) {

                    Color color;
                    if (EditorGUIUtility.isProSkin)
                    {
                        color = new Color(62f / 255f, 95f / 255f, 150f / 255f, 1f);
                    }
                    else
                    {
                        color = new Color(62f / 255f, 125f / 255f, 231f / 255f, 1f);
                    }

                    GUI.DrawTexture(rect, SA_IconManager.GetIcon(color));
                }

                EditorGUI.BeginChangeCheck();
                if (GUI.Toggle(rect, SelectedTabIndex == i, section.Content, Constants.sectionElement)) {
                    if (section.CanBeSelected) {
                        SelectedTabIndex = i;
                    }

                }
                if (EditorGUI.EndChangeCheck()) {
                    GUIUtility.keyboardControl = 0;
                }
            }

            GUILayout.EndScrollView();
            GUILayout.Space(10f);

            GUILayout.BeginVertical(new GUILayoutOption[0]);
            SelectedSection.Draw();
            GUILayout.Space(5f);
            GUILayout.EndVertical();

            GUILayout.Space(10f);
            GUILayout.EndHorizontal();
        }

        private SA_ESC_PropertiesSection SelectedSection {
            get {
                return m_sections[SelectedTabIndex];
            }
        }

        public static SA_ESC_PropertiesContants Constants {
            get {
                if (m_constants == null) {
                    m_constants = new SA_ESC_PropertiesContants();
                }

                return m_constants;
            }
        }
    }
}