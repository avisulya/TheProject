using System.Collections.Generic;
using UnityEngine;
 
public class TarotManager : MonoBehaviour
{
    [SerializeField] private RunState           runState;
    [SerializeField] private List<TarotData>    allTarots;
    [SerializeField] private TarotOfferUI       tarotOfferUI;
 
    // Call this when the player reaches a treasure room
    public void OfferTarotChoice()
    {
        var shuffled = new List<TarotData>(allTarots);
        shuffled.Sort((a, b) => Random.Range(-1, 2));
 
        var offer = new List<TarotData>();
        for (int i = 0; i < Mathf.Min(3, shuffled.Count); i++)
            offer.Add(shuffled[i]);
 
        tarotOfferUI.Show(offer, OnTarotChosen);
    }
 
    private void OnTarotChosen(TarotData chosen)
    {
        runState.activeTarotIds.Add(chosen.id);
        ApplyTarot(chosen);
    }
 
    private void ApplyTarot(TarotData t)
    {
        switch (t.effect)
        {
            case TarotEffect.IncreaseDamage:
                runState.damageMultiplier += t.value; break;
            case TarotEffect.IncreaseSpeed:
                runState.speedMultiplier  += t.value; break;
            case TarotEffect.IncreaseMaxHealth:
                runState.extraHearts      += Mathf.RoundToInt(t.value); break;
        }
    }
}