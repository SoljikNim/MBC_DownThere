using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Haptics;

public class TriggerButtonHand : MonoBehaviour
{
    public InputActionReference interactButton;
    public TriggerButton currentButton;
    public HapticImpulsePlayer haptic;

    private void FixedUpdate()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ButtonObject"))
        {
            haptic.SendHapticImpulse(0.3f, 0.25f);
            currentButton = other.GetComponent<TriggerButton>();
            currentButton.ChangeColor(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("ButtonObject"))
        {
            currentButton.ChangeColor(false);
            currentButton = null;
        }
    }
    void OnEnable()
    {
        interactButton.action.performed += HideInteraction;
    }

    void HideInteraction(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            if (currentButton != null)
            {
                currentButton.MoveScene();
            }
        }
    }
}
