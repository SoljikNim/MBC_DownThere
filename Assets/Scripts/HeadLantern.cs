using UnityEngine;

/// <summary>
/// 플레이어의 헤드랜턴 활성화를 제어하는 통합 스크립트입니다.
/// 이 스크립트는 필드에 배치된 픽업 아이템 오브젝트에 직접 붙어야 합니다.
/// 1. 3D 버튼 클릭 시 헤드랜턴을 켜고 버튼을 비활성화합니다. (외부 호출)
/// 2. 필드에 있는 픽업 아이템과 플레이어 머리의 지정된 Collider가 접촉하면 
///    아이템을 파괴하고 헤드랜턴을 켭니다. (OnTriggerEnter)
/// </summary>
public class HeadLantern : MonoBehaviour
{
    [Header("Player Lantern Setup (플레이어 헤드랜턴)")]
    [Tooltip("실제로 플레이어 머리 위치에 있는 헤드랜턴 GameObject (라이트 및 메쉬 포함)")]
    public GameObject playerHeadLantern;

    [Header("Pickup Item Settings (픽업 발동 조건)")]
    [Tooltip("플레이어 머리쪽에 있는, 픽업을 발동시킬 특정 Collider (Sphere Collider Trigger 등)")]
    public Collider playerHeadActivationCollider; // 인스펙터에서 지정할 플레이어의 Collider

    private bool isActivated = false;

    void Start()
    {
        // 1. 초기 상태: 헤드랜턴 비활성화
        if (playerHeadLantern != null && playerHeadLantern.activeSelf)
        {
            playerHeadLantern.SetActive(false);
            Debug.Log("Head Lantern initialized to OFF state.");
        }

        // 2. 이 오브젝트에 RigidBody가 있는지 확인 (물리 충돌/트리거 수신 필수 조건)
        if (GetComponent<Rigidbody>() == null)
        {
            Debug.LogWarning($"Rigidbody missing on {gameObject.name}. Adding one now. Remember to freeze rotation/position if needed.");
            Rigidbody rb = gameObject.AddComponent<Rigidbody>();
            rb.isKinematic = false; // 기본적으로 물리 적용
        }

        // 3. 플레이어 활성화 콜라이더 설정 확인 (픽업 기능 필수)
        if (playerHeadActivationCollider == null)
        {
            Debug.LogError("Player Head Activation Collider is not assigned! Pickup function will not work. Please drag the player's Head Collider into the Inspector slot.");
        }
    }

    //-------------------------------------------------------------
    // 1. 3D 버튼 상호작용 방식 (LanternTriggerButton 역할)
    //-------------------------------------------------------------

    /// <summary>
    /// XR Push Button의 'On Click' 이벤트에 연결될 함수입니다. (버튼 작동용)
    /// </summary>
    public void ActivateLanternFromButton(GameObject buttonObject)
    {
        if (isActivated) return;

        if (playerHeadLantern == null)
        {
            Debug.LogError("Player Head Lantern is not assigned in the Inspector!");
            return;
        }

        // 버튼을 통한 활성화 시도 로그
        Debug.Log($"Attempting to activate lantern via button: {buttonObject.name}");

        PerformActivationLogic();

        // 2. 버튼 오브젝트 비활성화 (튜토리얼 버튼은 한 번만 작동하도록)
        if (buttonObject != null)
        {
            buttonObject.SetActive(false);
            Debug.Log($"Button object '{buttonObject.name}' deactivated.");
        }
    }


    //-------------------------------------------------------------
    // 2. 픽업 아이템 접촉 방식 (LanternPickup 역할)
    //-------------------------------------------------------------

    /// <summary>
    /// 픽업 아이템(이 스크립트가 붙은 오브젝트)의 OnTriggerEnter 함수입니다.
    /// 플레이어 머리의 지정된 Collider와 접촉하면 발동합니다.
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        // 디버깅: 어떤 콜라이더가 접촉했는지 확인
        Debug.Log($"Trigger entered by: {other.gameObject.name}. Target collider: {playerHeadActivationCollider?.name ?? "NULL"}");

        // 이미 활성화되었거나, 지정된 플레이어 머리 콜라이더가 아니면 무시
        if (isActivated)
        {
            Debug.Log("Lantern already activated, ignoring trigger.");
            return;
        }

        if (other != playerHeadActivationCollider)
        {
            Debug.Log("Not the designated head collider, ignoring.");
            return;
        }

        // 지정된 플레이어 머리 콜라이더와 접촉한 경우
        Debug.Log("Lantern contacted with the designated Player Head Collider. Activating player lantern.");

        PerformActivationLogic();

        // 3. 필드에 있는 3D 랜턴 아이템 파괴
        // **주의: Destroy(gameObject)가 성공적으로 완료되어야 합니다.**
        Destroy(gameObject);
    }

    //-------------------------------------------------------------
    // 공통 로직
    //-------------------------------------------------------------

    private void PerformActivationLogic()
    {
        if (isActivated) return;

        // 1. 헤드랜턴 활성화 (불 켜기)
        // **핵심: 이 오브젝트가 실제로 Light 컴포넌트를 켜는 오브젝트인지 확인하세요.**
        if (playerHeadLantern != null)
        {
            playerHeadLantern.SetActive(true);
            isActivated = true;
            Debug.Log("Head Lantern activated successfully.");
        }
        else
        {
            Debug.LogError("playerHeadLantern is NULL. Activation failed.");
        }
    }
}