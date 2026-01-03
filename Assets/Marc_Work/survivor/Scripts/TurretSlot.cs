using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretSlot : MonoBehaviour
{
    public GameObject currentTurret;

    public void PlaceTurret(GameObject turretPrefab)
    {
        if (currentTurret != null) Destroy(currentTurret);
        currentTurret = Instantiate(turretPrefab, transform.position, Quaternion.identity, transform);

        Tower_shoot tower = currentTurret.GetComponent<Tower_shoot>();
        if (tower != null)
        {
            // Récupère les valeurs de base
            float baseDamage = tower.GetBulletDamage();
            float baseFireRate = tower.GetFireRate();
            float basePenetration = tower.GetBulletPenetration();
            float baseSpeed = tower.GetBulletVelocity();
            float baseRange = tower.GetTargetingRange();

            // Application des bonus globaux
            tower.bullet_damage = baseDamage * (1f + GameManager.instance.bonusDamage);
            tower.fire_rate = baseFireRate * (1f + GameManager.instance.bonusFireRate);
            tower.bullet_penetration = basePenetration * (1f - GameManager.instance.bonusPenetration);
            tower.bullet_velocity = baseSpeed * (1f + GameManager.instance.bonusSpeed);
            tower.targetingRange = baseRange * (1f + GameManager.instance.bonusRange);

        }
    }

    public void RemoveTurret()
    {
        if (currentTurret != null)
        {
            Destroy(currentTurret);
            currentTurret = null;
        }
    }
}
