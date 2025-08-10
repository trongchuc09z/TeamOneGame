using Spine.Unity;
using UnityEngine;
using DG.Tweening;

public class EnemyTungTungController : MonoBehaviour
{
    public static EnemyTungTungController Instance { get; private set; }

    [Header("Spine")]
    public SkeletonAnimation skeletonAnimation;

    [Header("Walk Settings")]
    public Transform leftPoint;
    public Transform rightPoint;
    public float walkSpeed = 2f;

    [Header("Run Settings")]
    public float runSpeed = 5f;
    public float scareDuration = 1f;
    public float detectDuration = 1f; // thời gian đứng detect

    private Tween walkTween;
    private Tween runTween;
    private bool isWalking = false;
    private bool isRunningAway = false;
    private bool isDetecting = false;

    private float fixedY;

    private void Awake()
    {
        // Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        if (skeletonAnimation == null)
            skeletonAnimation = GetComponent<SkeletonAnimation>();

        fixedY = transform.position.y;
    }

    private void Start()
    {
        PlayWalkAnimation();
    }

    public void PlayWalkAnimation()
    {
        if (isRunningAway || isDetecting) return;

        StopAllActions();

        skeletonAnimation.AnimationState.SetAnimation(0, "walk", true);
        isWalking = true;

        walkTween = transform.DOMoveX(rightPoint.position.x, walkSpeed)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Yoyo)
            .OnStepComplete(() => FlipEnemy());
    }

    public void PlayDetectSequence()
    {
        if (isRunningAway || isDetecting) return;

        StopAllActions();
        isDetecting = true;

        // 1. Detect
        skeletonAnimation.AnimationState.SetAnimation(0, "detect", true);
    }

    public void PlayRunSequence()
    {
        if (isRunningAway) return;

        isRunningAway = true;
        StopAllActions();

        // 1. Scare
        skeletonAnimation.AnimationState.SetAnimation(0, "scare", false);

        // 2. Sau scareDuration mới chạy
        DOVirtual.DelayedCall(scareDuration, () =>
        {
            skeletonAnimation.AnimationState.SetAnimation(0, "run", true);

            bool facingRight = transform.localScale.x > 0;
            float targetX = facingRight ? 15f : -15f;

            runTween = transform.DOMoveX(targetX, runSpeed)
                .SetEase(Ease.Linear);
        });
    }

    private void StopAllActions()
    {
        isWalking = false;
        walkTween?.Kill();
        runTween?.Kill();
    }

    private void FlipEnemy()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;

        transform.position = new Vector3(transform.position.x, fixedY, transform.position.z);
    }

    private void LateUpdate()
    {
        if (isWalking)
        {
            transform.position = new Vector3(transform.position.x, fixedY, transform.position.z);
        }
    }
}
