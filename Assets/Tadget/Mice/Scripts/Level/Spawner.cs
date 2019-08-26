using UnityEngine;

public class Spawner
{
    private int levelId;

    public Spawner()
    {
        this.levelId = -1;
    }

    public void SetLevelId(int id)
    {
        this.levelId = id;
    }

    public void Spawn()
    {
        Debug.Log("Spawning objects for level " + levelId);
    }

    public void Despawn()
    {
        Debug.Log("Despawning objects");
    }
}