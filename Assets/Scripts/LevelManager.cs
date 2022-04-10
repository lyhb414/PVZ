using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// �ؿ�״̬
/// </summary>
public enum LevelState
{
    //��ʼ��Ϸ
    Start,
    //ս����
    Fighting,
    //����
    Over
}

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    private LevelState currLevelState;

    private bool isOver = false;

    //�Ƿ���ˢ�½�ʬ
    private bool isUpdateZombie;

    //��ǰ�ڼ��죨�ؿ�����
    private int currLevel;
    public int CurrLevel
    {
        get => currLevel;
        set
        {
            currLevel = value;
            StartLevel(currLevel);
        }
    }

    //�ؿ��еĲ������׶Σ�
    private int currStage;

    public UnityAction levelStartAction;


    public int CurrStage { get => currStage; 
    set
        {
            currStage = value;
            UIManager.Instance.UpdateStageNum(currStage);
            if (currStage>3)
            {
                //ɱ����ǰ�ؿ���ȫ����ʬ���ͽ�����һ��
                ZombieManager.Instance.AddAllZombieDeadAction(OnAllZombieDeadAction);
                CurrLevelState = LevelState.Over;
            }

        }
    }

    public LevelState CurrLevelState
    {
        get => currLevelState;
        set
        {
            currLevelState = value;
            switch (currLevelState)
            {
                case LevelState.Start:
                    //����UI�����
                    UIManager.Instance.SetMainPanelActive(false);
                    //ˢ�½�ʬ��Ľ�ʬ
                    ZombieManager.Instance.UpdateZombie(5);
                    //������ƶ����Ҳ࣬�۲�ؿ���ʬ�����ƻ���࣬�л���ս��״̬
                    CameraController.Instance.StartMove(LevelStartCameraBackAction);
                    break;
                case LevelState.Fighting:
                    //��ʾUI
                    UIManager.Instance.SetMainPanelActive(true);
                    //20s��ˢ�½�ʬ
                    UpdateZombie(1, 20.0f);
                    break;
                case LevelState.Over:
                    SkySunManager.Instance.StopCreateSun();
                    break;
            }
        }
    }



    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        CurrLevel = 1;
    }

    /// <summary>
    /// ��ʼ�ؿ�
    /// </summary>
    public void StartLevel(int level)
    {
        if(isOver)
        {
            return;
        }
        currLevel = level;
        UIManager.Instance.UpdateDayNum(currLevel);
        CurrStage = 0;
        CurrLevelState = LevelState.Start;
    }
    void Update()
    {
        if(isOver)
        {
            return;
        }
        FSM();
    }

    private void FSM()
    {
        switch (CurrLevelState)
        {
            case LevelState.Start:
                break;
            case LevelState.Fighting:
                //���û��ˢ�½�ʬ����ˢ�½�ʬ
                if (isUpdateZombie == false)
                {
                    //��ʬˢ�µ�ʱ��
                    float updateTime = Random.Range(15 - currStage / 2, 20 - currStage / 2);
                    //��ʬˢ�µ�����
                    int updateNum = Random.Range(1, 3 + currLevel);
                    UpdateZombie(updateNum, updateTime);
                    CurrStage += 1;
                }
                break;
            case LevelState.Over:
                break;
        }
    }

    /// <summary>
        /// �ؿ���ʼʱ������ع��Ҫִ�еķ���
        /// </summary>
    private void LevelStartCameraBackAction()
    {
        //�����⿪ʼ����
        SkySunManager.Instance.StartCreateSun(6);

        //��ʼ��ʾUI��Ч
        UIManager.Instance.ShowLevelStartEF();

        ZombieManager.Instance.ClearZombie();

        CurrLevelState = LevelState.Fighting;

        //�ؿ�ս����ʼʱ��Ҫ��������
        if (levelStartAction != null)
        {
            levelStartAction();
        }

    }
    
    private void UpdateZombie(int zombieNum,float createCD)
    {
        StartCoroutine(DoUpdateZombie(zombieNum, createCD));
    }

    IEnumerator DoUpdateZombie(int zombieNum,float createCD)
    {
        isUpdateZombie = true;
        yield return new WaitForSeconds(createCD);
        ZombieManager.Instance.UpdateZombie(zombieNum);
        ZombieManager.Instance.ZombieStartMove();
        isUpdateZombie = false;
    }

    /// <summary>
    /// ���ӹؿ���ʼ�¼��ļ�����
    /// </summary>
    /// <param name=""></param>
    public void AddLevelStartActionListener(UnityAction action)
    {
        levelStartAction += action;
    }

    /// <summary>
    /// ��ȫ����ʬ����ʱ�������¼�
    /// </summary>
    private void OnAllZombieDeadAction()
    {
        //��������
        CurrLevel += 1;
        //ִ��һ��֮���Լ��Ƴ�ί��
        ZombieManager.Instance.RemoveAllZombieDeadAction(OnAllZombieDeadAction);
    }

    /// <summary>
    /// ��Ϸ����
    /// </summary>
    public void GameOver()
    {
        StopAllCoroutines();

        //Ч��
        AudioManager.Instance.PlayEFMusic(GameManager.Instance.GameConf.ZombieEat);
        AudioManager.Instance.PlayEFMusic(GameManager.Instance.GameConf.GameOver);


        isOver = true;
        //�߼�
        SkySunManager.Instance.StopCreateSun();
        ZombieManager.Instance.ClearZombie();


        //UI
        UIManager.Instance.GameOver();
    }

}