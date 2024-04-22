using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public delegate void eventItemCLick(int value);

public class ThemeController : MonoBehaviour {

    public CanvasGroup MainCanvas;
    public List<Image> Tile1, Tile2, Tile3, Tile4, Tile5, Tile6, Tile7;
    public List<Image> Bg;
    public List<Sprite> ListBg;
    public GameObject BgHolder;

    public GameObject CurTile, CurBg;
    public bool IsShowing;

    private int currentTile = 0, currentBg = 0;

    void Start()
    {
        Bg.Clear();
        
        var item2 = BgHolder.GetComponentsInChildren<Image>();
        foreach (var item in item2) {
            Bg.Add(item);
        }
    }

    public void ShowTheme()
    {
        IsShowing = true;
        MainCanvas.alpha = 1;
        transform.localPosition = Vector2.zero;
        InitUi();
    }

    public void HideTheme()
    {
        IsShowing = false;
        MainCanvas.alpha = 0;
        transform.position = new Vector2(10000, 10000);
    }

    public void OnBackClick()
    {
        SoundManager.instance.SoundOn(SoundManager.SoundIngame.Click);
        HideTheme();
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

    public void OnChangeTile(bool isLeft)
    {
        SoundManager.instance.SoundOn(SoundManager.SoundIngame.Click);
        if (isLeft)
        {
            currentTile--;
        }
        else
        {
            currentTile++;
        }
        if (currentTile < 0)
        {
            currentTile = 6;
        }
        else if (currentTile > 6)
        {
            currentTile = 0;
        }
        SceneManager.instance.SetCurrentTile(currentTile);
        InitUi();
    }

    public void OnChangeBg(bool isLeft)
    {
        SoundManager.instance.SoundOn(SoundManager.SoundIngame.Click);
        if (isLeft)
        {
            currentBg--;
        }
        else
        {
            currentBg++;
        }
        if (currentBg < 0)
        {
            currentBg = ListBg.Count - 1;
        }
        else if (currentBg > ListBg.Count - 1)
        {
            currentBg = 0;
        }
        SceneManager.instance.SetCurrentBg(currentBg);
        InitUi();
        SceneManager.instance.SetMainBg();
    }

    private void InitUi()
    {
        for (int i = 0; i < 3; i++)
        {
            Tile1[i].sprite = SceneManager.instance.PlayGameController.SpriteCard5[i];
            Tile2[i].sprite = SceneManager.instance.PlayGameController.SpriteCard1[i];
            Tile3[i].sprite = SceneManager.instance.PlayGameController.SpriteCard2[i];
            Tile4[i].sprite = SceneManager.instance.PlayGameController.SpriteCard3[i];
            Tile5[i].sprite = SceneManager.instance.PlayGameController.SpriteCard4[i];
            Tile6[i].sprite = SceneManager.instance.PlayGameController.SpriteCard[i];
            Tile7[i].sprite = SceneManager.instance.PlayGameController.SpriteCard6[i];
        }

        for (int i = 0; i < Bg.Count; i++) {
            Bg[i].sprite = ListBg[i];
        }
        
        SetData();
    }

    void SetData() {
        currentTile = SceneManager.instance.GetCurrentTile();
        currentBg = SceneManager.instance.GetCurrentBg();
        CurBg.transform.position = Bg[currentBg].transform.position;
        
        if (currentTile == 0) {
            CurTile.transform.position = Tile1[1].transform.position;
        } else if (currentTile == 1) {
            CurTile.transform.position = Tile2[1].transform.position;
        } else if (currentTile == 2) {
            CurTile.transform.position = Tile3[1].transform.position;
        } else if (currentTile == 3) {
            CurTile.transform.position = Tile4[1].transform.position;
        } else if (currentTile == 4) {
            CurTile.transform.position = Tile5[1].transform.position;
        } else if (currentTile == 5) {
            CurTile.transform.position = Tile6[1].transform.position;
        } else {
            CurTile.transform.position = Tile7[1].transform.position;
        }
    }

    public void OnTileClick(int index) {
        SoundManager.instance.SoundOn(SoundManager.SoundIngame.Click);
        SceneManager.instance.SetCurrentTile(index);
        SetData();
    }

    public void OnBgClick(int index) {
        SoundManager.instance.SoundOn(SoundManager.SoundIngame.Click);
        SceneManager.instance.SetCurrentBg(index);
        SetData();
        SceneManager.instance.SetMainBg();
    }
}
