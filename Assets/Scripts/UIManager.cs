using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    private Text sunNumText;
    private GameObject mainPanel;

    //��ǰ��ֲ�￨Ƭ
    private UIPlantCard currCard;

    private LevelStartEF LevelStartEF;
    private LVInfoPanel LVInfoPanel;
    private SetPanel setPanel;
    private OverPanel overPanel;

    public UIPlantCard CurrCard 
    { get => currCard; 
        set
        {
            if(currCard==value)
            {
                return;
            }
            //�ÿ���һ����Ƭ��״̬
            if (currCard != null) 
            {
                currCard.WantPlant = false;
            }
            currCard = value;
        }  
    }

    private void Awake()
    {
        Instance = this;
        mainPanel = transform.Find("MainPanel").gameObject;
        sunNumText = transform.Find("MainPanel/SunNumText").GetComponent<Text>();
        LevelStartEF = transform.Find("LevelStartEF").GetComponent<LevelStartEF>();
        LVInfoPanel = transform.Find("LVInfoPanel").GetComponent<LVInfoPanel>();
        setPanel= transform.Find("SetPanel").GetComponent<SetPanel>();
        setPanel.gameObject.SetActive(false);

        overPanel = transform.Find("OverPanel").GetComponent<OverPanel>();
        overPanel.gameObject.SetActive(false);
    }
    void Start()
    {
        
    }
    /// <summary>
    /// ��������������ʾ
    /// </summary>
    /// <param name="num"></param>
    public void UpdateSunNum(int num)
    {
        sunNumText.text = num.ToString();
    }

    /// <summary>
    /// ��ȡ��������Text����
    /// </summary>
    public Vector3 GetSunNumTextPos()
    {
        return sunNumText.transform.position;
    }

    /// <summary>
    /// ������������ʾ
    /// </summary>
    /// <param name="isShow"></param>
    public void SetMainPanelActive(bool isShow)
    {
        mainPanel.SetActive(isShow);
    }

    /// <summary>
    /// ��ʾ�ؿ���ʼʱ��Ч��
    /// </summary>
    public void ShowLevelStartEF()
    {
        LevelStartEF.Show();
    }

    public void UpdateDayNum(int day)
    {
        LVInfoPanel.UpdateDayNum(day);
    }
    public void UpdateStageNum(int stage)
    {
        LVInfoPanel.UpdateStageNum(stage);
    }

    public void ShowSetPanel()
    {
        AudioManager.Instance.PlayEFMusic(GameManager.Instance.GameConf.Pause);
        setPanel.Show(true);
    }

    public void GameOver()
    {
        overPanel.gameObject.SetActive(true);
        overPanel.Over();
    }

}
