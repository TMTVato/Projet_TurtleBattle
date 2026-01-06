using UnityEngine;

public class TurretSlot : MonoBehaviour
{
    public GameObject currentTurret;
    public int turretTypeIndex;
    //Place une tourelle dans le slot
    public void PlaceTurret(GameObject turretPrefab)
    {
        if (currentTurret != null) Destroy(currentTurret); // Supprime la tourelle existante s'il y en a une
        currentTurret = Instantiate(turretPrefab, transform.position, Quaternion.identity, transform); // Instancie la nouvelle tourelle

        TurretUIManager ui = FindObjectOfType<TurretUIManager>();
        // Trouve l'index de la tourelle placée
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
        // Applique les bonus de jeu à la tourelle placée
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
        AudioManager.instance.PlaySFX(AudioManager.SFX.Select);
    }

    public void RemoveTurret()
    {

        if (currentTurret != null)
        {
            Destroy(currentTurret); // Supprime la tourelle existante
            currentTurret = null;
            TurretUIManager ui = FindObjectOfType<TurretUIManager>();
            //UIManager pour mettre à jour le compteur
            if (ui != null)
                ui.OnTurretRemoved(turretTypeIndex);
        }
    }
}
