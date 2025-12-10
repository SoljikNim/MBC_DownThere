using MikeNspired.XRIStarterKit;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class FlashlightSlider : MonoBehaviour
{
    public Light flashlight;      // 스포트라이트
    public XRSlider slider;       // XR Slider 컴포넌트

    private void OnEnable()
    {
        slider.OnValueChange.AddListener(OnSliderValueChanged);
    }

    private void OnDisable()
    {
        slider.OnValueChange.RemoveListener(OnSliderValueChanged);
    }

    public void OnSliderValueChanged(float value)
    {
        // value 범위가 MinPosition ~ MaxPosition이므로
        // 0~1이 아님에 주의해야 한다.

        // 예: Min = -0.5, Max = 0.5 이면
        // 가운데가 0, 오른쪽 끝이 0.5

        // 원하는 기준으로 다시 매핑하면 좋음.
        // 예: 0 이상일 때 켜짐
        flashlight.enabled = value > 0f;
    }
}