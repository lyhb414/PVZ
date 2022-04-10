using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��Ϸ����
/// </summary>
[CreateAssetMenu(fileName ="GameConf",menuName ="GameConf")]
public class GameConf : ScriptableObject
{
    [Tooltip("����")]
    public GameObject Sun;

    [Header("ֲ��")]
    [Tooltip("̫����")]
    public GameObject SunFlower;
    [Tooltip("�㶹����")]
    public GameObject Peashooter;
    [Tooltip("���ǽ")]
    public GameObject Wallnut;

    [Header("��ʬ")]
    [Tooltip("��ͨ��ʬ")]
    public GameObject Zombie;
    [Tooltip("��ʬ��ͷ")]
    public GameObject Zombie_Head;

    [Header("����")]
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
    [Tooltip("��ʬ����1")]
    public AudioClip ZombieGroan1;
    [Tooltip("��ʬ����2")]
    public AudioClip ZombieGroan2;
    [Tooltip("��ʬ����3")]
    public AudioClip ZombieGroan3;
    [Tooltip("��ʬ����4")]
    public AudioClip ZombieGroan4;
    [Tooltip("��ʬ����5")]
    public AudioClip ZombieGroan5;
    [Tooltip("��ʬ����6")]
    public AudioClip ZombieGroan6;

    [Header("�ӵ�")]
    [Tooltip("�㶹")]
    public GameObject Bullet1;
    [Tooltip("�㶹_����")]
    public Sprite Bullet1Nor;
    [Tooltip("�㶹_����")]
    public Sprite Bullet1Hit;
}
