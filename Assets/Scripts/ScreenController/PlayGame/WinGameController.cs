using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class WinGameController : MonoBehaviour
{
    public Sprite Active, Deactive;
    public List<Image> IconHint;

    public Text ScoreText;
    public Text Time;
    public GameObject HintObj;
    int minutes = 0;
    int seconds = 0;

    public List<Text> CurrentLevel;
    public GameObject Border;

    public Button NextLevel;

    public void InitStar(int best, int curTime)
    {
        seconds = best % 60;
        minutes = best / 60;
        if (minutes >= 99)
            minutes = 99;
        ScoreText.text = string.Format("{0:00} : {1:00}", minutes, seconds);

        seconds = curTime % 60;
        minutes = curTime / 60;
        if (minutes >= 99)
            minutes = 99;
        Time.text = string.Format("{0:00} : {1:00}", minutes, seconds);
        CheckHint();
        
        NextLevel.interactable = SceneManager.instance.LastLevelForView != 60;
    }

    public void OnNextLevel()
    {
        SoundManager.instance.SoundOn(SoundManager.SoundIngame.Click);
        gameObject.SetActive(false);
        SceneManager.instance.PlayGameController.HidePlayGame();
        if (SceneManager.instance.m_GameMode != SceneManager.GameMode.Daily)
        {
            int level = SceneManager.instance.CurrentMapData.Level;
            if (SceneManager.instance.AllMapData.Count > level)
            {
                SceneManager.instance.CurrentMapData = SceneManager.instance.AllMapData[level];
                SceneManager.instance.LastLevel = SceneManager.instance.CurrentMapData.Level;
                SceneManager.instance.PreLastLevel = SceneManager.instance.LastLevel;
                var tempLevel= SceneManager.instance.CurrentMapData.Level % 60;

                if (tempLevel == 0)
                {
                    tempLevel = 60;
                }

                SceneManager.instance.LastLevelForView = tempLevel;

                if (SceneManager.instance.LastLevel > SceneManager.instance.MaxUserLevel)
                {
                    SceneManager.instance.MaxUserLevel = SceneManager.instance.LastLevel;
                }
                SceneManager.instance.PlayGameController.InitTable();
                SceneManager.instance.LoadingController.ShowLoading(LoadingController.SwitchScene.PlayGame);
            }
            else
            {
                SceneManager.instance.LoadingController.ShowLoading(LoadingController.SwitchScene.Level);
            }
            SceneManager.instance.PlayGameController.SetLevel();
        }
        else
        {
            if (SceneManager.instance.DailyController.CurrentDaily <= DateTime.Today.DayOfYear)
            {
                SceneManager.instance.DailyController.setNextDaily();        
            }
            SceneManager.instance.PlayGameController.InitTable();
            SceneManager.instance.LoadingController.ShowLoading(LoadingController.SwitchScene.PlayGame);
            DateTime _currentDaily = new DateTime(DateTime.Now.Year, 1, 1).AddDays(SceneManager.instance.DailyController.CurrentDaily - 1);
            SceneManager.instance.PlayGameController.showDaily(_currentDaily);
        }
    }

    public void OnRetryClick()
    {
        SoundManager.instance.SoundOn(SoundManager.SoundIngame.Click);
        gameObject.SetActive(false);
        SceneManager.instance.PlayGameController.HidePlayGame();
        SceneManager.instance.PlayGameController.InitTable();
        SceneManager.instance.LoadingController.ShowLoading(LoadingController.SwitchScene.PlayGame);
    }

    private void CheckHint()
    {
        HintObj.SetActive(true);
        if (SceneManager.instance.m_GameMode != SceneManager.GameMode.Daily)
        {
            var temp = SceneManager.instance.GetHintLevel();
            foreach (var item in IconHint)
            {
                item.sprite = Deactive;
            }

            var diff = SceneManager.instance.LastLevelForView % 5;
            if (diff == 0)
            {
                diff = 5;
            }
            int currentLevel = SceneManager.instance.LastLevelForView - diff;
            for (int i = 0; i < CurrentLevel.Count; i++)
            {
                CurrentLevel[i].text = (currentLevel + i + 1).ToString();
                if (i == diff - 1)
                {
                    Border.transform.position = CurrentLevel[i].transform.position;
                }
            }

            if (SceneManager.instance.LastLevel / 5 == SceneManager.instance.MaxUserLevel / 5)
            {
                for (int i = 0; i < temp; i++)
                {
                    IconHint[i].sprite = Active;
                }
            }
            else
            {
                foreach (var item in IconHint)
                {
                    item.sprite = Active;
                }
            }
        }
        else
        {
            HintObj.SetActive(false);
            Border.transform.position = new Vector3(10000, 10000, 10000);
        }
    }

    public void onButtonHomeClick()
    {
        this.gameObject.SetActive(false);
        SceneManager.instance.PlayGameController.HidePlayGame();
        SceneManager.instance.HomeController.ShowHome();
    }
}
