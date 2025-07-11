using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool IsGameOver { get; private set; } = false;

    public float Player1Health = 100f;
    public float Player2Health = 100f;
    public int DefenseCount = 0;
    public float ParryProgress = 0f;  // 패링 준비 상태 (0f ~ 1f)

    public enum Winner { None, Player1, Player2 }
    public Winner GameWinner { get; private set; } = Winner.None;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    // 1P 데미지
    public void TakeDamageToPlayer1(float damage)
    {
        if (IsGameOver) return;
        Player1Health -= damage;
        if (Player1Health < 0) Player1Health = 0;
        if (Player1Health == 0)
        {
            GameWinner = Winner.Player2;
            GameOver();
        }
    }

    // 2P 데미지
    public void TakeDamageToPlayer2(float damage)
    {
        if (IsGameOver) return;
        Player2Health -= damage;
        if (Player2Health < 0) Player2Health = 0;
        if (Player2Health == 0)
        {
            GameWinner = Winner.Player1;
            GameOver();
        }
    }

    // 방어 횟수 증가
    public void IncreaseDefenseCount()
    {
        DefenseCount++;
    }

    // 패링 진행 상태 업데이트
    public void UpdateParryProgress(float progress)
    {
        ParryProgress = Mathf.Clamp01(progress);
    }

    // 1P 체력 회복
    public void HealPlayer1(float healAmount)
    {
        Player1Health += healAmount;
        if (Player1Health > 100f) Player1Health = 100f;
    }

    // 2P 체력 회복
    public void HealPlayer2(float healAmount)
    {
        Player2Health += healAmount;
        if (Player2Health > 100f) Player2Health = 100f;
    }

    // 게임오버 처리
    public void GameOver()
    {
        if (IsGameOver) return;
        IsGameOver = true;
        Debug.Log("게임 오버! 승자: " + GameWinner);
        // TODO: 게임 오버 UI 표시, 애니메이션, 재시작/메인화면 이동 등 추가
    }
}