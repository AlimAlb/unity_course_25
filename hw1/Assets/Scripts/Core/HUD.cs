using UnityEngine;
using TMPro;

public class HUD : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI speedText;

    private void Start()
    {
        if (player != null)
            player.OnHealthChanged += UpdateHP;

        UpdateHP(player.HP, player.MaxHP);
    }

    private void OnDestroy()
    {
        if (player != null)
            player.OnHealthChanged -= UpdateHP;
    }

    private void Update()
    {
        if (scoreText != null)
            scoreText.text = $"Score: {GameManager.I.Score}";

        if (speedText != null)
            speedText.text = $"x{GameManager.I.SpeedMultiplier:0.00}";
    }

    private void UpdateHP(int current, int max)
    {
        if (hpText != null)
            hpText.text = $"HP: {current}/{max}";
    }
}