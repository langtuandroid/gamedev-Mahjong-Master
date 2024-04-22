////////////////////////////////////////////////////////////////////////////////
//  
// @module Assets Common Lib
// @author Osipov Stanislav (Stan's Assets) 
// @support support@stansassets.com
// @website https://stansassets.com
//
////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System;
using System.Collections;

using SA.Foundation.Async;

namespace SA.Foundation.Utility {


	public static class SA_ScreenUtil  {


        /// <summary>
        /// Takes a screenshot.
        /// </summary>
        /// <param name="callback"> Result callback.</param> 
		public static void TakeScreenshot( Action<Texture2D> callback) {
            SA_Coroutine.Start(TakeScreenshotCoroutine(0, callback));
		}

        public static void TakeScreenshot(int maxSize, Action<Texture2D> callback) {
            SA_Coroutine.Start(TakeScreenshotCoroutine(maxSize, callback));
        }



        private static IEnumerator TakeScreenshotCoroutine(int maxSize, Action<Texture2D> callback) {

            yield return new WaitForEndOfFrame();
            // Create a texture the size of the screen, RGB24 format
            int width =  Screen.width;
            int height = Screen.height;
            Texture2D tex = new Texture2D(width, height, TextureFormat.RGB24, false);

            // Read screen contents into the texture
            tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            tex.Apply();


            int biggestSide = width;
            if(height > width) {
                biggestSide = height;
            }
            //TODO fix it, looks ugly
            if (biggestSide >  maxSize) {

                float scale = (float)maxSize / (float)biggestSide;
               
                Texture2D resized = SA_IconManager.ResizeTexture(tex, SA_IconManager.ImageFilterMode.Nearest, scale);
                tex = resized;
            }
           

            callback.Invoke(tex);

        }

    }

}

