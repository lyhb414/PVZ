using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Shovel : MonoBehaviour,IPointerClickHandler,IPointerEnterHandler,IPointerExitHandler,IPointerDownHandler
{
    private Transform shovelImg;

    //�Ƿ���ʹ�ò���
    private bool isShoveling;

    public bool IsShoveling { get => isShoveling; 
        set
        {
            isShoveling = value;
            //��Ҫ��ֲ��
            if(isShoveling)
            {
                shovelImg.rotation = Quaternion.Euler(0, 0, 45);
            }
            //�Ѳ��ӷŻ�ȥ
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
        //�����Ҫ��ֲ��
        if(IsShoveling)
        {
            shovelImg.position = Input.mousePosition;
            //���������ж��Ƿ�Ҫ����ֲ��
            if(Input.GetMouseButtonDown(0))
            {
                Grid grid = GridManager.Instance.GetGridByMouse();
                //���û��ֲ�ֱ�����������߼�
                if(grid.CurrPlantBase==null)
                {
                    return;
                }
                //���������ֲ��ľ���С��1.5�������
                if(Vector2.Distance(Camera.main.ScreenToWorldPoint(Input.mousePosition),grid.CurrPlantBase.transform.position)<0.5f)
                {
                    AudioManager.Instance.PlayEFMusic(GameManager.Instance.GameConf.Shovel);
                    grid.CurrPlantBase.Dead();
                    IsShoveling = false;
                }

            }

            //����Ҽ�ȡ������״̬
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
