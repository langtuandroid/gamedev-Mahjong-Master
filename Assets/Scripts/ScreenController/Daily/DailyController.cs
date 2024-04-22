using System;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DailyController : MonoBehaviour
{
    public TrophyController TrophyController;
    public List<DayController> AllDay = new List<DayController>();
    public GameObject AllDayHolder;
    public CanvasGroup Group;
    public int CurrentDaily = -1;
    public Text TextMonth;
    public int DayOfMonth;
    public Text AllTime;
    public Text ThisMonth;
    public Image ButtonPlayText;
    public Sprite Play, Replay;
    public GameObject Resolve, Unresolve;
    public Button ButtonPlay;
    public GameObject PopUp;
    public GameObject BGBottom;
    public RectTransform RectBGBottom;
    public Image CupWin;
    public Image CupPopUp;
    public Text Month;
    public Text Info;
    public GameObject PopUpCupInfo;
    int CurrentMonth;
    int MaxMonth;
    int CurrentYear;
    int Today;
    int monthFirstInstall;
    public bool IsShowing = false;
    private bool isReplay = false;

    public GameObject WinDaily;

    private int totalGameComplete = 0;
    public Image Left, Right;
    private static string KEY_CALENDAR = "Calendar";
    private static string KEY_FIRSTINSTALL = "FirstInstall";
    private static string KEY_CURRENT_YEAR = "KEY_CURRENT_YEAR";
    private int totalDay;
    private string[] mData;

    public Text MaxDateInMonth;
    public Slider CupSlider;

    public GameObject TextPopup;
    public Sprite YellowSquare;
    public bool FirstShow = true;
    public bool AlreadySelect = false;

    List<string> MonthList = new List<string>()
    {
        "January", "February", "March", "April",
        "May", "June", "July", "August",
        "September", "October", "November", "December",
    };

    public enum CUPTYPE
    {
        None = 1,
        Brone = 2,
        Silver = 3,
        Gold = 4
    }

    void Awake()
    {
        initTotalDay();
        if (!PlayerPrefs.HasKey(KEY_CALENDAR))
        {
            initKeyCalendar();
        }

        if (!PlayerPrefs.HasKey(KEY_CURRENT_YEAR))
        {
            PlayerPrefs.SetInt(KEY_CURRENT_YEAR, DateTime.Today.Year);
        }
        if (!PlayerPrefs.HasKey(KEY_FIRSTINSTALL))
        {
            PlayerPrefs.SetInt(KEY_FIRSTINSTALL, DateTime.Today.Month);
        }
        mData = getCalendarData().Split('*');
        checkDateTime();
        CurrentDaily = DateTime.Today.DayOfYear;
        CurrentMonth = DateTime.Today.Month;
    }

    void checkDateTime()
    {
        int currentYear = DateTime.Today.Year;
        if (currentYear > PlayerPrefs.GetInt(KEY_CURRENT_YEAR))
        {
            PlayerPrefs.SetInt(KEY_CURRENT_YEAR, currentYear);
            PlayerPrefs.SetInt(KEY_FIRSTINSTALL, 1);
            resetData();
        }
    }

    void resetData()
    {
        int length = mData.Length;
        for (int i = 0; i < mData.Length; i++)
        {
            mData[i] = "0-0-0";
        }
        saveData();
    }

    void saveData()
    {
        int lenght = mData.Length;
        string data = "";
        for (int i = 0; i < lenght - 1; i++)
        {
            data += mData[i] + "*";
        }
        data += mData[lenght - 1];
        PlayerPrefs.SetString(KEY_CALENDAR, data);
    }

    public void ShowDaily(bool isDone = false, bool isPlayGame = false)
    {
        FirstShow = true;
        AlreadySelect = false;
        PopUpCupInfo.gameObject.SetActive(false);
        TrophyController.HideTrophy();
        Today = DateTime.Today.Day;
        Group.alpha = 1;
        transform.localPosition = new Vector3(0, 0, 0);
        Group.blocksRaycasts = true;
        IsShowing = true;

        if (isPlayGame)
        {
            SoundManager.instance.SoundOn(SoundManager.SoundIngame.Bonus);
            WinDaily.SetActive(true);
        }
        else
        {
            CurrentDaily = DateTime.Today.DayOfYear;
            setNextDaily();
        }

        GetMonthDate(CurrentMonth);
        checkNewMonth();
        if (CurrentMonth >= monthFirstInstall && monthFirstInstall != 1)
            Left.color = new Color32(255, 255, 255, 255);
        else
            Left.color = new Color32(133, 133, 133, 255);
        Right.color = CurrentMonth != MaxMonth ? new Color32(255, 255, 255, 255) : new Color32(133, 133, 133, 255);
        FirstShow = false;
    }

    public void OnHideWinDaily()
    {
        SoundManager.instance.SoundOn(SoundManager.SoundIngame.Click);
        WinDaily.SetActive(false);
    }

    public void HideDaily()
    {
        Group.alpha = 0;
        transform.localPosition = new Vector3(10000, 10000, 0);
        Group.blocksRaycasts = false;
        IsShowing = false;
    }

    public void onButtonBackClick()
    {
        SoundManager.instance.SoundOn(SoundManager.SoundIngame.Click);
        HideDaily();
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

    void initKeyCalendar()
    {
        string calendar = "";
        //lock => scores
        for (int i = 0; i < totalDay; i++)
        {
            if (i > 0)
                calendar += "*";
            calendar += "0-0-0";
        }
        PlayerPrefs.SetString(KEY_CALENDAR, calendar);
    }

    public string getCalendarData()
    {
        return PlayerPrefs.GetString(KEY_CALENDAR);
    }

    bool isPlayCompleteDaily(int day)
    {
        string[] daily = mData[day - 1].Split('-');
        if (daily[0] == "0")
            return false;
        else
            return true;
    }

    void initTotalDay()
    {
        int year = DateTime.Today.Year;
        if ((year % 4 == 0 && year % 100 != 0) || (year % 400 == 0))
        {
            totalDay = 366;
        }
        else
        {
            totalDay = 365;
        }
    }

    void loadDataCalendar()
    {

    }

    void Start()
    {

        var objs = AllDayHolder.GetComponentsInChildren<DayController>();

        foreach (var item in objs)
        {
            AllDay.Add(item);
        }
        CurrentDaily = DateTime.Today.DayOfYear;
        DayOfMonth = DateTime.Today.Day;
        var date = DateTime.Today;
        CurrentMonth = date.Month;
        CurrentYear = date.Year;
        Today = date.Day;
        MaxMonth = CurrentMonth;
        GetMonthDate(CurrentMonth);
        monthFirstInstall = getMonthFirstInstall();
        totalGameComplete = getTotalGameComplete();
        setNextDaily();
        TrophyController.init();
    }

    public void Reset()
    {
        GetMonthDate(CurrentMonth, true);
    }

    int tweenId = -1;
    private void TweenLeft()
    {
        var color = Left.color.a;
        tweenId = LeanTween.value(color, color == 0 ? 1 : 0, 0.25f).setOnUpdate(OnUpdateLeft).id;
    }

    private void OnUpdateLeft(float value)
    {
        Left.color = new Color(1, 1, 1, value);
    }

    public bool isSameMonth = false;
    public int LastIndexNotClear = -1;
    void GetMonthDate(int month, bool isReset = false)
    {
        CancelInvoke("TweenLeft");
        LeanTween.cancel(tweenId);
        showCup();
        var firstDay = new DateTime(DateTime.Today.Year, month, 1);
        TextMonth.text = MonthList[month - 1];
        var last = DateTime.DaysInMonth(firstDay.Year, month);
        ThisMonth.text += "/" + last;
        MaxDateInMonth.text = last.ToString();
        var lastDay = new DateTime(DateTime.Today.Year, month, last);
        var firstIndex = Convert.ToInt32(firstDay.DayOfWeek);
        var index = 1;
        LastIndexNotClear = -1;
        var checkAll = checkCompleteAllUntilToday();
        isSameMonth = DateTime.Today.Month - 1 == month;
        if (checkAll && CurrentMonth >= monthFirstInstall && monthFirstInstall != 1)
        {
            InvokeRepeating("TweenLeft", 1f, 0.5f);

        }
        List<bool> CheckComplete = new List<bool>();
        for (int i = 0; i < AllDay.Count; i++)
        {
            if (i < firstIndex || index > last)
            {

            }
            else
            {
                bool isCompleteDaily = isPlayCompleteDaily(firstDay.DayOfYear - firstIndex + i);
                CheckComplete.Add(isCompleteDaily);
            }
         }

        for(int i = 0; i < CheckComplete.Count; i++)
        {
            if (!CheckComplete[i])
            {
                LastIndexNotClear = i + 1;
            }
        }
        
          for (int i = 0; i < AllDay.Count; i++)
           {
            if (i < firstIndex || index > last)
            {
                AllDay[i].gameObject.transform.localScale = Vector2.zero;
            }
            else
            {
                var block = CurrentMonth == MaxMonth;
                if (block && index <= Today)
                {
                    block = false;
                }
                AllDay[i].gameObject.transform.localScale = Vector2.one;

                if (checkAll && index == 1)
                {
                    AllDay[i].SetData(index, false, block, firstDay.DayOfYear + i - firstIndex, CheckComplete[index - 1], isReset, true, true);
                }
                else if (checkAll && index == Today)
                {
                    AllDay[i].SetData(index, false, block, firstDay.DayOfYear + i - firstIndex, CheckComplete[index - 1], isReset, true, false);
                }
                else
                {
                    AllDay[i].SetData(index, false, block, firstDay.DayOfYear + i - firstIndex, CheckComplete[index - 1], isReset);
                }
                index++;
            }
        }
    }

    public void OnChangeClick(bool isLeft)
    {
        if (isLeft)
        {
            if (CurrentMonth >= monthFirstInstall && monthFirstInstall != 1)
            {
                CurrentMonth--;
                GetMonthDate(CurrentMonth);
            }
            else
            {
                ShowWaring();
            }
        }
        else
        {
            if (CurrentMonth < MaxMonth)
            {
                CurrentMonth++;
                GetMonthDate(CurrentMonth);
            }
            else
            {
                ShowWaring();
            }
        }
        if (CurrentMonth >= monthFirstInstall && monthFirstInstall != 1)
            Left.color = new Color32(255, 255, 255, 255);
        else
            Left.color = new Color32(133, 133, 133, 255);
        Right.color = CurrentMonth != MaxMonth ? new Color32(255, 255, 255, 255) : new Color32(133, 133, 133, 255);
    }

    public void saveDaily(int time, int star)
    {
        var data = mData[CurrentDaily - 1].Split('-');
        data[0] = "1";
        if(int.Parse(data[1]) < time && int.Parse(data[1]) !=0)
            data[1] = time.ToString();
        if (int.Parse(data[2]) < star)
        {
            data[2] = star.ToString();
        }
        mData[CurrentDaily - 1] = data[0] + "-" + data[1] + "-" + data[2];
        saveData();
        totalGameComplete = getTotalGameComplete();
        // setNextDaily();
    }

    public bool IsNewDaily()
    {
        var data = mData[CurrentDaily - 1].Split('-');
        return data[2] == "0";
    }

    public void onButtonCloseClick()
    {
        SoundManager.instance.SoundOn(SoundManager.SoundIngame.Click);
        HideDaily();
        SceneManager.instance.HomeController.ShowHome();
    }

    public void onButtonPlayClick()
    {
        if (CurrentDaily <= DateTime.Today.DayOfYear)
        {
            SceneManager.instance.m_GameMode = SceneManager.GameMode.Daily;
            var saveGame = PlayerPrefs.GetInt("SAVE" + SceneManager.instance.GameModeSave) == 1 && CurrentDaily == PlayerPrefs.GetInt("setDate");
            if (!saveGame)
            {
                PlayerPrefs.SetInt("SAVE" + SceneManager.instance.GameModeSave, 0);
            }

            SceneManager.instance.HomeController.HideHome();
            SceneManager.instance.DailyController.HideDaily();
            SceneManager.instance.m_GameMode = SceneManager.GameMode.Daily;
            SceneManager.instance.PlayGameController.InitTable();
            SceneManager.instance.LoadingController.ShowLoading(LoadingController.SwitchScene.PlayGame);
            DateTime _currentDaily = new DateTime(DateTime.Now.Year, 1, 1).AddDays(CurrentDaily - 1);
            SceneManager.instance.PlayGameController.showDaily(_currentDaily);
        }
    }

    public void initDailyPlayGame()
    {
        DateTime _currentDaily = new DateTime(DateTime.Now.Year, 1, 1).AddDays(CurrentDaily - 1);
        SceneManager.instance.PlayGameController.showDaily(_currentDaily);
    }

    public void setCurrentDaily(int day)
    {
        CurrentDaily = day;
    }

    public void ShowDailyDay(int day, int dayOfMonth, bool isComplete, bool isBlock)
    {
        CurrentDaily = day;
        if (isComplete)
        {
            Resolve.SetActive(true);
            Unresolve.SetActive(false);
            ButtonPlayText.sprite = Replay;
            ButtonPlay.interactable = true;
        }
        else
        {
            Resolve.SetActive(false);
            Unresolve.SetActive(true);
            ButtonPlayText.sprite = Play;
            if (isBlock)
            {
                ButtonPlay.interactable = false;
            }
            else
            {
                ButtonPlay.interactable = true;
            }
        }
    }

    private int getNumberGameInMonth(int month)
    {
        var day = new DateTime(DateTime.Today.Year, month, 1);
        int currentDay = day.DayOfYear - 1;
        int numberDayOfMonth = DateTime.DaysInMonth(DateTime.Today.Year, month);
        int count = 0;
        for (int i = 0; i < numberDayOfMonth; i++)
        {
            string[] data = mData[currentDay + i].Split('-');
            if (data[0] == "1")
                count++;
        }
        return count;
    }

    public void setNextDaily()
    {
        var day = new DateTime(DateTime.Today.Year, CurrentMonth, 1);
        int currentDay = day.DayOfYear - 1;
        int numberDayOfMonth = DateTime.DaysInMonth(DateTime.Today.Year, CurrentMonth);

        for (int i = CurrentDaily - 1; i < DateTime.Today.DayOfYear; i++)
        {
            string[] data = mData[i].Split('-');
            if (data[0] == "0")
            {
                CurrentDaily = i + 1;
                DayOfMonth = CurrentDaily - currentDay;
                return;
            }
        }

        for (int i = DateTime.Today.DayOfYear - 1; i >= currentDay; i--)
        {
            string[] data = mData[i].Split('-');
            if (data[0] == "0")
            {
                CurrentDaily = i + 1;
                DayOfMonth = CurrentDaily - currentDay;
                return;
            }
        }

        CurrentDaily = DateTime.Today.DayOfYear;
        DayOfMonth = CurrentDaily - currentDay;
    }

    private int getTotalGameComplete()
    {
        int count = 0;
        for (int i = 0; i < totalDay; i++)
        {
            string[] data = mData[i].Split('-');
            if (data[0] == "1")
                count++;
        }
        return count;
    }

    public void showCup()
    {
        int numberGame = getNumberGameInMonth(CurrentMonth);
        int numberDayOfMonth = DateTime.DaysInMonth(DateTime.Today.Year, CurrentMonth);
        ThisMonth.text = numberGame.ToString();
        AllTime.text = totalGameComplete.ToString();
        CupSlider.value = (float)numberGame / (float)numberDayOfMonth;
    }

    public int getMonthFirstInstall()
    {
        return PlayerPrefs.GetInt(KEY_FIRSTINSTALL);
    }

    private void ShowWaring()
    {
        TextPopup.SetActive(true);
    }

    
    public int[] getNumOfGameInMonth(int month)
    {
        DateTime firstDayOfMonth = new DateTime(DateTime.Today.Year, month, 1);
        int firstDay = firstDayOfMonth.DayOfYear - 1;
        int dayOfMonth = DateTime.DaysInMonth(DateTime.Today.Year, month);
        int Count = 0;
        for (int i = 0; i < dayOfMonth; i++)
        {
            string[] data = mData[firstDay + i].Split('-');
            if (data[0] == "1")
                Count++;
        }
        int[] result = new int[3];
        result[0] = Count;
        if (Count < 10)
            result[1] = (int)CUPTYPE.None;
        else if (Count < 20)
            result[1] = (int)CUPTYPE.Brone;
        else if (Count < dayOfMonth)
            result[1] = (int)CUPTYPE.Silver;
        else result[1] = (int)CUPTYPE.Gold;
        result[2] = dayOfMonth;
        return result;
    }

    public int getFirstInstall()
    {
        return PlayerPrefs.GetInt(KEY_FIRSTINSTALL);
    }

    public void onButtonTrophyClick()
    {
        TrophyController.ShowTrophy();
    }

    private void checkDoneInMonth()
    {
        DateTime firstDayOfMonth = new DateTime(DateTime.Today.Year, CurrentMonth, 1);
        int firstDay = firstDayOfMonth.DayOfYear - 1;
        int dayOfMonth = DateTime.DaysInMonth(DateTime.Today.Year, CurrentMonth);
        int Count = 0;
        for (int i = 0; i < dayOfMonth; i++)
        {
            string[] data = mData[firstDay + i].Split('-');
            if (data[0] == "1")
                Count++;
        }
        if (Count == 10 || Count == 20)
        {
            ShowCupWin(Count);
        }
    }

    private bool checkCompleteAllUntilToday()
    {
        DateTime firstDayOfMonth = new DateTime(DateTime.Today.Year, CurrentMonth, 1);
        int firstDay = firstDayOfMonth.DayOfYear - 1;
        var allDay = (DateTime.Today - firstDayOfMonth).TotalDays;
        for (int i = 0; i < allDay; i++)
        {
            string[] data = mData[firstDay + i].Split('-');
            if (data[0] != "1")
                return false;
        }
        return true;
    }

    private void ShowCupWin(int count)
    {
        if (count >= 10)
        {
            CupWin.sprite = TrophyController.Bronze;
        }

        if (count >= 20)
        {
            CupWin.sprite = TrophyController.Silver;
        }

        if (count >= 30)
        {
            CupWin.sprite = TrophyController.Gold;
        }
        CupWin.gameObject.SetActive(true);
        LeanTween.delayedCall(2.5f, () =>
        {
            LeanTween.alpha(CupWin.gameObject, 0, 0.5f).setOnComplete(() =>
            {
                CupWin.gameObject.SetActive(false);
            });
        });
    }

    public int getCurrentDaily()
    {
        return CurrentDaily;
    }

    public void ShowDailyMonth(string month, int numOfGame, int cupType, int dayOfMonth)
    {
        Month.text = month;

        if (cupType == (int)DailyController.CUPTYPE.None)
        {
            CupPopUp.sprite = TrophyController.Disable;
        }
        else if (cupType == (int)DailyController.CUPTYPE.Brone)
        {
            CupPopUp.sprite = TrophyController.Bronze;
        }
        else if (cupType == (int)DailyController.CUPTYPE.Silver)
        {
            CupPopUp.sprite = TrophyController.Silver;
        }
        else
        {
            CupPopUp.sprite = TrophyController.Gold;
        }

        if (numOfGame < 10)
        {
            Info.text = "Solved " + numOfGame + " out of " + dayOfMonth + "\n"
                + (10 - numOfGame) + " until Bronze";
        }
        else if (numOfGame < 20)
        {
            Info.text = "Solved " + numOfGame + " out of " + dayOfMonth + "\n"
                + (20 - numOfGame) + " until Silver";
        }
        else if (numOfGame < dayOfMonth)
        {
            Info.text = "Solved " + numOfGame + " out of " + dayOfMonth + "\n"
                + (dayOfMonth - numOfGame) + " until Gold";
        }
        else
        {
            Info.text = "Solved " + numOfGame + " out of " + dayOfMonth + "\n"
           + "Gold is earned";
        }
    }

    public void checkNewMonth()
    {
        if (CurrentMonth >= 2)
        {
            int currentGameInMonth = getNumberGameInMonth(CurrentMonth);
            if (currentGameInMonth == 0)
            {
                int gamePrevMonth = getNumberGameInMonth(CurrentMonth - 1);

                if (gamePrevMonth >= 10)
                {
                    int[] result = getNumOfGameInMonth(CurrentMonth - 1);
                    int numOfGameInMonth = result[0];
                    int cupType = result[1];
                    int numOfDayInMonth = result[2];

                    if (!PlayerPrefs.HasKey((CurrentMonth - 1) + "-" + CurrentYear))
                    {
                        ShowDailyMonth(MonthList[CurrentMonth - 2], numOfGameInMonth, cupType, numOfDayInMonth);
                        PopUpCupInfo.gameObject.SetActive(true);
                        PlayerPrefs.SetString((CurrentMonth - 1) + "-" + CurrentYear, "1");
                    }
                }
            }
            else
            {
                int[] result = getNumOfGameInMonth(CurrentMonth);
                int numOfGameInMonth = result[0];
                int cupType = result[1];
                int numOfDayInMonth = result[2];
                if (currentGameInMonth == numOfDayInMonth && !PlayerPrefs.HasKey(CurrentMonth + "-" + CurrentYear))
                {
                    ShowDailyMonth(MonthList[CurrentMonth - 1], numOfGameInMonth, cupType, numOfDayInMonth);
                    PopUpCupInfo.gameObject.SetActive(true);
                    PlayerPrefs.SetString(CurrentMonth + "-" + CurrentYear, "1");
                }
            }
        }
    }

    public void onButtonCloseCupPopUpInfo()
    {
        PopUpCupInfo.gameObject.SetActive(false);
    }

    public void OnCloseWarning()
    {
        TextPopup.SetActive(false);
    }

    public int getCurrentTime()
    {
        var data = mData[CurrentDaily - 1].Split('-');
        data[0] = "1";
        return int.Parse(data[1]);
    }

    public bool isDoneDailyToday()
    {
        int today = DateTime.Today.DayOfYear;
        var data = mData[today - 1].Split('-');
        if (int.Parse(data[0]) == 1)
            return true;
        else
            return false;
    }
}
