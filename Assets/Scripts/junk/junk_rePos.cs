using Unity.VisualScripting;
using UnityEngine;

public class junk_rePos : DragDrop
{
    [SerializeField] protected Transform newItem;
    [SerializeField] protected Transform oldItem;
    protected string itemName;

    private void Start()
    {
        LoadComponent();
    }

    private void Update()
    {
        Moving();
        RePostion();
    }

    protected void LoadComponent()
    {
        itemName = gameObject.name;
        posStart = transform.position;
        newItem = transform.Find(itemName + "_new");
        if (newItem != null)
            newItem.gameObject.SetActive(false);
        oldItem = transform.Find(itemName + "_old");

    }

    protected void RePostion()
    { 
        if (Input.GetMouseButtonUp(0) && isDragging) // nếu nhả chuột trái và đang kéo
        {
            isDragging = false;
            Debug.Log("EndDrag");
            if (isMouseOver)
            {
                Debug.Log("isMouseOver");
                oldItem.gameObject.SetActive(false);
                ActiveObject();
            }
            else
            {
                transform.position = posStart;
            }
        }
    }

    protected void ActiveObject()
    {
        newItem.position = posStart;
        newItem.gameObject.SetActive(true);
    }
}
