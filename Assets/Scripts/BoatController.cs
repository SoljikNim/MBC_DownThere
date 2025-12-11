using UnityEngine;
using UnityEngine.Playables;
using System.Collections;
using System.Collections.Generic;

public class BoatController : MonoBehaviour
{
    [Header("Wave Simulation Settings")]
    public float waveHeight = 0.5f;
    public float waveSpeed = 1f;
    public float rockingAngle = 5f;

    [Header("Movement Settings")]
    public float forwardSpeed = 5f;
    public string playerTag = "Player";
    public string stopZoneTag = "StopZone";

    [Header("Player Interaction Settings")]
    [Tooltip("출발 시 비활성화하고, 도착 시 활성화할 플레이어의 이동 관련 컴포넌트들 (예: Locomotion System, Move Provider)")]
    public MonoBehaviour[] playerMovementComponents;

    [Header("Environmental FX Settings")]
    public float fogIncreaseDuration = 3f;
    public float targetFogDensity = 0.15f;
    public Material skyboxBefore;
    public Material skyboxAfter;
    public float skyboxBlendDuration = 3f;

    [Header("Timeline")]
    public PlayableDirector drowningTimeline;

    private Rigidbody rb;
    private Vector3 initialPosition;
    private bool isMoving = false;

    public GameObject[] boatBarriers;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = true;
    }

    void Start()
    {
        initialPosition = transform.position;

        if (skyboxBefore != null)
            RenderSettings.skybox = skyboxBefore;
    }

    void FixedUpdate()
    {
        if (isMoving)
            transform.position += transform.forward * forwardSpeed * Time.fixedDeltaTime;
    }

    void Update()
    {
        // 움직이는 중일 때도 파도 시뮬레이션은 계속됨 (필요에 따라 조건 추가 가능)
        SimulateWaves();
    }

    void SimulateWaves()
    {
        // 이동 중일 때는 Z축(전진) 위치가 계속 바뀌므로, Y축(높이)만 파동을 줍니다.
        // 현재 위치 기반으로 파도 계산
        float newY = initialPosition.y + Mathf.Sin(Time.time * waveSpeed) * waveHeight;

        // 이동 중이 아닐 때는 제자리에서 상하 운동, 이동 중일 때는 현재 X, Z 유지하면서 Y만 변경
        // 주의: 이동 로직(FixedUpdate)과 충돌하지 않도록 Y값만 덮어씌웁니다.
        Vector3 currentPos = transform.position;
        transform.position = new Vector3(currentPos.x, newY, currentPos.z);

        float rocking = Mathf.Sin(Time.time * waveSpeed * 0.7f) * rockingAngle;
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, rocking);
    }

    private void OnTriggerEnter(Collider other)
    {
        // 1. 플레이어 탑승 (출발하지 않음, 부모 설정만 함)
        if (other.CompareTag(playerTag))
        {
            other.transform.SetParent(this.transform);
            // StartMovement(); // <-- 자동 출발 로직 제거됨
        }
        // 2. 도착 지점 도달
        else if (other.CompareTag(stopZoneTag))
        {
            Debug.Log("Stop Zone triggered.");

            // 플레이어 이동 잠금 해제
            SetPlayerMovement(true);

            // 보트 감속 + 연출 시작
            StartCoroutine(BoatSlowDownSequence());

            if (drowningTimeline != null)
                drowningTimeline.Play();
        }
    }

    /// <summary>
    /// 핸들 스크립트(BoatHandle)에서 호출하는 함수입니다.
    /// </summary>
    public void TryStartBoat()
    {
        if (!isMoving)
        {
            StartMovement();
            // 보트 출발 시 플레이어 이동 잠금
            SetPlayerMovement(false);
        }
    }

    public void StartMovement()
    {
        isMoving = true;
        // 이동 시작 시점의 위치를 기준으로 파도 계산을 하기 위해 갱신할 수도 있음
        // initialPosition = transform.position; 
        Debug.Log("Boat Started Moving!");
    }

    private void SetPlayerMovement(bool enable)
    {
        if (playerMovementComponents != null)
        {
            foreach (var component in playerMovementComponents)
            {
                if (component != null) component.enabled = enable;
            }
        }
        Debug.Log($"Player Movement set to: {enable}");
    }

    IEnumerator BoatSlowDownSequence()
    {
        float duration = 3f;
        float startSpeed = forwardSpeed;
        float time = 0;

        // 1. 보트 감속
        while (time < duration)
        {
            time += Time.deltaTime;
            forwardSpeed = Mathf.Lerp(startSpeed, 0f, time / duration);
            yield return null;
        }
        forwardSpeed = 0;

        // 2. 안개 짙어짐
        StartCoroutine(FogIncrease());

        // 3. 스카이박스 서서히 변경
        StartCoroutine(SkyboxBlend());

        // 4. 콜라이더 비활성화
        if (boatBarriers != null)
        {
            foreach (GameObject b in boatBarriers)
                if (b != null) b.SetActive(false);
        }

        // 5. 모든 연출 끝날 때 타임라인 재생 (필요 시)
        // OnTriggerEnter에서 이미 재생했으므로 여기선 생략하거나, 추가 연출용으로 사용
        // yield return new WaitForSeconds(3f);
        // drowningTimeline?.Play();
    }

    IEnumerator FogIncrease()
    {
        float startFog = RenderSettings.fogDensity;
        float time = 0;

        while (time < fogIncreaseDuration)
        {
            time += Time.deltaTime;
            RenderSettings.fogDensity = Mathf.Lerp(startFog, targetFogDensity, time / fogIncreaseDuration);
            yield return null;
        }
    }

    IEnumerator SkyboxBlend()
    {
        if (skyboxBefore == null || skyboxAfter == null) yield break;

        float time = 0;
        while (time < skyboxBlendDuration)
        {
            time += Time.deltaTime;
            float t = time / skyboxBlendDuration;
            RenderSettings.skybox.Lerp(skyboxBefore, skyboxAfter, t);
            yield return null;
        }
        RenderSettings.skybox = skyboxAfter;
    }
}