using UnityEngine;
using UnityEngine.UI;

public class GameUIController : MonoBehaviour
{
    public Slider healthSlider;
    public Text defenseCountText;
    public Image parryTimingBar;

    private void Update()
    {
        // 체력 슬라이더 업데이트
        healthSlider.value = GameManager.Instance.PlayerHealth;

        // 방어 횟수 업데이트
        defenseCountText.text = "방어 횟수: " + GameManager.Instance.DefenseCount;

        // 패링 타이밍 바 업데이트 (예: 패링 준비 중일 때 표시)
        parryTimingBar.fillAmount = GameManager.Instance.ParryProgress;
    }
}

