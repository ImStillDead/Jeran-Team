using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] Image fillImage;

    int maxHealth;

    public void Setup(int maxHP)
    {
        maxHealth = maxHP;
        UpdateHealth(maxHP);
    }

    public void UpdateHealth(int currentHP)
    {
        fillImage.fillAmount = (float)currentHP / maxHealth;
    }
}