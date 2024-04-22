using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace SA.Foundation.Editor
{

    [Serializable]
    public class SA_HyperToolbar 
    {

        [SerializeField] List<SA_HyperLabel> m_buttons;



        public void AddButtons(params SA_HyperLabel[] buttons) {

            if(m_buttons == null) {
                m_buttons = new List<SA_HyperLabel>();
            }

            foreach(var newBtn in buttons) {
                m_buttons.Add(newBtn);
            }

            ValidateButtons();
        }


        private void ValidateButtons() {

            if(m_buttons.Count == 0) {
                return;
            }


            bool hasActive = false;
            foreach (var button in m_buttons) {
              if(button.IsSelectionLock) {
                    hasActive = true;
                    break;
                }
            }

            if(!hasActive) {
                m_buttons[0].LockSelectedState(true);
            }
        }


        public void SetSelectedIndex(int index) {
           
            foreach(var button in m_buttons) {
                button.LockSelectedState(false);
            }

            var selectedButton = m_buttons[index];
            selectedButton.LockSelectedState(true);
        }



        public int Draw() {

            if(m_buttons == null) {
                return 0;
            }
           

            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.Space();

                for(int i = 0; i < m_buttons.Count; i++) {
                    var button = m_buttons[i];
                    float width = button.CalcSize().x + 5f;
                    bool clikc = button.Draw(GUILayout.Width(width));
                    if (clikc) {
                        SetSelectedIndex(m_buttons.IndexOf(button));
                    }
                }
                EditorGUILayout.Space();
            }
            EditorGUILayout.EndHorizontal();

            foreach (var button in m_buttons) {
                if(button.IsSelectionLock) {
                    return m_buttons.IndexOf(button);
                }
            }

            return 0;
        }


        public List<SA_HyperLabel> Buttons {
            get {
                return m_buttons;
            }
        }
    }
	
}
