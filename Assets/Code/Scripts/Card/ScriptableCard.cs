using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "CardData")]
public class ScriptableCard : ScriptableObject
{
    [field: SerializeField] public string cardName;
    [field: SerializeField, TextArea] public string description;
    [field: SerializeField] public int points;
    [field: SerializeField] public Texture2D cardImage;

    [field: SerializeField] public int costBlue;
    [field: SerializeField] public int costRed;
    [field: SerializeField] public int costGreen;
    [field: SerializeField] public int costWhite;
    [field: SerializeField] public int costBlack;
    
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
