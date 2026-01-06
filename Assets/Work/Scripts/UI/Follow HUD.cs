using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowHUD : MonoBehaviour
{
    public Transform target; // La cible à suivre (joueur ou tortue)
    public float offsetY = 50f; // Offset vertical en pixels
    RectTransform rectTransform;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void FixedUpdate()
    {
        if (target == null) return;
        // Convertir la position du monde en position écran = permet de suivre un objet 3D avec une UI 2D
        Vector3 screenPos = Camera.main.WorldToScreenPoint(target.position);
        screenPos.y += offsetY;
        rectTransform.position = screenPos; // Met à jour la position de l'élément UI
    }
}