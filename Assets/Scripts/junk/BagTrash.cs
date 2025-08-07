using UnityEngine;

public class BagTrash : junk_Fly
{
    [SerializeField] private Transform unhealthyBag; // túi rác không lành
    [SerializeField] private Transform healthyBag;   // túi rác lành
    [SerializeField] private Transform otherBag;     // túi rác thối

    protected void Start()
    {
        unhealthyBag = GetComponent<Transform>().Find("trash_bag_1_0");
        healthyBag = GetComponent<Transform>().Find("trash_bag_2_0");
        otherBag = GetComponent<Transform>().Find("trash_fart_bag_0");

        healthyBag.gameObject.SetActive(false);
        otherBag.gameObject.SetActive(false);
    }
    protected override void Update()
    {
        Moving();
        CheckBag();
        Fly();
    }
    protected void CheckBag()
    {
        if(isDragging == true)
        {
            unhealthyBag.gameObject.SetActive(false);
            healthyBag.gameObject.SetActive(true);
        }
    }
    protected override void Fly()
    {
        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            isDragging = false;
            Debug.Log("EndDrag");
            if (isMouseOver)
            {
                Debug.Log("isMouseOver");
                unhealthyBag.gameObject.SetActive(false);
                healthyBag.gameObject.SetActive(false);

            }
            else
            {
                // đang kéo và nhả chuột trái ra nhưng không trên Player
                unhealthyBag.gameObject.SetActive(true);
                healthyBag.gameObject.SetActive(false);
                transform.position = posStart;
            }
        }
    }


}