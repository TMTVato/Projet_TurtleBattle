using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLogic : MonoBehaviour
{
    // Enemy stats
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

    // Réinitialise l'ennemi lorsqu'il est activé
    void OnEnable()
    {
        isAlive = true;
        coll.enabled = true;
        rigid.simulated = true;
        spriter.sortingOrder = 2;
        anim.SetBool("Dead", false);
        health = maxHealth;

    }
    //Initialisation de l'ennemi avec les données de spawn
    public void Initialisation(SpawnData data)
    {
        anim.runtimeAnimatorController = animCon[data.spriteType];
        speed = data.speed;
        maxHealth = data.health;
        health = data.health;
        target = data.Target.GetComponent<Rigidbody2D>();
    }

    
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

    // Détecte les collisions avec les balles
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //ignore si ce n'est pas une balle, si l'ennemi est mort ou stun
        if ((!collision.CompareTag("Bullet")) || !isAlive || (isStun == true))
        {
            return;
        }
        //prend les dégâts de la balle
        Bullet bullet = collision.GetComponent<Bullet>();
        if (bullet != null)
        {
            TakeDamage(bullet.damage);
        }
    }
    // Applique les dégâts à l'ennemi
    public void TakeDamage(float damage)
    {
        //réduit la vie de l'ennemi
        health -= damage;
        StartCoroutine(KnockBack());
        //vérifie si l'ennemi est mort ou stun
        if (health > 0)
        {
            anim.SetTrigger("Hit");
            AudioManager.instance.PlaySFX(AudioManager.SFX.Hit);
            isStun = true;
            StartStuntime = Time.time;
        }
        //si mort, lance la séquence de mort
        else
        {
            isAlive = false;
            coll.enabled = false;
            rigid.simulated = false;
            spriter.sortingOrder = 1;
            anim.SetBool("Dead", true);
            GameManager.instance.kill++;
            GameManager.instance.GetExp();
            if (GameManager.instance.isLive)
            {
                AudioManager.instance.PlaySFX(AudioManager.SFX.Dead);
            }
            
            //Dead();
        }
    }

    // Coroutine pour l'effet knockback 
    private IEnumerator KnockBack()
    {
        yield return wait;
        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 dirVec = transform.position - playerPos;
        // Ajoute knockback
        rigid.AddForce(dirVec.normalized * 1.2f, ForceMode2D.Impulse);
    }

    // Désactive l'ennemi
    private void Dead()
    {
        gameObject.SetActive(false);
    }


    public void Update()
    {
        CheckisStun(); //check si l'ennemi doit être stun.
    }


    // Vérifie si l'ennemi est en état de stun
    private void CheckisStun()
    {
        if ((isStun == true) && (Time.time >= StartStuntime + hitstun_duration))
        {
            isStun = false;
        }
    }

}
