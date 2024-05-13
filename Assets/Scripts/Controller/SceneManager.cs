using UnityEngine;
using System.Collections.Generic;
using System.IO;
using AOT;
using System;
using UnityEngine.UI;
public class SceneManager : MonoBehaviour
{
    public enum ScreenEnum
    {
        Home,
        Map,
        Level
    }

    public enum GameMode
    {
        Arcade,
        Classic,
        Daily
    }

    string BannerId = "ca-app-pub-3277015025246847/4326032326";
    string FullScreenId = "ca-app-pub-3277015025246847/4653335385";
    string VideosId = "ca-app-pub-3277015025246847/9386787313";

    public GameMode m_GameMode = GameMode.Arcade;
    public GameMode GameModeSave = GameMode.Arcade;

    public ScreenEnum PreviousScreen = ScreenEnum.Home;

    public const string USER_DATA = "USER_DATA";
    public const string TIP_DATA = "TIP_DATA";
    public const string RATE_DATA = "RATE_DATA";
    public const string LEVEL_RATING = "LEVEL_RATING";
    public const string BG_SOUND = "BG_SOUND";
    public const string EFFECT_SOUND = "EFFECT_SOUND";
    public const string LIGHT = "LIGHT";
    public const string SHUFFLE = "SHUFFLE";

    public const string TILES = "TILES";
    public const string BACKGROUND = "BACKGROUND";
    public const string TIMEGIFT = "TIMEGIFT";

    public Image MainBg;

    public MapData CurrentMapData;
    public List<MapData> AllMapData = new List<MapData>();
    public List<string> MapsData = new List<string>();
    public int LastLevel = 0;
    public int PreLastLevel = 0;
    public int MaxUserLevel = 0;
    public int MapIndex = 0;

    public string RateURL = "";
    public string MoreGameURL = "";

    public AdMobBanner Banner;
    public AdMobBannerInterstitial InterStitial;
    public float TimeShowInterstitial = 90f;
    public List<List<int>> X_Pos = new List<List<int>>();
    public List<List<int>> Y_Pos = new List<List<int>>();
    public List<List<int>> Z_Pos = new List<List<int>>();


    public List<List<int>> X_PosDaily = new List<List<int>>();
    public List<List<int>> Y_PosDaily = new List<List<int>>();
    public List<List<int>> Z_PosDaily = new List<List<int>>();

    public bool StartShowBanner = false;
    public bool IsLoadVideosSuccess = false;

    public int LastLevelForView = 0;

    public static SceneManager instance;
    void Awake()
    {
        GoogleMobileAdSettings.Instance.Android_BannersUnitId = BannerId;
        GoogleMobileAdSettings.Instance.Android_InterstisialsUnitId = FullScreenId;
        GoogleMobileAdSettings.Instance.Android_RewardedVideoAdUnitId = VideosId;

        if (!GoogleMobileAd.IsInited)
        {
            GoogleMobileAd.Init();
        }

        X_Pos.Clear();
        Y_Pos.Clear();
        Z_Pos.Clear();
        SceneManager.instance = this;

        if (!PlayerPrefs.HasKey(RATE_DATA))
        {
            PlayerPrefs.SetInt(RATE_DATA, 0);
            PlayerPrefs.SetInt(LEVEL_RATING, 0);
        }

        if (!PlayerPrefs.HasKey(TIMEGIFT))
        {
            var time = DateTime.Now.Ticks;
            PlayerPrefs.SetString(TIMEGIFT, time.ToString());
        }


        GoogleMobileAd.OnInterstitialClosed += OnHideIntertitialBanner;
        GoogleMobileAd.OnInterstitialLoaded += OnLoadIntertitialBannerSuccess;

        GoogleMobileAd.OnRewardedVideoAdClosed += HandleOnRewardedVideoAdClosed;
        GoogleMobileAd.OnRewardedVideoLoaded += HandleOnRewardedVideoLoaded;
        GoogleMobileAd.OnRewardedVideoAdFailedToLoad += OnAdFailedToLoad;
        GoogleMobileAd.StartRewardedVideo();
        LeanTween.delayedCall(1f, () =>
        {
            GoogleMobileAd.LoadRewardedVideo();
        });
    }

    public void HandleOnRewardedVideoLoaded()
    {
        IsLoadVideosSuccess = true;
    }

    public void HandleOnRewardedVideoAdClosed()
    {
        AddHint(Config.instance.VideosGift);
        PlayGameController.HintNum.text = Config.instance.VideosGift.ToString();
    }

    public long GetLastTimeReceiveGift()
    {
        var time = PlayerPrefs.GetString(TIMEGIFT);
        return Convert.ToInt64(time);
    }

    public void SetLastTimeReceiveGift()
    {
        var eightHour = TimeSpan.TicksPerHour * 8;
        var time = DateTime.Now.AddTicks(eightHour).Ticks;
        PlayerPrefs.SetString(TIMEGIFT, time.ToString());
    }

    public void OnAdFailedToLoad(int error)
    {
        IsLoadVideosSuccess = false;
        Debug.LogError(error);
    }

    bool isLoadBanner = false;
    public void OnLoadIntertitialBannerSuccess()
    {
        isLoadBanner = true;
    }

    public int CurrentTotalStar()
    {
        int _currentStar = 0;
        foreach(var item in AllMapData)
        {
            _currentStar += item.Star;
        }

        return _currentStar;
    }

    public void LoadNewIntertitialBanner()
    {
        GoogleMobileAd.LoadInterstitialAd();
    }

    public enum ObjectManager
    {
        MainMenu,
        Message,
        PlayGame,
        Option,
    }

    public enum ObjectDialog
    {
        WinLose,
    }


    public enum TypeInit
    {
        Immediately,
        Delays,
    }

    private GameObject preScene;
    private GameObject preDialog;

    public LoadingController LoadingController;
    public HomeController HomeController;
    public LevelController LevelController;
    public PlayGameController PlayGameController;
    public DailyController DailyController;
    public SettingController SettingController;
    public ThemeController ThemeController;
    public MapStageController MapStageController;
    public AchievementController AchievementController;
    public DailyGiftController DailyGiftController;

    void Start()
    {
        if (!PlayerPrefs.HasKey(USER_DATA))
        {
            CreateNewDataUser();
        }

        LoadMapData();
        LoadUserData();
        SetMainBg();

        // Start game UI
        LoadingController.ShowLoading(LoadingController.SwitchScene.Home);
        HomeController.HideHome();
        LevelController.HideLevel();
        PlayGameController.HidePlayGame();
        DailyController.HideDaily();
        SettingController.HideSetting();
        ThemeController.HideTheme();
        MapStageController.HideMap();
        AchievementController.HideAchi();
        DailyGiftController.HideDailyGift();
    }

    void CreateNewDataUser()
    {
        PlayerPrefs.SetInt("HINT", 5);

        string userData = "";
        // => lock => star => score
        for (int j = 0; j < Config.instance.MaxLevel; j++)
        {
            if (j != 0)
            {
                userData += "+0-0-0";
            }
            else
            {
                userData += "+1-0-0";
            }
            
        }
        PlayerPrefs.SetString(USER_DATA, userData);
    }

    void LoadUserData()
    {
        try
        {
            string dataLoader = PlayerPrefs.GetString(USER_DATA);
            LastLevel = Config.instance.MaxLevel;
            PreLastLevel = Config.instance.MaxLevel;
            MaxUserLevel = Config.instance.MaxLevel;
            var level = dataLoader.Split('+');
            for (int j = 1; j < level.Length; j++)
            {
                var data = level[j].Split('-');
                MapData newLevel = new MapData(j, int.Parse(data[0]), int.Parse(data[1]), int.Parse(data[2]));
                AllMapData.Add(newLevel);
                if(!newLevel.IsLock)
                {
                    LastLevel = newLevel.Level - 1;
                    if (j == level.Length - 1)
                    {
                        LastLevel = newLevel.Level;
                    }
                    MaxUserLevel = LastLevel;
                }
            }
            PreLastLevel = LastLevel;

            for (int i = level.Length; i <= Config.instance.MaxLevel;i++)
            {
                int isLock = 0;
                if(AllMapData[i - 2].IsLock == false 
                    && AllMapData[i - 2].Score > 0)
                {
                    isLock = 1;
                }
                MapData newLevel = new MapData(i, isLock, 0, 0);
                AllMapData.Add(newLevel);
            }
        }
        catch
        {
            CreateNewDataUser();
            LoadUserData();
        }
    }

    public void SavePoint() {
        bool firstUnlock = false;
        for (int i = 0; i < AllMapData.Count; i++)
        {
            if (AllMapData[i].Level == CurrentMapData.Level)
            {
                AllMapData[i] = CurrentMapData;
                if(i + 1 < AllMapData.Count)
                {
                    AllMapData[i + 1].IsLock = false;
                    if (!firstUnlock) {
                        firstUnlock = true;
                        if (i + 1 > MaxUserLevel)
                        {
                            MaxUserLevel = i + 1;
                        }
                    }
                }
                break;
            }
        }
        SaveData();
    }

    void SaveData()
    {
        string userData = "";
        // => lock => star => score
        for (int j = 0; j < AllMapData.Count; j++)
        {
            int isLock = 1;
            if (AllMapData[j].IsLock)
                isLock = 0;

            userData += "+" + isLock + "-" + 
                AllMapData[j].Star + "-" + AllMapData[j].Score;
        }
        PlayerPrefs.SetString(USER_DATA, userData);
    }

    public bool IsOffTip()
    {
        if(!PlayerPrefs.HasKey(TIP_DATA))
        {
            PlayerPrefs.SetString(TIP_DATA, "FALSE");
            return true;
        }

        if (PlayerPrefs.GetString(TIP_DATA) == "TRUE")
        {
            return true;
        }

        return false;
    }

    public void SetTip()
    {
        if (IsOffTip())
        {
            PlayerPrefs.SetString(TIP_DATA, "FALSE");
        }
        else
        {
            PlayerPrefs.SetString(TIP_DATA, "TRUE");
        }
    }

    bool checkHide = false;
    float _preTimeCheckHide = 0;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnButtonBackNativeClick();
        }

        _preTimeCheckHide += Time.deltaTime;
        if (_preTimeCheckHide > 2f && !checkHide)
        {
            if (Screen.width > Screen.height)
            {
                Banner.HideBanner();
                checkHide = true;
            }
        }
    }

    public void OnButtonBackNativeClick()
    {
        if (HomeController.IsShowing)
        {
            Application.Quit();
        }
        else if (MapStageController.IsShowing)
        {
            MapStageController.OnBackClick();
        }
        else if (PlayGameController.IsShowing)
        {
            PlayGameController.OnBackClick();
        }
        else if (LevelController.IsShowing)
        {
            LevelController.OnBackClick();
        }
        else if (ThemeController.IsShowing)
        {
            ThemeController.OnBackClick();
        }
        else if (DailyController.IsShowing)
        {
            DailyController.onButtonCloseClick();
        }
    }

    private bool lastBanner = false;

    public void ShowBanner(bool isNormal)
    {
        
    }

    public void ShowIntertitialBanner()
    {
        InterStitial.ShowBanner();
    }


    public void OnHideIntertitialBanner()
    {
        isLoadBanner = false;
        LoadNewIntertitialBanner();
    }

    public bool CanShowIntertitialBanner()
    {
        return isLoadBanner;
    }

    public bool LoadToLevelScene = false;

    public int TotalStar()
    {
        int star = 0;

        foreach (var data in AllMapData)
            star += data.Star;

        return star;
    }

    void LoadMapData()
    {
        try
        {
            var start = 1;
            while (start < Config.instance.MaxLevel)
            {
                var name = start + "-" + (start + 9);
                var txt = Resources.LoadAll("Level/" + name);
                Array.Sort(txt, (x,y) => {
                    var a = int.Parse(x.name.Split('_')[1]);
                    var b = int.Parse(y.name.Split('_')[1]);
                    return a > b ? -1 : 1;
                });
                Array.Reverse(txt);

                foreach(var item in txt)
                {
                    TextAsset _level = Resources.Load("Level/" + name + "/" + item.name) as TextAsset;
                    if (_level != null)
                    {
                        var temp = _level.text.Substring(1, _level.text.Length - 2);
                        var charList = temp.Split(',');
                        var temp2 = new List<int>();
                        int num2;
                        foreach(var num in charList)
                        {
                            if (int.TryParse(num, out num2))
                            {
                                temp2.Add(num2);
                            }
                        }
                        if (item.name.Contains("position_x"))
                        {
                            X_Pos.Add(temp2);
                        }
                        else if (item.name.Contains("position_y"))
                        {
                            Y_Pos.Add(temp2);
                        } else
                        {
                            Z_Pos.Add(temp2);
                        }
                    }
                }
                start = start + 10;
            }

            for(var i = 1; i < 5; i++)
            {
                var name2 = "Daily/Daily-Pack" + i;
                
                var fileName = new List<string>()
                {
                    "Daily1-10",
                    "Daily11-20",
                    "Daily21-30",
                    "Daily31-40"
                };

                foreach(var file in fileName)
                {
                    var txt2 = Resources.LoadAll(name2 + "/" + file);
                    Array.Sort(txt2, (x, y) => {
                        var a = int.Parse(x.name.Split('_')[1]);
                        var b = int.Parse(y.name.Split('_')[1]);
                        return a > b ? -1 : 1;
                    });
                    Array.Reverse(txt2);
                    foreach (var item in txt2)
                    {
                        TextAsset _level = Resources.Load(name2 + "/" + file + "/" + item.name) as TextAsset;
                        if (_level != null)
                        {
                            var temp = _level.text.Substring(1, _level.text.Length - 2);
                            var charList = temp.Split(',');
                            var temp2 = new List<int>();
                            int num2;
                            foreach (var num in charList)
                            {
                                if (int.TryParse(num, out num2))
                                {
                                    temp2.Add(num2);
                                }
                            }
                            if (item.name.Contains("position_x"))
                            {
                                X_PosDaily.Add(temp2);
                            }
                            else if (item.name.Contains("position_y"))
                            {
                                Y_PosDaily.Add(temp2);
                            }
                            else
                            {
                                Z_PosDaily.Add(temp2);
                            }
                        }
                    }
                }
            }
        }
        catch (IOException e)
        {
        }
    }

    public bool CheckSoundBg()
    {
        return PlayerPrefs.GetInt(BG_SOUND) == 0;
    }

    public bool CheckSoundEffect()
    {
        return PlayerPrefs.GetInt(EFFECT_SOUND) == 0;
    }

    public bool CheckLight()
    {
        return PlayerPrefs.GetInt(LIGHT) == 0;
    }

    public bool CheckShuffle()
    {
        return PlayerPrefs.GetInt(SHUFFLE) == 0;
    }

    public bool OnOffBg()
    {
        var value = PlayerPrefs.GetInt(BG_SOUND) == 0 ? 1 : 0;
        PlayerPrefs.SetInt(BG_SOUND, value);
        return value == 1 ? false : true;
    }

    public bool OnOffEffect()
    {
        var value = PlayerPrefs.GetInt(EFFECT_SOUND) == 0 ? 1 : 0;
        PlayerPrefs.SetInt(EFFECT_SOUND, value);
        return value == 1 ? false : true;
    }

    public bool OnOffLight()
    {
        var value = PlayerPrefs.GetInt(LIGHT) == 0 ? 1 : 0;
        PlayerPrefs.SetInt(LIGHT, value);
        return value == 1 ? false : true;
    }

    public bool OnOffShuffle()
    {
        var value = PlayerPrefs.GetInt(SHUFFLE) == 0 ? 1 : 0;
        PlayerPrefs.SetInt(SHUFFLE, value);
        return value == 1 ? false : true;
    }

    public int GetCurrentTile()
    {
        return PlayerPrefs.GetInt(TILES);
    }

    public void SetCurrentTile(int cur)
    {
        PlayerPrefs.SetInt(TILES, cur);
    }

    public int GetCurrentBg()
    {
        return PlayerPrefs.GetInt(BACKGROUND);
    }

    public void SetCurrentBg(int cur)
    {
        PlayerPrefs.SetInt(BACKGROUND, cur);
    }

    public void SetMainBg()
    {
        var index = GetCurrentBg();
        PlayGameController.BG.sprite = ThemeController.ListBg[index];
    }

    public int GetHint()
    {
        return PlayerPrefs.GetInt("HINT");
    }

    public void AddHint(int times)
    {
        PlayerPrefs.SetInt("HINT", GetHint() + times);
    }

    public int GetCurrentDaily()
    {
        return PlayerPrefs.GetInt("DAILY_GIFT");
    }

    public int SetCurrentDaily()
    {
        var currentDay = GetCurrentDaily();
        if (currentDay >= 6)
            currentDay = 0;
        return PlayerPrefs.GetInt("DAILY_GIFT", currentDay + 1);
    }

    public bool CheckDailyGift()
    {
        var curTick = DateTime.Now;
        if (PlayerPrefs.GetString("DAILY") != "")
        {
            var diff = curTick.Subtract(DateTime.Parse(PlayerPrefs.GetString("DAILY")));
            return diff.Days > 0;
        }
        return true;
    }

    public void SetDailyGift()
    {
        PlayerPrefs.SetString("DAILY", DateTime.Now.ToString());
    }

    public bool CheckPlayed()
    {
        return PlayerPrefs.GetInt("PLAYED") == 1;
    }

    public void SetPlayed()
    {
        PlayerPrefs.GetInt("PLAYED", 1);
    }

    public int GetAllLobster()
    {
        var total = 0;
        foreach(var item in AllMapData)
        {
            total += item.Star;
        }
        return total;
    }

    public int GetHintLevel()
    {
        return PlayerPrefs.GetInt("HINT_LEVEL",0);
    }

    public void SetHintLevel()
    {
        int cur = GetHintLevel();
        cur += 1;
        if (cur > 5)
        {
            cur = 1;
        }
        PlayerPrefs.SetInt("HINT_LEVEL", cur);
    }

    public void ShowVideoAds() {
        if (IsLoadVideosSuccess) {
            GoogleMobileAd.ShowRewardedVideo();
        }
    }
}
