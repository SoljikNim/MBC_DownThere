using UnityEngine;

/// <summary>
/// 보트 핸들에 부착하여 손이 닿았을 때 보트를 출발시키는 스크립트입니다.
/// </summary>
public class BoatHandle : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("보트를 제어하는 메인 BoatController 스크립트")]
    public BoatController boatController;

    [Tooltip("왼손 태그 이름")]
    public string leftHandTag = "LeftHand";
    [Tooltip("오른손 태그 이름")]
    public string rightHandTag = "RightHand";

    private void OnTriggerEnter(Collider other)
    {
        // 손이 닿았는지 확인
        if (other.CompareTag(leftHandTag) || other.CompareTag(rightHandTag))
        {
            Debug.Log($"Hand ({other.tag}) touched the handle!");

            if (boatController != null)
            {
                // 보트 출발 신호 보내기
                boatController.TryStartBoat();
            }
            else
            {
                Debug.LogError("BoatController is not assigned on the BoatHandle script!");
            }
        }
    }
}