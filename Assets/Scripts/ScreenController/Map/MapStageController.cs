using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapStageController : MonoBehaviour {

	public CanvasGroup Main;
	public GameObject Template, MapBox;
	public List<MapTemplateController> AllTemplate = new List<MapTemplateController>();
    public Text Lobster;
    public bool IsShowing;
    public GameObject PopupBlock;

    public GameObject NotiDaily;
    public GameObject NotiTrophy;

    public void ShowMap()
	{
        SceneManager.instance.AchievementController.CheckUserData();
        CheckSoundIcon();
        IsShowing = true;
        Main.alpha = 1;
        Main.blocksRaycasts = true;
        this.gameObject.transform.localPosition = Vector2.zero;
        ReloadMap();
        PopupBlock.SetActive(false);

        if (SceneManager.instance.DailyController.isDoneDailyToday())
            NotiDaily.SetActive(false);
        else
            NotiDaily.SetActive(true);

        NotiTrophy.SetActive(SceneManager.instance.AchievementController.CheckGetAllBtn());
    }

	public void HideMap()
	{
        IsShowing = false;
        Main.alpha = 0;
        Main.blocksRaycasts = false;
        this.gameObject.transform.localPosition = new Vector2(10000, 10000);
        SceneManager.instance.PreviousScreen = SceneManager.ScreenEnum.Map;
    }

	void Start()
	{
		LeanTween.delayedCall(0.1f, () =>
		{
			initMap();
		});
	}

	void initMap()
	{
		for(var i = 0; i < Config.instance.MapName.Count; i++) 
		{
			GameObject template = Instantiate(Template) as GameObject;
			template.transform.SetParent(MapBox.transform);
			template.transform.localScale = Vector3.one;
			template.SetActive(true);
			var temp = template.GetComponent<MapTemplateController>();
			temp.InitTemplate(i);
			AllTemplate.Add(temp);
		}
	}

	public void OnBackClick()
	{
		HideMap();
		SceneManager.instance.HomeController.ShowHome();
		SoundManager.instance.SoundOn(SoundManager.SoundIngame.Click);
	}

	void ReloadMap()
	{
		for(var i = 0; i < Config.instance.MapName.Count; i++) 
		{
			AllTemplate[i].InitTemplate(i);
		}
        Lobster.text = SceneManager.instance.GetAllLobster().ToString();
	}

    public void OnShowBlock()
    {
        PopupBlock.SetActive(true);
    }

    public void OnHideBlock()
    {
        PopupBlock.SetActive(false);
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
