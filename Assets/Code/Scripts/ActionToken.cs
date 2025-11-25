using UnityEngine;

public class ActionToken : MonoBehaviour
{
    public bool isAvailable = true;
    
    public void Consume()
    {
        isAvailable = false;
    }

    public void Refresh()
    {
        isAvailable = true;
    }
}
