using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Monster_Movement : MonoBehaviour
{
    public Animator anim;
    public NavMeshAgent agent;
    public float wanderSpeed = 3.0f;
    public float chaseSpeed = 6.0f;
    public float detectionRange = 5.0f;
    public float missRange = 10.0f;
    public float missTime = 5.0f;

    public Transform eyePos;
    public Transform target;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetWander()
    {
        target = null;
        anim.SetTrigger("Walk");
        agent.speed = wanderSpeed;
    }

    void SetChase()
    {
        anim.SetTrigger("Run");
        agent.speed = chaseSpeed;
    }

    IEnumerator Miss_OutOfSight()
    {
        while (target != null)
        {
            float distance = Vector3.Distance(transform.position, target.position);
            if (distance > missRange)
            {
                yield return new WaitForSeconds(missTime);
                distance = Vector3.Distance(transform.position, target.position);
                if (distance > missRange)
                {
                    SetWander();
                    yield break;
                }
            }
            
            if (CheckTarget_OutOfSight())
            {
                yield return new WaitForSeconds(missTime);
                if (CheckTarget_OutOfSight())
                {
                    SetWander();
                    yield break;
                } 
            }

            yield return null;
        }
    }

    // Inspector에서 검사할 레이어를 설정할 수 있도록 추가 (기본: 모든 레이어)
    public LayerMask obstructionMask = ~0;
    bool CheckTarget_OutOfSight()
    {
        if (eyePos != null && target != null)
        {
            RaycastHit hit;
            // Linecast가 어떤 콜라이더에 맞았는지 검사 (obstructionMask로 레이어 필터링)
            if (Physics.Linecast(eyePos.position, target.position, out hit, obstructionMask))
            {
                // 맞은 콜라이더가 타겟(또는 자식)이라면 장애물이 아닌 것으로 간주
                if (hit.collider != null && !(hit.collider.transform == target || hit.collider.transform.IsChildOf(target)))
                {
                    return true;
                }
            }
        }
        return false;
    }
}
