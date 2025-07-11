using UnityEngine;

public class ParrySystem : MonoBehaviour
{
    public GameObject parryTimingBar;  // 패링 타이밍 표시 바
    private float parryWindowTime = 0.2f;  // 패링 타이밍 윈도우
    private bool isParrying = false;
    private float parryTimer;

    void Update()
    {
        if (isParrying)
        {
            parryTimer += Time.deltaTime;
            if (parryTimer >= parryWindowTime)
            {
                // 패링 실패 처리
                ResetParry();
            }
        }
    }

    // 패링을 시작
    public void StartParry()
    {
        isParrying = true;
        parryTimer = 0f;
        parryTimingBar.SetActive(true);  // 타이밍 바 활성화
    }

    // 패링을 성공시키기
    public void PerformParry()
    {
        if (parryTimer <= parryWindowTime)
        {
            // 패링 성공 처리
            Debug.Log("패링 성공!");
            ResetParry();
        }
    }

    // 패링을 리셋
    private void ResetParry()
    {
        isParrying = false;
        parryTimingBar.SetActive(false);  // 타이밍 바 비활성화
    }
}

