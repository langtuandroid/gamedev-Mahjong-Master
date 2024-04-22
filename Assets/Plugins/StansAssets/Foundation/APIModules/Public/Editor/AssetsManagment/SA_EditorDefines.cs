using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace SA.Foundation.Editor
{
   
    /// <summary>
    /// Provides method for managing the <see cref="PlayerSettings"/> script defines 
    /// </summary>
    public static class SA_EditorDefines 
    {

        /// <summary>
        /// Attempts to add a new #define constant to the Player Settings
        /// </summary>
        /// <param name="newDefineCompileConstant">constant to attempt to define</param>
        /// <param name="targetGroups">platforms to add this for (default will add to all platforms)</param>
        public static void AddCompileDefine(string newDefineCompileConstant, params BuildTargetGroup[] targetGroups) {
           
            if (targetGroups.Length == 0)
                targetGroups = (BuildTargetGroup[])Enum.GetValues(typeof(BuildTargetGroup));

            foreach (BuildTargetGroup grp in targetGroups) {
                if (grp == BuildTargetGroup.Unknown)        //the unknown group does not have any constants location
                    continue;

                string defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(grp);
                if (!defines.Contains(newDefineCompileConstant)) {
                    if (defines.Length > 0)         //if the list is empty, we don't need to append a semicolon first
                        defines += ";";

                    defines += newDefineCompileConstant;
                    PlayerSettings.SetScriptingDefineSymbolsForGroup(grp, defines);
                }
            }
        }

        /// <summary>
        /// Attempts to remove a #define constant from the Player Settings
        /// </summary>
        /// <param name="defineCompileConstant">define constant</param>
        /// <param name="targetGroups">platforms to add this for (default will add to all platforms)</param>
        public static void RemoveCompileDefine(string defineCompileConstant, params BuildTargetGroup[] targetGroups) {
            if (targetGroups.Length == 0)
                targetGroups = (BuildTargetGroup[])Enum.GetValues(typeof(BuildTargetGroup));

            foreach (BuildTargetGroup grp in targetGroups) {
                string defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(grp);
                int index = defines.IndexOf(defineCompileConstant, StringComparison.CurrentCulture);
                if (index < 0)
                    continue;           //this target does not contain the define
                else if (index > 0)
                    index -= 1;         //include the semicolon before the define
                                        //else we will remove the semicolon after the define

                //Remove the word and it's semicolon, or just the word (if listed last in defines)
                int lengthToRemove = Math.Min(defineCompileConstant.Length + 1, defines.Length - index);

                //remove the constant and it's associated semicolon (if necessary)
                defines = defines.Remove(index, lengthToRemove);

                PlayerSettings.SetScriptingDefineSymbolsForGroup(grp, defines);
            }
        }
      
    }
}