using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementController : MonoBehaviour {

	public CanvasGroup Main;
	public List<AchievementTemplateController> TemplateList = new List<AchievementTemplateController>();
	public GameObject Template;
	public GameObject ParentObj;
	public GameObject GetAll;
    public GameObject PopUp;
    public Text TotalDone;

	public List<int> UserAchi = new List<int>();

    public int LastClearLevel = 0;

	void Start()
	{
		LeanTween.delayedCall(0.5f, () => {
			InitAchi();
			InitUserData();
		});
	}

	public void ShowAchi()
	{
		Main.alpha = 1;
		Main.blocksRaycasts = true;
		CheckUserData();
		CheckGetAllBtn();
        checkTotalDoneAchi();
		gameObject.transform.localPosition = Vector2.zero;
	}

	public void HideAchi()
	{
		Main.alpha = 0;
		Main.blocksRaycasts = false;
		gameObject.transform.localPosition = new Vector2(10000, 10000);
	}

	public void OnBackClick()
	{
		SoundManager.instance.SoundOn(SoundManager.SoundIngame.Click);
		HideAchi();
        if (SceneManager.instance.PreviousScreen == SceneManager.ScreenEnum.Home)
        {
            SceneManager.instance.HomeController.ShowHome();
        }
        else if (SceneManager.instance.PreviousScreen == SceneManager.ScreenEnum.Level)
        {
            SceneManager.instance.LevelController.ShowLevel();
        } else
        {
            SceneManager.instance.MapStageController.ShowMap();
        }
	}

	private List<string> title = new List<string>()
	{
		"First Game",
		"First Hint",
		"First Shuffle"
	};

	private List<string> des = new List<string>()
	{
		"Clear first game",
		"First time using Hint",
		"First time using Shuffle",
	};

	private string mapAchi = "Clear {0} level in map";

	void InitAchi()
	{
		List<int> level = new List<int>()
		{
			10,
			20,
			40,
			60
		};
		foreach (var item in Config.instance.MapName)
		{
			foreach (var lev in level)
			{
				title.Add(item);
                des.Add(string.Format(mapAchi, lev));
			}
		}

		for (var i = 0; i < title.Count; i++)
		{
			GameObject template = Instantiate(Template) as GameObject;
			template.transform.SetParent(ParentObj.transform);
			template.transform.localScale = Vector3.one;
			template.SetActive(true);
			var temp = template.GetComponent<AchievementTemplateController>();
			var cur = 0;
			int block = 0;
			if (i >= 3)
			{
				var index = i - 3;
				if (index % 4 == 3)
				{
					cur = 60;
				} 
				else if (index % 4 == 2)
				{
					cur = 40;
				} 
				else if (index % 4 == 1)
				{
					cur = 20;
				} 
				else
				{
					cur = 10;
				}
				block = (i - 3) / 4 * 60;
			}
			temp.InitUI(i, title[i], des[i], cur, block, block + cur);
			TemplateList.Add(temp);
		}
	}

    public void InitUserData()
    {
        var data = PlayerPrefs.GetString("USER_ACHI");
        var partData = data.Split('-');
        foreach (var item in partData)
        {
            if (item != "")
            {
                UserAchi.Add(int.Parse(item));
            }
        }
        for (var i = UserAchi.Count; i < TemplateList.Count; i++)
        {
            UserAchi.Add(0);
        }

		CheckUserData();
	}

	public void SaveAchi()
	{
		var data = "";
		foreach(var item in UserAchi)
		{
			data += item + "-";
		}

		data = data.TrimEnd('-');
		PlayerPrefs.SetString("USER_ACHI", data);
	}

	public void EditAchi(int index, int value)
	{
		if (index > UserAchi.Count)
			return;
		UserAchi[index] = value;
		SaveAchi();
	}

	public void CheckUserData()
	{
        LastClearLevel = 0;
        LastClearLevel = SceneManager.instance.PreLastLevel;

        for (var i = 0; i < TemplateList.Count; i++)
		{
			TemplateList[i].CheckData(i, UserAchi[i]);
            if (UserAchi[i] == 2)
            {
                TemplateList[i].transform.SetAsLastSibling();
            }
		}
    }

	public void OnGetAllClick()
	{
		foreach (var item in TemplateList) {
			item.OnThisGiftClick();
		}
		SoundManager.instance.SoundOn(SoundManager.SoundIngame.Click);
	}

	public bool CheckGetAllBtn()
	{
		GetAll.SetActive(false);
		foreach(var item in UserAchi)
		{
			if (item == 1)
			{
				GetAll.SetActive(true);
				return true;
			}
		}
        return false;
	}

    public void showPopupGetAchi()
    {
        PopUp.SetActive(true);
        SoundManager.instance.SoundOn(SoundManager.SoundIngame.Bonus);
    }

    public bool isGetGift()
    {
        for (int i = 0; i < TemplateList.Count; i++)
        {
           if(TemplateList[i].isGet(i, UserAchi[i]) == true)
            {
                return true;
            }
        }
        return false;
    }

    private void checkTotalDoneAchi()
    {
        int count = 0;
        for (int i = 0; i < TemplateList.Count; i++)
        {
            if (UserAchi[i] > 0)
                count++;
        }
        TotalDone.text = string.Format("{0}/{1}", count, TemplateList.Count);
    }

    public void onButtonOkClick()
    {
        SoundManager.instance.SoundOn(SoundManager.SoundIngame.Click);
        PopUp.SetActive(false);
    }
}
