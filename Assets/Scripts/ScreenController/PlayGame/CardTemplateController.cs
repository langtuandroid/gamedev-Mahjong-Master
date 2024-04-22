using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class CardTemplateController : MonoBehaviour {
    public PlayGameController MainController;
    public CardBase CurrentCard;
    public Image CardIndex, Block;
    public Vector2 DefaultPos;

    public void LockClick(bool lockClick)
    {
        CurrentCard.CanClick = !lockClick;

        if (lockClick)
        {
            if (MainController.IsHightLight)
            {
                Block.color = new Color(0, 0, 0, 167 / 225f);
            }
            Block.raycastTarget = true;
        }
        else
        {
            if (MainController.IsHightLight)
            {
                Block.color = new Color(1, 1, 1, 0);
            }

            Block.raycastTarget = false;
        }
    }

    public void HightLight()
    {
        if (CurrentCard.CanClick)
        {
            Block.color = new Color(1, 1, 1, 0);
        }
        else
        {
            Block.color = new Color(0, 0, 0, 167 / 225f);
        }
    }

    public void ResetImageCardIndex()
    {
        CardIndex.sprite = MainController.MainSpriteCard[CurrentCard.CardIndex];
    }

    public void IgnoreClick(bool ignore)
    {
            CardIndex.raycastTarget = !ignore;
    }
}
