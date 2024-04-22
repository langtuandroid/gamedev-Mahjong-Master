using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class TrophyController : MonoBehaviour
{

    // Use this for initialization
    public List<DailyMonth> Months;
    public Image Cup;
    public Text Month;
    public Text Info;
    public CanvasGroup group;
    public GameObject Result;
    public GameObject BGBottom;
    public RectTransform RectBGBottom;
    private bool isShow = false;
    public Sprite Bronze, Silver, Gold, Disable;
    public Transform Border;

    public void ShowTrophy()
    {
        init();
        group.alpha = 1;
        group.blocksRaycasts = true;
        transform.localPosition = new Vector3(0, 0, 0);
        isShow = true;
    }

    public void HideTrophy()
    {
        group.alpha = 0;
        group.blocksRaycasts = false;
        transform.localPosition = new Vector3(10000, 10000, 0);
        isShow = false;
    }

    public void ShowDailyMonth(string month, int numOfGame, int cupType, int dayOfMonth)
    {
        Month.text = month;
        if (cupType == (int)DailyController.CUPTYPE.None)
        {
            Cup.sprite = Disable;
        }
        else if (cupType == (int)DailyController.CUPTYPE.Brone)
        {
            Cup.sprite = Bronze;
        }
        else if (cupType == (int)DailyController.CUPTYPE.Silver)
        {
            Cup.sprite = Silver;
        }
        else
        {
            Cup.sprite = Gold;
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

    public void onButtonDoneClick()
    {
        HideTrophy();
    }

    public void init()
    {
        int firstInstall = SceneManager.instance.DailyController.getFirstInstall();
        int currentMonth = DateTime.Today.Month;
        for (int i = 0; i < 12; i++)
        {
            //if ((i < firstInstall - 1) || (i >= currentMonth))
            //    Months[i].disable();
            //else
                Months[i].init(i + 1, this);
        }
    }
}
