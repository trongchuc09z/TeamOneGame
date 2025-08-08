using UnityEngine;
using DG.Tweening;

public class BagTrash : junk_Fly
{
    [SerializeField] private Transform unhealthyBag; // túi rác không lành
    [SerializeField] private Transform healthyBag;   // túi rác lành
    [SerializeField] private Transform otherBag;     // túi rác thối

    // Đặt đúng vị trí di chuyển
    [SerializeField] private Vector3 moveTargetA = new Vector3(-2, 7, 0);
    [SerializeField] private Vector3 moveTargetB = new Vector3(2, 7, 0);

    private Tween moveTween;
    private Tween rotateTween;

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
                otherBag.gameObject.SetActive(true);

                // Đặt vị trí bắt đầu cho otherBag
                otherBag.localPosition = moveTargetA;

                // DOTween: Xoay theo trục Z một chiều
                rotateTween?.Kill();
                rotateTween = otherBag.DOLocalRotate(
                    new Vector3(0, 0, 360), // Xoay 360 độ quanh trục Z
                    10f, // Thời gian xoay 1 vòng
                    RotateMode.FastBeyond360
                ).SetEase(Ease.Linear)
                 .SetLoops(-1, LoopType.Incremental); // Lặp vô hạn, chỉ xoay một chiều

                // DOTween: Di chuyển qua lại giữa hai vị trí
                moveTween?.Kill();
                moveTween = otherBag.DOLocalMove(moveTargetB, 1.5f)
                    .SetEase(Ease.InOutSine)
                    .SetLoops(-1, LoopType.Yoyo);
                PlayerController.Instance.PlayWalkAnimation(); // Phát hoạt ảnh đi bộ của Player
            }
            else
            {
                unhealthyBag.gameObject.SetActive(true);
                healthyBag.gameObject.SetActive(false);
                transform.position = posStart;
                otherBag.gameObject.SetActive(false);
            }
        }
    }


}