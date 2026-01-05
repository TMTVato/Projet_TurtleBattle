using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Tower_shoot : MonoBehaviour
{

    [Header("References")]
    public GameObject projectile;
    [SerializeField] private GameObject Arrow;
    [SerializeField] private Transform ArrowRotationPoint;
    [SerializeField] private LayerMask enemyMask;

    [Header("Attributes")]
    [SerializeField] public float targetingRange;
    [SerializeField] public float fire_rate;
    public float bullet_velocity;
    public float bullet_life_time;
    public float bullet_size;
    public float bullet_damage;
    public float bullet_penetration;

    private Transform target = null;
    private bool shooting = false;

    // Valeurs de base pour les bonus
    private float base_fire_rate;
    private float base_bullet_damage;
    private float base_bullet_penetration;
    private float base_speed;
    private float base_targetingRange;

    private void Awake()
    {
        base_fire_rate = fire_rate;
        base_bullet_damage = bullet_damage;
        base_bullet_penetration = bullet_penetration;
        base_speed = bullet_velocity;
        base_targetingRange = targetingRange; // Ajoute cette ligne
    }

    // Méthode à appeler pour appliquer les bonus
    public void ApplyBonuses(float bonusDamage, float bonusFireRate, float bonusPenetration, float bonusSpeed, float bonusRange)
    {
        bullet_damage = base_bullet_damage * (1f + bonusDamage);
        fire_rate = base_fire_rate / (1f + bonusFireRate);
        bullet_penetration = base_bullet_penetration * (1f + bonusPenetration);
        bullet_velocity = base_speed * (1f + bonusSpeed);
        targetingRange = base_targetingRange * (1f + bonusRange);

        if (shooting)
        {
            CancelInvoke();
            InvokeRepeating("SpawnBullet", fire_rate, fire_rate);
        }
    }

    private void Update()
    {

        if (target != null) //if there is a target
        {
            StartShooting(); //Start shooting phase
            RotateTowardsTarget(); //rotate turret towards target

            if (!CheckTargetIsInRange()) //if target is out of range, lose current target
            {
                target = null;
            }
        }
        else //if there is no target
        {
            StopShooting(); //Stop shooting phase
            FindTarget(); //will search for a target. If it finds it, set it to "target" variable  

        }
    }
    

    private void FindTarget() //target the nearest enemy in range in the ennemy layer mask
    {
        RaycastHit2D[] targets = Physics2D.CircleCastAll(transform.position, targetingRange, (Vector2)transform.position, 0f, enemyMask);
        float diff = 100;

        foreach (RaycastHit2D hit in targets)
        {
            Vector3 myPos = transform.position;
            Vector3 targetPos = hit.transform.position;
            float curDiff = Vector3.Distance(myPos, targetPos);
            if (curDiff < diff)
            {
                diff = curDiff;
                target = hit.transform;
            }
        }
    }

    private void RotateTowardsTarget() //rotate turret towards target
    {
        float angle = Mathf.Atan2(target.position.y - transform.position.y, target.position.x - transform.position.x) * Mathf.Rad2Deg;

        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        ArrowRotationPoint.rotation = targetRotation;
    }

    private bool CheckTargetIsInRange() //check if target is still in range
    {
        return Vector2.Distance(target.position, transform.position) <= targetingRange;
    }

    private void StartShooting()
    {
        if (shooting == false) { shooting = true; InvokeRepeating("SpawnBullet", fire_rate, fire_rate); } //start shooting 
        if (Arrow.activeInHierarchy == false) { Arrow.SetActive(true); } //show arrow 
    } //Start shooting phase

    private void StopShooting()
    {
        if (shooting == true) { shooting = false; CancelInvoke(); } //stop shooting 
        if (Arrow.activeInHierarchy == true) { Arrow.SetActive(false); } //hide arrow 
    } //Stop shooting phas

    private void SpawnBullet()
    {
        Vector3 dir = (target.position - transform.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f; //ajuste angle to match sprite orientation
        Quaternion rot = Quaternion.Euler(0, 0, angle);

        GameObject bulletObj = Instantiate(projectile, transform.position, rot);
        Bullet bullet = bulletObj.GetComponent<Bullet>();
        if (bullet != null)
        {
            bullet.Init(
                bullet_damage,
                bullet_penetration,
                bullet_velocity,
                bullet_life_time,
                bullet_size,
                dir
            );
        }
    }


    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.red;
        Handles.DrawWireDisc(transform.position, transform.forward, targetingRange);
    }//draw max range when tower is selected in the editor


    public void SetFireRate(float newRate)
    {
        fire_rate = newRate;
        if (shooting)
        {
            CancelInvoke();
            InvokeRepeating("SpawnBullet", fire_rate, fire_rate);
        }
    }
    public float GetFireRate() => fire_rate;

    public void SetBulletDamage(float value) => bullet_damage = value;
    public float GetBulletDamage() => bullet_damage;

    public void SetBulletPenetration(float value) => bullet_penetration = value;
    public float GetBulletPenetration() => bullet_penetration;

    public void SetBulletVelocity(float value) => bullet_velocity = value;
    public float GetBulletVelocity() => bullet_velocity;

    public void SetTargetingRange(float value) => targetingRange = value;
    public float GetTargetingRange() => targetingRange;
}
