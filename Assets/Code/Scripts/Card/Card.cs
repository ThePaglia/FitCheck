using UnityEngine;
[RequireComponent(typeof(CardUI))]
public class Card : MonoBehaviour
{
    [field: SerializeField] public ScriptableCard CardData { get; private set; }

    public void SetUp(ScriptableCard data)
    {
        CardData = data;
        GetComponent<CardUI>().SetCardUI();
    }
}
