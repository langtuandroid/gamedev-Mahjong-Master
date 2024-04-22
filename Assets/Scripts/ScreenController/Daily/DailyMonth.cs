using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DailyMonth : MonoBehaviour
{ 
    public Button backGround;
    public Image cup;
    public Text currentMonth;
    public Text Total;
    private int numOfGameInMonth;
    private int cupType;
    private int numOfDayInMonth;
    private int month;

    private bool firstTime = false;

    List<string> MonthList = new List<string>()
    {
        "January", "February", "March", "April",
        "May", "June", "July", "August",
        "September", "October", "November", "December",
    };
    void Start()
    {

    }

    public void init(int month, TrophyController controller)
    {
        backGround.interactable = true;
        cup.gameObject.SetActive(true);
        currentMonth.gameObject.SetActive(true);
        this.month = month;
        currentMonth.text = MonthList[month - 1];
        int[] result = SceneManager.instance.DailyController.getNumOfGameInMonth(month);
        numOfGameInMonth = result[0];
        cupType = result[1];
        numOfDayInMonth = result[2];

        if (cupType == (int)DailyController.CUPTYPE.None)
        {
            cup.sprite = controller.Disable;
        }
        else if (cupType == (int)DailyController.CUPTYPE.Brone)
        {
            cup.sprite = controller.Bronze;
        }
        else if (cupType == (int)DailyController.CUPTYPE.Silver)
        {
            cup.sprite = controller.Silver;
        }
        else
        {
            cup.sprite = controller.Gold;
        }
        if (month == SceneManager.instance.DailyController.getFirstInstall())
        {
            firstTime = true;
            onClick();
        }
        Total.text = numOfGameInMonth + "/" + numOfDayInMonth;
    }

    public void onClick()
    {
        SceneManager.instance.DailyController.TrophyController.ShowDailyMonth(MonthList[month - 1], numOfGameInMonth, cupType, numOfDayInMonth);
        SceneManager.instance.DailyController.TrophyController.Border.position = this.transform.position;
        if (!firstTime)
        {
            SoundManager.instance.SoundOn(SoundManager.SoundIngame.Click);
        }
        firstTime = false;
    }

    public void disable()
    {
        backGround.interactable = false;
        cup.gameObject.SetActive(false);
        currentMonth.gameObject.SetActive(false);
        Total.text = "";
    }
}
