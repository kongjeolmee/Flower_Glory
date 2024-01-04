using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Monster Data", fileName = "MonsterData")]
public class MonsterData : ScriptableObject
{
    public int hp = 3;
    public int tileAtk = 1;
    public int playerAtk = 1;
    //public float atkSpd;
    public float atkDelBefore = 2;
    public float atkDelAfter = 5;
    public float speed;
    public float attackRange;
    public float knockbackRange;
    public Monster.TargetType targetType;
    public Monster.MoveType moveType;
    public AnimatorOverrideController overrideController;

}
