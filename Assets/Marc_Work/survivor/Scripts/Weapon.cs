using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int id;
    public int prefabId;
    public float damage;
    public int count;
    public float speed;

    PlayerLogic player;
    float timer;

    private void Awake()
    {
        player = GameManager.instance.player;
    }


    public void Init(ItemData data)
    {

        // Basic set
        name = data.itemName;
        transform.parent = player.transform;
        transform.localPosition = Vector3.zero;

        //Property set
        id = data.itemId;
        damage = data.base_dmg;
        count = data.base_count;

        for (int i = 0; i < GameManager.instance.poolManager.prefabs.Length; i++)
        {
            if (data.proj == GameManager.instance.poolManager.prefabs[i])
            {
                prefabId = i;
                break;
            }
        }

        //initialize weapon stats from item data
        switch (id)
        {
            case 0: //shovel
                speed = 150;

                Batch();
                break;
           
            default:
                speed = 0.4f;
                break;
        }
        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);

    }

    void Batch() {
        for (int i = 0; i < count; i++) {

            Transform bullet;

            if(i < transform.childCount)
            {
                bullet = transform.GetChild(i);
            }
            else
            {
                bullet = GameManager.instance.poolManager.Get(prefabId).transform;
                bullet.parent = transform;
            }
            

            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;

            Vector3 rotVec = Vector3.forward * 360 * i /count;
            bullet.Rotate(rotVec);
            bullet.Translate(bullet.up * 1.5f, Space.World);

            bullet.GetComponent<Bullet>().Init(damage, -1, Vector3.zero) ; //infinite penetration
        }


    }
    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.isLive) return;
        switch (id)
        {
            case 0: 
               transform.Rotate(Vector3.back * speed * Time.deltaTime);
                break;

            default:
                timer += Time.deltaTime;
                if (timer > speed)
                {
                    timer = 0;
                    Fire();
                }
                break;
        }

        //if (Input.GetButtonDown("Jump"))
        //    LevelUp(5, 1);
    }

    public void LevelUp(float damage, int count)
    {
        this.damage = damage;
        this.count += count;

        if( id==0)
            Batch();
        

        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }

    public void Fire()
    {
        if (!player.scanner.nearestTarget)
            return;

        Vector3 targetpos = player.scanner.nearestTarget.position;
        Vector3 dir = targetpos - transform.position;
        dir = dir.normalized;

        GameObject bulletObj = GameManager.instance.poolManager.Get(prefabId);
        if (bulletObj == null)
            return;

        Transform bullet = bulletObj.transform;
        bullet.position = transform.position;
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);
        bullet.GetComponent<Bullet>().Init(damage, count, dir);
    }
}
