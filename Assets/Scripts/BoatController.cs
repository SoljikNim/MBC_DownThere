using UnityEngine;

// 이 스크립트를 보트 오브젝트에 부착하세요.
// 보트 오브젝트에는 Rigidbody 컴포넌트와 Is Trigger가 체크된 Collider가 필요합니다.
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class BoatController : MonoBehaviour
{
    // === 파도 시뮬레이션 설정 ===
    [Header("Wave Simulation Settings")]
    public float waveHeight = 0.5f;     // 출렁임 최대 높이 (파고)
    public float waveSpeed = 1f;        // 출렁임 속도 (주기)
    public float rockingAngle = 5f;     // 좌우 기울임 최대 각도

    // === 전진 및 탑승 설정 ===
    [Header("Movement & Trigger Settings")]
    public float forwardSpeed = 5f;     // 보트 전진 속도
    public string playerTag = "Player"; // 플레이어(XR Origin)의 태그

    private Rigidbody rb;
    private Vector3 initialPosition;    // 보트의 초기 Y 위치를 저장
    private bool isMoving = false;      // 보트의 전진 상태
    [Header("Stop Zone Settings")]
    public string stopZoneTag = "StopZone"; // 보트가 멈춰야 할 영역의 태그
    // 플레이어 오브젝트를 직접 연결하는 대신, 태그를 사용하거나 다른 방식으로 플레이어를 식별해야 합니다.
    // 여기서는 "Player" 태그를 사용하도록 가정합니다.
    // **도착 시 비활성화할 콜라이더/벽 오브젝트 리스트**
    // 유니티 인스펙터 창에서 여기에 콜라이더 벽 오브젝트들을 드래그하여 연결해야 합니다.
    public GameObject[] boatBarriers;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        // Kinematic으로 설정하여 물리 엔진이 아닌 스크립트로 직접 움직임을 제어하도록 합니다.
        // Rigidbody는 여전히 Trigger 감지와 AddForce 처리에 사용됩니다.
        rb.isKinematic = true;

        // 보트가 회전하는 것을 막기 위해 회전 제한을 설정할 수도 있습니다.
        // rb.freezeRotation = true; 
    }

    void Start()
    {
        // 시작 시 보트의 초기 위치를 저장합니다.
        // 여기서부터 출렁임이 시작됩니다.
        initialPosition = transform.position;
    }

    // 물리 기반 업데이트 (전진 처리에 사용)
    void FixedUpdate()
    {
        // 보트가 움직임 상태일 때만 전진합니다.
        if (isMoving)
        {
            // Rigidbody를 직접 움직이지 않고 Kinematic으로 설정했을 경우:
            transform.position += transform.forward * forwardSpeed * Time.fixedDeltaTime;
        }
    }

    // 일반 업데이트 (출렁임 및 회전 처리에 사용)
    void Update()
    {
        // 파도 시뮬레이션 (출렁임)
        SimulateWaves();
    }

    // === 파도 시뮬레이션 함수 ===
    void SimulateWaves()
    {
        // 1. 수직 출렁거림 (Y축) 계산
        // 시간에 따른 사인파 함수를 이용해 주기적인 상하 움직임 계산
        float newY = initialPosition.y + Mathf.Sin(Time.time * waveSpeed) * waveHeight;

        // 보트의 위치 업데이트 (X, Z는 그대로 유지, Y만 출렁임)
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);

        // 2. 좌우 기울어짐 (Roll) 계산
        // 출렁임 주기와 약간 다르게 설정하여 비동기적인 움직임 연출
        float rocking = Mathf.Sin(Time.time * waveSpeed * 0.7f) * rockingAngle;

        // 보트의 회전 업데이트 (Y축(Yaw)은 전진 방향 유지, X축(Pitch)와 Z축(Roll) 기울임)
        // 여기서는 X축과 Z축 중 하나를 선택하여 기울임을 적용합니다. (배의 좌우 롤링이므로 주로 Z축 또는 X축)
        // Z축에 적용: 좌우 롤링
        // transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, rocking);

        // X축에 적용: 앞뒤 피칭과 Z축 롤링을 결합할 수도 있지만, 단순 롤링만 하려면 Z축이 더 자연스럽습니다.
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, rocking);
    }


    // === 탑승 감지 (Trigger) 함수 ===
    // 플레이어가 보트의 콜라이더 영역에 진입했을 때 호출됩니다.
    private void OnTriggerEnter(Collider other)
    {
        // 1. 플레이어 탑승 감지
        if (other.CompareTag(playerTag))
        {
            Debug.Log("플레이어 탑승 감지. 자동 전진 시작.");

            // 플레이어를 보트의 자식으로 설정
            other.transform.SetParent(this.transform);

            // 움직임 시작
            StartMovement();
        }
        // 2. 정지 영역 진입 감지
        else if (other.CompareTag(stopZoneTag))
        {
            Debug.Log("정지 영역 진입 감지. 보트 자동 정지.");

            // 움직임 정지
            StopMovement();

            // (선택 사항) 정지 후 플레이어의 움직임을 보트와 분리하려면 이 코드를 추가합니다.
            // 다른 오브젝트의 transform.parent를 null로 설정합니다.
             if (transform.childCount > 0)
            {
                transform.GetChild(0).SetParent(null);
             }
        }
    }

    // 탑승 상태를 해제해야 한다면 OnTriggerExit을 구현할 수도 있습니다.

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            other.transform.SetParent(null); // 자식 설정 해제
           StopMovement(); // 필요 시 움직임 정지 함수 호출
        }
    }
    

    // === 움직임 시작/정지 제어 ===
    public void StartMovement()
    {
        isMoving = true;
        // 보트의 초기 위치를 다시 설정하여 움직임 시작 시 끊김이 없도록 합니다.
        initialPosition = transform.position;
    }

    public void StopMovement()
    {
        isMoving = false;
        // 정지 시 속도를 0으로 만들어 멈춥니다. (Kinematic일 경우 필요 없을 수 있습니다.)
        // **콜라이더 벽 비활성화 로직**
        if (boatBarriers != null)
        {
            foreach (GameObject barrier in boatBarriers)
            {
                if (barrier != null)
                {
                    // 벽 오브젝트를 비활성화 (씬에서 보이지 않게 하고 상호작용도 막음)
                    barrier.SetActive(false);
                    Debug.Log($"콜라이더 벽 {barrier.name} 비활성화 완료.");
                }
            }
        }
    }
}