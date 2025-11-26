using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Deck")]
public class ScriptableDeck : ScriptableObject
{
    [field: SerializeField] public List<ScriptableCard> CardsInDeck { get; private set; }
}
