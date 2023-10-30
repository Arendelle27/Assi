using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Tool
{
    //放在静态类中this可以使SpriteRender可以直接调用.IsClickDown
    public static bool IsTorch(this SpriteRenderer spriteRenderer,Vector3 mousePosition)//判断连线是否触碰到点
    {
        if (spriteRenderer.sprite == null)
            return false;
        var pos = new Vector3(mousePosition.x, mousePosition.y, spriteRenderer.transform.position.z);
        var bounds = spriteRenderer.bounds;
        return bounds.Contains(pos);//如果鼠标垫在图形内部，返回true
    }

    public static characterType CeKIndToChaTy(CellKind kind)
    {
        switch (kind)
        {
            case CellKind.DuMogu:
                return characterType.DMG;
            case CellKind.HuoLongGuo:
                return characterType.HLG;
            case CellKind.TaHuang:
                return characterType.TH;
            case CellKind.ZuiChunHua:
                return characterType.ZCH;
            default:
                return characterType.Player;
        }
    }



}
