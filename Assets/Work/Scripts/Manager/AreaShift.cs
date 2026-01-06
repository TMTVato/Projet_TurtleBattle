using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Gère le repositionnement des tilemaps et ennemis pour créer un monde infini
public class Reposition : MonoBehaviour
{
    Collider2D coll; // Référence au collider de l'objet

    void Awake()
    {
        coll = GetComponent<Collider2D>();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Détecte si c'est pas le player qui sort de la zone
        if (!collision.CompareTag("Area"))
            return;

        // Récupère les positions du player et de l'objet
        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 myPos = transform.position;

        // Déplace l'objet selon son tag
        switch (transform.tag)
        {
            case "Ground":
                // Calcule la différence de position
                float diffx = playerPos.x - myPos.x;
                float diffy = playerPos.y - myPos.y;
                float dirX = diffx < 0 ? -1 : 1; // Direction horizontale (-1 ou 1)
                float dirY = diffy < 0 ? -1 : 1; // Direction verticale (-1 ou 1)

                // Convertit en valeurs absolues
                diffx = Mathf.Abs(diffx);
                diffy = Mathf.Abs(diffy);

                // Déplace sur l'axe dominant
                if (diffx > diffy)
                {
                    transform.Translate(Vector3.right * dirX * 40); // Déplacement horizontal
                }
                else if (diffx < diffy)
                {
                    transform.Translate(Vector3.up * dirY * 40); // Déplacement vertical
                }
                break;
            case "Enemy":
                if (coll.enabled)
                {
                    // Calcule la distance vers le player
                    Vector3 dist = playerPos - myPos;
                    Vector3 ran = new Vector3(Random.Range(-3, 3), 0); // Variation aléatoire horizontale
                    // Repositionne l'ennemi devant le player
                    transform.Translate(ran + dist * 2);
                }
                break;

        }
    }
}