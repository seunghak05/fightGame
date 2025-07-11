using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    public bool isPlayer1 = true;         // 1P/2P 구분
    public Slider healthSlider;           // 체력 슬라이더 UI

    private void Start()
    {
        healthSlider.maxValue = 100f;
        healthSlider.value = isPlayer1 ? GameManager.Instance.Player1Health : GameManager.Instance.Player2Health;
    }

    private void Update()
    {
        // GameManager의 체력 값을 UI에 반영
        if (isPlayer1)
            healthSlider.value = GameManager.Instance.Player1Health;
        else
            healthSlider.value = GameManager.Instance.Player2Health;
    }

    // 데미지를 받을 때
    public void TakeDamage(float damage)
    {
        if (isPlayer1)
            GameManager.Instance.TakeDamageToPlayer1(damage);
        else
            GameManager.Instance.TakeDamageToPlayer2(damage);
    }

    // 죽음 처리 (GameManager에서 처리하므로 여기선 필요 없음)
}