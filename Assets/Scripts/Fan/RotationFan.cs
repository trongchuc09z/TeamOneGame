using UnityEngine;
using DG.Tweening; // Cần import DOTween

public class RotationFan : MonoBehaviour
{
    public float rotationDuration = 2f; // Thời gian quay toàn bộ
    public int numberOfRotations = 4; // Số vòng quay
    public GameObject fanBlade; // Lưỡi quạt
    public GameObject Wind;
    private bool isRotating = false;
    public AudioClip audioSource;
    public AudioClip audiofart;
    private void OnMouseDown()
    {
        if (isRotating) return; // Đang quay thì bỏ qua
        isRotating = true; // Đánh dấu là đang quay
        // Reset rotation về 0 (nếu cần)
        fanBlade.transform.localRotation = Quaternion.identity;
        GameManager.Instance.AddScore();
        Wind.SetActive(true); // Bật hiệu ứng gió
        GameManager.Instance.PlaySound(audioSource);
        GameManager.Instance.PlaySound(audiofart); // Phát âm thanh khi bắt đầu quay
        // Quay số vòng đã set
        fanBlade.transform
            .DOLocalRotate(new Vector3(0, 0, 360f * numberOfRotations), rotationDuration, RotateMode.FastBeyond360)
            .SetEase(Ease.OutCubic) // Chậm dần khi dừng
            .OnComplete(() =>
            {
                Wind.SetActive(false); // Tắt hiệu ứng gió khi quay xong
            });
        PlayerController.Instance.timeSlider.value = 0f; // Reset thanh thời gian
        PlayerController.Instance.sliderTimer = 0f; // Reset timer
        // 💨 Gọi animation fart cho Player
        if (PlayerController.Instance != null && PlayerController.Instance.skeletonAnimation != null)
        {
            PlayerController.Instance.skeletonAnimation.AnimationState.SetAnimation(0, "idle_fart", false);
            // Sau khi fart xong, về idle
            PlayerController.Instance.skeletonAnimation.AnimationState.AddAnimation(0, "idle", true, 0);
        }
    }
}
