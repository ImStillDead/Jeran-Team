using UnityEngine;

public interface ISave
{
    void LoadGame();


    void SaveGame(GameData data);

    void SaveRun(GameData data);
}
    
