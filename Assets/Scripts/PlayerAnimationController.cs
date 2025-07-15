// ------------------------------
// ✅ 상태 기반 애니메이션 제어 템플릿 (커스텀 가능 + 주석 포함)
// ------------------------------

using UnityEngine;

// 상태 정의 열거형 (원하는 상태 자유롭게 추가 가능)
public enum CharacterState
{
    Idle,
    Run,
    Jump,
    Fall,
    Guard,
    Attack
}

// ------------------------------
// 🎮 캐릭터 동작 및 입력 처리 클래스
// ------------------------------
[RequireComponent(typeof(Rigidbody2D))]
public class CharacterController2D : MonoBehaviour
{
    private Rigidbody2D rigid;

    [Header("입력 키 설정")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode crouchKey = KeyCode.C;
    public KeyCode attackKey = KeyCode.J;
    public KeyCode guardKey = KeyCode.K;

    private bool isGrounded = false;
    private bool isCrouching = false;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // 숙이기 토글 처리
        if (Input.GetKeyDown(crouchKey))
            isCrouching = !isCrouching;
    }

    // 외부에서 접근할 상태 값들
    public bool IsJumping => !isGrounded && rigid.linearVelocity.y > 0.1f;
    public bool IsFalling => rigid.linearVelocity.y < -0.1f;
    public bool IsRunning => Mathf.Abs(rigid.linearVelocity.x) > 0.1f;
    public bool IsCrouching => isCrouching;
    public bool IsAttacking => Input.GetKeyDown(attackKey);
    public bool IsGuarding => Input.GetKey(guardKey);

    // 착지 판정 처리 (바닥 태그 필요)
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground"))
            isGrounded = true;
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground"))
            isGrounded = false;
    }
}

// ------------------------------
// 🎞️ 애니메이션 상태 제어 클래스 (Animator와 연동)
// ------------------------------
[RequireComponent(typeof(Animator), typeof(CharacterController2D))]
public class CharacterAnimationController : MonoBehaviour
{
    private Animator animator;
    private CharacterController2D controller;

    private CharacterState currentState;

    void Awake()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController2D>();
    }

    void Update()
    {
        DetermineState();
        UpdateAnimatorParameters();
    }

    // 상태 판별 함수: 우선순위에 따라 조건 검사
    private void DetermineState()
    {
        if (controller.IsAttacking)
            ChangeState(CharacterState.Attack);
        else if (controller.IsGuarding)
            ChangeState(CharacterState.Guard);
        else if (controller.IsJumping)
            ChangeState(CharacterState.Jump);
        else if (controller.IsFalling)
            ChangeState(CharacterState.Fall);
        else if (controller.IsRunning)
            ChangeState(CharacterState.Run);
        else
            ChangeState(CharacterState.Idle);
    }

    // 애니메이터 파라미터 적용 함수
    private void UpdateAnimatorParameters()
    {
        animator.SetBool("isRunning", currentState == CharacterState.Run);
        animator.SetBool("isJumping", currentState == CharacterState.Jump);
        animator.SetBool("isCrouching", controller.IsCrouching);
        animator.SetBool("isGuarding", currentState == CharacterState.Guard);

        // Trigger는 반복 호출 방지용 체크 필요
        if (currentState == CharacterState.Attack)
        {
            animator.ResetTrigger("Attack");
            animator.SetTrigger("Attack");
        }
    }

    // 상태 변경 함수
    private void ChangeState(CharacterState newState)
    {
        if (currentState != newState)
        {
            currentState = newState;
            // 상태 변경 이벤트 처리 가능 (예: 사운드 재생 등)
        }
    }
}

// ------------------------------
// 📘 애니메이터 파라미터 설정 가이드
// ------------------------------
// Parameters (Animator 창에서 생성 필요):
// - isRunning (bool)
// - isJumping (bool)
// - isCrouching (bool)
// - isGuarding (bool)
// - Attack (Trigger)

// 상태 트랜지션 예시:
// - AnyState → Attack : Attack Trigger
// - Idle → Run : isRunning == true
// - Run → Idle : isRunning == false
// - Jump → Fall : y < 0 or Exit Time
// - Run/Idle → Jump : isJumping == true

// 🔧 이 구조는 캐릭터 커스터마이징에 유리하며,
// 상태 추가나 AI 연동에도 확장 가능
