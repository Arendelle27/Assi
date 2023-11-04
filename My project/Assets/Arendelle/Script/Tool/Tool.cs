using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    public static characterType KIndToChaTy(PlantKind kind)
    {
        switch (kind)
        {
            case PlantKind.DuMogu:
                return characterType.DMG;
            case PlantKind.HuoLongGuo:
                return characterType.HLG;
            case PlantKind.TaHuang:
                return characterType.TH;
            case PlantKind.ZuiChunHua:
                return characterType.ZCH;
            default:
                return characterType.Player;
        }
    }
}
