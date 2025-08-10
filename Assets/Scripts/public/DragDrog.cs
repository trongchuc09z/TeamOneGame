using UnityEngine;

public class DragDrop : MonoBehaviour
{
    protected Vector3 posStart;       // vị trí ban đầu
    protected bool isDragging = false;
    protected bool isMouseOverPlayer = false;

    [Header("Player Detect")]
    public string playerTag = "Player"; // gán tag cho player để check

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {
        if (isDragging)
        {
            // Cập nhật vị trí theo chuột
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 10f; // khoảng cách từ camera đến object
            transform.position = Camera.main.ScreenToWorldPoint(mousePos);
        }
    }

    protected virtual void OnMouseDown()
    {
        isDragging = true;
    }

    protected virtual void OnMouseUp()
    {
        isDragging = false;

        if (isMouseOverPlayer)
        {
            OnDropToPlayer();
        }
        else
        {
            OnDropFail();
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(playerTag))
        {
            isMouseOverPlayer = true;
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(playerTag))
        {
            isMouseOverPlayer = false;
        }
    }

    /// <summary>
    /// Override để xử lý khi thả trúng player
    /// </summary>
    protected virtual void OnDropToPlayer() { }

    /// <summary>
    /// Override để xử lý khi thả thất bại
    /// </summary>
    protected virtual void OnDropFail()
    {
        // Mặc định trả về vị trí ban đầu
        transform.position = posStart;
    }
}