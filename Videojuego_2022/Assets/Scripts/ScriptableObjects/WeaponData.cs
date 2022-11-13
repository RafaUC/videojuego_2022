using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="Weapon", menuName="Weapon")]
public class WeaponData : ScriptableObject
{
    [Header("Info") ]
    public string weaponName;

    [Header("Attack") ]
    public int type;  //0 = bullet Gun 1 = Conic attack
    public float damage;
    public float roundsPerSecond;
    public float velocity;
    public float reach;
    public float fieldOfAtack;
    public float recoil;
    public float mass;

    [Header("Reload") ]
    public int ammo;
    public int loadedAmmo;
    public int maxAmmo;
    public int reloadSize;
    public float reloadTime;

    
    public bool isReloading;
    public bool attackReleased;
}
