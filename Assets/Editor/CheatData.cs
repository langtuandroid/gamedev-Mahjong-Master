using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;

public class CheatData : MonoBehaviour {

    [MenuItem("CheatData/Clear UserData")]
    private static void ClearData()
    {
        PlayerPrefs.DeleteAll();
    }

    [MenuItem("CheatData/Cheat Win")]
    private static void CheatWin()
    {
        if (SceneManager.instance.PlayGameController.GetComponent<CanvasGroup>().alpha == 1)
            SceneManager.instance.PlayGameController.WinGame();
    }
    
    [MenuItem("CheatData/Add hint")]
    private static void AddHint()
    {
        SceneManager.instance.AddHint(100);
    }
}
