using UnityEngine;

/// <summary>
/// 씬에 있는 모든 아이템의 머테리얼에 플레이어의 위치를 전달하여 
/// 거리 기반의 아웃라인 강도(Emission)를 조절하는 스크립트입니다.
/// 이 스크립트는 플레이어(카메라)에 부착되어야 합니다.
/// </summary>
public class ItemOutlineController : MonoBehaviour
{
    
    // 쉐이더 그래프에서 PlayerPosition 속성의 'Reference' 이름과 정확히 일치해야 합니다.
    private const string PlayerPositionPropertyName = "_PlayerPosition";

    void Update()
    {
        // Global Property로 플레이어 위치를 전달합니다.
        Shader.SetGlobalVector(PlayerPositionPropertyName, transform.position);
    }
}