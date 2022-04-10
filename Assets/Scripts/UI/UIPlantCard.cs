using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// ��Ƭ����״̬
/// </summary>
public enum CardState
{
    CanPlant,
    NotCD,
    NotSun,
    NotAll
}

public class UIPlantCard : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IPointerClickHandler
{
    //����ͼƬ�����
    private Image maskImg;

    //����ͼƬ�����
    private Image image;

    //��ֲ��Ҫ��������
    public int wantSunNum;

    //��Ҫ�����������ı�
    private Text wantSunText;

    //��ֲ��ȴʱ��
    public float cdTime;

    //��ǰʱ�䣺���ڼ�����ȴ
    private float currTimeForCd;

    //��ֲֲ��CD�Ƿ�ת�� 
    private bool canPlant;

    //ֲ���Ԥ����
    private GameObject prefab;

    //�Ƿ���Ҫ��ֲֲ��
    private bool wantPlant;

    //����������ֲ��
    private PlantBase plant;

    //�����е�͸��ֲ��
    private PlantBase plantInGrid;

    //��ǰ���ƶ�Ӧ��ֲ������
    public PlantManager.PlantType cardPlantType;

    private CardState cardState=CardState.NotAll;
    public CardState CardState { get => cardState;
        set
        {
            if (cardState == value)
            {
                return;
            }
            
            switch (value)
            {
                case CardState.CanPlant:
                    //CDû�����֣�������������
                    maskImg.fillAmount = 0;
                    image.color = Color.white;
                    break;
                case CardState.NotCD:
                    //CD�����֣�������������
                    image.color = Color.white;
                    if(cardState==CardState.NotAll)
                    {
                        break;
                    }
                    StartCD();
                    break;
                case CardState.NotSun:
                    //CDû�����֣������ǻ谵��
                    maskImg.fillAmount = 0;
                    image.color = new Color(0.75f,0.75f,0.75f);
                    break;
                case CardState.NotAll:
                    //CD�����֣������ǻ谵��
                    image.color = new Color(0.75f, 0.75f, 0.75f);
                    if (cardState == CardState.NotCD)
                    {
                        break;
                    }
                    StartCD();
                    break;
            }
            cardState = value;
        }
    }

    public bool CanPlant { get => canPlant;
        set
        {
            canPlant = value;
            CheckState();
        }

    }

    public bool WantPlant { get => wantPlant; 
        set
        {
            wantPlant = value;
            if(wantPlant)
            {
                AudioManager.Instance.PlayEFMusic(GameManager.Instance.GameConf.CanPlant);
                prefab = PlantManager.Instance.GetPlantByType(cardPlantType);
                plant = PoolManager.Instance.GetObj(prefab).GetComponent<PlantBase>();
                plant.transform.SetParent(PlantManager.Instance.transform);
                plant.InitForCreate(false,cardPlantType,Vector2.zero);
            }
            else
            {
                if (plant != null)
                {
                    plant.Dead();
                    plant = null;
                }
            }
        }
    }

    /// <summary>
    /// ��ʼ����CD
    /// </summary>
    private void StartCD()
    {
        maskImg.fillAmount = 1;
        currTimeForCd = cdTime;
        StartCoroutine(CalCD());
    }

    /// <summary>
    /// ����CD
    /// </summary>
    /// <returns></returns>
    IEnumerator CalCD()
    {
        while (currTimeForCd>=0)
        {
            yield return new WaitForSeconds(0.1f);
            currTimeForCd -= 0.1f;
            maskImg.fillAmount = currTimeForCd / cdTime;
        }
        CanPlant = true;
    }

    void Start()
    {
        maskImg = transform.Find("Mask").GetComponent<Image>();
        image = GetComponent<Image>();
        wantSunText = transform.Find("SunCost").GetComponent<Text>();
        wantSunText.text = wantSunNum.ToString();
        CanPlant = true;
        WantPlant = false;
        PlayerManager.Instance.AddSunNumUpdateActionListener(CheckState);
        LevelManager.Instance.AddLevelStartActionListener(OnLevelStartAction);
    }

    private void Update()
    {
        if (WantPlant && plant != null) 
        {
            //��ֲ��������
            Vector3 mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Grid grid = GridManager.Instance.GetGridByMouse();

            plant.transform.position = new Vector3(mousePoint.x, mousePoint.y, 0);

            //�������������ϣ���ʾ͸����ֲ��
            if (grid.havePlant == false && Vector2.Distance(mousePoint, grid.position) < 1) 
            {
                if (plantInGrid == null)
                {
                    plantInGrid = PoolManager.Instance.GetObj(prefab).GetComponent<PlantBase>();
                    plantInGrid.transform.SetParent(PlantManager.Instance.transform);

                    plantInGrid.InitForCreate(true,cardPlantType, grid.position);
                }
                else
                {
                    plantInGrid.transform.position = grid.position;
                }

                //�������������ֲ
                if (Input.GetMouseButtonDown(0))
                {
                    plant.InitForPlant(grid,cardPlantType);
                    AudioManager.Instance.PlayEFMusic(GameManager.Instance.GameConf.PlacePlant);
                    plant = null;
                    if (plantInGrid != null)
                    {
                        plantInGrid.Dead();
                        plantInGrid = null;
                    }
                    WantPlant = false;
                    CanPlant = false;

                    //��ֲ�ɹ������������
                    PlayerManager.Instance.SunNum -= wantSunNum;
                }
            }
            else
            {
                if (plantInGrid != null)
                {
                    plantInGrid.Dead();
                    plantInGrid = null;
                }
            }
            //����Ҽ�ȡ������
            if(Input.GetMouseButtonDown(1))
            {
                CancelPlant();
            }

        }
    }

    /// <summary>
    /// ȡ����ֲ
    /// </summary>
    private void CancelPlant()
    {
        if (plantInGrid != null)
        {
            plantInGrid.Dead();
            plantInGrid = null;
        }
        if (plant != null)
        {
            AudioManager.Instance.PlayEFMusic(GameManager.Instance.GameConf.CanPlant);
            plant.Dead();
            plant = null;
        }
        WantPlant = false;
    }


    /// <summary>
    /// �ڹؿ�ս����ʼʱ��Ҫ��������
    /// </summary>
    private void OnLevelStartAction()
    {
        CancelPlant();
        CanPlant = true;
    }

    /// <summary>
    /// ״̬���
    /// </summary>
    public void CheckState()
    {
        if (canPlant && PlayerManager.Instance.SunNum >= wantSunNum)
        {
            CardState = CardState.CanPlant;
        }
        else if (!canPlant && PlayerManager.Instance.SunNum >= wantSunNum)
        {
            CardState = CardState.NotCD;
        }
        else if (canPlant && PlayerManager.Instance.SunNum < wantSunNum)
        {
            CardState = CardState.NotSun;
        }
        else
        {
            CardState = CardState.NotAll;
        }
    }


    //�������
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (CardState != CardState.CanPlant) return;
        transform.localScale = new Vector2(1.05f, 1.05f);
    }
    //����Ƴ�
    public void OnPointerExit(PointerEventData eventData)
    {
        if (CardState!=CardState.CanPlant) return;
        transform.localScale = new Vector2(1, 1);
    }


    //�����
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (CardState != CardState.CanPlant)
            {
                AudioManager.Instance.PlayEFMusic(GameManager.Instance.GameConf.CannotPlant);
                return;
            }
            transform.localScale = new Vector2(1, 1);
            if (!WantPlant)
            {
                UIManager.Instance.CurrCard = this;
                WantPlant = true;
            }
        }
    }
}
