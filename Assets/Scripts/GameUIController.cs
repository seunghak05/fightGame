using UnityEngine;
using UnityEngine.UI;

public class GameUIController : MonoBehaviour
{
    public Slider player1HealthSlider;
    public Slider player2HealthSlider;
    public Text defenseCountText;
    public Image parryTimingBar;

    private void Update()
    {
        // 1P, 2P 체력 슬라이더 업데이트
        player1HealthSlider.value = GameManager.Instance.Player1Health;
        player2HealthSlider.value = GameManager.Instance.Player2Health;

        // 방어 횟수 업데이트
        defenseCountText.text = "방어 횟수: " + GameManager.Instance.DefenseCount;

        // 패링 타이밍 바 업데이트
        parryTimingBar.fillAmount = GameManager.Instance.ParryProgress;
    }
}