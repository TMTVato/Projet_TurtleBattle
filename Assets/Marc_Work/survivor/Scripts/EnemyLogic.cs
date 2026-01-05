using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLogic : MonoBehaviour
{

    public float speed;
    public float health;
    public float maxHealth;
    public float hitstun_duration;
    public RuntimeAnimatorController[] animCon;

    public Rigidbody2D target;
    bool isAlive;
    private bool isStun = false;
    public float StartStuntime ;

    Rigidbody2D rigid;
    Collider2D coll;
    SpriteRenderer spriter;
    Animator anim;

    private WaitForFixedUpdate wait;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        wait = new WaitForFixedUpdate();
        coll = GetComponent<Collider2D>();
    }


    void OnEnable()
    {
        target = GameManager.instance.player.GetComponent<Rigidbody2D>();
        isAlive = true;
        coll.enabled = true;
        rigid.simulated = true;
        spriter.sortingOrder = 2;
        anim.SetBool("Dead", false);
        health = maxHealth;

    }

    public void Initialisation(SpawnData data)
    {
        anim.runtimeAnimatorController = animCon[data.spriteType];
        speed = data.speed;
        maxHealth = data.health;
        health = data.health;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!GameManager.instance.isLive) return;
        if (!isAlive || anim.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
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
        if (!GameManager.instance.isLive) return;
        if (!isAlive)
        {
            return;
        }
        //flip sprite dépendant de la direction
        spriter.flipX = target.position.x < rigid.position.x;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((!collision.CompareTag("Bullet")) || !isAlive || (isStun == true))
        {
            return;
        }

        Bullet bullet = collision.GetComponent<Bullet>();
        if (bullet != null)
        {
            TakeDamage(bullet.damage);
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        StartCoroutine(KnockBack());
        if (health > 0)
        {
            anim.SetTrigger("Hit");
            isStun = true;
            StartStuntime = Time.time;
        }
        else
        {
            isAlive = false;
            coll.enabled = false;
            rigid.simulated = false;
            spriter.sortingOrder = 1;
            anim.SetBool("Dead", true);
            GameManager.instance.kill++;
            GameManager.instance.GetExp();
            //Dead();
        }
    }

    // Coroutine for knockback effect
    private IEnumerator KnockBack()
    {
        yield return wait;
        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 dirVec = transform.position - playerPos;
        // Apply knockback force
        rigid.AddForce(dirVec.normalized * 1.2f, ForceMode2D.Impulse);
    }

    // Method called when the enemy is dead
    private void Dead()
    {
        gameObject.SetActive(false);
    }


    public void Update()
    {
        CheckisStun(); //check si l'ennemi doit être stun.
    }



    private void CheckisStun()
    {
        if ((isStun == true) && (Time.time >= StartStuntime + hitstun_duration))
        {
            isStun = false;
        }
    }

}
