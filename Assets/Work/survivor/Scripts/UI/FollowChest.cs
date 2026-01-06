using UnityEngine;

public class FollowChest : MonoBehaviour
{
    public Transform chestTarget;
    public Vector3 offset = new Vector3(0, 50, 0);
    private RectTransform rectTransform;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Start()
    {
        FindChest();
    }

    void FindChest()
    {
        GameObject chest = GameObject.FindGameObjectWithTag("Chest");
        if (chest != null)
        {
            chestTarget = chest.transform;
            Debug.Log("Coffre trouvé et assigné au timer !");
        }
        
    }

    void LateUpdate()
    {
        if (chestTarget == null)
        {
            FindChest();
            return;
        }

        Vector3 screenPos = Camera.main.WorldToScreenPoint(chestTarget.position);
        rectTransform.position = screenPos + offset;
    }
}