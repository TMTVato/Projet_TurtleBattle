using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Cinemachine.DocumentationSortingAttribute;

public class HUD : MonoBehaviour
{
    public enum Infotype
    {
        Exp,
        Level,
        Kill,
        Time,
        Health

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
        switch(type)
        {
            case Infotype.Exp:
                float curExp = GameManager.instance.exp;
                float maxExp = GameManager.instance.nextExp[Mathf.Min(GameManager.instance.level, GameManager.instance.nextExp.Length - 1)];
                mySlider.value = curExp / maxExp;
                break;

            case Infotype.Level:
                myText.text = string.Format("Lv.{0:F0}", GameManager.instance.level); //Format text to show level 
                break;

            case Infotype.Kill:
                myText.text = string.Format("{0:F0}", GameManager.instance.kill); //Format text to show kill count 
                break;

            case Infotype.Time:
                float remainTime = GameManager.instance.maxGameTime - GameManager.instance.gameTime;
                int minutes = Mathf.FloorToInt(remainTime / 60f);
                int seconds = Mathf.FloorToInt(remainTime % 60f);
                myText.text = string.Format("{0:D2}:{1:D2}", minutes, seconds); //Format text to show remaining time

                break;

            case Infotype.Health:
                float curHP = GameManager.instance.HP;
                float maxHP = GameManager.instance.maxHP;
                mySlider.value = curHP / maxHP;
                break;

            default:

                break;
        }
    }
}
