using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;

public class TurtleLogic : MonoBehaviour
{
    public Transform head;
    public Transform legFL, legFR, legBL, legBR;
    public float legAnimSpeed = 3f;
    public float legAnimAmount = 20f;

    void Update()
    {
        // Animation de la tête
        if (head != null && GameManager.instance.player != null)
        {
            Vector3 dir = GameManager.instance.player.transform.position - head.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            head.rotation = Quaternion.Euler(0, 0, angle);
        }

        // Animation des pattes
        float anim = Mathf.Sin(Time.time * legAnimSpeed) * legAnimAmount;

        if (legFL != null) legFL.localRotation = Quaternion.Euler(0, 0, anim);
        if (legBR != null) legBR.localRotation = Quaternion.Euler(0, 0, anim);
        if (legFR != null) legFR.localRotation = Quaternion.Euler(0, 0, -anim);
        if (legBL != null) legBL.localRotation = Quaternion.Euler(0, 0, -anim);
    }


    void OnCollisionStay2D(Collision2D collision)
    {
        if (!GameManager.instance.isLive) return;
        // Ignore les collisions avec le joueur
        if (collision.gameObject.CompareTag("Player")) return;

        // Inflige des dégâts à la tortue via le GameManager
        GameManager.instance.turtleHP -= Time.deltaTime * 10;

        // Si la tortue meurt
        if (GameManager.instance.turtleHP <= 0)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
            GameManager.instance.GameOver();
        }
    }
}
