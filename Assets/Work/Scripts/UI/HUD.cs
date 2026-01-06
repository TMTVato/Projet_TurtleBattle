using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Cinemachine.DocumentationSortingAttribute;

public class HUD : MonoBehaviour
{
    //Enum pour définir le type d'information à afficher HUD
    public enum Infotype
    {
        Exp,
        Level,
        Kill,
        Time,
        Health,
        TurtleHealth

    }

    public Infotype type;

    Text myText;
    Slider mySlider; 
    private void Awake()
    {
        myText = GetComponent<Text>();
        mySlider = GetComponent<Slider>();
    }

    private void LateUpdate()
    {
        switch(type) //Switch case pour changer l'HUD  en fonction du type d'info
        {
            case Infotype.Exp:
                float curExp = GameManager.instance.exp;
                float maxExp = GameManager.instance.nextExp[Mathf.Min(GameManager.instance.level, GameManager.instance.nextExp.Length - 1)]; 
                mySlider.value = curExp / maxExp; //EXP correspond au ratio entre l'exp cur et l'exp max pour le niveau suivant et l'affiche dans le Slider
                break;

            case Infotype.Level:
                myText.text = string.Format("Lv.{0:F0}", GameManager.instance.level); //Format le texte pour afficher le niveau
                break;

            case Infotype.Kill:
                myText.text = string.Format("{0:F0}", GameManager.instance.kill); //Format le texte pour afficher le nombre de kill
                break;

            case Infotype.Time:
                float remainTime = GameManager.instance.maxGameTime - GameManager.instance.gameTime;
                //Récupère le temps restant en minutes et secondes
                int minutes = Mathf.FloorToInt(remainTime / 60f);
                int seconds = Mathf.FloorToInt(remainTime % 60f);
                myText.text = string.Format("{0:D2}:{1:D2}", minutes, seconds); //Format le texte pour afficher le temps restant en minutes et secondes

                break;

            case Infotype.Health:
                float curHP = GameManager.instance.HP;
                float maxHP = GameManager.instance.maxHP;
                mySlider.value = curHP / maxHP; //Slider de la santé du joueur
                break;

            case Infotype.TurtleHealth:
                float curTurtleHP = GameManager.instance.turtleHP;
                float maxTurtleHP = GameManager.instance.turtleMaxHP;
                mySlider.value = curTurtleHP / maxTurtleHP; //Slider de la santé de la tortue
                break;

            default:

                break;
        }
    }
}
