using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class LoseGameController : MonoBehaviour {

    public List<Sprite> Number;
    public List<Image> LevelTag;

    public void SetLevel(int level)
    {
        foreach (var item in LevelTag)
        {
            item.sprite = null;
        }

        string _level = level.ToString();
        for (int i = 0; i < _level.Length; i++)
        {
            LevelTag[i].sprite = Number[int.Parse(_level[i].ToString())];
        }
    }

    public void OnBackClick()
    {
        SoundManager.instance.SoundOn(SoundManager.SoundIngame.Click);
        this.gameObject.SetActive(false);
        SceneManager.instance.PlayGameController.HidePlayGame();
        SceneManager.instance.LoadingController.ShowLoading(LoadingController.SwitchScene.Level);
    }

    public void OnRetryClick()
    {
        SoundManager.instance.SoundOn(SoundManager.SoundIngame.Click);
        this.gameObject.SetActive(false);
        SceneManager.instance.PlayGameController.HidePlayGame();
        SceneManager.instance.PlayGameController.InitTable();
        SceneManager.instance.LoadingController.ShowLoading(LoadingController.SwitchScene.PlayGame);
    }
}
