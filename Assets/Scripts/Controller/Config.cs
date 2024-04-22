using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Config : MonoBehaviour {

	public static Config instance;
	public int MaxLevel = 0;
	public int LevelPerMap = 60;

	public List<string> MapName = new List<string>();
	public List<int> GiftPerDay = new List<int>();

    public int BoxGift = 1;
    public int VideosGift = 1;

	void Awake()
	{
		Config.instance = this;
	}
}
