using UnityEngine;

public class HeadLantern : MonoBehaviour
{
    [Header("타겟 설정")]
    [Tooltip("활성화시킬 플레이어 머리의 헤드랜턴 오브젝트")]
    public GameObject Lanternlight;

    [Tooltip("충돌을 감지할 플레이어 머리 오브젝트의 태그 이름 (반드시 유니티 Tag 설정과 같아야 함)")]
    public string targetTag = "PlayerHead";

    private bool isActivated = false;

    // 1. 버튼으로 켜는 함수 (외부 호출용)
    public void ActivateLanternFromButton(GameObject buttonObject)
    {
        if (isActivated) return;
        TurnOnLight();
        if (buttonObject != null) buttonObject.SetActive(false);
    }

    // 2. 머리에 가져다 대면 켜지는 함수 (물리 충돌)
    private void OnTriggerEnter(Collider other)
    {
        // 디버그용: 무엇이랑 부딪혔는지 무조건 출력
        // Debug.Log($"충돌 감지됨: {other.gameObject.name} (Tag: {other.tag})");

        if (isActivated) return;

        // 태그가 일치하는지 확인 (가장 확실한 방법)
        if (other.CompareTag(targetTag))
        {
            Debug.Log("플레이어 머리 태그 확인됨! 장착 진행.");
            TurnOnLight();
            Destroy(gameObject); // 픽업 아이템 삭제
        }
    }

    // 3. 슬라이더 값 변경 시 호출될 함수 (추가됨)
    // 슬라이더 컴포넌트의 On Value Change 이벤트에 이 함수를 연결하세요.
    public void CheckSliderValue(float value)
    {
        if (isActivated) return;

        // 슬라이더 값이 1 (부동소수점 오차 고려 0.99 이상)이 되면 켜기
        if (value >= 0.99f)
        {
            Debug.Log("슬라이더 값 1 도달! 라이트 켜기.");
            TurnOnLight();
        }
    }

    private void TurnOnLight()
    {
        if (Lanternlight != null)
        {
           Lanternlight.SetActive(true);
            isActivated = true;
        }
    }
}