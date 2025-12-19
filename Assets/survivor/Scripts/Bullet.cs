using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage;
    public float per;//penetration

    public void Init(float damage, float per)
    {
        this.damage = damage;
        this.per = per;
    }
}
