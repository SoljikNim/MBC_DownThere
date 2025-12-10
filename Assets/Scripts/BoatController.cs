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

    [Header("Environmental FX Settings")]
    public float fogIncreaseDuration = 3f;                 // 안개 짙어지는 시간
    public float targetFogDensity = 0.15f;                 // 최종 안개 농도
    public Material skyboxBefore;                          // 기존 스카이박스
    public Material skyboxAfter;                           // 변경될 스카이박스
    public float skyboxBlendDuration = 3f;                 // 스카이박스 전환 시간

    [Header("Timeline")]
    public PlayableDirector drowningTimeline;              // 물에 빠지는 타임라인

    private Rigidbody rb;
    private Vector3 initialPosition;
    private bool isMoving = false;

    public GameObject[] boatBarriers;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }

    void Start()
    {
        initialPosition = transform.position;

        // 스카이박스 초기값 설정
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
        SimulateWaves();
    }

    void SimulateWaves()
    {
        float newY = initialPosition.y + Mathf.Sin(Time.time * waveSpeed) * waveHeight;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);

        float rocking = Mathf.Sin(Time.time * waveSpeed * 0.7f) * rockingAngle;
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, rocking);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            other.transform.SetParent(this.transform);
            StartMovement();
        }
        else if (other.CompareTag(stopZoneTag))
        {
            Debug.Log("Stop Zone triggered: starting cinematic slow down.");

            StartCoroutine(BoatSlowDownSequence());
        }
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
                b?.SetActive(false);
        }

        // 5. 모든 연출 끝날 때 타임라인 재생
        yield return new WaitForSeconds(3f);
        drowningTimeline?.Play();
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

            // 스카이박스를 서서히 교체
            RenderSettings.skybox.Lerp(skyboxBefore, skyboxAfter, t);

            yield return null;
        }
        RenderSettings.skybox = skyboxAfter;
    }

    public void StartMovement()
    {
        isMoving = true;
        initialPosition = transform.position;
    }
}