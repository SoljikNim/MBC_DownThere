using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InterfaceManager : MonoBehaviour
{
    public GameObject visible;
    public GameObject whiteOut;
    public AudioSource hologramVisible;

    public Vector2 defaultHeartRate = new Vector2(90,110);
    public Vector2 chaseHeartRate = new Vector2(120, 140);

    public TextMeshProUGUI heartRateText;
    public Image staminaImg;
    public Image batteryImage;
    public TextMeshProUGUI batteryText;

    public Transform currentEnemyPos;

    WaitForSeconds default_heartRateWait = new WaitForSeconds(0.8f);
    WaitForSeconds chase_heartRateWait = new WaitForSeconds(0.4f);

    public Player_Main palyerMain;
    private void OnEnable()
    {
        SetVisible(false);
        StartCoroutine(HeartRateCor());
        SetStamina(1);
        SetBattery(1);
    }

    public void SetVisible(bool _bool)
    {
        whiteOut.SetActive(true);
        Invoke(nameof(DeactiveWhite), 0.05f);
        visible.SetActive(_bool);
        if (_bool)
            hologramVisible.Play();
    }

    void DeactiveWhite()
    {
        whiteOut.SetActive(false);
    }

    public void SetEnemy(Transform _enemyPos)
    {
        currentEnemyPos = _enemyPos;
    }
    public void SetStamina(float _per)
    {
        staminaImg.fillAmount = _per;
    }

    public void SetBattery(float _per)
    {
        batteryImage.fillAmount = _per;
        batteryText.text = $"{_per*100:000}%";
    }

    IEnumerator HeartRateCor()
    {
        while (true)
        {
            float targetRate = Random.Range(defaultHeartRate.x, defaultHeartRate.y);
            if (currentEnemyPos != null)
                targetRate = Random.Range(chaseHeartRate.x, chaseHeartRate.y);

            heartRateText.text = $"{targetRate:000}";

            if (currentEnemyPos == null)
                yield return default_heartRateWait;
            else
                yield return chase_heartRateWait;
        }
    }
}
