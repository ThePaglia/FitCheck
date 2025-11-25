using UnityEngine;
using UnityEngine.Rendering;

public class Card : MonoBehaviour
{
    public GameManager gm;
    public int points;

    // TODO: change to fabric type, for now it's splendor colors
    public int costBlue;
    public int costRed;
    public int costGreen;
    public int costWhite;
    public int costBlack;

    public enum CardType
    {
        Head,
        Arms,
        Torso,
        Legs,
        Shoes
    }
    public string cardName;
    public string description;

    public Ability ability;
    public CardType cardType;

    public Card(int costBlue, int costRed, int costGreen, int costWhite, int costBlack, int points, string cardName, string description, Ability ability, CardType cardType)
    {
        this.costBlue = costBlue;
        this.costRed = costRed;
        this.costGreen = costGreen;
        this.costWhite = costWhite;
        this.costBlack = costBlack;
        this.points = points;
        this.cardName = cardName;
        this.description = description;
        this.ability = ability;
        this.cardType = cardType;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gm = FindFirstObjectByType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
