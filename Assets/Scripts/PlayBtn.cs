using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// VR 게임 시작 시 초기 상태를 제어하고, Play 버튼 클릭으로 게임을 시작합니다.
/// 1. 게임 시작 시 이동 관련 컴포넌트를 비활성화합니다.
/// 2. 타이틀 패널을 표시하고 버튼 깜빡임을 시작합니다.
/// 3. 버튼 클릭 시 타이틀 패널을 끄고 이동 관련 컴포넌트를 활성화하고, 네온 사인을 끕니다.
/// </summary>
public class PlayBtn : MonoBehaviour
{
    // --- 유니티 인스펙터 설정 항목 ---

    [Header("UI & Panel Setup (타이틀 화면 설정)")]
    [Tooltip("타이틀 화면 전체를 담고 있는 Canvas 또는 Panel GameObject")]
    public GameObject titlePanel;

    [Tooltip("클릭할 'Play' 버튼 컴포넌트")]
    public Button playButton;

    [Header("VR Control Components (VR 이동 제어)")]
    [Tooltip("게임 시작 시 비활성화하고, Play 시 활성화할 이동 관련 컴포넌트들을 할당하세요.")]
    public MonoBehaviour[] movementComponents;

    // --- 네온 사인 제어 추가 항목 ---
    [Header("Neon Sign Control (네온 사인 제어)")]
    [Tooltip("네온 사인 조각(0, 1, 2, 3)들의 모든 Renderer 컴포넌트를 할당하세요.")]
    public Renderer[] neonSignRenderers; // 단일 Renderer에서 배열로 변경!

    [Tooltip("쉐이더 그래프에서 발광/깜빡임을 제어하는 Float/Int 속성의 이름. (예: _FlickerToggle)")]
    // 이 이름은 쉐이더 그래프에서 설정한 이름과 정확히 일치해야 합니다!
    public string emissionControlPropertyName = "_FlickerToggle";
    // --- 추가 항목 끝 ---

    // --- 내부 변수 ---
    private Coroutine blinkCoroutine;
    public static bool IsGameStarted { get; private set; } = false; // 기본값은 false

    void Start()
    {
        // 1. 초기 상태: VR 플레이어 이동 관련 컴포넌트 비활성화 (이동 잠금)
        SetPlayerMovement(false);

        // 2. 타이틀 화면 표시
        if (titlePanel != null)
        {
            titlePanel.SetActive(true);
        }

        // 3. 버튼에 클릭 이벤트 리스너 연결 및 깜빡임 시작
        if (playButton != null)
        {
            // 버튼 클릭 시 OnPlayClicked 함수가 호출되도록 연결
            playButton.onClick.AddListener(OnPlayClicked);
        }
     }

    /// <summary>
    /// Play 버튼 클릭 시 호출되는 함수입니다.
    /// </summary>
    public void OnPlayClicked()
    {
        Debug.Log("Play Button Clicked. Starting Game...");
    // 2. 타이틀 화면 끄기
        if (titlePanel != null)
        {
            titlePanel.SetActive(false);
        }

        // 3. VR 플레이어 이동 가능하게 설정
        SetPlayerMovement(true);

        // 4. 네온 사인 전원 끄기 (추가된 로직)
        TurnOffNeonSign();
    }

    /// <summary>
    /// 네온 사인 쉐이더의 발광을 완전히 끕니다.
    /// </summary>
    private void TurnOffNeonSign()
    {
        if (neonSignRenderers.Length == 0)
        {
            Debug.LogWarning("No Neon Sign Renderers are assigned in the PlayBtn script.");
            return;
        }

        foreach (Renderer neonRenderer in neonSignRenderers)
        {
            if (neonRenderer != null)
            {
                // material에 접근하면 새로운 Material 인스턴스가 생성되어 개별 제어가 가능합니다.
                Material neonMaterial = neonRenderer.material;

                if (neonMaterial.HasProperty(emissionControlPropertyName))
                {
                    // Emission 제어 변수를 0으로 설정하여 해당 네온 조각을 끕니다.
                    neonMaterial.SetFloat(emissionControlPropertyName, 0f);
                }
                else
                {
                    // 에러 메시지는 한 번만 출력되도록 처리할 수도 있지만,
                    // 디버깅을 위해 어느 Renderer에서 문제가 생겼는지 보여주는 것이 좋습니다.
                    Debug.LogError($"Shader property '{emissionControlPropertyName}' not found on the material of: {neonRenderer.gameObject.name}.");
                }
            }
        }
        Debug.Log("All Neon Sign components turned OFF successfully.");
    }
/// <summary>
/// 할당된 이동 관련 컴포넌트들을 활성화/비활성화합니다.
/// </summary>
/// <param name="enable">true면 활성화, false면 비활성화</param>
private void SetPlayerMovement(bool enable)
    {
        foreach (var component in movementComponents)
        {
            if (component != null)
            {
                component.enabled = enable;
            }
        }
    }
  
}