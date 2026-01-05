using UnityEngine;

public class TurretSlot : MonoBehaviour
{
    public GameObject currentTurret;
    public int turretTypeIndex;

    public void PlaceTurret(GameObject turretPrefab)
    {
        if (currentTurret != null) Destroy(currentTurret);
        currentTurret = Instantiate(turretPrefab, transform.position, Quaternion.identity, transform);

        TurretUIManager ui = FindObjectOfType<TurretUIManager>();
        if (ui != null)
        {
            for (int i = 0; i < ui.turretPrefabs.Length; i++)
            {
                if (ui.turretPrefabs[i] == turretPrefab)
                {
                    turretTypeIndex = i;
                    break;
                }
            }
        }

        Tower_shoot tower = currentTurret.GetComponent<Tower_shoot>();
        if (tower != null)
        {
            tower.ApplyBonuses(
                GameManager.instance.bonusDamage,
                GameManager.instance.bonusFireRate,
                GameManager.instance.bonusPenetration,
                GameManager.instance.bonusSpeed,
                GameManager.instance.bonusRange
            );
        }
    }

    public void RemoveTurret()
    {
        if (currentTurret != null)
        {
            Destroy(currentTurret);
            currentTurret = null;
            TurretUIManager ui = FindObjectOfType<TurretUIManager>();
            if (ui != null)
                ui.OnTurretRemoved(turretTypeIndex);
        }
    }
}
