using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class DayController : MonoBehaviour
{
    public Image Main;
    public Sprite Normal, Current;
    public List<Image> DayText = new List<Image>();
    public List<Sprite> Number = new List<Sprite>();
    public Sprite Complete;
    public Image Block;
    private int currentDay;
    private int day;
    private bool isComplete;
    private bool isBlock;

    public void SetData(int day, bool isComplete, bool isBlock, int currentDay, bool isCompleteDaily = false, bool isReset = false, bool isSpecial = false, bool isFirst = false)
    {
        isSpecial = false;
        isFirst = false;
        Main.sprite = Normal;
        this.day = day;
        this.currentDay = currentDay;
        this.isComplete = isCompleteDaily;
        this.isBlock = isBlock;
        foreach (var item in DayText)
        {
            item.sprite = null;
        }

        var temp = day.ToString();
        if (temp.Length == 1)
            temp = "0" + temp;
        for (int i = 0; i < temp.Length; i++)
        {
            var ind = int.Parse(temp[i].ToString());
            if (!isCompleteDaily)
            {
                DayText[i].gameObject.SetActive(true);
                DayText[i].sprite = Number[ind];
            }
            else
            {
                if (i == 0)
                    DayText[0].sprite = Complete;
                else
                    DayText[i].gameObject.SetActive(false);
            }
            DayText[i].SetNativeSize();
        }
        Block.color = isBlock ? new Color32(255, 255, 255, 255) : new Color32(0, 0, 0, 0);
        if (!isReset)
        {
            if (currentDay == SceneManager.instance.DailyController.CurrentDaily || (SceneManager.instance.DailyController.isSameMonth && SceneManager.instance.DailyController.LastIndexNotClear == day && !SceneManager.instance.DailyController.AlreadySelect))
            {
                Main.sprite = Current;
                SceneManager.instance.DailyController.ShowDailyDay(currentDay, day, isCompleteDaily, isBlock);
            }

            if ((SceneManager.instance.DailyController.FirstShow || !SceneManager.instance.DailyController.AlreadySelect) && isSpecial)
            {
                if (isFirst)
                {
                    Main.sprite = SceneManager.instance.DailyController.YellowSquare;
                }
                else
                {
                    Main.sprite = Normal;
                }
            }
        }
        Block.raycastTarget = isBlock;

    }

    public void OnThisButtonClick()
    {
        SceneManager.instance.DailyController.Reset();
        SceneManager.instance.DailyController.DayOfMonth = day;
        Main.sprite = Current;
        SceneManager.instance.DailyController.AlreadySelect = true;
        SceneManager.instance.DailyController.ShowDailyDay(currentDay,day, isComplete, isBlock);
    }
}
