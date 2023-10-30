using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect
{
    public CellAccType type;

   

    public virtual void Attack(CellEnemy at,CellAcc beat)//被碰撞时触发,被攻击时发动攻击
    {
            beat.CurHP -= at.Idleattack;
    }




}
