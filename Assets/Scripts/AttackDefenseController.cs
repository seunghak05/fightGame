using UnityEngine;

public class AttackDefenseController : MonoBehaviour
{
    public float attackCooldown = 0.5f;  // 공격 쿨타임
    private bool canAttack = true;        // 공격 가능 여부
    public float damage = 10f;            // 공격 데미지

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();  // 애니메이터 컴포넌트 할당
    }

    void Update()
    {
        if (Input.GetButtonDown("Attack") && canAttack)  // 공격 버튼 눌렀을 때
        {
            PerformAttack();
        }

        if (Input.GetButtonDown("Defend"))  // 방어 버튼 눌렀을 때
        {
            PerformDefense();
        }
    }

    // 공격 수행
    private void PerformAttack()
    {
        animator.SetTrigger("Attack");  // 애니메이션 트리거
        canAttack = false;  // 공격 후 쿨타임
        Invoke("ResetAttack", attackCooldown);  // 일정 시간 후 공격 가능

        // 공격 판정 코드 (예: 다른 플레이어와 충돌 시 데미지 처리)
    }

    // 방어 수행
    private void PerformDefense()
    {
        animator.SetTrigger("Defend");  // 방어 애니메이션 트리거
        // 방어 기능 추가 (패링 시스템 포함)
    }

    // 공격 가능 상태로 복귀
    private void ResetAttack()
    {
        canAttack = true;
    }
}

