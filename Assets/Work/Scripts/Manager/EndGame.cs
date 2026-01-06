using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : MonoBehaviour
{
    public GameObject[] titles;

   public void Lose()
    {
        titles[0].SetActive(true); // Active le titre de défaite
    }

    public void Win()
    {
        titles[1].SetActive(true); // Active le titre de victoire
    }
}
