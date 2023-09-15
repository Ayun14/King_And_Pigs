using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Zombie Data", menuName = "Scriptable Object/Zombie Data", order = int.MaxValue)]
public class ZombieData : ScriptableObject // 상속받기
{
    [SerializeField]
    private string zombieName;
    public string ZombieName { get { return zombieName; } }
    [SerializeField]
    private int hp;
    public int Hp { get { return hp; } }
    [SerializeField]
    private int damage;
    public int Damage { get { return damage; } }
    [SerializeField]
    private float sightRange;
    public float SightRange { get { return sightRange; } }
    [SerializeField]
    private float moveSpeed;
    public float MoveSpeed { get { return moveSpeed; } }
}
