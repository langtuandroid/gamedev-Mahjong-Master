using System;
using SA.Foundation.UtilitiesEditor;
using UnityEditor;
using UnityEngine;

namespace SA.Foundation.EditorStylesCollection
{

    public class SA_ESC_UIExampleElement
    {

        protected const string EXAMPLE_SOURCE_PATH = "Plugins/StansAssets/Support2018/Modules/Editor/EditorStylesCollection/Scripts/Examples/";

        private Action m_drawDelegate;
        protected GenericMenu m_genericMenu;

        protected string[] m_сontextMenuElements = new string[] { "ShowScript", "Copy" };

        protected string m_name;
        protected string m_scriptName;
        protected int m_lineNumber;

        public SA_ESC_UIExampleElement(string elementName, string scriptName, int lineNumber, Action callback) {
            m_name = elementName;

            FormScriptPath(scriptName);

            m_lineNumber = lineNumber;
            m_drawDelegate = callback;
        }

        /// <summary>
        /// Creates the generic context menu.
        /// </summary>
        public void CreateGenericContextMenu() {
            m_genericMenu = new GenericMenu();

            FillElements();

            m_genericMenu.ShowAsContext();
        }

        /// <summary>
        /// Forms the script path.
        /// </summary>
        /// <param name="scriptName">Script name.</param>
        private void FormScriptPath(string scriptName) {
            m_scriptName = EXAMPLE_SOURCE_PATH + scriptName;
        }

        /// <summary>
        /// Fills the generic context menu elements.
        /// </summary>
        private void FillElements() {
            m_genericMenu.AddItem(new GUIContent(m_сontextMenuElements[0]), false, OnScriptShowed);
            m_genericMenu.AddSeparator("");
            m_genericMenu.AddItem(new GUIContent(m_сontextMenuElements[1]), false, OnCodeCopied);
        }

        /// <summary>
        /// On script showed handler.
        /// </summary>
        void OnScriptShowed() {
            SA_EditorUtility.OpenScript(m_scriptName, m_lineNumber);
        }

        /// <summary>
        /// Copies the code to buffer.
        /// </summary>
        void OnCodeCopied() {
            //TODO
        }

        /// <summary>
        /// Gets the name of instance.
        /// </summary>
        /// <value>The name.</value>
        public string Name {
            get {
                return m_name;
            }
        }

        /// <summary>
        /// Draw this instance.
        /// </summary>
        public void Draw() {
            m_drawDelegate.Invoke();
        }
    }
}
