using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class TemplateLevelController : MonoBehaviour
{
    public List<Sprite> LevelNumber = new List<Sprite>();
    public List<Image> LevelName = new List<Image>();
    public Image Star;
    public Image BgLevel;
    public Text BestTime;
    public Image Gift;

    int level = 0;

    private MapData CurrentData;

    public void InitData(MapData currentData)
    {
        CurrentData = currentData;

        level = (CurrentData.Level % 60);
        if (level == 0)
        {
            level = 60;
        }
        foreach(var item in LevelName)
        {
            item.sprite = null;
        }

        Gift.color = new Color(1, 1, 1, 0);
        if (CurrentData.Level % 5 == 0 && CurrentData.Score == 0)
        {
            Gift.color = new Color(1, 1, 1, 1);
        }

        for (int i = 0; i < level.ToString().Length; i++)
        {
            LevelName[i].sprite = LevelNumber[int.Parse(level.ToString()[i].ToString())];
        }

        //CurrentData.IsLock = false; ToDO Uncomit for open all levels
        if (CurrentData.IsLock)
        {
            BgLevel.color = new Color(1, 1, 1, 1);
            Star.gameObject.SetActive(false);
            BestTime.text = "";
        }
        else
        {
            BgLevel.color = new Color(1, 1, 1, 0);
            
            var CurrentTime = CurrentData.Score;
            if (CurrentTime != 0)
            {
                Star.gameObject.SetActive(true);
                BestTime.text = string.Format("{0:00} : {1:00}", CurrentTime / 60, CurrentTime % 60);
            }
            else
            {
                Star.gameObject.SetActive(false);
                BestTime.text = "";
            }
        }
    }

    public void OnThisLevelClick()
    {
        SoundManager.instance.SoundOn(SoundManager.SoundIngame.Click);
        if (CurrentData != null && !CurrentData.IsLock)
        {
            SceneManager.instance.LastLevelForView = level;
            SceneManager.instance.m_GameMode = SceneManager.GameMode.Arcade;
            SceneManager.instance.LastLevel = CurrentData.Level;
            if (SceneManager.instance.LastLevel > SceneManager.instance.MaxUserLevel)
            {
                SceneManager.instance.MaxUserLevel = SceneManager.instance.LastLevel;
            }
            SceneManager.instance.CurrentMapData = CurrentData;
            SceneManager.instance.LevelController.HideLevel();
            SceneManager.instance.PlayGameController.InitTable();
            SceneManager.instance.LoadingController.ShowLoading(LoadingController.SwitchScene.PlayGame);
        }
    }
}
