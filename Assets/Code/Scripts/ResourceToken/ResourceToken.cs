using System.Net;
using UnityEngine;

public class ResourceToken : MonoBehaviour
{
    public static readonly Color Grey = Color.grey;
    public static readonly Color Blue = Color.blue;
    public static readonly Color Copper = new Color(184f / 255f, 115f / 255f, 51f / 255f);
    public static readonly Color Green = Color.green;
    public static readonly Color Red = Color.red;

    public static readonly Color GlitterColor;

    public enum ResourceTokenType
    {
        DuctTape,
        Glue,
        Wire,
        Fabric,
        Plastic,
        Glitter
    }
    public ResourceTokenType resourceTokenType;

    public Color resourceTokenColor;

    private void Awake()
    {
        resourceTokenColor = GetColorForType(resourceTokenType);
    }

    public static Color GetColorForType(ResourceTokenType type)
    {
        return type switch
        {
            ResourceTokenType.DuctTape => new Color(Grey.r, Grey.g, Grey.b),
            ResourceTokenType.Glue => new Color(Blue.r, Blue.g, Blue.b),
            ResourceTokenType.Wire => new Color(Copper.r, Copper.g, Copper.b),
            ResourceTokenType.Fabric => new Color(Green.r, Green.g, Green.b),
            ResourceTokenType.Plastic => new Color(Red.r, Red.g, Red.b),
            ResourceTokenType.Glitter => GlitterColor,
            _ => Color.magenta,
        };
    }
}
