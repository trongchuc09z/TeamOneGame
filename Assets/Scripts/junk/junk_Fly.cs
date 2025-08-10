using UnityEngine;
using DG.Tweening;

public class JunkItem : DragDrop
{
    public bool isGoodItem = false; // false = xấu, true = tốt

    public enum ItemType
    {
        OneState,       // Kéo vào player thì biến mất
        TwoStates,      // old -> new
        ThreeStates     // old -> drag -> new
    }

    public enum TwoStateAction
    {
        ReturnToStart,  // Quay lại vị trí ban đầu
        DoAnimation     // Xoay + di chuyển qua lại
    }

    [Header("Config")]
    public ItemType itemType;
    public TwoStateAction twoStateAction;
    public bool newStateDoAnimation = false; // cho new chạy animation nếu cần

    [Header("References")]
    public Transform oldItem;      // sprite ban đầu
    public Transform newItem;      // sprite khi hoàn thành
    public Transform draggingItem; // sprite khi đang kéo (nếu có)

    [Header("Animation Targets")]
    public Vector2 moveTargetA = new Vector2(-2, 0);
    public Vector2 moveTargetB = new Vector2(2, 0);

    private Tween moveTween;
    private Tween rotateTween;
    public AudioClip fartSound;
    public bool isUsed = false;
    private Rigidbody2D rb;
    protected override void Start()
    {
        // Auto find nếu chưa gán
        if (oldItem == null) oldItem = transform.Find(gameObject.name + "_old");
        if (newItem == null) newItem = transform.Find(gameObject.name + "_new");
        if (draggingItem == null) draggingItem = transform.Find(gameObject.name + "_drag");

        // Ẩn các sprite phụ ban đầu
        if (newItem != null) newItem.gameObject.SetActive(false);
        if (draggingItem != null) draggingItem.gameObject.SetActive(false);

        // Nếu không có drag → tự coi là TwoStates
        if (draggingItem == null && itemType == ItemType.ThreeStates)
        {
            itemType = ItemType.TwoStates;
        }
        posStart = transform.position;
        rb = GetComponent<Rigidbody2D>();
    }

    private void SetActiveOnly(Transform target)
    {
        if (oldItem != null) oldItem.gameObject.SetActive(false);
        if (newItem != null) newItem.gameObject.SetActive(false);
        if (draggingItem != null) draggingItem.gameObject.SetActive(false);
        if (target != null) target.gameObject.SetActive(true);
    }

    protected override void OnMouseDown()
    {
        base.OnMouseDown();

        // Khi bắt đầu kéo
        if (itemType == ItemType.ThreeStates && draggingItem != null)
        {
            SetActiveOnly(draggingItem);
        }
    }

    protected override void OnDropToPlayer()
    {
        rb.simulated = false; // Tắt vật lý khi thả
        if (fartSound != null)
            GameManager.Instance.PlaySound(fartSound);
        PlayerController.Instance.timeSlider.value = 0f; // Reset thanh thời gian
        PlayerController.Instance.sliderTimer = 0f; // Reset timer
        // 💨 Gọi animation fart cho Player
        if (PlayerController.Instance != null && PlayerController.Instance.skeletonAnimation != null)
        {
            PlayerController.Instance.skeletonAnimation.AnimationState.SetAnimation(0, "idle_fart", false);
            // Sau khi fart xong, về idle
            PlayerController.Instance.skeletonAnimation.AnimationState.AddAnimation(0, "idle", true, 0);
        }
        switch (itemType)
        {
            case ItemType.OneState:
                gameObject.SetActive(false);
                break;

            case ItemType.TwoStates:
                HandleTwoStates();
                break;

            case ItemType.ThreeStates:
                HandleThreeStates();
                break;
        }
        if(isGoodItem)
        {
            GameManager.Instance.AddScore();
        }
        else
        {
            GameManager.Instance.LoseGame();
        }
    }

    protected override void OnDropFail()
    {
        if (itemType == ItemType.ThreeStates)
        {
            SetActiveOnly(oldItem);
        }
        else
        {
            transform.position = posStart;
        }
    }

    private void HandleTwoStates()
    {
        SetActiveOnly(newItem);

        if (twoStateAction == TwoStateAction.DoAnimation)
        {
            StartMoveAnimation();
        }
        else
        {
            transform.position = posStart;
        }
    }

    private void HandleThreeStates()
    {
        SetActiveOnly(newItem);

        if (newStateDoAnimation)
        {
            StartMoveAnimation();
        }
    }

    private void StartMoveAnimation()
    {
        if (newItem == null) return;

        rotateTween?.Kill();
        moveTween?.Kill();

        // Gán vị trí ban đầu của animation
        newItem.position = moveTargetA;

        rotateTween = newItem.DOLocalRotate(
            new Vector3(0, 0, 360),
            10f,
            RotateMode.FastBeyond360
        ).SetEase(Ease.Linear)
         .SetLoops(-1, LoopType.Incremental);

        newItem.DOMove(moveTargetB, 1.5f)
    .SetEase(Ease.InOutSine)
    .SetLoops(-1, LoopType.Yoyo);

    }
}