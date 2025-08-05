using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UnitAI : MonoBehaviour
{
    /// <summary>
    /// unitbase와 함께 유닛들의 행동 로직을 정해주는 스크립트입니다.
    /// 소환된 유닛이 앞으로 이동하고, 적을 공격하고, 상대 기지를 공격하는 행동의 우선순위를 정해줍니다.
    /// </summary>

    public enum UnitState//유닛의 행동 상태를 정하기 위한 enum
    {
        Moving,
        Attacking,
        Idle,
        Dead,
    }


    private Animator animator;//0이 승리 모션, 1이 이동, 2가 공격, 3번이 죽는 모션입니다.
    private UnitBase unit;
    private UnitState currentState = UnitState.Moving;//소환되면 전진하니까 무빙을 기본으로
    private float attackTimer = 0f;
    private bool canAttack = true;

    [SerializeField] private LayerMask targetLayer;

    private Transform target;

    private void Awake()
    {
        unit = GetComponent<UnitBase>();
        animator = GetComponent<Animator>();
    }


    // Update is called once per frame
    void Update()
    {

        switch (currentState)
        {
            case UnitState.Moving:
                //animator.SetInteger("State", 1);
                animator.SetBool("Run", true);
                MoveForward();
                LookForTarget();
                break;

            case UnitState.Attacking:
                if (canAttack)
                {
                    Debug.Log("공격 루틴 실행");
                    StartCoroutine(AttackRoutine());
                }
                //animator.SetInteger("State", 2);
                //animator.SetTrigger("Attack");
                Attack();
                break;

            case UnitState.Idle:
                animator.SetBool("Run", false);
                //animator.SetInteger("State", 0);
                break;

            case UnitState.Dead:
                animator.SetTrigger("Die");
                //animator.SetInteger("State", 3);
                break;
        }


    }

    IEnumerator AttackRoutine()
    {
        canAttack = false;
        animator.SetTrigger("Attack");

        // 실제 데미지는 애니메이션 이벤트로 타이밍 맞춰 호출됨
        yield return new WaitForSeconds(unit.entry.UnitData.attackCooldown);

        canAttack = true;
    }


    private void MoveForward()
    {
        //보는 방향 전환을 위한 로컬 스케일
        Vector3 scale = transform.localScale;
        if (unit.entry.UnitData.teamType == UnitTeamType.Ally)
        {
            transform.localScale = new Vector3(-Mathf.Abs(scale.x), scale.y, scale.z);
        }
        Vector3 dir = unit.entry.UnitData.teamType == UnitTeamType.Ally ? Vector3.right : Vector3.left;
        transform.Translate(dir * unit.entry.UnitData.moveSpeed * (1 + unit.entry.UnitUpgrade) * Time.deltaTime);
    }

    private void LookForTarget()
    {
        // 범위 내 적 유닛 또는 기지 감지
        Collider2D targetCollider = Physics2D.OverlapCircle(transform.position, unit.entry.UnitData.attackRange + (1 + unit.entry.UnitUpgrade) * 0.1f, targetLayer);
        if (targetCollider != null)
        {
            //Debug.Log($"상대 감지 대상 : {targetCollider.name}");
            target = targetCollider.transform;
            currentState = UnitState.Attacking;
            //Debug.Log($"상태 변경 완료 : 현재 상태 {currentState.ToString()}");
            attackTimer = 0;
        }
    }

    private void Attack()
    {
        if (target == null)
        {
            Debug.Log("대상을 못 찾았습니다. 이동합니다.");
            currentState = UnitState.Moving;
            return;
        }

        attackTimer += Time.deltaTime;
        if (attackTimer >= unit.entry.UnitData.attackCooldown)
        {
            //var targetHealth = target.GetComponent<UnitBase>();
            //if (targetHealth != null)
            //{
            //    targetHealth.TakeDamage(unit.data.attackPower);
            //}
            if (unit.entry.UnitData.targetType == AttackTargetType.SingleTarget)
            {
                Debug.Log("대상 발견 단일 공격을 시행합니다.");
                AttackSingleTarget();
                SoundManager.Instance.PlaySound(SoundType.SFX, "SFX_Attack_2", false);
                Collider2D targetCollider = Physics2D.OverlapCircle(transform.position, unit.entry.UnitData.attackRange + (1 + unit.entry.UnitUpgrade) * 0.1f, targetLayer);
                if (targetCollider == null)
                {
                    target = null;
                }
            }
            else if (unit.entry.UnitData.targetType == AttackTargetType.AreaOfEffect)
            {
                Debug.Log("대상 발견 광역 공격을 시행합니다.");
                AttackMultipleTargets();
                Collider2D targetCollider = Physics2D.OverlapCircle(transform.position, unit.entry.UnitData.attackRange + (1 + unit.entry.UnitUpgrade) * 0.1f, targetLayer);
                if (targetCollider == null)
                {
                    target = null;
                }
            }

            attackTimer = 0;
        }

        // 타겟이 죽었는지 체크
        if (target.TryGetComponent<UnitBase>(out var unitBase) && unitBase.entry.IsDead() == true)
        {

            target = null;
            Debug.Log("대상이 죽었습니다. 이동합니다.");
            currentState = UnitState.Moving;

        }
    }

    public void Die()
    {
        Debug.Log($"{unit.name}이 죽었습니다.");
        currentState = UnitState.Dead;
        this.enabled = false;
    }


    private void AttackSingleTarget()
    {
        if (target == null) return;

        if (target.TryGetComponent<UnitBase>(out var targetUnit))
        {
            targetUnit.TakeDamage(this.unit.entry.UnitData.attackPower * (1 + this.unit.entry.UnitUpgrade));
        }
        else if (target.TryGetComponent<Player>(out var player))
        {
            player.status.TakeDamage(this.unit.entry.UnitData.attackPower * (1 + this.unit.entry.UnitUpgrade));
        }
        // 공격 이펙트 출력
        if (unit.entry.UnitData.attackEffectPrefab != null)
        {
            Instantiate(unit.entry.UnitData.attackEffectPrefab, target.position, Quaternion.identity);
        }
    }

    private void AttackMultipleTargets()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, unit.entry.UnitData.attackRange + (1 + unit.entry.UnitUpgrade) * 0.1f, targetLayer);

        // 공격 이펙트 출력하기 위한 스크립트
        var targetUnit = target.GetComponent<UnitBase>();
        if (targetUnit != null)
        {

            if (unit.entry.UnitData.attackEffectPrefab != null)
            {
                Instantiate(unit.entry.UnitData.attackEffectPrefab, target.position, Quaternion.identity);
            }
        }
        // 범위 공격에 닿은 적들 데미지 계산하기 위한 스크립트
        foreach (var hit in hits)
        {
            var enemy = hit.GetComponent<UnitBase>();
            if (enemy != null && unit.entry.UnitData.teamType != this.unit.entry.UnitData.teamType)
            {
                enemy.TakeDamage(this.unit.entry.UnitData.attackPower * (1 + this.unit.entry.UnitUpgrade));
            }
        }

    }

    public void SetIdle()
    {
        currentState = UnitState.Idle;
    }
}

