using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaShift : MonoBehaviour
{
    private int tilemapSize = 40;
    Collider2D coll;


    void Awake()
    {
        coll = GetComponent<Collider2D>();
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        //detecte si c'est pas le player qui sort de la zone
        if (!collision.CompareTag("Area")) 
        {
            return;
        }
        //detecte la position du player par rapport à la zone
        Vector3 playerPosition = GameManager.instance.player.transform.position;
        Vector3 myPos = transform.position;
        float diffX = Mathf.Abs(playerPosition.x - myPos.x);
        float diffY = Mathf.Abs(playerPosition.y - myPos.y);

        //detecte la direction du player
        Vector3 playerDirection = GameManager.instance.player.movementInput;
        float dirX = playerDirection.x < 0 ? -1 : 1;
        float dirY = playerDirection.y < 0 ? -1 : 1;


        //déplace la tilemap en fonction de la position et direction du player
        switch (transform.tag) {
            case "Ground":
                if(diffX > diffY)
                {
                   transform.Translate(Vector3.right * dirX * tilemapSize);
                }
                else if (diffY > diffX)
                {
                    transform.Translate(Vector3.up * dirY * tilemapSize);
                }

                break;
            case "Enemy":
                if(coll.enabled) {                    
                    transform.Translate(playerDirection * tilemapSize/2 + new Vector3(Random.Range(-3f,3f), Random.Range(-3f, 3f),0f));
                }

                break;
            



        }

    }
}
