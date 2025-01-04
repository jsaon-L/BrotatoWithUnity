using UnityEngine;

public sealed class EntityID 
{
   
    private static int _id = 0;
    public static int GetID 
    { 
        get { return ++_id; } 
    }

    public static void ResetID()
    {
        _id = 0;
    }
}
