using UnityEngine;

public class ActionToken : MonoBehaviour
{
    public bool IsAvailable { get; private set; } = true;

    public void Consume()
    {
        IsAvailable = false;
    }

    public void Refresh()
    {
        IsAvailable = true;
    }
}
