using UnityEngine;
using System.Collections.Generic;

public class CardBase
{

    public int CardIndex { set; get; }
    
    public int CardLayer { set; get; }
    
    //For card which can click or not
    public bool CanClick { set; get; }

    public int Position { set; get; }

    public List<int> PositionBlock { set; get; }

    public CardBase()
    {
        PositionBlock = new List<int>();
    }
}
