using UnityEngine;
using TMPro;
using System.Collections;

/// <summary>
/// TMP_Text 컴포넌트에 타이핑 효과를 적용하는 스크립트입니다.
/// 게임 시작 시 자동 실행 대신, TimeLine Signal 또는 OnTriggerEnter 이벤트에 의해 호출됩니다.
/// </summary>
public class Typewriter : MonoBehaviour
{
    [Header("UI Component")]
    [Tooltip("타이핑 효과를 적용할 TMP_Text 컴포넌트를 연결하세요.")]
    public TMP_Text textUI;

    [Header("Typing Settings")]
    [Tooltip("글자 하나가 출력되는 간격입니다. (값이 작을수록 빠릅니다. 예: 0.05f)")]
    public float typingSpeed = 0.05f;

    [Header("Trigger Settings (OnTriggerEnter 사용 시)")]
    [TextArea(3, 10)]
    [Tooltip("트리거 진입 시 출력될 대사입니다.")]
    public string triggerText = "트리거 영역에 진입했습니다.";

    [Tooltip("트리거는 한 번만 작동해야 하는 경우 체크하세요.")]
    public bool triggerOnce = true;

    // 현재 타이핑 중인 코루틴을 저장하여 중복 실행을 방지합니다.
    private Coroutine typingCoroutine;
    private bool hasTriggered = false;

    void Awake()
    {
        // textUI 컴포넌트가 할당되지 않았거나 TMP_Text 컴포넌트가 없다면 컴포넌트에서 찾습니다.
        if (textUI == null)
        {
            textUI = GetComponent<TMP_Text>();
            if (textUI == null)
            {
                Debug.LogError("Typewriter: TMP_Text component is required on this GameObject or must be assigned in the Inspector.");
            }
        }

        // 시작 시 텍스트를 비워둡니다.
        if (textUI != null)
        {
            textUI.text = "";
        }
    }

    /// <summary>
    /// 1. TimeLine Signal 또는 다른 스크립트에서 호출될 때 사용합니다.
    /// (Signal Receiver에 연결하여 string 매개변수를 받도록 설정)
    /// </summary>
    /// <param name="text">출력할 전체 텍스트</param>
    public void OnSignalReceived(string text)
    {
        if (textUI != null && !string.IsNullOrEmpty(text))
        {
            StartTypingCoroutine(text, typingSpeed);
        }
    }

    /// <summary>
    /// 2. OnTriggerEnter 이벤트로 호출될 때 사용합니다.
    /// (이 GameObject에 Collider와 Rigidbody가 있어야 작동합니다.)
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        // 플레이어 태그를 확인하고, 한 번만 트리거되도록 설정되었는지 확인합니다.
        if (other.CompareTag("Player") && (!triggerOnce || !hasTriggered))
        {
            StartTypingCoroutine(triggerText, typingSpeed);
            hasTriggered = true; // 트리거 작동 기록
        }
    }

    /// <summary>
    /// 타이핑 코루틴을 시작하고, 이전 코루틴이 있다면 중지합니다.
    /// </summary>
    /// <param name="fullText">출력할 전체 텍스트</param>
    /// <param name="delay">글자당 대기 시간</param>
    private void StartTypingCoroutine(string fullText, float delay)
    {
        // 이미 타이핑 중이라면 기존 코루틴을 중지합니다. (새 대사가 덮어쓰도록)
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }
        typingCoroutine = StartCoroutine(TypeText(fullText, delay));
    }

    /// <summary>
    /// 실제 타이핑 효과를 구현하는 코루틴입니다.
    /// </summary>
    private IEnumerator TypeText(string fullText, float delay)
    {
        textUI.text = ""; // 텍스트를 초기화하고 빈 문자열부터 시작합니다.

        // 텍스트를 한 글자씩 출력합니다.
        foreach (char c in fullText)
        {
            textUI.text += c;
            yield return new WaitForSeconds(delay);
        }

        typingCoroutine = null; // 타이핑이 완료되면 코루틴 참조를 해제합니다.
    }
}