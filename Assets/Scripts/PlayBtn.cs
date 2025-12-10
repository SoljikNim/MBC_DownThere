using UnityEngine;
// using UnityEngine.UI; // Button 컴포넌트가 UI Button이 아닌 3D XR Button으로 대체되었으므로 필요 없습니다.
// using System.Collections; // Coroutine을 사용하지 않으므로 필요 없습니다.

/// <summary>
/// VR 게임 시작 시 초기 상태를 제어하고, 3D 게임 오브젝트(네온 사인 등)와의 상호작용으로 게임을 시작합니다.
/// 1. 게임 시작 시 이동 관련 컴포넌트를 비활성화합니다. (시야는 자유로움)
/// 2. 타이틀 패널을 표시합니다.
/// 3. 3D 버튼 상호작용 시 타이틀 패널을 끄고 이동 관련 컴포넌트를 활성화하고, 네온 사인을 끕니다.
/// </summary>
public class PlayBtn : MonoBehaviour
{
    // --- Unity Inspector Settings ---

    [Header("UI & Panel Setup (Title Screen Setup)")]
    [Tooltip("The Canvas or Panel GameObject holding the entire title screen.")]
    public GameObject titlePanel;

    // UI Button은 3D 상호작용으로 대체되므로 제거합니다.
    // public Button playButton; 

    [Header("VR Control Components (VR Movement Control)")]
    [Tooltip("Assign movement components to disable at start and enable upon Play.")]
    public MonoBehaviour[] movementComponents;

   

    [Header("Neon Sign Control (Neon Sign Control)")]
    [Tooltip("All Renderer components of the neon sign pieces (0, 1, 2, 3).")]
    public Renderer[] neonSignRenderers;

    [Tooltip("The name of the Float/Int property in the Shader Graph controlling the emission/flicker. (e.g., _FlickerToggle)")]
    public string emissionControlPropertyName = "_FlickerToggle";
    // --- End of additional items ---

    // --- Internal Variables ---
    public static bool IsGameStarted { get; private set; } = false; // Default is false

    void Start()
    {
        // 1. Initial State: Disable VR player movement components (Movement locked)
        // 시야 제어는 완전히 해제합니다. SetPlayerViewTracking 호출 제거.
        SetPlayerMovement(false);

        // 2. Display the title screen
        if (titlePanel != null)
        {
            titlePanel.SetActive(true);
        }
    }
    
    public void OnPlayClicked()
    {
        if (IsGameStarted) return; // Prevent double start

        Debug.Log("3D Play Button Clicked. Starting Game...");
        IsGameStarted = true;

        // 1. Turn off the title screen
        if (titlePanel != null)
        {
            titlePanel.SetActive(false);
        }

        // 2. Enable VR player movement
        SetPlayerMovement(true);

        // 3. Turn off the Neon Sign power
        TurnOffNeonSign();

        // 시야 추적을 다시 활성화할 필요가 없습니다. (Start()에서 비활성화하지 않았기 때문)
    }

    /// <summary>
    /// Disables/Enables the assigned movement components.
    /// </summary>
    /// <param name="enable">true to enable, false to disable</param>
    private void SetPlayerMovement(bool enable)
    {
        foreach (var component in movementComponents)
        {
            if (component != null)
            {
                component.enabled = enable;
            }
        }
        Debug.Log($"Player Movement set to: {enable}");
    }
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
                Material neonMaterial = neonRenderer.material;

                if (neonMaterial.HasProperty(emissionControlPropertyName))
                {
                    // Set the emission control variable to 0 to turn off the neon piece.
                    neonMaterial.SetFloat(emissionControlPropertyName, 0f);
                }
                else
                {
                    Debug.LogError($"Shader property '{emissionControlPropertyName}' not found on the material of: {neonRenderer.gameObject.name}.");
                }
            }
        }
        Debug.Log("All Neon Sign components turned OFF successfully.");
    }


}