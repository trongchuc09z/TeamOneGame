using UnityEngine;

public class SetActive : ResestComponent
{
    [SerializeField] protected Transform targetObject;

    private void Awake()
    {
        targetObject.gameObject.SetActive(false);
    }

    private void Update()
    {
        SetActiveTrue();
    }
    protected override void LoadComponent()
    {
        foreach (Transform child in transform)
        {
            if (child.name == (transform.name + "_false") )
            {
                targetObject = child;
                break;
            }
        }
    }

    protected void SetActiveTrue() 
    {
        if (Input.GetMouseButtonDown(0)) // Bấm chuột trái
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null && hit.collider.CompareTag("Door"))
            {
                targetObject.gameObject.SetActive(true);
            }
        }
    }
}
