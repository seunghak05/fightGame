using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    public float maxHealth = 100f;    // 최대 체력
    public float currentHealth;       // 현재 체력
    public Slider healthSlider;       // 체력 슬라이더 UI

    void Start()
    {
        currentHealth = maxHealth;    // 초기 체력 설정
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
    }

    // 데미지를 받을 때
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        healthSlider.value = currentHealth;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // 죽음 처리
    private void Die()
    {
        Debug.Log("플레이어 사망!");
        // 사망 처리 (애니메이션, 게임 종료 등)
    }
}

