using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowHUD : MonoBehaviour
{
    RectTransform rectTransform;
    // Start is called before the first frame update
    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rectTransform.position = Camera.main.WorldToScreenPoint(GameManager.instance.player.transform.position);
    }
}
