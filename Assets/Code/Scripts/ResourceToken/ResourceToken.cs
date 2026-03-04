using UnityEngine;

public class ResourceToken : MonoBehaviour
{
    public static Color Red = Color.red;
    public static Color Pink = Color.hotPink;
    public static Color Blue = Color.blue;
    public static Color Grey = Color.grey;
    public static Color Black = Color.black;

    public static Color GlitterColor;

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
            ResourceTokenType.DuctTape => Red,
            ResourceTokenType.Glue => Pink,
            ResourceTokenType.Wire => Blue,
            ResourceTokenType.Fabric => Grey,
            ResourceTokenType.Plastic => Black,
            ResourceTokenType.Glitter => GlitterColor,
            _ => Color.magenta,
        };
    }
}
