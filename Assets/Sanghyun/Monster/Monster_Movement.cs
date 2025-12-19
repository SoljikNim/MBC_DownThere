using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Monster_Movement : MonoBehaviour
{
    public Monster_FindTarget findTarget;
    public Monster1_JumpScare jumpScare;
    public Animator anim;
    public NavMeshAgent agent;
    public bool freezeMove = false;
    public float wanderRange = 10.0f;
    public float wanderSpeed = 3.0f;
    public float chaseSpeed = 6.0f;
    public float detectionRange = 5.0f;
    public float catchRange = 1.0f;
    public float missRange = 10.0f;
    public float missTime = 5.0f;

    public Transform eyePos;
    public Transform target;
    public Player_Main player;

    public AudioSource stunSfx;

    public Collider wanderBound;
    void Start()
    {
        findTarget = GetComponentInChildren<Monster_FindTarget>();
        jumpScare = GetComponent<Monster1_JumpScare>();
        SetWander();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Movement();
    }

    public void Movement()
    {
        if (freezeMove) return;
        if (target != null)
        {
            agent.SetDestination(target.position);
        }
        else
        {
            if (agent.hasPath)
            {
                //print("가는중");
            }
            else
            {
                Vector3 randomDestPos = Random.insideUnitSphere * wanderRange;
                if (wanderBound != null)
                    randomDestPos = GetPathInWanderBound();
                agent.SetDestination(transform.position + randomDestPos);
                //print("목표지점 설정");
            }
        }
    }
    public Vector3 GetPathInWanderBound()
    {
        // wanderBound의 AABB 내에서 랜덤 샘플을 시도하고,
        // 샘플이 콜라이더 내부인지 확인한 뒤 NavMesh 상의 위치로 스냅합니다.
        // 실패 시 fallback으로 기존 Random.insideUnitSphere * wanderRange 반환.
        if (wanderBound == null)
            return Random.insideUnitSphere * wanderRange;

        const int maxAttempts = 30;
        Bounds bounds = wanderBound.bounds;
        for (int i = 0; i < maxAttempts; i++)
        {
            Vector3 sample = new Vector3(
                Random.Range(bounds.min.x, bounds.max.x),
                Random.Range(bounds.min.y, bounds.max.y),
                Random.Range(bounds.min.z, bounds.max.z)
            );

            // sample이 콜라이더 내부인지 검사 (ClosestPoint가 sample과 같으면 내부)
            Vector3 closest = wanderBound.ClosestPoint(sample);
            if (Vector3.Distance(closest, sample) > 0.001f)
            {
                // 콜라이더 내부가 아님 -> 다음 시도
                continue;
            }

            // NavMesh 위 유효한 지점으로 스냅
            NavMeshHit hit;
            // 샘플한 지점에서 반경 2m 이내로 NavMesh를 찾음
            if (NavMesh.SamplePosition(sample, out hit, 2.0f, NavMesh.AllAreas))
            {
                return hit.position - transform.position;
            }
            else
            {
                // NavMesh에 가까운 지점이 없더라도, 지면 방향으로 raycast하여 높이를 맞추는 시도
                RaycastHit downHit;
                if (Physics.Raycast(sample + Vector3.up * 5f, Vector3.down, out downHit, 10f))
                {
                    Vector3 grounded = downHit.point;
                    // grounded가 콜라이더 내부인지 확인
                    Vector3 closestG = wanderBound.ClosestPoint(grounded);
                    if (Vector3.Distance(closestG, grounded) <= 0.001f)
                    {
                        if (NavMesh.SamplePosition(grounded, out hit, 2.0f, NavMesh.AllAreas))
                            return hit.position - transform.position;
                    }
                }
            }
        }

        // 모든 시도가 실패하면 fallback: wanderBound 중심으로 향하는 방향에 랜덤 범위 적용
        Vector3 fallbackDir = (bounds.center - transform.position).normalized;
        if (float.IsNaN(fallbackDir.x)) // 안전장치
            fallbackDir = Random.insideUnitSphere.normalized;

        return fallbackDir * Mathf.Min(wanderRange, Mathf.Max(bounds.extents.x, bounds.extents.z));
    }

    public void SetWander()
    {
        player.currentEnemy = null;
        agent.ResetPath();
        target = null;
        findTarget.target = null;
        player = null;
        findTarget.player_main = null;
        anim.SetTrigger("Walk");
        agent.speed = wanderSpeed;
    }

    public void SetChase()
    {
        anim.SetTrigger("Run");
        agent.speed = chaseSpeed;
    }

    public void Stun()
    {
        StartCoroutine(StunCor());
    }

    public float stunTime = 5f;
    IEnumerator StunCor()
    {
        freezeMove = true;
        agent.ResetPath();
        anim.SetTrigger("Stun");
        stunSfx.Play();
        yield return new WaitForSeconds(stunTime);
        freezeMove = false;
        if (target != null)
            SetChase();
        else
            SetWander();
    }

    public IEnumerator TryCatchTarget()
    {
        while (target != null)
        {
            float distance = Vector3.Distance(eyePos.position, target.position);
            if (distance <= catchRange)
            {
                if (player.isHide && !watchPlayerHiding)
                {
                    SetWander();
                    yield break;
                }
                freezeMove = true;
                agent.ResetPath();
                ActiveJumpScare();
                yield break;
            }
            yield return null;
        }
    }

    public bool watchPlayerHiding = false;
    public void PlayerHiding()
    {
        if (!CheckTarget_OutOfSight() && !freezeMove)
        {
            watchPlayerHiding = true;
        }
    }

    public void ActiveJumpScare()
    {
        if (!player.isHide)
        {
            jumpScare.StartJumpScare1();
        }
        else
        {
            if (watchPlayerHiding)
            {
                if (player.currentHideObject != null)
                {
                    transform.position = player.currentHideObject.MonsterPos.position;
                    transform.rotation = player.currentHideObject.MonsterPos.rotation;
                    jumpScare.StartJumpScare2();
                }
                else
                    jumpScare.StartJumpScare1();
            }
        }
    }

    public IEnumerator Miss_OutOfSight()
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
                print("시야 가림");
                yield return new WaitForSeconds(missTime);
                if (CheckTarget_OutOfSight())
                {
                    print("타켓 취소");
                    SetWander();
                    yield break;
                } 
            }

            yield return null;
        }
    }

    // Inspector에서 검사할 레이어를 설정할 수 있도록 추가 (기본: 모든 레이어)
    public LayerMask obstructionMask = ~0;
    public bool CheckTarget_OutOfSight()
    {
        if (eyePos != null && target != null)
        {
            RaycastHit hit;
            // Linecast가 어떤 콜라이더에 맞았는지 검사 (obstructionMask로 레이어 필터링)
            Debug.DrawLine(eyePos.position, target.position, Color.red, 0.1f);
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
