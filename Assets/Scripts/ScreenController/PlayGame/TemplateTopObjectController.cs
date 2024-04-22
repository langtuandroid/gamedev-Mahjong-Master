using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TemplateTopObjectController : MonoBehaviour
{
    public Sprite CheckBoxOn, CheckBoxOff;
    public Image GoldCard,CheckBox, Checker;

    public void HideGoldCard(bool isHide)
    {
        if (isHide)
        {
            GoldCard.color = new Color(1, 1, 1, 0);
        }
        else
        {
            GoldCard.color = new Color(1, 1, 1, 1);
        }
    }

    public void CheckCardTop(bool isComplete)
    {
        if(!isComplete)
        {
            CheckBox.sprite = CheckBoxOff;
            Checker.color = new Color(1, 1, 1, 0);
        }
        else
        {
            CheckBox.sprite = CheckBoxOn;
            Checker.color = new Color(1, 1, 1, 1);
        }
    }
}
