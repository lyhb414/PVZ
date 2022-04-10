using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LVInfoPanel : MonoBehaviour
{
    //����
    private Text dayNumText;
    //����
    private Text stageNumText;

    void Awake()
    {
        dayNumText = transform.Find("DayNumText").GetComponent<Text>();
        stageNumText = transform.Find("StageNumText").GetComponent<Text>();
    }
    void Start()
    {

    }

    public void UpdateDayNum(int day)
    {
        dayNumText.text = "��ǰ��" + day + "��";
    }
    public void UpdateStageNum(int stage)
    {
        if(stage==0)
        {
            stageNumText.text ="��ʬ��������";
            return;
        }
        stageNumText.text = "��" + stage + "����ʬ����";
    }
}
