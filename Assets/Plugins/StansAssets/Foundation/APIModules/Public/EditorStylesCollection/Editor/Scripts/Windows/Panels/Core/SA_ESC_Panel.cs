using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SA.Foundation.EditorStylesCollection
{
    public abstract class SA_ESC_Panel : SA_ESC_IPropertiesPanel
    {

        protected const int ELEMENTS_IN_ROW_LIMIT = 2;

        protected List<SA_ESC_UIExampleElement> m_elements;

        private readonly EditorWindow m_window;

        public SA_ESC_Panel(EditorWindow window) {
            m_window = window;
        }

        public void OnGUI() {
            Draw();

            Window.Repaint();
        }

        private void Draw() {
            int lines = CountLines();

            int currentElementIndex = 0;

            for (int i = 0; i < lines; i++) {

                GUILayout.BeginHorizontal(GUILayout.MaxWidth(800f));
                {
                    int currentCountElementsInRow = 0;

                    for (int j = currentElementIndex; j < m_elements.Count; j++)
                    {

                        GUILayout.BeginVertical(GUI.skin.box, GUILayout.MaxWidth(350f), GUILayout.MaxHeight(80f));
                        {
                            GUILayout.BeginHorizontal();
                            {
                                GUILayout.BeginVertical();
                                {
                                    GUILayout.Space(5);
                                    GUILayout.Label(m_elements[j].Name, EditorStyles.boldLabel);
                                }
                                GUILayout.EndVertical();

                                GUILayout.FlexibleSpace();

                                GUILayout.BeginVertical();
                                {
                                    GUILayout.Space(10);
                                    if (GUILayout.Button("", (GUIStyle)"PaneOptions")) {
                                        m_elements[j].CreateGenericContextMenu();
                                    }
                                }
                                GUILayout.EndVertical();
                            }
                            GUILayout.EndHorizontal();

                            GUILayout.Space(5);
                            m_elements[j].Draw();
                        }
                        GUILayout.EndVertical();



                        currentElementIndex++;
                        currentCountElementsInRow++;

                        if (currentCountElementsInRow >= ELEMENTS_IN_ROW_LIMIT)
                            break;
                    }
                }
                GUILayout.EndHorizontal();
            }
        }

        private int CountLines() {
            int remainder = m_elements.Count % 2;

            return (m_elements.Count / 2) + remainder;
        }

        public EditorWindow Window {
            get {
                return m_window;
            }
        }

        public virtual bool CanBeSelected {
            get {
                return true;
            }
        }
    }
}
