using UnityEngine;

public class InputManager : MonoBehaviour
{
    public float baseWidth = 1920f; // kích thước màn hình gốc bạn thiết kế
    public float baseHeight = 1080f;

    void Start()
    {
        ScaleToScreen();
    }

    void ScaleToScreen()
    {
        float screenRatioW = Screen.width / baseWidth;
        float screenRatioH = Screen.height / baseHeight;
        float scale = Mathf.Min(screenRatioW, screenRatioH); // giữ tỉ lệ

        transform.localScale *= scale;
    }
}
