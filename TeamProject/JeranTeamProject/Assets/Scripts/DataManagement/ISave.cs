using UnityEngine;

public interface ISave
{
    void Load(GameData data);

    void Save(ref GameData data);
}
    
