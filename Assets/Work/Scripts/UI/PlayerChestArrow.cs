using UnityEngine;

public class PlayerChestArrow : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject arrow;
    [SerializeField] private Transform arrowRotationPoint;

    private Transform chestTarget = null;
    private bool isInChestZone = false;

    private void Start()
    {
        if (arrow != null)
            arrow.SetActive(false);
    }

    private void Update()
    {
        //Si le coffre existe et que le joueur n'est pas dans la zone du coffre
        if (chestTarget != null && !isInChestZone)
        {
            ShowArrow(); //Indicateur position coffre 
            RotateTowardsChest();
        }
        else
        {
            HideArrow();
        }
    }
    //Faire tourner la flèche vers le coffre
    private void RotateTowardsChest()
    {
        //Calculer l'angle entre la position du joueur et celle du coffre
        float angle = Mathf.Atan2(chestTarget.position.y - transform.position.y,
                                   chestTarget.position.x - transform.position.x) * Mathf.Rad2Deg;
        //Appliquer la rotation à la flèche
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        arrowRotationPoint.rotation = targetRotation;
    }
    //Méthodes pour : Afficher la flèche ou cacher la flèche
    private void ShowArrow()
    {
        if (arrow != null && !arrow.activeInHierarchy)
            arrow.SetActive(true);
    }

    private void HideArrow()
    {
        if (arrow != null && arrow.activeInHierarchy)
            arrow.SetActive(false);
    }

    public void SetChestTarget(Transform chest)
    {
        chestTarget = chest;
        isInChestZone = false;
    }

    public void ClearChestTarget()
    {
        chestTarget = null;
        HideArrow();
    }

    public void SetInZone(bool inZone)
    {
        isInChestZone = inZone;
    }
}