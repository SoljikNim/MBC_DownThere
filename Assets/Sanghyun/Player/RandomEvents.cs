using System.Collections;
using UnityEngine;

public class RandomEvents : MonoBehaviour
{
    public GameObject kraken;
    public Transform krakenPos;
    public float eventChance = 0.2f;
    public Vector2 eventTime = new Vector2(10, 20);

    private void Start()
    {
        StartCoroutine(EventCor());
    }

    IEnumerator EventCor()
    {
        while (true)
        {
            float randomTimer = Random.Range(eventTime[0], eventTime[1]);
            while (randomTimer > 0)
            {
                randomTimer -= Time.deltaTime;
                yield return null;
            }
            float randomChance = Random.Range(0, 1);
            if (randomChance <= eventChance)
            {
                KrakenEvent();
            }
            yield return null;
        }
    }

    void KrakenEvent()
    {
        Instantiate(kraken, krakenPos.position, krakenPos.rotation);
    }
}
