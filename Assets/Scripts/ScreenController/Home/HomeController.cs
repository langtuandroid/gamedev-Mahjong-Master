using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class HomeController : MonoBehaviour {

    private CanvasGroup Home;
    public Image GiftObj;
    public GameObject NotiDaily;
    public GameObject NotiTrophy;
    public GameObject PopUpGift;
    public GameObject TextTime;
    public Text TimeCountDown;

    public bool IsShowing;
    private long TimeGift = 0;

    void Awake()
    {
        Home = GetComponent<CanvasGroup>();
    }


    public void ShowHome()
    {
        CheckSoundIcon();
        SceneManager.instance.AchievementController.CheckUserData();
        IsShowing = true;
        Home.alpha = 1;
        Home.blocksRaycasts = true;
        PopUpGift.SetActive(false);
        if (SceneManager.instance.DailyController.isDoneDailyToday())
            NotiDaily.SetActive(false);
        else
            NotiDaily.SetActive(true);

        NotiTrophy.SetActive(SceneManager.instance.AchievementController.CheckGetAllBtn());
        gameObject.transform.localPosition = Vector2.zero;
        if (SceneManager.instance.CheckDailyGift() && SceneManager.instance.CheckPlayed())
        {
            SceneManager.instance.DailyGiftController.ShowDailyGift();
            HideHome();
        }

        handleCheckCoolDown();
        SceneManager.instance.ShowBanner(true);
    }



    public void HideHome()
    {
        IsShowing = false;
        Home.alpha = 0;
        Home.blocksRaycasts = false;
        gameObject.transform.localPosition = new Vector2(10000, 10000);
        cancelCountDown();
        SceneManager.instance.PreviousScreen = SceneManager.ScreenEnum.Home;
    }

    public void OnSettingClick()
    {
        SoundManager.instance.SoundOn(SoundManager.SoundIngame.Click);
        SceneManager.instance.SettingController.ShowSetting();
        if (SceneManager.instance.HomeController.IsShowing)
        {
            HideHome();
        }
        else if (SceneManager.instance.LevelController.IsShowing)
        {
            SceneManager.instance.LevelController.HideLevel();
        }
        else
        {
            SceneManager.instance.MapStageController.HideMap();
        }
    }

    public void OnPlayClick()
    {
        SoundManager.instance.SoundOn(SoundManager.SoundIngame.Click);
        LoadLevel();
    }

    public void LoadLevel()
    {
        SoundManager.instance.SoundOn(SoundManager.SoundIngame.Click);
        HideHome();
        SceneManager.instance.MapStageController.ShowMap();
    }

    public void OnRateClick()
    {
        SoundManager.instance.SoundOn(SoundManager.SoundIngame.Click);
        Application.OpenURL(SceneManager.instance.RateURL);
    }

    public void OnMoreGameClick()
    {
        SoundManager.instance.SoundOn(SoundManager.SoundIngame.Click);
        Application.OpenURL(SceneManager.instance.MoreGameURL);
    }

    public void OnGiftClick()
    {
        SoundManager.instance.SoundOn(SoundManager.SoundIngame.Daily);
        SceneManager.instance.AddHint(Config.instance.BoxGift);
        SceneManager.instance.SetLastTimeReceiveGift();
        GiftObj.color = new Color32(255, 255, 255, 80);
        GiftObj.raycastTarget = false;
        PopUpGift.SetActive(true);
    }

    public void OnAdsClick() {
        SceneManager.instance.ShowVideoAds();
        SoundManager.instance.SoundOn(SoundManager.SoundIngame.Click);
    }

    public void OnThemeClick()
    {
        SoundManager.instance.SoundOn(SoundManager.SoundIngame.Click);
        if (SceneManager.instance.HomeController.IsShowing)
        {
            HideHome();
        }
        else if (SceneManager.instance.LevelController.IsShowing)
        {
            SceneManager.instance.LevelController.HideLevel();
        }
        else
        {
            SceneManager.instance.MapStageController.HideMap();
        }
        SceneManager.instance.ThemeController.ShowTheme();
    }

    public void OnTrophyClick()
    {
        SoundManager.instance.SoundOn(SoundManager.SoundIngame.Click);
        if (SceneManager.instance.HomeController.IsShowing)
        {
            HideHome();
        }
        else if (SceneManager.instance.LevelController.IsShowing)
        {
            SceneManager.instance.LevelController.HideLevel();
        }
        else
        {
            SceneManager.instance.MapStageController.HideMap();
        }
        SceneManager.instance.AchievementController.ShowAchi();
    }

    public void OnDailyClick()
    {
        SoundManager.instance.SoundOn(SoundManager.SoundIngame.Click);
        if (SceneManager.instance.HomeController.IsShowing)
        {
            HideHome();
        }
        else if (SceneManager.instance.LevelController.IsShowing)
        {
            SceneManager.instance.LevelController.HideLevel();
        }
        else
        {
            SceneManager.instance.MapStageController.HideMap();
        }
        SceneManager.instance.DailyController.ShowDaily();
    }

    public void onButtonPopUpGiftClick()
    {
        SoundManager.instance.SoundOn(SoundManager.SoundIngame.Click);
        PopUpGift.SetActive(false);
        TextTime.SetActive(true);
        TimeCountDown.text = String.Format("{0}h{1}m{2}s", TimeGift / TimeSpan.TicksPerHour, ((TimeGift / TimeSpan.TicksPerSecond) / 60) % 60, (TimeGift / TimeSpan.TicksPerSecond) % 60);
        handleCountDown();
    }

    private void countDown()
    {
        TimeGift -= TimeSpan.TicksPerSecond;
        TimeCountDown.text = String.Format("{0}h{1}m{2}s", TimeGift / TimeSpan.TicksPerHour, ((TimeGift / TimeSpan.TicksPerSecond) / 60) % 60, (TimeGift / TimeSpan.TicksPerSecond) % 60);
        if (TimeGift <=0)
        {
            cancelCountDown();
            GiftObj.color = new Color32(255, 255, 255, 255);
            GiftObj.raycastTarget = true;
            TimeCountDown.gameObject.SetActive(false);
        }
        
    }
    private void handleCountDown()
    {
        cancelCountDown();
        TimeGift = (SceneManager.instance.GetLastTimeReceiveGift() - DateTime.Now.Ticks);
        TimeCountDown.text = String.Format("{0}h{1}m{2}s", TimeGift / TimeSpan.TicksPerHour, ((TimeGift / TimeSpan.TicksPerSecond) / 60) % 60, (TimeGift / TimeSpan.TicksPerSecond) % 60);
        InvokeRepeating("countDown", 1, 1);
    }

    private void cancelCountDown()
    {
        TimeGift = 0;
        TimeCountDown.text = "0h0m0s";
        CancelInvoke();
    }

    void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus)
        {
            handleCheckCoolDown();
        }
    }

    private void handleCheckCoolDown()
    {
        if (SceneManager.instance.GetLastTimeReceiveGift() < DateTime.Now.Ticks)
        {
            GiftObj.color = new Color32(255, 255, 255, 255);
            GiftObj.raycastTarget = true;
            TextTime.SetActive(false);
        }
        else
        {
            GiftObj.color = new Color32(255, 255, 255, 80);
            GiftObj.raycastTarget = false;
            TimeGift = (SceneManager.instance.GetLastTimeReceiveGift() - DateTime.Now.Ticks);
            TimeCountDown.text = String.Format("{0}h{1}m{2}s", TimeGift / TimeSpan.TicksPerHour, ((TimeGift / TimeSpan.TicksPerSecond) / 60) % 60, (TimeGift / TimeSpan.TicksPerSecond) % 60);
            TextTime.SetActive(true);
            handleCountDown();
        }
    }

    public Image SoundIcon;
    public void OnSoundClick()
    {
        SoundManager.instance.SoundOn(SoundManager.SoundIngame.PlayGameBtn);
        var temp = SceneManager.instance.OnOffEffect();
        SoundManager.instance.SetSound(temp ? 1 : 0);
        CheckSoundIcon();
    }

    private void CheckSoundIcon()
    {
        if (SceneManager.instance.CheckSoundEffect())
        {
            SoundIcon.color = new Color32(255, 255, 255, 255);
        }
        else
        {
            SoundIcon.color = new Color32(137, 137, 137, 137);
        }
    }
}
