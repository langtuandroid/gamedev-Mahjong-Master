using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayGameController : MonoBehaviour
{
    public Sprite Block1, Block2, HightLightSpr1, HightLightSpr2;
    public int MaxPoint = 6000;
    public int ShufflePoint = 60;
    public int HintPoint = 60;
    public int NormalPoint = 1;
    public int PointPerMatch = 10;
    private const int MaxCard = 128;
    private int MAX_SIZE_TABLE = 20;
    public bool IsShowing;
    public GameObject PopUpGift;
    public GameObject DailyNotif;
    public int TimeDelayHideDaily = 5;
    public Text DailyText;
    private String CurrentDaily = "";

    public GameObject Pos1, Pos2;

    public enum GameStatus
    {
        Normal,
        Win,
        Lose,
        Pause
    }

    public GameStatus CurrentStatus = GameStatus.Normal;
    public bool IsHightLight = false;

    public GameObject Pause, Win, Rate;
    public RectTransform FirstIndex;
    private float StartPosX, StartPosY;

    public float WidthPerCard = 0;
    public float HeightPerCard = 0;

    public int CurrentLevel;
    public int CurrentPoint;

    private CanvasGroup PlayGame;
    private int CurMaxCard = 0;
    public List<Sprite> SpriteCard;
    public List<Sprite> SpriteCard1;
    public List<Sprite> SpriteCard2;
    public List<Sprite> SpriteCard3;
    public List<Sprite> SpriteCard4;
    public List<Sprite> SpriteCard5;
    public List<Sprite> SpriteCard6;
    public List<Sprite> MainSpriteCard;

    public List<GameObject> TemplateGroup = new List<GameObject>();
    public List<GameObject> AllCard = new List<GameObject>();
    public List<List<CardTemplateController>> CardPerLayer = new List<List<CardTemplateController>>();
    public List<CardTemplateController> AllCardTemplate = new List<CardTemplateController>();
    public List<CardTemplateController> AllCardTemplateHandler = new List<CardTemplateController>();
    public List<List<int>> CardPerLayerIndex = new List<List<int>>();

    public GameObject TemplateCard;
    public GameObject Selected;
    public GameObject HightLight1, HightLight2;
    public GameObject TableCard;

    private List<int> indexPerLayer = new List<int>();
    public List<CardTemplateController> FirstListCard = new List<CardTemplateController>();
    public List<CardTemplateController> SecondListCard = new List<CardTemplateController>();

    public Button UndoBtn, ShuffleBtn;

    public Text Time, HintNum;
    public Animator HintBtn;
    private float timeCheckHint = 0f;
    bool canClick = true;
    private int CurrentTime = 0;
    int continousShuffle = 0;
    int currentTimeHint = 0;

    public GameObject AnimHolder, AnimTarget, AnimStart,Anim1, Anim2, QuitTar, RepTar, Quit, Replay, PauseBtn, AnimTemp;
    private Vector2 posAnimHolder;

    public GameObject AdHint;
    public Image SoundIcon;
    private int curShowHint = -1;

    private float boardScale = 1f;
    private Vector3 defaultBoardPos = new Vector3(0, 265f, 0f);

    public Image BG;

    int tweenId = 0;

    public GameObject Dis1, Dis2;
    
    void Awake()
    {
        PlayGame = GetComponent<CanvasGroup>();
    }

    void Start() {
        posAnimHolder = AnimHolder.transform.position;
        StartPosX = FirstIndex.localPosition.x;
        StartPosY = FirstIndex.localPosition.y;
        DefautBtnListY = BtnList.transform.localPosition.y;
        DefaultMoveY = MoveBtn.transform.localPosition.y;
        NextBtnY = DefautBtnListY - 500f;
        NextMoveY = DefaultMoveY - 150f;
    }

    public void ShowPlayGame()
    {
        HintNum.text = SceneManager.instance.GetHint().ToString();
        if (HintNum.text == "0") {
            HintNum.text = "+";
        }
        PopUpGift.SetActive(false);
        IsShowing = true;
        PlayGame.alpha = 1;
        PlayGame.blocksRaycasts = true;
        gameObject.transform.localPosition = Vector2.zero;
        isClickPause = false;
        Quit.transform.localScale = Vector2.zero;
        Quit.transform.position = PauseBtn.transform.position;
        Replay.transform.localScale = Vector2.zero;
        Replay.transform.position = PauseBtn.transform.position;
        clickingPause = false;
        CheckSoundIcon();
    }

    public void InitTable()
    {
        
        var index = SceneManager.instance.GetCurrentTile();

        if (index == 0)
        {
            MainSpriteCard = SpriteCard5;
        }
        else if (index == 1)
        {
            MainSpriteCard = SpriteCard1;
        }
        else if (index == 2)
        {
            MainSpriteCard = SpriteCard2;
        }
        else if (index == 3)
        {
            MainSpriteCard = SpriteCard3;
        }
        else if (index == 4)
        {
            MainSpriteCard = SpriteCard4;
        }
        else if (index == 5)
        {
            MainSpriteCard = SpriteCard;
        }
        else 
        {
            MainSpriteCard = SpriteCard6;
        }

        TemplateCard.GetComponent<CardTemplateController>().Block.sprite = MainSpriteCard[0];
        HightLight1.GetComponent<Image>().sprite = MainSpriteCard[0];
        HightLight2.GetComponent<Image>().sprite = MainSpriteCard[0];
        curShowHint = -1;
        CurrentStatus = GameStatus.Normal;
        Pause.SetActive(false);
        Win.SetActive(false);
        LoadMapData();
        OnShowRate(true);
        HintNum.text = SceneManager.instance.GetHint().ToString();
    }

    public void SetLevel()
    {
        CurrentLevel = SceneManager.instance.CurrentMapData.Level;
        // Level.text = "Level: " + CurrentLevel.ToString();
    }

    private void ClearCache()
    {
        LeanTween.cancelAll();
        continousShuffle = 0;
        foreach (var obj in AllCard)
        {
            Destroy(obj);
        }

        CurrentPoint = MaxPoint;
        CardPerLayerIndex.Clear();
        indexPerLayer.Clear();
        AllCardTemplate.Clear();
        AllCard.Clear();
        AllCardTemplateHandler.Clear();
        CardPerLayer.Clear();
        FirstListCard.Clear();
        SecondListCard.Clear();
        Selected.SetActive(false);
        isSelected = false;
        preSelectedCard = null;
        canClick = true;
        ShuffleBtn.interactable = true;
    }

    public void LoadMapData() {
        ResetHintAnim();
        CheckShuffleBtn();
        StopCoroutine("reShuffle");
        Time.text = "00:00";
        CurrentTime = 0;
        ClearCache();
        GenerateMap();
        StartToLoopPoint();
        LoopTime();
        AnimHolder.transform.position = AnimStart.transform.position;
        foreach (var card in AllCardTemplateHandler)
        {
            card.HightLight();
        }
    }

    private void LoopTime()
    {
        if (CurrentStatus == GameStatus.Normal)
        {
            LeanTween.value(0, 1, 1f).setOnComplete(s =>
            {
                CurrentTime += 1;
                Time.text = string.Format("{0:00} : {1:00}", CurrentTime / 60, CurrentTime % 60);
                LoopTime();
            });
        }
    }

    public void StartToLoopPoint()
    {
        if (CurrentStatus == GameStatus.Normal)
        {
            if (CurrentPoint > 0)
            {
                LeanTween.value(0, 1, 1f).setOnComplete(s =>
                  {
                      if (IsHightLight)
                      {
                          CurrentPoint -= NormalPoint * 2;
                      }
                      else
                      {
                          CurrentPoint -= NormalPoint;
                      }

                      if (CurrentPoint < 0)
                          CurrentPoint = 0;
                      StartToLoopPoint();
                  });
            }
        }
    }

    public void HidePlayGame()
    {
        IsShowing = false;
        PlayGame.alpha = 0;
        PlayGame.blocksRaycasts = false;
        this.gameObject.transform.localPosition = new Vector2(10000, 10000);
    }

    //Map size is 20 * 20
    private void GenerateMap()
    {
        InitListCard();
        FilterPerLayer();
    }

    int indexDaily = 0;
    private void InitListCard()
    {
        var total = 0;
        if (SceneManager.instance.m_GameMode == SceneManager.GameMode.Arcade)
        {
            CurrentLevel = SceneManager.instance.CurrentMapData.Level;
            total = SceneManager.instance.X_Pos[CurrentLevel - 1].Count / 2;
        }
        else
        {
            try
            {
                total = SceneManager.instance.X_PosDaily[SceneManager.instance.DailyController.CurrentDaily].Count / 2;
            }
            catch
            {
                indexDaily = UnityEngine.Random.Range(0, SceneManager.instance.X_Pos.Count - 1);
                total = SceneManager.instance.X_Pos[indexDaily - 1].Count / 2;
            }
        }

        int curIndex = 0;
        for (int i = 0; i < total; i++)
        {
            if (curIndex >= MainSpriteCard.Count)
            {
                curIndex = 0;
            }

            //first card
            GameObject tempObj = Instantiate(TemplateCard) as GameObject;
            CardTemplateController tempCard = tempObj.GetComponent<CardTemplateController>();
            tempCard.CurrentCard = new CardBase();
            tempCard.CurrentCard.CardIndex = curIndex;
            tempCard.CardIndex.sprite = MainSpriteCard[curIndex];
            tempCard.name = i.ToString();
            AllCardTemplate.Add(tempCard);
            AllCard.Add(tempObj);
            
            //second card
            GameObject tempObj2 = Instantiate(TemplateCard) as GameObject;
            CardTemplateController tempCard2 = tempObj2.GetComponent<CardTemplateController>();
            tempCard2.CurrentCard = new CardBase();
            tempCard2.CurrentCard.CardIndex = curIndex;
            tempCard2.CardIndex.sprite = MainSpriteCard[curIndex];
            tempCard2.name = i + "aa";
            AllCardTemplate.Add(tempCard2);
            AllCard.Add(tempObj2);

            curIndex++;
        }
    }

    private void FilterPerLayer()
    {
        LoopCreateMap();
        OnCheckBlockAllCard();
        SortMapBeforePlay();
    }

    private void ResetHintAnim() {
        timeCheckHint = 0f;
        HintBtn.enabled = false;
        HintBtn.GetComponent<Image>().color = new Color(1,1,1,1);
        HintBtn.transform.position = new Vector3(HintBtn.transform.position.x, PauseBtn.transform.position.y);
    }

    List<int> xPos = new List<int>();
    List<int> yPos = new List<int>();
    List<int> zPos = new List<int>();

    private void LoopCreateMap()
    {
        xPos.Clear();
        yPos.Clear();
        zPos.Clear();
        if(SceneManager.instance.m_GameMode == SceneManager.GameMode.Arcade)
        {
            foreach(var item in SceneManager.instance.X_Pos[CurrentLevel - 1])
            {
                xPos.Add(item);
            }

            foreach (var item in SceneManager.instance.Y_Pos[CurrentLevel - 1])
            {
                yPos.Add(item);
            }

            foreach (var item in SceneManager.instance.Z_Pos[CurrentLevel - 1])
            {
                zPos.Add(item);
            }
        }
        else
        {
            try
            {

                foreach (var item in SceneManager.instance.X_PosDaily[SceneManager.instance.DailyController.CurrentDaily])
                {
                    xPos.Add(item);
                }

                foreach (var item in SceneManager.instance.Y_PosDaily[SceneManager.instance.DailyController.CurrentDaily])
                {
                    yPos.Add(item);
                }

                foreach (var item in SceneManager.instance.Z_PosDaily[SceneManager.instance.DailyController.CurrentDaily])
                {
                    zPos.Add(item);
                }
            }
            catch
            {
                foreach (var item in SceneManager.instance.X_Pos[indexDaily - 1])
                {
                    xPos.Add(item);
                }

                foreach (var item in SceneManager.instance.Y_Pos[indexDaily - 1])
                {
                    yPos.Add(item);
                }

                foreach (var item in SceneManager.instance.Z_Pos[indexDaily - 1])
                {
                    zPos.Add(item);
                }
            }
        }
        var cardLength = xPos.Count;
        var y = 0;
        int minX = 1000, minY = 1000, maxX = 0, maxY = 0;
        int maxZ = 0;
        for (int i = 0; i < cardLength; i++)
        {
            while (zPos[i] >= CardPerLayer.Count)
            {
                CardPerLayer.Add(new List<CardTemplateController>());
                ResetIndexPerLayer(CardPerLayer.Count - 1);
            }

            int random = UnityEngine.Random.Range(0, AllCardTemplate.Count);
            var card = AllCardTemplate[random];
            card.CurrentCard.CardLayer = zPos[i];
            card.CurrentCard.Position = (xPos[i] - 1) + (MAX_SIZE_TABLE) * (yPos[i] - 1);
            int posX = xPos[i] - 1;
            if (posX <= minX)
            {
                minX = posX;
                if (zPos[i] + 1 > maxZ)
                {
                    maxZ = zPos[i] + 1;
                }
            }
            else if (posX >= maxX)
            {
                maxX = posX;
                if (zPos[i] + 1 > maxZ)
                {
                    maxZ = zPos[i] + 1;
                }
            }
            int posY = yPos[i] - 1;

            if (posY < minY)
            {
                minY = posY;
            }
            else if (posY > maxY)
            {
                maxY = posY;
            }
            card.transform.SetParent(TemplateGroup[zPos[i]].transform);
            card.transform.localScale = Vector3.one;
            card.transform.localPosition = new Vector2(StartPosX + posX * WidthPerCard, StartPosY - posY * HeightPerCard);
            card.DefaultPos = card.transform.position;
            CardPerLayer[zPos[i]].Add(card);
            AllCardTemplate.RemoveAt(random);
            card.IgnoreClick(false);
            AllCardTemplateHandler.Add(card);
        }
        var sizeX = maxX - minX;
        var sizeY = maxY - minY;
        var tempSize = maxZ == CardPerLayer.Count ? CardPerLayer.Count : 0;
        var size = sizeX < sizeY ? sizeY + tempSize : sizeX + tempSize;
        if (sizeX <= 8 && sizeY <=8)
        {
            boardScale = 1.7f;
        }
        else if (size <= 12)
        {
            boardScale = 1.5f;
        }
        else if (size <= 14)
        {
            boardScale = 1.3f;
        }
        else if (size <= 16)
        {
            boardScale = 1.15f;
        }
        else if (size <= 18)
        {
            boardScale = 1.1f;
        }
        else if (size <= 20)
        {
            boardScale = 1f;
        }
        else if (size <= 24)
        {
            boardScale = 0.9f;
        }
        else if (size <= 28)
        {
            boardScale = 0.8f;
        }
        foreach (var item in TemplateGroup)
        {
            item.transform.localScale = new Vector3(boardScale, boardScale, boardScale);
        }

        if (minY <= 2)
        {
            var temp = 40;
            if (minY == 0 || minY == 1)
            {
                temp = 80;
            } else if (minY == -1)
            {
                temp = 120;
            }
            TableCard.transform.localPosition = new Vector3(0, defaultBoardPos.y - temp, 0);
        }
        else
        {
            TableCard.transform.localPosition = defaultBoardPos;
        }
        Selected.transform.localScale = new Vector3(boardScale, boardScale, boardScale);
        HightLight1.transform.localScale = new Vector3(boardScale, boardScale, boardScale);
        HightLight2.transform.localScale = new Vector3(boardScale, boardScale, boardScale);
    }

    private void FilterListIndex(int _value)
    {
        List<int> ActiveObj = new List<int>();

        if ((_value + 1) % MAX_SIZE_TABLE != 0)
        {
            ActiveObj.Add(_value + 1);
            ActiveObj.Add(_value + MAX_SIZE_TABLE + 1);
            ActiveObj.Add(_value - (MAX_SIZE_TABLE - 1));
        }

        ActiveObj.Add(_value - MAX_SIZE_TABLE);
        ActiveObj.Add(_value + MAX_SIZE_TABLE);

        if (_value % MAX_SIZE_TABLE != 0)
        {
            ActiveObj.Add(_value + MAX_SIZE_TABLE - 1);
            ActiveObj.Add(_value - 1);
            ActiveObj.Add(_value - (MAX_SIZE_TABLE + 1));
        }

        foreach (var item in ActiveObj)
        {
            indexPerLayer.Remove(item);
        }
        indexPerLayer.Remove(_value);
    }

    private void OnCheckBlockAllCard()
    {
        CardPerLayerIndex.Clear();
        foreach (var layer in CardPerLayer)
        {
            List<int> tempList = new List<int>();
            foreach (var card in layer)
            {
                tempList.Add(card.CurrentCard.Position);
            }
            CardPerLayerIndex.Add(tempList);
        }

        CheckBlockPerCard();
    }

    int shuffleTimes = 0;
    public List<CardTemplateController> ListCardCanClick = new List<CardTemplateController>();
    private void CheckBlockPerCard()
    {
        CheckUndo();
        ListCardCanClick.Clear();
        foreach (var layer in CardPerLayer)
        {
            foreach (var card in layer)
            {
                bool isBlock = false;
                for(int i = card.CurrentCard.CardLayer + 1; i < TemplateGroup.Count; i++)
                {
                    if(CardPerLayerIndex.Count > i)
                    {
                        isBlock = CheckCardInTopLayer(card, i);
                        if (isBlock)
                            break;
                    }
                }

                card.CurrentCard.CanClick = !isBlock;
                if (isBlock)
                    card.LockClick(true);
                else
                    card.LockClick(CheckCardInLayer(card, CardPerLayerIndex[card.CurrentCard.CardLayer]));

                if (card.CurrentCard.CanClick)
                {
                    ListCardCanClick.Add(card);
                }
            }
        }

        if (ListCardCanClick.Count == 0) {
            LeanTween.delayedCall(1f, () => { WinGame(); });
        }
        else if (OnCheckLose())
        {
            OnShuffleLose();
        }
        else
        {
            foreach (var card in AllCardTemplateHandler)
            {
                card.HightLight();
            }
        }
    }

    private bool isCheckShuffle;
    IEnumerator reShuffle() {
        OffHint();
        if (!isCheckShuffle) {
            isCheckShuffle = true;
            var temp = new List<CardTemplateController>();
            foreach (var item in CardPerLayer) {
                foreach (var card in item) {
                    temp.Add(card);
                }
            }

            CheckBlockPerCard();

            if (temp.Count == 2)
            {
                temp[0].CurrentCard.CardLayer = 0;
                temp[0].CurrentCard.Position = 149;

                var indexCard = temp[0].CurrentCard.Position;
                int posX = indexCard % MAX_SIZE_TABLE;
                int posY = indexCard / MAX_SIZE_TABLE;
                temp[0].transform.SetParent(TemplateGroup[0].transform);
                temp[0].transform.localScale = Vector3.one;
                temp[0].transform.localPosition = new Vector2(StartPosX + posX * WidthPerCard, StartPosY - posY * HeightPerCard);

                temp[1].CurrentCard.CardLayer = 0;
                temp[1].CurrentCard.Position = 151;

                var indexCard2 = temp[1].CurrentCard.Position;
                temp[1].CurrentCard.CardIndex = temp[0].CurrentCard.CardIndex;
                temp[1].ResetImageCardIndex();
                posX = indexCard2 % MAX_SIZE_TABLE;
                posY = indexCard2 / MAX_SIZE_TABLE;
                temp[1].transform.SetParent(TemplateGroup[0].transform);
                temp[1].transform.localScale = Vector3.one;
                temp[1].transform.localPosition = new Vector2(StartPosX + posX * WidthPerCard, StartPosY - posY * HeightPerCard);
                CardPerLayer.Clear();
                CardPerLayer.Add(temp);
                temp[0].LockClick(false);
                temp[1].LockClick(false);
            }
            else if (ListCardCanClick.Count < 2)
            {
                RandomRemainCard();
            }
            else
            {
                int index = 0;
                do
                {
                    List<int> tempList = new List<int>();
                    foreach (var item in CardPerLayer)
                    {
                        foreach (var card in item)
                        {
                            tempList.Add(card.CurrentCard.CardIndex);
                        }
                    }

                    int numberOfMatch = ListCardCanClick.Count % 2 == 0 ? ListCardCanClick.Count / 2 : (ListCardCanClick.Count - 1) / 2;
                    List<string> cardMatched = new List<string>();

                    for (int i = 0; i < numberOfMatch; i++)
                    {
                        int random = UnityEngine.Random.Range(0, tempList.Count);
                        ListCardCanClick[i].CurrentCard.CardIndex = tempList[random];
                        ListCardCanClick[i].ResetImageCardIndex();
                        ListCardCanClick[i + numberOfMatch].CurrentCard.CardIndex = ListCardCanClick[i].CurrentCard.CardIndex;
                        ListCardCanClick[i + numberOfMatch].ResetImageCardIndex();
                        cardMatched.Add(ListCardCanClick[i].name);
                        cardMatched.Add(ListCardCanClick[i + numberOfMatch].name);
                        tempList.RemoveAt(random);
                        tempList.Remove(ListCardCanClick[i].CurrentCard.CardIndex);
                    }

                    foreach (var item in CardPerLayer)
                    {
                        foreach (var card in item)
                        {
                            if (!cardMatched.Contains(card.name))
                            {
                                int random = UnityEngine.Random.Range(0, tempList.Count);
                                card.CurrentCard.CardIndex = tempList[random];
                                tempList.RemoveAt(random);
                                card.ResetImageCardIndex();
                            }
                        }
                    }
                    yield return null;
                    index++;
                } while (!CheckShuffle() && index < 3);
            }

            LeanTween.delayedCall(0.2f, f =>
            {
                LeanTween.cancel(tweenId);
                LeanTween.scale(TableCard, Vector3.one, 0.1f).setOnComplete(g =>
                {
                    LeanTween.delayedCall(0.2f, s =>
                    {
                        canClick = true;
                        ShuffleBtn.interactable = true;
                        continousShuffle = 0;
                        isCheckShuffle = false;
                    });
                    foreach (var card in AllCardTemplateHandler)
                    {
                        card.HightLight();
                    }
                });
            });
        }
    }

    private void SortMapBeforePlay()
    {
        List<int> tempList = new List<int>();
        foreach (var item in CardPerLayer)
        {
            foreach (var card in item)
            {
                tempList.Add(card.CurrentCard.CardIndex);
            }
        }

        int numberOfMatch = ListCardCanClick.Count % 2 == 0 ? ListCardCanClick.Count / 2 : (ListCardCanClick.Count - 1) / 2;
        List<string> cardMatched = new List<string>();

        for (int i = 0; i < numberOfMatch; i++)
        {
            int random = UnityEngine.Random.Range(0, tempList.Count);
            ListCardCanClick[i].CurrentCard.CardIndex = tempList[random];
            ListCardCanClick[i].ResetImageCardIndex();
            ListCardCanClick[i + numberOfMatch].CurrentCard.CardIndex = ListCardCanClick[i].CurrentCard.CardIndex;
            ListCardCanClick[i + numberOfMatch].ResetImageCardIndex();
            cardMatched.Add(ListCardCanClick[i].name);
            cardMatched.Add(ListCardCanClick[i + numberOfMatch].name);
            tempList.RemoveAt(random);
            tempList.Remove(ListCardCanClick[i].CurrentCard.CardIndex);
        }

        foreach (var item in CardPerLayer)
        {
            foreach (var card in item)
            {
                if (!cardMatched.Contains(card.name))
                {
                    int random = UnityEngine.Random.Range(0, tempList.Count);
                    card.CurrentCard.CardIndex = tempList[random];
                    tempList.RemoveAt(random);
                    card.ResetImageCardIndex();
                }
            }
        }
    }

    private bool CheckShuffle() 
    {
        ListCardCanClick.Clear();
        foreach (var layer in CardPerLayer)
        {
            foreach (var card in layer)
            {
                bool isBlock = false;
                for(int i = card.CurrentCard.CardLayer + 1; i < TemplateGroup.Count; i++)
                {
                    if(CardPerLayerIndex.Count > i)
                    {
                        isBlock = CheckCardInTopLayer(card, i);
                        if (isBlock)
                            break;
                    }
                }

                card.CurrentCard.CanClick = !isBlock;
                if (isBlock)
                    card.LockClick(true);
                else
                    card.LockClick(CheckCardInLayer(card, CardPerLayerIndex[card.CurrentCard.CardLayer]));

                if (card.CurrentCard.CanClick)
                {
                    ListCardCanClick.Add(card);
                }
            }
        }

        List<int> tempList = new List<int>();
        CardTemplateController first = null;

        foreach (var item in ListCardCanClick)
        {
            if (!tempList.Contains(item.CurrentCard.CardIndex))
            {
                tempList.Add(item.CurrentCard.CardIndex);
            }
            else
            {
                first = item;
                break;
            }
        }

        if (first != null && first.CardIndex != null)
        {
            foreach (var item in ListCardCanClick)
            {
                if (item.CurrentCard.CardIndex == first.CurrentCard.CardIndex)
                {
                    return true;
                }
            }
        }
        return false;
    }

    bool CheckCardInTopLayer(CardTemplateController card,int layerIndex)
    {
        List<int> tempList = CardPerLayerIndex[layerIndex];

        int _value = card.CurrentCard.Position;

        List<int> temp2 = new List<int>() 
        {
           _value, _value + 1, _value - 1, _value + MAX_SIZE_TABLE, _value - MAX_SIZE_TABLE, _value + MAX_SIZE_TABLE + 1, _value + MAX_SIZE_TABLE - 1, _value - MAX_SIZE_TABLE + 1, _value - MAX_SIZE_TABLE - 1
        };

        if (tempList.Count > 0)
        {
            foreach (var item in tempList) 
            {
                foreach(var item2 in temp2) 
                {
                    if (item == item2)
                        return true;
                }
            }
        }

        return false;
    }

    bool CheckCardInLayer(CardTemplateController card, List<int> tempList)
    {
        int _value = card.CurrentCard.Position;

        bool blockRight = false;
        if ((_value + 2) % MAX_SIZE_TABLE == 0 || (_value + 2) % MAX_SIZE_TABLE == 0)
            blockRight = false;
        else if ((_value + 1) % MAX_SIZE_TABLE != 0)
        {
            if (tempList.Contains(_value + MAX_SIZE_TABLE + 2) || tempList.Contains(_value - (MAX_SIZE_TABLE - 2))
                || tempList.Contains(_value + 2))
                blockRight = true;
        }

        bool blockLeft = false;
        if ((_value - 1) % MAX_SIZE_TABLE == 0 || (_value - 2) % MAX_SIZE_TABLE == 0)
            blockLeft = false;
        else if (_value % MAX_SIZE_TABLE != 0)
        {
            if (tempList.Contains(_value + MAX_SIZE_TABLE - 2) || tempList.Contains(_value - (MAX_SIZE_TABLE + 2))
                || tempList.Contains(_value - 2))
                blockLeft = true;
        }

        if (blockLeft && blockRight)
            return true;

        return false;
    }

    public void WinGame()
    {
        if (CurrentStatus == GameStatus.Win)
        {
            return;
        }
        CurrentStatus = GameStatus.Win;
        
        Time.text = string.Format("{0:00} : {1:00}", CurrentTime / 60, CurrentTime % 60);
        SceneManager.instance.SetPlayed();
        LeanTween.cancelAll();
        SceneManager.instance.ShowIntertitialBanner();
        OnShowRate(false);
        if (SceneManager.instance.m_GameMode != SceneManager.GameMode.Daily)
        {
            SoundManager.instance.SoundOn(SoundManager.SoundIngame.Win);
            var best = SceneManager.instance.CurrentMapData.Score;
            if (best > CurrentTime || best == 0)
                best = CurrentTime;
            if(SceneManager.instance.CurrentMapData.Score == 0)
            {
                if(SceneManager.instance.GetHintLevel() == 4)
                {
                    SceneManager.instance.AddHint(1);
                    PopUpGift.SetActive(true);
                    SoundManager.instance.SoundOn(SoundManager.SoundIngame.Bonus);
                }
                SceneManager.instance.SetHintLevel();
                if (SceneManager.instance.AchievementController.UserAchi[0] == 0)
                {
                    SceneManager.instance.AchievementController.UserAchi[0] = 1;
                    SceneManager.instance.AchievementController.SaveAchi();
                }
            }
            if (SceneManager.instance.CurrentMapData.Score == 0 || CurrentTime < SceneManager.instance.CurrentMapData.Score)
            {
                SceneManager.instance.CurrentMapData.Score = CurrentTime;
            }
            SceneManager.instance.CurrentMapData.Star = 1;
            Win.GetComponent<WinGameController>().InitStar(best, CurrentTime);

            SceneManager.instance.SavePoint();
        }
        else
        {
            var best = SceneManager.instance.DailyController.getCurrentTime();

            if (best > CurrentTime || best == 0)
                best = CurrentTime + 1;
            var isNew = SceneManager.instance.DailyController.IsNewDaily();
            SceneManager.instance.DailyController.saveDaily(CurrentTime + 1, 1);
            SceneManager.instance.DailyController.ShowDaily(false, isNew);

            HidePlayGame();
        }
        Win.SetActive(true);
    }

    public void OnPauseGameClick()
    {
        SoundManager.instance.SoundOn(SoundManager.SoundIngame.Click);
        Pause.SetActive(true);
        //SceneManager.instance.ShowBanner(false);
        CurrentStatus = GameStatus.Pause;
    }

    public void OnSettingClick()
    {
        transform.position = new Vector3(10000, 10000);
        SoundManager.instance.SoundOn(SoundManager.SoundIngame.Click);
        CurrentStatus = GameStatus.Pause;
        SceneManager.instance.SettingController.ShowSetting(true);
    }

    public void UnPause()
    {
        LeanTween.cancelAll();
        CurrentStatus = GameStatus.Normal;
        LoopTime();
        StartToLoopPoint();
    }

    void Update() {
        if (CurrentStatus == GameStatus.Normal) {
            timeCheckHint += UnityEngine.Time.deltaTime;
            if (timeCheckHint > 10f) {
                HintBtn.enabled = true;
            }
            if (timeCheckHint > 15f)
            {
                ResetHintAnim();
            }
        }
    }

    public void OnBlockClick()
    {
        SoundManager.instance.SoundOn(SoundManager.SoundIngame.NoMatch);
    }

    public void OnShowRate(bool isShowRate)
    {
        int index = PlayerPrefs.GetInt(SceneManager.RATE_DATA);
        if (index == 0)
        {
            int level = PlayerPrefs.GetInt(SceneManager.LEVEL_RATING);
            if (level >= 3)
                level = 3;
            if (level == 3 && isShowRate)
            {
                Rate.SetActive(true);
                Rate.transform.SetAsLastSibling();
                PlayerPrefs.SetInt(SceneManager.LEVEL_RATING, 0);
            }
            else if (!isShowRate)
            {
                level++;
                PlayerPrefs.SetInt(SceneManager.LEVEL_RATING, level);
            }
        }
    }

    void ResetIndexPerLayer(int layer)
    {
        indexPerLayer.Clear();
        if (layer == 0)
        {
            for (int i = 0; i < 225; i++)
            {
                indexPerLayer.Add(i + 1);
            }
        }
        else
        {
            foreach (var item in CardPerLayer[layer - 1])
            {
                var value = item.CurrentCard.Position;
                if (!indexPerLayer.Contains(value + 1) && value + 1 < MAX_SIZE_TABLE * MAX_SIZE_TABLE)
                {
                    indexPerLayer.Add(value + 1);
                }

                if (!indexPerLayer.Contains(value - 1) && value - 1 >= 0)
                {
                    indexPerLayer.Add(value - 1);
                }

                if (!indexPerLayer.Contains(value - MAX_SIZE_TABLE) && value - MAX_SIZE_TABLE >= 0)
                {
                    indexPerLayer.Add(value - MAX_SIZE_TABLE);
                }

                if (!indexPerLayer.Contains(value + MAX_SIZE_TABLE) && value + 1 < MAX_SIZE_TABLE * MAX_SIZE_TABLE)
                {
                    indexPerLayer.Add(value + MAX_SIZE_TABLE);
                }

                if ((value + 1) % MAX_SIZE_TABLE != 0)
                {
                    if (!indexPerLayer.Contains(value + MAX_SIZE_TABLE + 1) && value + 1 < MAX_SIZE_TABLE * MAX_SIZE_TABLE)
                    {
                        indexPerLayer.Add(value + MAX_SIZE_TABLE + 1);
                    }

                    if (!indexPerLayer.Contains(value - (MAX_SIZE_TABLE - 1)) && value - (MAX_SIZE_TABLE - 1) >= 0)
                    {
                        indexPerLayer.Add(value - (MAX_SIZE_TABLE - 1));
                    }
                }

                if (value % MAX_SIZE_TABLE != 0)
                {
                    if (!indexPerLayer.Contains(value + MAX_SIZE_TABLE - 1) && value + 1 < MAX_SIZE_TABLE * MAX_SIZE_TABLE)
                    {
                        indexPerLayer.Add(value + MAX_SIZE_TABLE - 1);
                    }

                    if (!indexPerLayer.Contains(value - (MAX_SIZE_TABLE + 1)) && value - (MAX_SIZE_TABLE + 1) >= 0)
                    {
                        indexPerLayer.Add(value - (MAX_SIZE_TABLE + 1));
                    }
                }
            }
        }
    }

    private bool isSelected = false;
    private CardTemplateController preSelectedCard;
    public void OnSelectCardClick(CardTemplateController card) {
        ResetHintAnim();
        OnHidePause();
        if (!canClick)
            return;

        if(isAnimHightlight)
        {
            LeanTween.cancel(hintTween);
            OffHint();
        }

        if (!isSelected)
        {
            isSelected = true;
            Selected.SetActive(true);
            Selected.transform.position = card.transform.position;
            preSelectedCard = card;
            SoundManager.instance.SoundOn(SoundManager.SoundIngame.FlipCard);
        }
        else
        {
            if (preSelectedCard != null)
            {
                SoundManager.instance.SoundOn(SoundManager.SoundIngame.FlipCard);
                if (preSelectedCard.CurrentCard.CardIndex == card.CurrentCard.CardIndex)
                {
                    FirstListCard.Add(preSelectedCard);
                    SecondListCard.Add(card);
                    var centerX = (preSelectedCard.transform.position.x + card.transform.position.x) / 2;
                    var centerY = (preSelectedCard.transform.position.y + card.transform.position.y) / 2;
                    float buffer = 300f;
                    Vector3 first1 = new Vector3(centerX - buffer, centerY);
                    Vector3 first2 = new Vector3(centerX + buffer, centerY);
                    float buffer1 = 0f, buffer2 = 0f;

                    bool isLeft = false;

                    if (preSelectedCard.transform.position.x <= card.transform.position.x)
                    {
                        first1 = new Vector3(centerX - buffer, centerY);
                        first2 = new Vector3(centerX + buffer, centerY);
                        buffer1 = -(Dis1.transform.position.x - Dis2.transform.position.x) / 2;
                        buffer2 = (Dis1.transform.position.x - Dis2.transform.position.x) / 2;
                        isLeft = true;
                    }
                    else
                    {
                        first1 = new Vector3(centerX + buffer, centerY);
                        first2 = new Vector3(centerX - buffer, centerY);
                        buffer1 = (Dis1.transform.position.x - Dis2.transform.position.x) / 2;
                        buffer2 = -(Dis1.transform.position.x - Dis2.transform.position.x) / 2;
                    }

                    if (isLeft)
                    {
                        DisappearCard(preSelectedCard, true, new Vector3(centerX + buffer1 - 3f, centerY), first1);
                        DisappearCard(card, false, new Vector3(centerX + buffer2 + 3f, centerY), first2);
                    }
                    else
                    {
                        DisappearCard(card, false, new Vector3(centerX + buffer2 - 3f, centerY), first2);
                        DisappearCard(preSelectedCard, true, new Vector3(centerX + buffer1 + 3f, centerY), first1);
                    }
                    CurrentPoint += PointPerMatch;
                    CheckBlockPerCard();

                    preSelectedCard = null;
                    isSelected = false;
                    Selected.SetActive(false);
                }
                else
                {
                    SoundManager.instance.SoundOn(SoundManager.SoundIngame.FlipCard);
                    preSelectedCard = card;
                    Selected.SetActive(true);
                    Selected.transform.position = card.transform.position;
                    isSelected = true;
                }
            }
        }
    }

    public void OnBgClick()
    {
        if (!canClick)
            return;

        preSelectedCard = null;
        isSelected = false;
        Selected.SetActive(false);
    }

    public void OnUndoClick()
    {
        SoundManager.instance.SoundOn(SoundManager.SoundIngame.Click);
        if (!canClick)
            return;

        if (FirstListCard.Count > 0 && SecondListCard.Count > 0)
        {
            var lastIndex = FirstListCard.Count - 1;
            AppearCard(FirstListCard[lastIndex]);
            AppearCard(SecondListCard[lastIndex]);
            if(CurrentPoint > 0)
                CurrentPoint -= PointPerMatch;
            FirstListCard.RemoveAt(lastIndex);
            SecondListCard.RemoveAt(lastIndex);
        }
        CheckUndo();
    }

    private int tweenId1, tweenId2, tweenId3;
    bool stopTween = false;
    public LeanTweenType typeTween = LeanTweenType.easeInOutQuad;
    private void DisappearCard(CardTemplateController card, bool isFirstCard, Vector3 center, Vector3 targetFirst)
    {
        canClick = false;
        card.IgnoreClick(true);
        CardPerLayerIndex[card.CurrentCard.CardLayer].Remove(card.CurrentCard.Position);
        for (int i = CardPerLayer[card.CurrentCard.CardLayer].Count - 1; i >= 0; i--)
        {
            if (CardPerLayer[card.CurrentCard.CardLayer][i].CurrentCard.Position == card.CurrentCard.Position)
            {
                CardPerLayer[card.CurrentCard.CardLayer].RemoveAt(i);
                break;
            }
        }
        foreach(var item in ListCardCanClick)
        {
            if(item.CurrentCard.CardLayer == card.CurrentCard.CardLayer 
                && item.CurrentCard.Position == card.CurrentCard.Position)
            {
                ListCardCanClick.Remove(item);
                break;
            }
        }
        canClick = true;
        LeanTween.cancel(tweenId1);
        LeanTween.cancel(tweenId3);
        stopTween = true;
        card.transform.SetParent(AnimTemp.transform);
        LeanTween.move(card.gameObject, targetFirst, 0.2f).setOnComplete(s => {
            LeanTween.move(card.gameObject, center, 0.1f).setEase(typeTween).setOnComplete(s3 =>
            {

                if (isFirstCard)
                    SoundManager.instance.SoundOn(SoundManager.SoundIngame.MatchCard);
                LeanTween.delayedCall(0.2f, s5 =>
                {
                    var target = isFirstCard ? Anim1.transform.position : Anim2.transform.position;
                    LeanTween.scale(card.gameObject, Vector3.one, 0.2f);
                    LeanTween.move(card.gameObject, target, 0.2f).setOnComplete(f =>
                    {
                        card.transform.SetParent(AnimHolder.transform);
                        card.transform.localPosition = isFirstCard ? Pos1.transform.localPosition : Pos2.transform.localPosition;
                        tweenId3 = LeanTween.delayedCall(0.5f, () =>
                        {
                            if (!stopTween)
                            {
                                tweenId1 = LeanTween.move(AnimHolder.gameObject, AnimStart.transform.position, 0.2f).setOnComplete(s2 =>
                                {

                                }).id;
                            }
                        }).id;
                    });
                });
                
            });
        });

        LeanTween.cancel(tweenId2);
        tweenId2 = LeanTween.move(AnimHolder.gameObject, AnimTarget.transform.position, 0.18f).setOnComplete(s => {
            // CheckBlockPerCard();
            stopTween = false;
        }).id;
        
    }

    private void AppearCard(CardTemplateController card)
    {
        canClick = false;
        CardPerLayerIndex[card.CurrentCard.CardLayer].Add(card.CurrentCard.Position);
        CardPerLayer[card.CurrentCard.CardLayer].Add(card);
        
        CheckBlockPerCard();
        card.transform.SetParent(TemplateGroup[card.CurrentCard.CardLayer].transform);
        
        int indexCard = card.CurrentCard.Position;
        int posX = indexCard % MAX_SIZE_TABLE;
        int posY = indexCard / MAX_SIZE_TABLE;
        var target = new Vector2(StartPosX + posX * WidthPerCard, StartPosY - posY * HeightPerCard);
        
        LeanTween.moveLocal(card.gameObject, target, 0.2f).setOnComplete(s =>
        {
            canClick = true;
        });
    }

    bool isAnimHightlight = false;
    int hintTween = 0;
    public void OnHightLightClick()
    {
        ResetHintAnim();
        SoundManager.instance.SoundOn(SoundManager.SoundIngame.PlayGameBtn);
        if (SceneManager.instance.GetHint() == 0)
        {
            AdHint.SetActive(true);
            return;
        }

        if (isAnimHightlight)
            return;

        SceneManager.instance.AddHint(-1);
        HintNum.text = SceneManager.instance.GetHint().ToString();
        if (HintNum.text == "0") {
            HintNum.text = "+";
        }
        CurrentPoint -= HintPoint;
        if (CurrentPoint < 0)
            CurrentPoint = 0;

        OnBgClick();

        List<int> tempList = new List<int>();
        CardTemplateController first = null;
        CardTemplateController second = null;

        var index = 0;
        if (curShowHint >= ListCardCanClick.Count - 1) {
            curShowHint = -1;
        }
        foreach (var item in ListCardCanClick)
        {
            if (!tempList.Contains(item.CurrentCard.CardIndex))
            {
                tempList.Add(item.CurrentCard.CardIndex);
            }
            else {
                first = item;
                if (index > curShowHint) {
                    curShowHint = index;
                    break;
                }
            }

            index++;
        }

        if (first != null && first.CardIndex != null)
        {
            foreach (var item in ListCardCanClick)
            {
                if (item.CurrentCard.CardIndex == first.CurrentCard.CardIndex)
                {
                    second = item;

                    isAnimHightlight = true;
                
                    HightLight1.SetActive(true);
                    HightLight1.transform.position = first.transform.position;
                    HightLight2.SetActive(true);
                    HightLight2.transform.position = second.transform.position;
                    HightLight1.GetComponent<Image>().sprite = first.CardIndex.sprite;
                    HightLight2.GetComponent<Image>().sprite = second.CardIndex.sprite;
                    curShowHint++;
                    hintTween = LeanTween.delayedCall(4f, s =>
                    {
                        OffHint();
                    }).id;

                    break;
                }
            }
        }
        if (SceneManager.instance.AchievementController.UserAchi[1] == 0)
        {
            SceneManager.instance.AchievementController.UserAchi[1] = 1;
            SceneManager.instance.AchievementController.SaveAchi();
        }
    }

    void OffHint()
    {
        isAnimHightlight = false;

        HightLight1.SetActive(false);
        HightLight2.SetActive(false);
        ResetHintAnim();
    }

    bool blockDup = false;
    public void OnShuffleClick()
    {
        SoundManager.instance.SoundOn(SoundManager.SoundIngame.PlayGameBtn);
        if (!canClick)
            return;
        ShuffleBtn.interactable = false;

        canClick = false;
        FirstListCard.Clear();
        SecondListCard.Clear();
       
        if (CurrentPoint < 0)
            CurrentPoint = 0;

        tweenId = LeanTween.scale(TableCard, Vector3.zero, 0.5f).setOnComplete(s =>
        {
            StartCoroutine("reShuffle");
        }).id;

        if (SceneManager.instance.AchievementController.UserAchi[2] == 0)
        {
            SceneManager.instance.AchievementController.UserAchi[2] = 1;
            SceneManager.instance.AchievementController.SaveAchi();
        }
    }

    private void OnShuffleLose()
    {
        canClick = false;
        FirstListCard.Clear();
        SecondListCard.Clear();

        if (CurrentPoint < 0)
            CurrentPoint = 0;
        ShuffleBtn.interactable = false;
        tweenId = LeanTween.scale(TableCard, Vector3.zero, 0.5f).setOnComplete(s =>
        {
            StartCoroutine("reShuffle");
        }).id;

        if (SceneManager.instance.AchievementController.UserAchi[2] == 0)
        {
            SceneManager.instance.AchievementController.UserAchi[2] = 1;
            SceneManager.instance.AchievementController.SaveAchi();
        }
    }

    private bool OnCheckLose()
    {
        List<int> tempList = new List<int>();
        foreach (var item in ListCardCanClick)
        {
            if (!tempList.Contains(item.CurrentCard.CardIndex))
            {
                tempList.Add(item.CurrentCard.CardIndex);
            }
            else
            {
                return false;
            }
        }

        return true;
    }

    public void RandomRemainCard()
    {
        FirstListCard.Clear();
        SecondListCard.Clear();
        List<CardTemplateController> newList = new List<CardTemplateController>();
        foreach (var layer in CardPerLayer)
        {
            foreach (var card in layer)
            {
                newList.Add(card);
            }
        }
        CardPerLayer.Clear();
        AllCardTemplateHandler.Clear();

        ResetIndexPerLayer(0);
        int curCardOnThisLayer = newList.Count;
        List<int> positionCard = new List<int>();
        bool endWhile = false;

        while (positionCard.Count < curCardOnThisLayer && !endWhile)
        {
            for (int j = 0; j < curCardOnThisLayer; j++)
            {
                int tempIndex = UnityEngine.Random.Range(0, indexPerLayer.Count);
                if (tempIndex < indexPerLayer.Count)
                {
                    positionCard.Add(indexPerLayer[tempIndex]);
                    FilterListIndex(indexPerLayer[tempIndex]);
                }
                else
                {
                    endWhile = true;
                }
            }
        }

        List<CardTemplateController> newListCard = new List<CardTemplateController>();

        for (int j = 0; j < positionCard.Count; j++)
        {
            var card = newList[j];
            card.CurrentCard.CardLayer = 0;
            int indexCard = positionCard[j];
            card.CurrentCard.Position = indexCard;
            int posX = indexCard % MAX_SIZE_TABLE;
            int posY = indexCard / MAX_SIZE_TABLE;
            card.transform.SetParent(TemplateGroup[0].transform);
            card.transform.localScale = Vector3.one;
            card.transform.localPosition = new Vector2(StartPosX + posX * WidthPerCard, StartPosY - posY * HeightPerCard);
            newListCard.Add(card);
            card.IgnoreClick(false);
            AllCardTemplateHandler.Add(card);
        }
        CardPerLayer.Add(newListCard);
        OnCheckBlockAllCard();
        foreach (var item in TemplateGroup)
        {
            item.transform.localScale = Vector3.one;
        }
    }

    public void OnBackClick() {
        SoundManager.instance.SoundOn(SoundManager.SoundIngame.PlayGameBtn);
        HidePlayGame();
        if (SceneManager.instance.m_GameMode == SceneManager.GameMode.Daily) {
            SceneManager.instance.DailyController.ShowDaily();
        }
        else
        {
            //if (SceneManager.instance.LastLevel > 0)
            //{
            //    SceneManager.instance.LastLevel--;
            //}
            SceneManager.instance.LevelController.ShowLevel();
        }
    }

    private void CheckUndo()
    {
//        UndoBtn.interactable = FirstListCard.Count > 0;
    }

    public void CheckShuffleBtn()
    {
        ShuffleBtn.interactable = SceneManager.instance.CheckShuffle();
    }

    public void OnHideAdHint() {
        SoundManager.instance.SoundOn(SoundManager.SoundIngame.Click);
        AdHint.SetActive(false);
    }

    public void OnWatchVideoClick() {
        SoundManager.instance.SoundOn(SoundManager.SoundIngame.Click);
        OnHideAdHint();
        SceneManager.instance.ShowVideoAds();
    }

    public void onButtonPopUpOkClick()
    {
        SoundManager.instance.SoundOn(SoundManager.SoundIngame.Click);
        PopUpGift.SetActive(false);
    }

    private bool isClickPause, clickingPause;
    public void OnPauseClick() {
        SoundManager.instance.SoundOn(SoundManager.SoundIngame.PlayGameBtn);
        if (clickingPause) {
            return;
        }
        clickingPause = true;
        if (!isClickPause) {
            Replay.transform.localScale = Vector2.one;
            Quit.transform.localScale = Vector2.one;
            LeanTween.move(Replay, RepTar.transform.position, 0.2f).setOnComplete(s => {
                clickingPause = false;
                isClickPause = !isClickPause;  
            });
            LeanTween.move(Quit, QuitTar.transform.position, 0.2f);
        }
        else {
            LeanTween.move(Replay, PauseBtn.transform.position, 0.2f).setOnComplete(s => {
                Replay.transform.localScale = Vector2.zero;
                Quit.transform.localScale = Vector2.zero;
                clickingPause = false;
                isClickPause = !isClickPause;
            });
            LeanTween.move(Quit, PauseBtn.transform.position, 0.2f);
        }
    }

    public void OnHidePause()
    {
        if (isClickPause)
        {
            LeanTween.move(Replay, PauseBtn.transform.position, 0.2f).setOnComplete(s => {
                Replay.transform.localScale = Vector2.zero;
                Quit.transform.localScale = Vector2.zero;
                clickingPause = false;
                isClickPause = !isClickPause;
            });
            LeanTween.move(Quit, PauseBtn.transform.position, 0.2f);
        }
    }

    public void OnSoundClick() {
        SoundManager.instance.SoundOn(SoundManager.SoundIngame.PlayGameBtn);
        var temp = SceneManager.instance.OnOffEffect();
        SoundManager.instance.SetSound(temp ? 1 : 0);
        CheckSoundIcon();
    }

    private void CheckSoundIcon() {
        if (SceneManager.instance.CheckSoundEffect()) {
            SoundIcon.color = new Color32(255, 255, 255, 255);
        }
        else {
            SoundIcon.color = new Color32(137, 137, 137, 137);
        }
    }
    
    public void OnReplayClick() {
        SoundManager.instance.SoundOn(SoundManager.SoundIngame.Click);
        SceneManager.instance.PlayGameController.HidePlayGame();
        SceneManager.instance.PlayGameController.InitTable();
        SceneManager.instance.LoadingController.ShowLoading(LoadingController.SwitchScene.PlayGame);
    }

    public void showDaily(DateTime date)
    {
        DailyText.text = "Playing Daily Challenge " + date.Month + "/" + date.Day + "/" + date.Year;
        CurrentDaily = date.Day + "/" + date.Month + "/" + date.Year;
        DailyNotif.gameObject.SetActive(true);
        //if (SceneManager.instance.m_GameMode == SceneManager.GameMode.Daily)
        //{
        //    LevelText.gameObject.SetActive(true);
        //    var month = date.Month.ToString();
        //    if (month.Length == 1)
        //    {
        //        month = "0" + month;
        //    }

        //    var day = date.Day.ToString();
        //    if (day.Length == 1)
        //    {
        //        day = "0" + day;
        //    }
        //}
    }

    public void delayHideDailyNotif()
    {
        LeanTween.delayedCall(TimeDelayHideDaily, () =>
        {
            DailyNotif.gameObject.SetActive(false);
        });
    }

    public Sprite Up, Down;
    public GameObject BtnList, MoveBtn;
    public Image MoveBtnImg;
    public bool IsHideBtn = false;
    private float DefautBtnListY, DefaultMoveY, NextBtnY, NextMoveY;
    int tweenBtn = -1, tweenMove = -1;
    public void OnHideBtnListClick()
    {
        LeanTween.cancel(tweenBtn);
        LeanTween.cancel(tweenMove);
        IsHideBtn = !IsHideBtn;
        OnHidePause();
        if (IsHideBtn)
        {
            LeanTween.moveLocalY(BtnList, NextBtnY, 0.25f);
            LeanTween.moveLocalY(MoveBtn, NextMoveY, 0.25f);
            MoveBtnImg.sprite = Up;
        }
        else
        {
            LeanTween.moveLocalY(BtnList, DefautBtnListY, 0.25f);
            LeanTween.moveLocalY(MoveBtn, DefaultMoveY, 0.25f);
            MoveBtnImg.sprite = Down;
        }
    }
}
