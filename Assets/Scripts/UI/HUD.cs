using TMPro;
using UnityEngine;
using UnityEngine.UI;
 
public class HUD : MonoBehaviour
{
    [SerializeField] private Image[]   heartImages;    // array of heart sprites
    [SerializeField] private Sprite    heartFull;
    [SerializeField] private Sprite    heartEmpty;
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private TextMeshProUGUI xpText;
 
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
 
    private void UpdateHearts(int current, int max)
    {
        for (int i = 0; i < heartImages.Length; i++)
        {
            if (i >= max) { heartImages[i].gameObject.SetActive(false); continue; }
            heartImages[i].gameObject.SetActive(true);
            heartImages[i].sprite = i < current ? heartFull : heartEmpty;
        }
    }
 
    private void OnResourceChanged(ResourceType type, int value)
    {
        if (type == ResourceType.Gold)
            goldText.text = $"Gold: {value}";
    }
 
    private void OnLevelUp(int level)
    {
        xpText.text = $"Lv {level}";
    }
}