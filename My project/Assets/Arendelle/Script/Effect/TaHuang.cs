using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


    public class TaHuang:Effect
    {
    public override void Attack(CellEnemy at, CellAcc beat)
    {
            if ((beat.cellKind == CellKind.TaHuang && beat.isMove) || (beat.cellKind == CellKind.HuoLongGuo && beat.isMove))
                return;
            beat.CurHP -= at.attack;
    }

}
