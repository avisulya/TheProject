using UnityEngine;
 
[CreateAssetMenu(fileName = "FollowerData", menuName = "Followers/FollowerData")]
public class FollowerData : ScriptableObject
{
    public string followerName;
    public Sprite portrait;
    public int    loyalty;        // 0-100
    public int    age;
    public int    maxAge;
    public bool   isDead;
    public bool   isDissident;    // doubts the cult
    public float  hunger;         // 0-100, 100 = starving
    public float  happiness;      // 0-100
}