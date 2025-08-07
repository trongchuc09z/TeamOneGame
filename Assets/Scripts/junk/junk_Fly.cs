using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using static UnityEditor.Timeline.Actions.MenuPriority;

public class junk_Fly : DragDrop
{
    protected virtual void Update()
    {
        Moving();
        Fly();
    }

    protected virtual void Fly()
    {
        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            isDragging = false;
            Debug.Log("EndDrag");
            if (isMouseOver)
            {
                Debug.Log("isMouseOver");
                transform.gameObject.SetActive(false);

            }
            else
            {
                // đang kéo và nhả chuột trái ra nhưng không trên Player
                transform.position = posStart;
            }
        }
    }

}
