using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapTemplateController : MonoBehaviour
{
    public MapStageController MapController;
    public Image Bg, LockIcon, MainBg;
    public Sprite LockBg, UnlockBg;
    public GameObject So, LockText;
    public Text Lock, SoNumber, Name;
    public int Index;

    private bool isLock = false;

    public void InitTemplate(int index)
    {
        Index = index;
        Name.text = Config.instance.MapName[index];
        var lastLevel = SceneManager.instance.LastLevel;
        var unlock = false;
        if (index == 0)
        {
            unlock = true;
        }
        else
        {
            var star = 0;
            for (int i = (index - 1) * Config.instance.LevelPerMap; i < index * Config.instance.LevelPerMap; i++)
            {
                if (SceneManager.instance.AllMapData.Count > i && SceneManager.instance.AllMapData[i].Score > 0)
                    star += 1;
            }
            if (star == Config.instance.LevelPerMap)
            {
                unlock = true;
            }
        }
        //unlock = true; ToDO Uncomit for open all levels
        if (unlock)
        {
            Bg.sprite = UnlockBg;
            So.SetActive(true);
            LockText.SetActive(false);
            LockIcon.color = new Color(1,1,1,0);
            MainBg.color = new Color(1,1,1,0);
            isLock = false;
            var star = 0;
            for (int i = index * Config.instance.LevelPerMap; i < (index + 1) * Config.instance.LevelPerMap; i++)
            {
                if (SceneManager.instance.AllMapData.Count > i && SceneManager.instance.AllMapData[i].Score > 0)
                    star += 1;
            }
            SoNumber.text = star + "/" + Config.instance.LevelPerMap;
        }
        else
        {
            Bg.sprite = LockBg;
            So.SetActive(false);
            LockText.SetActive(true);
            LockIcon.color = new Color(1,1,1,1);
            MainBg.color = new Color(1,1,1,1);
            isLock = true;

            var star = 0;
            for (int i = 0; i < index * Config.instance.LevelPerMap; i++)
            {
                if (SceneManager.instance.AllMapData.Count > i && SceneManager.instance.AllMapData[i].Score > 0)
                    star += 1;
            }
            Lock.text = (index * Config.instance.LevelPerMap - star) + " to unlock";
        }
    }

    public void OnThisMapClick()
    {
        if (isLock)
        {
            SceneManager.instance.MapStageController.OnShowBlock();
        }
        else
        {
            SceneManager.instance.MapIndex = Index;
            SoundManager.instance.SoundOn(SoundManager.SoundIngame.Click);
            SceneManager.instance.MapStageController.HideMap();
            SceneManager.instance.LevelController.ShowLevel();
        }
        
    }
}
