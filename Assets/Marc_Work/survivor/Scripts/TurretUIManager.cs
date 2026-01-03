using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretUIManager : MonoBehaviour
{
    public GameObject[] turretPrefabs;
    private GameObject selectedTurret;

    public void SelectTurret(int index)
    {
        selectedTurret = turretPrefabs[index];
    }

    public void TryPlaceTurret(TurretSlot slot)
    {
        if (selectedTurret != null)
            slot.PlaceTurret(selectedTurret);
    }
}
