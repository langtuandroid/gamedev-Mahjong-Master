using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeSquareAreaFitter : MonoBehaviour
{
    public bool dontSafeBottom;
    RectTransform fittedRectTransform;
    Rect safeRectComponent;
    Vector2 minAnchorVector;
    Vector2 maxAnchorVector;

    private void Awake()
    {
        Application.targetFrameRate = 60;
        fittedRectTransform = GetComponent<RectTransform>();
        safeRectComponent = Screen.safeArea;
        minAnchorVector = safeRectComponent.position;
        maxAnchorVector = minAnchorVector + safeRectComponent.size;
        
        minAnchorVector.x /= Screen.width;
        minAnchorVector.y = dontSafeBottom ? minAnchorVector.y = 0 : minAnchorVector.y /= Screen.height;
        maxAnchorVector.x /= Screen.width;
        maxAnchorVector.y /= Screen.height;
        
        fittedRectTransform.anchorMin = minAnchorVector;
        fittedRectTransform.anchorMax = maxAnchorVector;

    }
}
