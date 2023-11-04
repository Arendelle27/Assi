using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

    public class AccDuMogu:Acc
{
    public override void OnTriggerEnter2D(Collider2D collision)//进入检测
    {
        if (collision.CompareTag("Plant"))
        {
            Enemy en = collision.GetComponent<Enemy>();
            if(en!=null)
            {
                if (en.curStatus == Unit_Status.dead)
                    return;
                SoundEffectManager.Instance.AccAttack();
                en.StartDecay();
            }
        }
    }
}
