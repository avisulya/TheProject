using UnityEngine;
 
[CreateAssetMenu(fileName = "TarotData", menuName = "Progression/TarotData")]
public class TarotData : ScriptableObject
{
    public string id;
    public string cardName;
    public Sprite cardArt;
    [TextArea]
    public string description;
    public TarotEffect effect;
    public float  value;   // how much to apply (damage %, speed %, etc.)
}