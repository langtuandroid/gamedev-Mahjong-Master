using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingController : MonoBehaviour {

	public Sprite On, Off, OnDisable, OffDisable;
	public List<Image> OnOffSetting = new List<Image>();

	public CanvasGroup MainCanvas;
    private bool isPlayGame = false;

    private void Start()
    {
        SoundManager.instance.SetMusic(SceneManager.instance.CheckSoundBg() ? 1 : 0);
        SoundManager.instance.SetSound(SceneManager.instance.CheckSoundEffect()? 1 : 0);
    }

    public void ShowSetting(bool playing = false)
	{
		isPlayGame = playing;
		MainCanvas.alpha = 1;
		this.transform.localPosition = Vector2.zero;
		CheckDefault();
	}

	public void HideSetting()
	{
		MainCanvas.alpha = 0;
		this.transform.position = new Vector2(10000, 10000);
	}

	public void OnChangeSetting(int index) 
	{
        if (index == 0)
		{
			var temp = SceneManager.instance.OnOffBg();
			CheckSetting(OnOffSetting[0], OnOffSetting[1], temp);
            SoundManager.instance.SetMusic(SceneManager.instance.CheckSoundBg() ? 1 : 0);

            temp = SceneManager.instance.OnOffEffect();
            CheckSetting(OnOffSetting[2], OnOffSetting[3], temp);
            SoundManager.instance.SetSound(SceneManager.instance.CheckSoundEffect() ? 1 : 0);
        }
		else if (index == 1)
		{
			var temp = SceneManager.instance.OnOffEffect();
			CheckSetting(OnOffSetting[2], OnOffSetting[3], temp);
            SoundManager.instance.SetSound(SceneManager.instance.CheckSoundEffect() ? 1 : 0);
        }
		else if (index == 2)
		{
			var temp = SceneManager.instance.OnOffLight();
			CheckSetting(OnOffSetting[4], OnOffSetting[5], temp);
		}
		else 
		{
			var temp = SceneManager.instance.OnOffShuffle();
			CheckSetting(OnOffSetting[6], OnOffSetting[7], temp);
		}
        SoundManager.instance.SoundOn(SoundManager.SoundIngame.Click);
    }

    private void CheckSetting(Image on, Image off, bool value)
	{
		if (value)
		{
			on.sprite = On;
			off.sprite = OffDisable;
		}
		else
		{
			on.sprite = OnDisable;
			off.sprite = Off;
		}
	}

	private void CheckDefault()
	{
		CheckSetting(OnOffSetting[0], OnOffSetting[1], SceneManager.instance.CheckSoundBg());
		CheckSetting(OnOffSetting[2], OnOffSetting[3], SceneManager.instance.CheckSoundEffect());
		CheckSetting(OnOffSetting[4], OnOffSetting[5], SceneManager.instance.CheckLight());
		CheckSetting(OnOffSetting[6], OnOffSetting[7], SceneManager.instance.CheckShuffle());
	}

	public void OnBackClick()
	{
		SoundManager.instance.SoundOn(SoundManager.SoundIngame.Click);
		HideSetting();
        if (!isPlayGame)
        {
            if (SceneManager.instance.PreviousScreen == SceneManager.ScreenEnum.Home)
            {
                SceneManager.instance.HomeController.ShowHome();
            }
            else if (SceneManager.instance.PreviousScreen == SceneManager.ScreenEnum.Level)
            {
                SceneManager.instance.LevelController.ShowLevel();
            }
            else
            {
                SceneManager.instance.MapStageController.ShowMap();
            }
        }
        else
        {
            SceneManager.instance.PlayGameController.transform.localPosition = Vector3.zero;
            SceneManager.instance.PlayGameController.CheckShuffleBtn();
        }
	}
}
