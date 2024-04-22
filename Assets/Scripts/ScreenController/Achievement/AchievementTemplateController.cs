using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementTemplateController : MonoBehaviour {

	public Image New, Gift, Get;
	public GameObject GetObj;
	public Text Title, Des, Percent;

	private bool isBlock;

	private int currentLevel = 0;
	private int blockLevel = 0;

	private int sta = 0;

	private int index = 0;

	private int realLevel;

	public void InitUI(int _index, string title, string des, int level, int block, int _realLevel)
	{
		index = _index;
		Title.text = title;
		Des.text = des;
		currentLevel = level;
		blockLevel = block;
		realLevel = _realLevel;
	}

	public void CheckData(int index, int status)
	{
		GetObj.SetActive(true);
		sta = status;
		isBlock = false;
		if (index < 3)
		{
			if (status == 0)
			{
				New.color = new Color (1,1,1,0);
				Gift.color = new Color (1,1,1,0);
				Get.fillAmount = 0f;
				Percent.text = "0/1";
			}
			else if (status == 1)
			{
				New.color = new Color (1,1,1,1);
				Gift.color = new Color (1,1,1,0);
				Get.fillAmount = 1f;
				Percent.text = "GET !";
			}
			else
			{
				New.color = new Color (1,1,1,0);
				Gift.color = new Color (1,1,1,1);
				Get.fillAmount = 1f;
				Percent.text = "GET !";
				GetObj.SetActive(false);
			}
		}
		else
		{
            var lastLevel = SceneManager.instance.AchievementController.LastClearLevel;
			GetObj.SetActive(true);
			if (status == 0)
			{
				New.color = new Color (1,1,1,0);
				Gift.color = new Color (1,1,1,0);
				Get.fillAmount = 0f;
				var curLevel = lastLevel - blockLevel;
				if (curLevel < 0)
				{
					curLevel = 0;
				}
                Percent.text = string.Format("{0}/{1}", curLevel, currentLevel);
				Get.fillAmount = (float)curLevel / currentLevel;
			}

			if (status == 0 && lastLevel >= currentLevel && lastLevel >= blockLevel + currentLevel)
			{
				status = 1;
                SceneManager.instance.AchievementController.UserAchi[index] = 1;
			}

			if (status == 1)
			{
				New.color = new Color (1,1,1,1);
				Gift.color = new Color (1,1,1,0);
				Get.fillAmount = 1f;
				Percent.text = "GET !";
			}
			else if (status == 2)
			{
				New.color = new Color (1,1,1,0);
				Gift.color = new Color (1,1,1,1);
				Get.fillAmount = 1f;
				Percent.text = "GET !";
				GetObj.SetActive(false);
			}

			sta = status;
		}
	}

    public bool isGet(int index, int status)
    {
        if (index < 3)
        {
            if (status == 1)
                return true;
        }
        else
        {
            if (status == 0 && SceneManager.instance.MaxUserLevel >= currentLevel)
            {
                return true;
            }
        }
        return false;
    }

    public void OnThisGiftClick()
	{
		if (sta == 1)
		{
            SoundManager.instance.SoundOn(SoundManager.SoundIngame.Click);
            SceneManager.instance.AchievementController.EditAchi(index, 2);
            sta = 2;
            SceneManager.instance.AddHint(1);
			New.color = new Color (1,1,1,0);
			Gift.color = new Color (1,1,1,1);
			Get.fillAmount = 1f;
			Percent.text = "GET !";
			SceneManager.instance.AchievementController.CheckGetAllBtn();
			GetObj.SetActive(false);
            SceneManager.instance.AchievementController.showPopupGetAchi();
        }
	}
}
