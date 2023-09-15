using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ZombieType
{
    Normal, Power, Sensor, Speed, Tank
}
public class ZombieSpawner : MonoBehaviour
{
    [SerializeField]
    private List<ZombieData> zombieDatas;
    [SerializeField]
    private GameObject zombiePrefab;

    void Start()
    {
        for (int i = 0; i < zombieDatas.Count; i++)
        {
            var zombie = SpawnZombie((ZombieType)i);
            zombie.PrintZombieData();
        }
    }

    public Zombie SpawnZombie(ZombieType type)
    {
        var newZombie = Instantiate(zombiePrefab).GetComponent<Zombie>();
        newZombie.zombieData = zombieDatas[(int)type];
        return newZombie;
    }

}
