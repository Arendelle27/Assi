using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class AccZuiChunHua : Acc
{

    public override void Init()
    {
        base.Init();
        toSl = StartCoroutine(ToSl());
    }

    Coroutine toSl;//对周围持续减速
    IEnumerator ToSl()
    {
        while (true)
        {
            if (PlantManager.Instance.enemyList.Count > 0)
            {
                foreach (var en in PlantManager.Instance.enemyList)
                {
                    if (en.Value.curStatus != Unit_Status.dead)
                    {
                        if ((en.Value.transform.position - this.transform.position).magnitude <= 3f)
                        {
                            en.Value.StartSlow();
                        }
                    }
                }
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

    public override void StopCor()
    {
        base.StopCor();
        if (this.toSl != null)
        {
            StopCoroutine(this.toSl);
            this.toSl = null;
        }
    }
}

