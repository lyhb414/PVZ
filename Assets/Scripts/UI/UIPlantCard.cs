using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// 卡片四种状态
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
    //遮罩图片的组件
    private Image maskImg;

    //自身图片的组件
    private Image image;

    //种植需要多少阳光
    public int wantSunNum;

    //需要阳光数量的文本
    private Text wantSunText;

    //种植冷却时间
    public float cdTime;

    //当前时间：用于计算冷却
    private float currTimeForCd;

    //种植植物CD是否转好 
    private bool canPlant;

    //植物的预制体
    private GameObject prefab;

    //是否需要种植植物
    private bool wantPlant;

    //用来创建的植物
    private PlantBase plant;

    //网格中的透明植物
    private PlantBase plantInGrid;

    //当前卡牌对应的植物类型
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
                    //CD没有遮罩，自身是明亮的
                    maskImg.fillAmount = 0;
                    image.color = Color.white;
                    break;
                case CardState.NotCD:
                    //CD有遮罩，自身是明亮的
                    image.color = Color.white;
                    if(cardState==CardState.NotAll)
                    {
                        break;
                    }
                    StartCD();
                    break;
                case CardState.NotSun:
                    //CD没有遮罩，自身是昏暗的
                    maskImg.fillAmount = 0;
                    image.color = new Color(0.75f,0.75f,0.75f);
                    break;
                case CardState.NotAll:
                    //CD有遮罩，自身是昏暗的
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
    /// 开始进入CD
    /// </summary>
    private void StartCD()
    {
        maskImg.fillAmount = 1;
        currTimeForCd = cdTime;
        StartCoroutine(CalCD());
    }

    /// <summary>
    /// 计算CD
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
            //让植物跟随鼠标
            Vector3 mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Grid grid = GridManager.Instance.GetGridByMouse();

            plant.transform.position = new Vector3(mousePoint.x, mousePoint.y, 0);

            //如果鼠标在网格上，显示透明的植物
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

                //如果点击鼠标则种植
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

                    //种植成功，则减少阳光
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
            //点击右键取消放置
            if(Input.GetMouseButtonDown(1))
            {
                CancelPlant();
            }

        }
    }

    /// <summary>
    /// 取消种植
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
    /// 在关卡战斗开始时需要做的事情
    /// </summary>
    private void OnLevelStartAction()
    {
        CancelPlant();
        CanPlant = true;
    }

    /// <summary>
    /// 状态检测
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


    //鼠标移入
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (CardState != CardState.CanPlant) return;
        transform.localScale = new Vector2(1.05f, 1.05f);
    }
    //鼠标移出
    public void OnPointerExit(PointerEventData eventData)
    {
        if (CardState!=CardState.CanPlant) return;
        transform.localScale = new Vector2(1, 1);
    }


    //鼠标点击
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
