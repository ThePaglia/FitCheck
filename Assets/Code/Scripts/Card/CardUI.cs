using UnityEngine;

public class CardUI : MonoBehaviour
{
    private Card card;
    [Header("Prefab Elements")] //references to objects in the card prefab
    [SerializeField] private MeshRenderer cardRenderer;
    [SerializeField] private int materialIndex = 0;

    private void Awake()
    {
        card = GetComponent<Card>();
        if (cardRenderer == null)
        {
            cardRenderer = GetComponent<MeshRenderer>();
        }
        SetCardUI();
    }

    private void SetCardUI()
    {
        if (card != null && card.CardData != null)
        {
            SetCardMaterial();
        }
    }

    private void SetCardMaterial()
    {
        if (card.CardData.cardImage == null) return;

        // Get materials array
        Material[] sharedMaterials = cardRenderer.sharedMaterials;

        if (materialIndex < sharedMaterials.Length)
        {
            sharedMaterials[materialIndex] = new Material(sharedMaterials[materialIndex]);
            sharedMaterials[materialIndex].mainTexture = card.CardData.cardImage;
            cardRenderer.materials = sharedMaterials;
        }
        else
        {
            Debug.LogWarning("Material index is out of range of the materials array.");
        }
    }
}
