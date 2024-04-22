using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DailyGiftController : MonoBehaviour {

	public CanvasGroup Main;

	public List<Animator> Daily = new List<Animator>();
	public List<GameObject> CheckTick;
	public List<Text> HintPerDay = new List<Text>();

	int curHint = 0;

	public void ShowDailyGift()
	{
		Main.alpha = 1;
		this.gameObject.transform.localPosition = Vector2.zero;

		for(var i = 0; i < Config.instance.GiftPerDay.Count; i++)
		{
			HintPerDay[i].text = "+" + Config.instance.GiftPerDay[i];
		}

		CheckCurrentGift();
	}

	public void HideDailyGift()
	{
		Main.alpha = 0;
		this.transform.position = new Vector2(10000, 10000);
	}

	public void OnOkClick()
	{
		SoundManager.instance.SoundOn(SoundManager.SoundIngame.Click);
		HideDailyGift();
		SceneManager.instance.SetCurrentDaily();
		SceneManager.instance.SetDailyGift();
		SceneManager.instance.AddHint(curHint);
		SceneManager.instance.HomeController.ShowHome();
	}

  private void CheckCurrentGift()
	{
		var curGift = SceneManager.instance.GetCurrentDaily();
		if (curGift == 6)
			curGift = 0;

		foreach (var item in CheckTick)
		{
			item.SetActive(false);
		}

		for(var i = 0; i < curGift; i++)
		{
			CheckTick[i].SetActive(true);
		}
		curHint = Config.instance.GiftPerDay[curGift];
        CheckTick[curGift].transform.position = new Vector3(1000, 1000);
		CheckTick[curGift].SetActive(true);
		LeanTween.delayedCall(0.5f, () => {
			Daily[curGift].Play("ReceiveHint", 0, 0); 
		});
	}
}
