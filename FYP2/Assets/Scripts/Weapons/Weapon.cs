using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {

    Sprite m_weaponSprite;

    public int m_iAttack;

    public enum WEAPON_TYPE
    {
        WT_SWORD = 0,
        WT_STAFF,
        WT_TOME,
        WT_MACE,
        WT_HARP,
        WT_BOW,
        WT_WARHAMMER,
        WT_WARAXE
    }

    public WEAPON_TYPE m_WT_WeaponType;
}
