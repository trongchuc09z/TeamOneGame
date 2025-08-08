using Spine.Unity;
using UnityEngine;

public class EnemyTungTungController : MonoBehaviour
{
    public static EnemyTungTungController Instance { get; private set; }

    public SkeletonAnimation skeletonAnimation;

    void Awake()
    {
        // Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        skeletonAnimation = GetComponent<SkeletonAnimation>();
        PlayWalkAnimation(); // Play animation on start
    }

    public void PlayWalkAnimation()
    {
        if (skeletonAnimation != null)
        {
            skeletonAnimation.AnimationState.SetAnimation(0, "walk", true); // Replace with your enemy walk animation name
        }
    }
}