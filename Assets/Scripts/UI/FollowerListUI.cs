using TMPro;
using UnityEngine;
using UnityEngine.UI;
 
public class FollowerListUI : MonoBehaviour
{
    [SerializeField] private FollowerManager followerManager;
    [SerializeField] private Transform       listContainer;
    [SerializeField] private GameObject      followerRowPrefab;
 
    public void RefreshList()
    {
        foreach (Transform child in listContainer) Destroy(child.gameObject);
 
        foreach (var f in followerManager.GetAll())
        {
            var row = Instantiate(followerRowPrefab, listContainer);
 
            // Assumes row has two TextMeshProUGUI children: name and status
            var texts = row.GetComponentsInChildren<TextMeshProUGUI>();
            if (texts.Length >= 2)
            {
                texts[0].text = f.Data.followerName;
                texts[1].text = $"Loyalty: {f.Data.loyalty}  Hunger: {f.Data.hunger:0}";
            }
 
            var portrait = row.GetComponentInChildren<Image>();
            if (portrait != null && f.Data.portrait != null)
                portrait.sprite = f.Data.portrait;
        }
    }
}