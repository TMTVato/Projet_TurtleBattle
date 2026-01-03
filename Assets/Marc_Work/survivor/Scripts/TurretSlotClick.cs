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
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            Vector2 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);

            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);


            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
               

                if (Input.GetMouseButtonDown(0))
                {
                   
                    TurretUIManager ui = FindObjectOfType<TurretUIManager>();
                    if (ui != null)
                    {
                      
                        ui.TryPlaceTurret(GetComponent<TurretSlot>());
                    }

                }
                else if (Input.GetMouseButtonDown(1))
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
