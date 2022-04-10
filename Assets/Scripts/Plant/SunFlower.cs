using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunFlower : PlantBase
{

    //产生阳光需要的时间
    private float createSunTime = 24;

    //变成金色需要的时间
    private float goldWantTime = 1;

    public override float MaxHp
    {
        get
        {
            return 300;
        }
    }

    void Start()
    {
        
    }

    /// <summary>
    /// 创建阳光
    /// </summary>
    private void CreateSun()
    {
        //StartCoroutine(DoCreateSUun());
        StartCoroutine(ColorEF(goldWantTime, new Color(1, 0.6f, 0),0.01f,InstantiateSun));
    }

    private void InstantiateSun()
    {
        Sun sun = PoolManager.Instance.GetObj(GameManager.Instance.GameConf.Sun).GetComponent<Sun>();
        sun.transform.SetParent(transform);
        //sun.transform.position = transform.position;
        //让阳光进行跳跃
        sun.InitForSunFlower(transform.position);
    }

    protected override void OnInitForPlant()
    {
        InvokeRepeating("CreateSun", createSunTime, createSunTime);
    }
}
