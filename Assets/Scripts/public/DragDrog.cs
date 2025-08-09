using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class DragDrop : MonoBehaviour
{
    protected bool isDragging = false; // Biến để kiểm tra xem đối tượng có đang được kéo hay không
    protected bool isMouseOver = false; // Biến để kiểm tra xem chuột có đang ở trên đối tượng hay không
    protected Vector3 offset;
    protected Rigidbody2D rb;
    protected Collider2D col;
    protected Vector2 posStart;

    void Awake()
    {
        AddCollider2D(); // Gọi hàm này trước
        AddRigidbody2D();
        posStart = transform.position;
    }

    protected void AddRigidbody2D()
    {
        if (this.rb != null) return;
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0; // Tắt trọng lực
        
    }

    protected void AddCollider2D()
    {
        if (this.col != null) return;
        col = GetComponent<Collider2D>();
        if (col == null) // Kiểm tra nếu chưa có Collider2D
        {
            // Tự động thêm một Collider2D vào
            col = gameObject.AddComponent<BoxCollider2D>();
            col.isTrigger = true; // Đặt là trigger để không can thiệp với vật lý
        }
    }

    protected virtual void Moving()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0; // đảm bảo ở mặt phẳng 2D

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero);
            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                isDragging = true;
                offset = transform.position - mouseWorldPos; // Tính offset
                //Debug.Log("OnPointerDown");
            }
        }

        if (Input.GetMouseButton(0) && isDragging)
        {
            transform.position = mouseWorldPos + offset;
            //Debug.Log("OnDrag");
        }
    }
    protected void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            isMouseOver = true; 
    }

    protected void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            isMouseOver = false;
    }
}