using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class LoadingController : MonoBehaviour {

    private CanvasGroup Loading;
    public Image SliderBar;
    public Text Percent;

    public enum SwitchScene
    {
        Home,
        Level,
        PlayGame,
    }

    void Awake()
    {
        Loading = GetComponent<CanvasGroup>();
    }


    public void ShowLoading(SwitchScene switchTo)
    {
        SceneManager.instance.LoadNewIntertitialBanner();
        SceneManager.instance.ShowBanner(true);
        Loading.alpha = 1;
        Loading.blocksRaycasts = true;
        this.gameObject.transform.localPosition = Vector2.zero;
        float timeLoading = switchTo == SwitchScene.Home ? 1f : 1f;
        LeanTween.value(0, 1, timeLoading).setOnUpdate(OnValueChange).setOnComplete(s =>
        {
            HideLoading(switchTo);
            if (switchTo != SwitchScene.PlayGame)
            {
                SceneManager.instance.ShowBanner(true);
            }
            else
            {
                SceneManager.instance.ShowBanner(false);
            }
        });
    }

    private void OnValueChange(float _value)
    {
        SliderBar.fillAmount = _value;
        Percent.text = Mathf.Round(_value * 100) + "%";
    }

    public void HideLoading(SwitchScene switchTo)
    {
        if(switchTo == SwitchScene.Home)
        {
            SceneManager.instance.HomeController.ShowHome();
        }
        else if (switchTo == SwitchScene.Level)
        {
            SceneManager.instance.LevelController.ShowLevel();
        }
        else
        {
            SceneManager.instance.PlayGameController.ShowPlayGame();
            SceneManager.instance.PlayGameController.delayHideDailyNotif();
        }

        Loading.alpha = 0;
        Loading.blocksRaycasts = false;
        this.gameObject.transform.localPosition = new Vector2(10000, 10000);
    }
}
