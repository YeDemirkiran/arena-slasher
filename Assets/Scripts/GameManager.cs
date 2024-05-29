using UnityEngine;

public class GameManager : MonoBehaviour
{
    void Start()
    {
        SetMouse(false);
    }

    public void SetMouse(bool on)
    {
        Cursor.visible = on;
        Cursor.lockState = on ? CursorLockMode.None : CursorLockMode.Locked;
    }
}