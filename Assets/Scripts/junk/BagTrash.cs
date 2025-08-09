using DG.Tweening;
using Spine.Unity;
using UnityEngine;

public class BagTrash : junk_Fly
{
    [SerializeField] private Transform unhealthyBag;
    [SerializeField] private Transform healthyBag;
    [SerializeField] private Transform otherBag;

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

                otherBag.localPosition = moveTargetA;

                rotateTween?.Kill();

                PlayerController.Instance.skeletonAnimation.AnimationState.SetAnimation(0, "idle_fart", false);

                // Chờ 1 giây rồi reset mọi thứ
                StartCoroutine(ResetAfterFart());

                rotateTween = otherBag.DOLocalRotate(
                    new Vector3(0, 0, 360),
                    10f,
                    RotateMode.FastBeyond360
                ).SetEase(Ease.Linear)
                 .SetLoops(-1, LoopType.Incremental);

                moveTween?.Kill();
                moveTween = otherBag.DOLocalMove(moveTargetB, 1.5f)
                    .SetEase(Ease.InOutSine)
                    .SetLoops(-1, LoopType.Yoyo);
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

    private System.Collections.IEnumerator ResetAfterFart()
    {
        yield return new WaitForSecondsRealtime(1f);
        // Reset slider và animation player
        PlayerController.Instance.ResetGameState();
    }
}