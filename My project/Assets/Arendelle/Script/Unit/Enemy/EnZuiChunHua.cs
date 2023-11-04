using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


    public class EnZuiChunHua:Enemy
{
    public override void Init()
    {
        base.Init();
        toSl = StartCoroutine(ToSl());
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Plant"))
        {
            Player pl = collision.GetComponent<Player>();
            if (pl != null)
            {
                this.canAss = true;
                if (this.curStatus == Unit_Status.dead)
                    return;
                this.blastSpeed = this.maxBlastSpeed;
                this.blastdir = (this.transform.position - pl.transform.position).normalized;
            }

            Acc acc = collision.GetComponent<Acc>();
            if (acc != null)
            {
                if (this.curStatus == Unit_Status.dead)
                    return;
                this.blastSpeed = this.maxBlastSpeed;
                this.blastdir = (this.transform.position - acc.transform.position).normalized;
            }
        }
    }


    Coroutine toSl;//对周围持续减速
    IEnumerator ToSl()
    {
        while (true)
        {
            if ((PlantManager.Instance.player.transform.position - this.transform.position).magnitude <= 3f)
            {
                PlantManager.Instance.player.StartSlow();
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

