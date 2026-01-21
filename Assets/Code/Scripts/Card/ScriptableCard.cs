using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "CardData")]
public class ScriptableCard : ScriptableObject
{
    public string cardName;
    [field: TextArea] public string description;
    public int points;
    public Texture2D cardImage;

    public int costDuctTape;
    public int costGlue;
    public int costWire;
    public int costFabric;
    public int costPlastic;

    public CardType cardType;
    public Ability ability;

    public enum CardType
    {
        Mask,
        Arms,
        Torso,
        Pants,
        Shoes
    }
}
