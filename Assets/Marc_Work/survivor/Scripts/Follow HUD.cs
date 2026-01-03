using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowHUD : MonoBehaviour
{
    public Transform target; // La cible à suivre (joueur ou tortue)
    RectTransform rectTransform;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void FixedUpdate()
    {
        if (target == null) return;
        rectTransform.position = Camera.main.WorldToScreenPoint(target.position);
    }
}
