using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
 
public class TarotOfferUI : MonoBehaviour
{
    [SerializeField] private GameObject      panel;
    [SerializeField] private TarotCardWidget cardWidgetPrefab;
    [SerializeField] private Transform       cardContainer;
 
    private Action<TarotData> _onChosen;
 
    public void Show(List<TarotData> cards, Action<TarotData> onChosen)
    {
        _onChosen = onChosen;
        panel.SetActive(true);
 
        foreach (Transform child in cardContainer) Destroy(child.gameObject);
 
        foreach (var card in cards)
        {
            var widget = Instantiate(cardWidgetPrefab, cardContainer);
            widget.Initialise(card, OnCardClicked);
        }
    }
 
    private void OnCardClicked(TarotData chosen)
    {
        panel.SetActive(false);
        _onChosen?.Invoke(chosen);
    }
}