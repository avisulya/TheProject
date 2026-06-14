using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
 
public class TarotCardWidget : MonoBehaviour
{
    [SerializeField] private Image              cardArt;
    [SerializeField] private TextMeshProUGUI    cardName;
    [SerializeField] private TextMeshProUGUI    cardDesc;
    [SerializeField] private Button             selectButton;
 
    private TarotData       _data;
    private Action<TarotData> _onSelected;
 
    public void Initialise(TarotData data, Action<TarotData> onSelected)
    {
        _data        = data;
        _onSelected  = onSelected;
        cardArt.sprite = data.cardArt;
        cardName.text  = data.cardName;
        cardDesc.text  = data.description;
        selectButton.onClick.AddListener(() => _onSelected?.Invoke(_data));
    }
}