using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Tool
{
    //���ھ�̬����this����ʹSpriteRender����ֱ�ӵ���.IsClickDown
    public static bool IsTorch(this SpriteRenderer spriteRenderer,Vector3 mousePosition)//�ж������Ƿ�������
    {
        if (spriteRenderer.sprite == null)
            return false;
        var pos = new Vector3(mousePosition.x, mousePosition.y, spriteRenderer.transform.position.z);
        var bounds = spriteRenderer.bounds;
        return bounds.Contains(pos);//���������ͼ���ڲ�������true
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
