using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UIElements;

public class InputManager : MonoSingleton<InputManager>
{
    public bool isAccMouseDown = false;
    public Vector2 mousePosition;


    public Queue<Vector2> mousePositionQueue = new Queue<Vector2>();
    void Start()
    {
        Observable
       .EveryUpdate()//开启Update的事件监听
       .Where(_ => Input.GetButtonDown("CellAcc")&&Game.Instance.Status==GameState.InGame)
       .Subscribe(_ =>
       {
           this.mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
           if (CellAccManager.Instance.cellAccList.Count != 0)
           {
               foreach (var cellAcc in CellAccManager.Instance.cellAccList)
               {
                   if (cellAcc.Value.spriteRenderer.IsTorch(InputManager.Instance.mousePosition))
                   {
                       this.isAccMouseDown = true;
                       this.mousePositionQueue.Clear();
                       StartCoroutine(MousePositionQueue());
                       cellAcc.Value.isMove = true;
                       StartCoroutine(cellAcc.Value.BragMove());
                       break;
                   }
               }
           }
       })
       .AddTo(this);//当游戏对象销毁时，自动取消订阅

        Observable
       .EveryUpdate()//开启Update的事件监听
       .Where(_ => Input.GetButtonUp("CellAcc"))
       .Subscribe(_ =>
       {
           this.isAccMouseDown = false;
           StopCoroutine(MousePositionQueue());
           if (CellAccManager.Instance.cellAccList.Count != 0 && Game.Instance.Status == GameState.InGame)
           {
               foreach (var cellAcc in CellAccManager.Instance.cellAccList)
               {
                   if (cellAcc.Value.isMove)
                   {
                       cellAcc.Value.isMove = false;
                       StopCoroutine(cellAcc.Value.BragMove());
                       break;
                   }
               }
           }
       })
       .AddTo(this);//当游戏对象销毁时，自动取消订阅
    }

    IEnumerator MousePositionQueue()
    {
        while(this.isAccMouseDown && Game.Instance.Status == GameState.InGame)
        {
            this.mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            this.mousePositionQueue.Enqueue(this.mousePosition);
            yield return new WaitForSeconds(0.02f);
        }
    }

    private void Update()
    {
    }


    public void Init()
    {
        this.mousePositionQueue.Clear();
    }

}
