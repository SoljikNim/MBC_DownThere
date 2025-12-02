using UnityEngine;

public class SkyboxChanger : MonoBehaviour
{
    // 인스펙터에서 할당할 지상 및 수중 스카이박스 Material
    public Material surfaceSkybox;
    public Material deepSeaSkybox;
    public float waterLevel = 0f; // 물의 높이 기준선

    private Camera mainCamera;

    void Start()
    {
        // 메인 카메라 컴포넌트 가져오기
        mainCamera = Camera.main;

        // 시작 시 지상 스카이박스로 설정
        if (mainCamera != null && surfaceSkybox != null)
        {
            RenderSettings.skybox = surfaceSkybox;
            // 카메라의 클리어 플래그가 Skybox로 설정되어 있는지 확인
            mainCamera.clearFlags = CameraClearFlags.Skybox;
        }
    }

    void Update()
    {
        // 플레이어의 Y 위치(또는 메인 카메라의 Y 위치)를 확인
        if (mainCamera.transform.position.y < waterLevel)
        {
            // 물속으로 들어감: 심해 스카이박스로 교체
            if (RenderSettings.skybox != deepSeaSkybox)
            {
                RenderSettings.skybox = deepSeaSkybox;
            }
        }
        else
        {
            // 물 위로 나옴: 지상 스카이박스로 교체
            if (RenderSettings.skybox != surfaceSkybox)
            {
                RenderSettings.skybox = surfaceSkybox;
            }
        }
    }
}