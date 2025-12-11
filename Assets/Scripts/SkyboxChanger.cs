using UnityEngine;
using UnityEngine.Playables; // 타임라인 제어를 위해 필수

public class SkyboxChanger : MonoBehaviour
{
    [Header("Timeline Settings")]
    [Tooltip("실행할 타임라인 디렉터 컴포넌트를 연결하세요.")]
    public PlayableDirector timelineToPlay;

    [Tooltip("이 높이 아래로 내려가면 타임라인이 실행됩니다.")]
    public float triggerHeight = 0f; // 기존 waterLevel 역할

    [Tooltip("체크하면 타임라인을 한 번만 실행하고 더 이상 작동하지 않습니다.")]
    public bool playOnce = true;

    private Camera mainCamera;
    private bool hasPlayed = false;

    void Start()
    {
        // 메인 카메라 컴포넌트 가져오기
        mainCamera = Camera.main;

        if (timelineToPlay == null)
        {
            Debug.LogWarning("SkyboxChanger: Timeline Director is not assigned!");
        }
    }

    void Update()
    {
        if (mainCamera == null) return;

        // 이미 재생했고, 한 번만 재생하기로 설정되어 있다면 검사 중단
        if (playOnce && hasPlayed) return;

        // 플레이어(카메라)의 Y 위치가 기준 높이보다 아래인지 확인
        if (mainCamera.transform.position.y < triggerHeight)
        {
            // 타임라인이 할당되어 있고, 아직 재생 중이 아니라면 (또는 조건에 따라) 실행
            if (timelineToPlay != null)
            {
                // 타임라인이 멈춰있는 상태에서만 실행 (중복 실행 방지)
                if (timelineToPlay.state != PlayState.Playing)
                {
                    timelineToPlay.Play();
                    Debug.Log("Specific height reached. Playing Timeline.");

                    hasPlayed = true;

                    // 한 번만 실행하는 경우, 불필요한 연산을 줄이기 위해 스크립트 비활성화
                    if (playOnce)
                    {
                        this.enabled = false;
                    }
                }
            }
        }
    }
}