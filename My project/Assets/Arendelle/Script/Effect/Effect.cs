using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect
{
    public CellAccType type;

   

    public virtual void Attack(CellEnemy at,CellAcc beat)//����ײʱ����,������ʱ��������
    {
            beat.CurHP -= at.Idleattack;
    }




}
