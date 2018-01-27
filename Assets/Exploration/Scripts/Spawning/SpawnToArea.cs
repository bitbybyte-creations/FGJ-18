using UnityEngine;
using System.Collections;

public class SpawnToArea : SpawnAtLocation
{
    public int spawnCount = 1;
    public int maxXPosition;
    public int maxYPosition;

    protected override void startSpawn()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            int x = UnityEngine.Random.Range(xPosition, maxXPosition);
            int y = UnityEngine.Random.Range(yPosition, maxYPosition);
            Spawn(x, y);
        }
    }



}
