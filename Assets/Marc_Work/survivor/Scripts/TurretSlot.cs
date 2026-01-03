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
