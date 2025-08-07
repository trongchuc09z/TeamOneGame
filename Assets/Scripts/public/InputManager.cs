using UnityEngine;

public class InputManager : MonoBehaviour
{ 
    [SerializeField] protected float onFiring;
    public static InputManager instance;
    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        InputMouseClick();
    }

    public float InputMouseClick()
    {
        onFiring = Input.GetAxis("Fire1"); 
        return onFiring;
    }
}
