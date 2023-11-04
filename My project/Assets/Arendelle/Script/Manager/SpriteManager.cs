using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum characterType
{
    Player,
    PlayerPri,
    TH,
    HLG,
    DMG,
    ZCH
}

public enum characterStatus
{
    Normal,
    Enemy,
    Injured
}

public class SpriteManager
{


    public Dictionary<characterType, Dictionary<characterStatus, Sprite>> characterSpriteDic;

    public SpriteManager()
    {
        Dictionary<characterStatus, Sprite> playerSpriteDic = new()
        {
            {characterStatus.Normal, Resources.Load<Sprite>("Art/Player/PlayerNor") },
            {characterStatus.Injured, Resources.Load<Sprite>("Art/Player/PlayerInj") },

        };


        Dictionary<characterStatus, Sprite> playerPriSpriteDic = new()
        {
            { characterStatus.Normal, Resources.Load<Sprite>("Art/Player/PlayerPriNor") },
            { characterStatus.Injured, Resources.Load<Sprite>("Art/Player/PlayerPriInj")},
        };

        Dictionary<characterStatus, Sprite> thSpriteDic = new()
        {
            { characterStatus.Normal, Resources.Load<Sprite>("Art/TH/THAcc") },
            { characterStatus.Injured, Resources.Load<Sprite>("Art/TH/THInj")},
            { characterStatus.Enemy, Resources.Load<Sprite>("Art/TH/THEn")},
        };

        Dictionary<characterStatus, Sprite> hlgSpriteDic = new()
        {
            { characterStatus.Normal, Resources.Load<Sprite>("Art/HLG/HLGAcc") },
            { characterStatus.Injured, Resources.Load<Sprite>("Art/HLG/HLGInj")},
            { characterStatus.Enemy, Resources.Load<Sprite>("Art/HLG/HLGEn")},
        };

        Dictionary<characterStatus, Sprite> dmgSpriteDic = new()
        {
            { characterStatus.Normal, Resources.Load<Sprite>("Art/DMG/DMGAcc") },
            { characterStatus.Injured, Resources.Load<Sprite>("Art/DMG/DMGInj")},
            { characterStatus.Enemy, Resources.Load<Sprite>("Art/DMG/DMGEn")},
        };

        Dictionary<characterStatus, Sprite> zchSpriteDic = new()
        {
               { characterStatus.Normal, Resources.Load<Sprite>("Art/ZCH/ZCHAcc") },
            { characterStatus.Injured, Resources.Load<Sprite>("Art/ZCH/ZCHInj")},
            { characterStatus.Enemy, Resources.Load<Sprite>("Art/ZCH/ZCHEn")},
        };

        characterSpriteDic = new Dictionary<characterType, Dictionary<characterStatus, Sprite>>()
        {
            {characterType.Player,playerSpriteDic },
            {characterType.PlayerPri,playerPriSpriteDic },
            {characterType.TH,thSpriteDic },
            {characterType.HLG,hlgSpriteDic },
            {characterType.DMG,dmgSpriteDic },
            {characterType.ZCH,zchSpriteDic },
        };

    }

}
