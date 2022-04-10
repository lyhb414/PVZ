using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkySunManager : MonoBehaviour
{
    public static SkySunManager Instance;

    //创建阳光的坐标Y
    private float createSkySunPosY = 7;

    //创建阳光的坐标X的范围
    private float createSkySunRightPosX = 4.3f;
    private float createSkySunLeftPosX = -7f;

    //阳光下落停止时的坐标Y的范围
    private float downSkySunMaxPosY = 2.5f;
    private float downSkySunMinPosY = -3.7f;

    private void Awake()
    {
        Instance = this;
    }

    public void StartCreateSun(float createSunCD)
    {
        InvokeRepeating("CreateSun", createSunCD, createSunCD);
    }

    public void StopCreateSun()
    {
        CancelInvoke();
    }

    /// <summary>
    /// 从天空生成阳光
    /// </summary>
    void CreateSun()
    {
        Sun sun = PoolManager.Instance.GetObj(GameManager.Instance.GameConf.Sun).GetComponent<Sun>();
        sun.transform.SetParent(transform);
        float downTarget_Y = Random.Range(downSkySunMinPosY, downSkySunMaxPosY);
        float Pos_X = Random.Range(createSkySunLeftPosX, createSkySunRightPosX);
        float Pos_Y = createSkySunPosY;
        sun.InitForSky(downTarget_Y, Pos_X, Pos_Y);    
    }
}
