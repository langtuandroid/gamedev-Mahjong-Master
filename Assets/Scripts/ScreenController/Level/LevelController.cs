using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class LevelController : MonoBehaviour {
    private const int LevelPerPage = 20;
    public GameObject PosScroll;
    public GameObject TemplatePos;
    public List<Image> TemplatePosList = new List<Image>();
    public Sprite CurrentPos, NormalPos;

    private CanvasGroup Level;
    public GameObject TemplateLevel;
    public List<RectTransform> Pages = new List<RectTransform>();
    public List<CanvasGroup> CanvasPage = new List<CanvasGroup>();
    public ScrollRect ScrollRect;
    public RectTransform ScrollView;

    private int currentPage = 0;
    private int currentPoolPage = 1;
    private int maxPage = 0;
    public List<List<TemplateLevelController>> PoolListController = new List<List<TemplateLevelController>>();
    public Text StarText, MapName;
    public bool IsShowing;

    public GameObject NotiDaily;
    public GameObject NotiTrophy;

    void Awake()
    {
        Level = GetComponent<CanvasGroup>();
    }

    void Start()
    {
        int temp = 0;
        if (Config.instance.MaxLevel % LevelPerPage != 0)
            temp = 1;
        maxPage = Config.instance.LevelPerMap / LevelPerPage + temp;
        InstanceObjectPool();
        InitLevel();
        InitScrollPos();
    }

    private void InitScrollPos()
    {
        for(int i = 0; i < maxPage;i++)
        {
            GameObject template = Instantiate(TemplatePos) as GameObject;
            template.transform.SetParent(PosScroll.transform);
            template.transform.localScale = Vector3.one;
            template.SetActive(true);
            TemplatePosList.Add(template.GetComponentInChildren<Image>());
        }
        if (currentPage != 0 && currentPage - 1 < TemplatePosList.Count) {
            TemplatePosList[currentPage - 1].sprite = CurrentPos;
        }
    }

    private void InitLevel()
    {
        int lastLevel = SceneManager.instance.PreLastLevel + 1;
        lastLevel = lastLevel % 60;

        int temp = 1;
        if (lastLevel % LevelPerPage == 0)
            temp = 0;
        if (currentPage != 0 && TemplatePosList.Count > currentPage - 1)
        {
            TemplatePosList[currentPage - 1].sprite = NormalPos;
        }

        currentPage = lastLevel / LevelPerPage + temp;
        if (currentPage > TemplatePosList.Count && TemplatePosList.Count > 0) 
        {
            currentPage = TemplatePosList.Count;
        }
        if (currentPage > 0 && TemplatePosList.Count > currentPage - 1)
        {
            TemplatePosList[currentPage - 1].sprite = CurrentPos;
        }
        SetDataPerPage(1, currentPage);

        if (currentPage + 1 <= maxPage)
        {
            SetDataPerPage(2, currentPage + 1);
        }
        else
        {
            CanvasPage[2].alpha = 0;
        }

        if (currentPage == 1)
        {
            CanvasPage[0].alpha = 0;
        }
        else
        {
            SetDataPerPage(0, currentPage - 1);
        }

        Pages[0].localPosition = new Vector2(-800f, 0);
        Pages[1].localPosition = Vector2.zero;
        Pages[2].localPosition = new Vector2(800f, 0);

    }

    public void SetDataPerPage(int poolPage, int curPage)
    {
        CanvasPage[poolPage].alpha = 1;

        int tempIndex = Config.instance.MaxLevel / LevelPerPage + 1;
        int maxInit = 0;
        if (curPage == tempIndex)
            maxInit = Config.instance.MaxLevel % LevelPerPage;
        else maxInit = LevelPerPage;
        for (int i = 0; i < LevelPerPage; i++)
        {
            var index = (curPage - 1) * LevelPerPage + i + SceneManager.instance.MapIndex * 60;
            if (index < 0)
                index = 0;
            if (i < maxInit && index < SceneManager.instance.AllMapData.Count)
            {
                PoolListController[poolPage][i].transform.localScale = Vector3.one;
                PoolListController[poolPage][i].InitData(SceneManager.instance.AllMapData[index]);
            }
            else
            {
                PoolListController[poolPage][i].transform.localScale = Vector3.zero;
            }
        }
    }

    private void InstanceObjectPool()
    {
        for (int i = 0; i < 3; i++)
        {
            List<TemplateLevelController> tempList = new List<TemplateLevelController>();
            for (int j = 0; j < LevelPerPage; j++)
            {
                var obj = Instantiate(TemplateLevel) as GameObject;
                obj.transform.SetParent(Pages[i].transform);
                obj.transform.localPosition = Vector3.zero;
                obj.transform.localScale = Vector3.one;
                obj.name = "i";
                obj.SetActive(true);
                var controller = obj.GetComponent<TemplateLevelController>();
                tempList.Add(controller);
            }
            PoolListController.Add(tempList);
        }
    }

    public void ShowLevel()
    {
        SceneManager.instance.AchievementController.CheckUserData();
        CheckSoundIcon();
        IsShowing = true;
        Level.alpha = 1;
        Level.blocksRaycasts = true;
        this.gameObject.transform.localPosition = Vector2.zero;
        ResetUi();
        if (SceneManager.instance.DailyController.isDoneDailyToday())
            NotiDaily.SetActive(false);
        else
            NotiDaily.SetActive(true);

        NotiTrophy.SetActive(SceneManager.instance.AchievementController.CheckGetAllBtn());
    }

    void ResetUi()
    {
        InitLevel();
        StarText.text = SceneManager.instance.TotalStar().ToString();
        if (SceneManager.instance.MapIndex < Config.instance.MapName.Count)
        { 
            MapName.text = Config.instance.MapName[SceneManager.instance.MapIndex];
        }
        else
        {
            MapName.text = "NEW WORLD";
        }
    }
    public void HideLevel()
    {
        IsShowing = false;
        Level.alpha = 0;
        Level.blocksRaycasts = false;
        this.gameObject.transform.localPosition = new Vector2(10000, 10000);
        SceneManager.instance.PreviousScreen = SceneManager.ScreenEnum.Level;
    }

    public void OnBackClick()
    {
        HideLevel();
        SceneManager.instance.MapStageController.ShowMap();
        SoundManager.instance.SoundOn(SoundManager.SoundIngame.Click);
    }

    public void OnEndDrag()
    {
        if(ScrollView.localPosition.x < -50f && currentPage < maxPage)
        {
            ScrollRect.enabled = false;
            LeanTween.moveLocalX(ScrollView.gameObject, -800f, 0.2f)
                .setOnComplete(OnMoveLeftComplete);
        }
        else if(ScrollView.localPosition.x > 50f && currentPage > 1)
        {
            ScrollRect.enabled = false;
            LeanTween.moveLocalX(ScrollView.gameObject, 800f, 0.2f)
                                .setOnComplete(OnMoveRightComplete);
        }
    }

    private void OnMoveRightComplete()
    {
        ScrollView.localPosition = new Vector2(0, 0);
        ScrollRect.enabled = true;
        MoveScroll(false);
    }

    private void OnMoveLeftComplete()
    {
        ScrollView.localPosition = new Vector2(0, 0);
        ScrollRect.enabled = true;
        MoveScroll(true);
    }

    private void MoveScroll(bool isRight)
    {
        if (isRight)
        {
            TemplatePosList[currentPage - 1].sprite = NormalPos;
            currentPage++;
            TemplatePosList[currentPage - 1].sprite = CurrentPos;

            for (int i = 0; i < Pages.Count;i++)
            {
                Pages[i].localPosition = new Vector2(Pages[i].localPosition.x - 800f, 0);
                if (Pages[i].localPosition.x < -801f)
                {
                    Pages[i].localPosition = new Vector2(800f, 0);
                    if (currentPage + 1 <= maxPage)
                    {
                        SetDataPerPage(i, currentPage + 1);
                    }
                    else
                    {
                        CanvasPage[i].alpha = 0;
                    }
                }
            }

            currentPoolPage++;
            if(currentPoolPage > 2)
            {
                currentPoolPage = 0;
            }
        }
        else
        {
            TemplatePosList[currentPage - 1].sprite = NormalPos;
            currentPage--;
            TemplatePosList[currentPage - 1].sprite = CurrentPos;

            for (int i = 0; i < Pages.Count; i++)
            {
                Pages[i].localPosition = new Vector2(Pages[i].localPosition.x + 800f, 0);
                if (Pages[i].localPosition.x > 801f)
                {
                    Pages[i].localPosition = new Vector2(-800f, 0);
                    if (currentPage - 1 >= 1)
                    {
                        SetDataPerPage(i, currentPage - 1);
                    }
                    else
                    {
                        CanvasPage[i].alpha = 0;
                    }
                }
            }

            currentPoolPage--;
            if (currentPoolPage < 0)
            {
                currentPoolPage = 2;
            }
        }
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
