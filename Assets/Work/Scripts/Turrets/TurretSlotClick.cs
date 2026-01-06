using UnityEngine;

public class TurretSlotClick : MonoBehaviour
{
    private Camera mainCamera;
    private Collider2D myCollider;

    void Start()
    {
        mainCamera = Camera.main;
        myCollider = GetComponent<Collider2D>();


    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)) //Clic gauche ou droite détecté
        {
            Vector2 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition); //Convertir la position de la souris en coordonnées du monde

            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero); //Lancer un rayon à partir de la position de la souris


            if (hit.collider != null && hit.collider.gameObject == gameObject) //Vérifier si le rayon a touché le slot de la tourelle 
            {
               

                if (Input.GetMouseButtonDown(0)) //Clic gauche pour placer une tourelle
                {
                   
                    TurretUIManager ui = FindObjectOfType<TurretUIManager>();
                    if (ui != null)
                    {
                      
                        ui.TryPlaceTurret(GetComponent<TurretSlot>());
                    }

                }
                else if (Input.GetMouseButtonDown(1)) //Clic droit pour retirer une tourelle
                {
                    
                    TurretSlot slot = GetComponent<TurretSlot>();
                    if (slot != null)
                    {
                        
                        slot.RemoveTurret();
                    }

                }
            }
        }
    }
}
