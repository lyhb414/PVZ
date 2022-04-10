using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Shovel : MonoBehaviour,IPointerClickHandler,IPointerEnterHandler,IPointerExitHandler,IPointerDownHandler
{
    private Transform shovelImg;

    //是否在使用铲子
    private bool isShoveling;

    public bool IsShoveling { get => isShoveling; 
        set
        {
            isShoveling = value;
            //需要铲植物
            if(isShoveling)
            {
                shovelImg.rotation = Quaternion.Euler(0, 0, 45);
            }
            //把铲子放回去
            else
            {
                shovelImg.rotation = Quaternion.Euler(0, 0, 0);
                shovelImg.position = transform.position;
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (!IsShoveling)
            {
                IsShoveling = true;
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //shovelImg.localScale = new Vector2(1.4f,1.4f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //shovelImg.localScale = new Vector2(1f, 1f);
    }

    private void Awake()
    {
        
    }

    void Start()
    {
        shovelImg = transform.Find("Image");
        LevelManager.Instance.AddLevelStartActionListener(OnLevelStartAction);
    }

    void Update()
    {
        //如果需要铲植物
        if(IsShoveling)
        {
            shovelImg.position = Input.mousePosition;
            //点击左键，判断是否要铲除植物
            if(Input.GetMouseButtonDown(0))
            {
                Grid grid = GridManager.Instance.GetGridByMouse();
                //如果没有植物，直接跳过所有逻辑
                if(grid.CurrPlantBase==null)
                {
                    return;
                }
                //如果鼠标距离植物的距离小于1.5，则铲除
                if(Vector2.Distance(Camera.main.ScreenToWorldPoint(Input.mousePosition),grid.CurrPlantBase.transform.position)<0.5f)
                {
                    AudioManager.Instance.PlayEFMusic(GameManager.Instance.GameConf.Shovel);
                    grid.CurrPlantBase.Dead();
                    IsShoveling = false;
                }

            }

            //点击右键取消铲子状态
            if(Input.GetMouseButtonDown(1))
            {
                AudioManager.Instance.PlayEFMusic(GameManager.Instance.GameConf.Shovel);
                IsShoveling = false;
            }
        }    
    }

    private void OnLevelStartAction()
    {
        IsShoveling = false;
    }
}
