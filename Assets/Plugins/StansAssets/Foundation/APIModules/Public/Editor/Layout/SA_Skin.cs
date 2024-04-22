using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SA.Foundation.Config;

namespace SA.Foundation.Editor
{
    /// <summary>
    /// Contains common styles and images for Stan's Assets Editor UI's
    /// </summary>
    public static class SA_Skin
    {

        public const string ABOUT_ICONS_PATH = SA_Config.STANS_ASSETS_EDITOR_ICONS + "About/";
        public const string GENERIC_ICONS_PATH = SA_Config.STANS_ASSETS_EDITOR_ICONS + "Generic/";
        public const string SOCIAL_ICONS_PATH = SA_Config.STANS_ASSETS_EDITOR_ICONS + "Social/";


        public static Texture2D GetAboutIcon(string iconName) {
            return SA_EditorAssets.GetTextureAtPath(ABOUT_ICONS_PATH + iconName);
        }

        public static Texture2D GetGenericIcon(string iconName) {
            return SA_EditorAssets.GetTextureAtPath(GENERIC_ICONS_PATH + iconName);
        }

        public static Texture2D GetSocialIcon(string iconName) {
            return SA_EditorAssets.GetTextureAtPath(SOCIAL_ICONS_PATH + iconName);
        }


    }
}