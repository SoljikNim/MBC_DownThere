using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit; // XR Grab Interactable 사용을 위해 추가

/// <summary>
/// 보트 핸들에 부착하여 손이 닿았을 때 보트를 출발시키는 스크립트입니다.
/// (XR Grab Interactable 이벤트 기반으로 양손 모두 잡았을 때 출발)
/// </summary>
public class BoatHandle : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("보트를 제어하는 메인 BoatController 스크립트")]
    public BoatController boatController;

    [Header("XR Grab Interactables")]
    [Tooltip("왼쪽 손잡이의 XR Grab Interactable 컴포넌트")]
    public UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable leftHandleGrab;
    [Tooltip("오른쪽 손잡이의 XR Grab Interactable 컴포넌트")]
    public UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable rightHandleGrab;

    private bool isLeftGrabbed = false;
    private bool isRightGrabbed = false;

    void Start()
    {
        // 1. 할당 여부 확인
        if (leftHandleGrab == null || rightHandleGrab == null)
        {
            Debug.LogError("Both Left and Right Handle Grab Interactables must be assigned in the Inspector.");
            return;
        }

        // 2. 이벤트 리스너 구독 설정
        // 왼쪽 손잡이 이벤트
        leftHandleGrab.selectEntered.AddListener(OnSelectEnteredLeft);
        leftHandleGrab.selectExited.AddListener(OnSelectExitedLeft);

        // 오른쪽 손잡이 이벤트
        rightHandleGrab.selectEntered.AddListener(OnSelectEnteredRight);
        rightHandleGrab.selectExited.AddListener(OnSelectExitedRight);
    }

    // 왼쪽 손잡이 이벤트 처리
    private void OnSelectEnteredLeft(SelectEnterEventArgs args)
    {
        isLeftGrabbed = true;
        CheckDualGrab();
    }

    private void OnSelectExitedLeft(SelectExitEventArgs args)
    {
        isLeftGrabbed = false;
    }

    // 오른쪽 손잡이 이벤트 처리
    private void OnSelectEnteredRight(SelectEnterEventArgs args)
    {
        isRightGrabbed = true;
        CheckDualGrab();
    }

    private void OnSelectExitedRight(SelectExitEventArgs args)
    {
        isRightGrabbed = false;
    }

    /// <summary>
    /// 양손 모두 잡았는지 확인하고 보트를 출발시킵니다.
    /// </summary>
    private void CheckDualGrab()
    {
        // 양손 모두 잡혔을 때만 출발
        if (isLeftGrabbed && isRightGrabbed)
        {
            Debug.Log("Dual Grab detected! Starting boat.");
            if (boatController != null)
            {
                boatController.TryStartBoat();

                // 보트가 출발했으므로 더 이상 이벤트 처리가 필요 없도록 리스너를 제거합니다.
                UnsubscribeEvents();
            }
            else
            {
                Debug.LogError("BoatController is not assigned on the BoatHandle script!");
            }
        }
    }

    /// <summary>
    /// 기능이 완료된 후 이벤트 리스너를 해제합니다.
    /// </summary>
    private void UnsubscribeEvents()
    {
        if (leftHandleGrab != null)
        {
            leftHandleGrab.selectEntered.RemoveListener(OnSelectEnteredLeft);
            leftHandleGrab.selectExited.RemoveListener(OnSelectExitedLeft);
        }
        if (rightHandleGrab != null)
        {
            rightHandleGrab.selectEntered.RemoveListener(OnSelectEnteredRight);
            rightHandleGrab.selectExited.RemoveListener(OnSelectExitedRight);
        }
    }

    // 기존 OnTriggerEnter 및 태그 변수는 XR Interaction Toolkit을 사용하므로 제거되었습니다.
}