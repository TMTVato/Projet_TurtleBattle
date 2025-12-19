using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLogic : MonoBehaviour
{

    public float speed = 2;
    public Rigidbody2D target;
    bool isAlive = true;

    Rigidbody2D rigid;
    SpriteRenderer spriter;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        
    }
    void Start()
    {
        target = GameManager.instance.player.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (!isAlive)
        {
            return;
        }

        //déplace l'ennemi vers le player
        Vector2 dirVec = (Vector2)target.position - rigid.position;
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
        rigid.velocity = Vector2.zero;

        

    }

    void LateUpdate()
    {
        if (!isAlive)
        {
            return;
        }
        //flip sprite dépendant de la direction
        spriter.flipX = target.position.x < rigid.position.x;
    }
}
