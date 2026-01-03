using UnityEngine;

public class TurretSlotClick : MonoBehaviour
{
    private Camera mainCamera;
    private Collider2D myCollider;

    void Start()
    {
        mainCamera = Camera.main;
        myCollider = GetComponent<Collider2D>();
        if (myCollider == null)
            Debug.LogWarning("Aucun Collider2D trouvé sur " + gameObject.name);
        else
            Debug.Log("Collider2D trouvé sur " + gameObject.name + " | Type: " + myCollider.GetType().Name);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            Vector2 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Debug.Log("Position souris (monde): " + mousePos);

            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
            if (hit.collider != null)
                Debug.Log("Raycast a touché: " + hit.collider.gameObject.name);
            else
                Debug.Log("Raycast n'a rien touché.");

            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                Debug.Log("Raycast hit sur " + gameObject.name);

                if (Input.GetMouseButtonDown(0))
                {
                    Debug.Log("Clic gauche détecté via Raycast sur " + gameObject.name);
                    TurretUIManager ui = FindObjectOfType<TurretUIManager>();
                    if (ui != null)
                    {
                        Debug.Log("TurretUIManager trouvé, tentative de placement de tourelle.");
                        ui.TryPlaceTurret(GetComponent<TurretSlot>());
                    }
                    else
                    {
                        Debug.LogWarning("TurretUIManager non trouvé !");
                    }
                }
                else if (Input.GetMouseButtonDown(1))
                {
                    Debug.Log("Clic droit détecté via Raycast sur " + gameObject.name);
                    TurretSlot slot = GetComponent<TurretSlot>();
                    if (slot != null)
                    {
                        Debug.Log("Suppression de la tourelle sur " + gameObject.name);
                        slot.RemoveTurret();
                    }
                    else
                    {
                        Debug.LogWarning("TurretSlot non trouvé sur " + gameObject.name);
                    }
                }
            }
        }
    }
}
