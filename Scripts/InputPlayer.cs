using UnityEngine;
using UnityEngine.InputSystem;

public class InputPlayer : MonoBehaviour
{
    // Keyboard.current,Mouse.current
    

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Debug.Log("space tuþuna basýldý");
        }

        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            Debug.Log("mouse sag tusa basýldý");
        }

        Vector2 pos = Mouse.current.position.ReadValue();
        Debug.Log("mouse pozisyon" + pos);
    }
}
