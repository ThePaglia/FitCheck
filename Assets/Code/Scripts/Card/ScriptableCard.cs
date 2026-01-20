using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "CardData")]
public class ScriptableCard : ScriptableObject
{
    [field: SerializeField] public string cardName;
    [field: SerializeField, TextArea] public string description;
    [field: SerializeField] public int points;
    [field: SerializeField] public Texture2D cardImage;

    [field: SerializeField] public int costDuctTape;
    [field: SerializeField] public int costGlue;
    [field: SerializeField] public int costWire;
    [field: SerializeField] public int costFabric;
    [field: SerializeField] public int costPlastic;
    
    [field: SerializeField] public CardType cardType;
    [field: SerializeField] public Ability ability;

    public enum CardType
    {
        Mask,
        Arms,
        Torso,
        Pants,
        Shoes
    }
}
