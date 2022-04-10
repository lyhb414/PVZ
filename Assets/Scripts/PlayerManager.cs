using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    //阳光数量
    private int sunNum = 100;
    //阳光数量更新时的事件
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
    ///增加阳光数量更新时的事件的监听
    /// </summary>
    public void AddSunNumUpdateActionListener(UnityAction action)
    {
        SunNumUpdateAction += action;
    }
}
