using System.Collections.Generic;
using UnityEngine;

public class ResourceTokenUI : MonoBehaviour
{
    private Transform resourceTokenContainer;

    void Awake()
    {
        resourceTokenContainer = GetComponent<Transform>();
    }

    public void UpdateResourceTokenUI(List<ResourceToken> resourceTokens)
    {
        // TODO: Just update the numbers on the UI without the complicated animation since it's not really needed (the tokens are all the same except for the type that also determines its color/texture)
        // It just needs to reparent the tokens to the UI and to disable everything 3D-wise
        // TODO: Metti a posto la UI su unity della hand e update qua
        foreach (ResourceToken resourceToken in resourceTokens)
        {
            // Disable 3D rendering
            MeshRenderer meshRenderer = resourceToken.GetComponent<MeshRenderer>();
            if (meshRenderer != null) meshRenderer.enabled = false;

            Collider collider = resourceToken.GetComponent<Collider>();
            if (collider != null) collider.enabled = false;

            // Reparent to resource token container (Canvas)
            RectTransform tokenRect = resourceToken.GetComponent<RectTransform>();
            if (tokenRect != null) tokenRect.SetParent(resourceTokenContainer);
        }
    }
}
