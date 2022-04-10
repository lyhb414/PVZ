using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ÓÎÏ·ÅäÖÃ
/// </summary>
[CreateAssetMenu(fileName ="GameConf",menuName ="GameConf")]
public class GameConf : ScriptableObject
{
    [Tooltip("Ñô¹â")]
    public GameObject Sun;

    [Header("Ö²Îï")]
    [Tooltip("Ì«Ñô»¨")]
    public GameObject SunFlower;
    [Tooltip("Íã¶¹ÉäÊÖ")]
    public GameObject Peashooter;
    [Tooltip("¼á¹ûÇ½")]
    public GameObject Wallnut;

    [Header("½©Ê¬")]
    [Tooltip("ÆÕÍ¨½©Ê¬")]
    public GameObject Zombie;
    [Tooltip("½©Ê¬µÄÍ·")]
    public GameObject Zombie_Head;

    [Header("ÒôÀÖ")]
    public GameObject EFAudio;
    public AudioClip ButtonClick;
    public AudioClip Pause;
    public AudioClip Shovel;
    public AudioClip CanPlant;
    public AudioClip CannotPlant;
    public AudioClip PlacePlant;
    public AudioClip SunClick;

    public AudioClip GameOver;

    public AudioClip ZombieEat;
    public AudioClip ZombieHurtForPea;
    [Tooltip("½©Ê¬ÉëÒ÷1")]
    public AudioClip ZombieGroan1;
    [Tooltip("½©Ê¬ÉëÒ÷2")]
    public AudioClip ZombieGroan2;
    [Tooltip("½©Ê¬ÉëÒ÷3")]
    public AudioClip ZombieGroan3;
    [Tooltip("½©Ê¬ÉëÒ÷4")]
    public AudioClip ZombieGroan4;
    [Tooltip("½©Ê¬ÉëÒ÷5")]
    public AudioClip ZombieGroan5;
    [Tooltip("½©Ê¬ÉëÒ÷6")]
    public AudioClip ZombieGroan6;

    [Header("×Óµ¯")]
    [Tooltip("Íã¶¹")]
    public GameObject Bullet1;
    [Tooltip("Íã¶¹_Õı³£")]
    public Sprite Bullet1Nor;
    [Tooltip("Íã¶¹_»÷ÖĞ")]
    public Sprite Bullet1Hit;
}
