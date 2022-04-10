using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    //��������
    private int sunNum = 100;
    //������������ʱ���¼�
    private UnityAction SunNumUpdateAction;
    public int SunNum
    {
        get => sunNum;
        set
        {
            sunNum = value;
            UIManager.Instance.UpdateSunNum(sunNum);
            if(SunNumUpdateAction!=null)
            {
                SunNumUpdateAction();
            }
        }
    }

    private void Awake()
    {
        Instance = this;
        
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    ///����������������ʱ���¼��ļ���
    /// </summary>
    public void AddSunNumUpdateActionListener(UnityAction action)
    {
        SunNumUpdateAction += action;
    }
}
