using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkySunManager : MonoBehaviour
{
    public static SkySunManager Instance;

    //�������������Y
    private float createSkySunPosY = 7;

    //�������������X�ķ�Χ
    private float createSkySunRightPosX = 4.3f;
    private float createSkySunLeftPosX = -7f;

    //��������ֹͣʱ������Y�ķ�Χ
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
    /// �������������
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
