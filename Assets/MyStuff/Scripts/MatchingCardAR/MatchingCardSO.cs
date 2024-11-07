using UnityEngine;


[CreateAssetMenu(fileName = "matchingCard", menuName = "SOs/matchingCard")]
public class MatchingCardSO : ScriptableObject
{
    public new string name;
    public string[] descriptions;
    public int id;
    public Color col;
    public Sprite img;

    public string getText()
    {
        int n = descriptions.Length;
        return descriptions[Random.Range(0, n)];
    }
}
