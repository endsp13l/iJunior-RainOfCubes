using UnityEngine;

public class BombSpawner : Spawner<Bomb>
{
    public void Spawn(Transform target)
    {
        GameObject bomb = Pool.Get();
        bomb.transform.position = target.position;
    }
}