using UnityEngine;

public class ParrySystem : MonoBehaviour
{
    public GameObject parryTimingBar;  // 패링 타이밍 표시 바
    public bool isPlayer1 = true;      // 1P/2P 구분

    private float parryWindowTime = 0.2f;  // 패링 타이밍 윈도우
    private bool isParrying = false;
    private float parryTimer;

    void Update()
    {
        if (isParrying)
        {
            parryTimer += Time.deltaTime;
            // 패링 진행도 GameManager에 전달
            float progress = Mathf.Clamp01(parryTimer / parryWindowTime);
            GameManager.Instance.UpdateParryProgress(progress);

            if (parryTimer >= parryWindowTime)
            {
                // 패링 실패 처리
                ResetParry();
            }
        }
        else
        {
            // 패링 중이 아닐 때 진행도 0으로 초기화
            GameManager.Instance.UpdateParryProgress(0f);
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
        GameManager.Instance.UpdateParryProgress(0f); // 진행도 초기화
    }
}