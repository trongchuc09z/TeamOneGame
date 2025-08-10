using Spine.Unity;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }

    public SkeletonAnimation skeletonAnimation;
    public Slider timeSlider;
    public float sliderTimer = 0f;
    [SerializeField] private float sliderDuration = 10f;

    private string currentAnimation = "";

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        timeSlider.value = 0f;
    }

    private void Update()
    {
        sliderTimer += Time.deltaTime;
        timeSlider.value = Mathf.Clamp01(sliderTimer / sliderDuration);

        string nextAnimation = "";

        if (timeSlider.value < 0.33f)
        {
            nextAnimation = "idle";
        }
        else if (timeSlider.value < 0.667f)
        {
            nextAnimation = "idle1";
        }
        else if (timeSlider.value < 1.0f)
        {
            nextAnimation = "idle2";
        }
        else // >= 1.0f
        {
            nextAnimation = "idle_fart";
            GameManager.Instance.LoseGame(); // Gọi hàm LoseGame khi đạt 100%
        }

        // Chỉ set animation khi thay đổi trạng thái
        if (nextAnimation != currentAnimation)
        {
            skeletonAnimation.AnimationState.SetAnimation(0, nextAnimation, true);
            currentAnimation = nextAnimation;
        }
    }

    public void ResetGameState()
    {
        sliderTimer = 0f;
        if (timeSlider != null)
            timeSlider.value = 0f;
        skeletonAnimation.AnimationState.SetAnimation(0, "idle", true);
    }
}