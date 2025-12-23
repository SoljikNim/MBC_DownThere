using UnityEngine;
using UnityEngine.InputSystem;

public class Starter : MonoBehaviour
{
    public InputActionReference inputAction;
    public PlayBtn playBtn;   // 인스펙터에서 할당
    public GameObject PlayObject;

    private void OnEnable()
    {
        if (inputAction != null)
            inputAction.action.started += OnInputStarted;
    }

    private void OnDisable()
    {
        if (inputAction != null)
            inputAction.action.started -= OnInputStarted;
    }

    private void OnInputStarted(InputAction.CallbackContext context)
    {
        Debug.Log("지정한 입력Action 실행됨!");
        TriggerMethod();
    }

    private void TriggerMethod()
    {
        Debug.Log("메서드 실행됨");
        PlayObject.SetActive(false);
        playBtn.OnPlayClicked();   // 인스턴스 메서드 실행
    }
}
