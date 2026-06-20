// Assets/Scripts/UI/HUD.cs
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [SerializeField] private Image[]         heartImages;
    [SerializeField] private Sprite          heartFull;
    [SerializeField] private Sprite          heartHalf;
    [SerializeField] private Sprite          heartEmpty;
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private TextMeshProUGUI levelText;

    private void OnEnable()
    {
        GameEvents.OnPlayerHealthChanged += UpdateHearts;
        GameEvents.OnResourceChanged     += OnResourceChanged;
        GameEvents.OnLevelUp             += OnLevelUp;
    }

    private void OnDisable()
    {
        GameEvents.OnPlayerHealthChanged -= UpdateHearts;
        GameEvents.OnResourceChanged     -= OnResourceChanged;
        GameEvents.OnLevelUp             -= OnLevelUp;
    }

    // current and max are in HALF-HEART units
    private void UpdateHearts(int current, int max)
    {
        int maxHearts = max / 2;   // how many heart icons to show at all

        for (int i = 0; i < heartImages.Length; i++)
        {
            if (i >= maxHearts) { heartImages[i].gameObject.SetActive(false); continue; }
            heartImages[i].gameObject.SetActive(true);

            int heartFloor = i * 2;          // half-points this heart icon represents
            int remaining  = current - heartFloor;

            if (remaining >= 2)      heartImages[i].sprite = heartFull;
            else if (remaining == 1) heartImages[i].sprite = heartHalf;
            else                     heartImages[i].sprite = heartEmpty;
        }
    }

    private void OnResourceChanged(ResourceType type, int value)
    {
        if (type == ResourceType.Gold) goldText.text = $"Gold: {value}";
    }

    private void OnLevelUp(int level) => levelText.text = $"Lv {level}";
}