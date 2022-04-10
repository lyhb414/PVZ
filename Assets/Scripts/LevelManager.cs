using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 关卡状态
/// </summary>
public enum LevelState
{
    //开始游戏
    Start,
    //战斗中
    Fighting,
    //结束
    Over
}

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    private LevelState currLevelState;

    private bool isOver = false;

    //是否在刷新僵尸
    private bool isUpdateZombie;

    //当前第几天（关卡数）
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

    //关卡中的波数（阶段）
    private int currStage;

    public UnityAction levelStartAction;


    public int CurrStage { get => currStage; 
    set
        {
            currStage = value;
            UIManager.Instance.UpdateStageNum(currStage);
            if (currStage>3)
            {
                //杀掉当前关卡的全部僵尸，就进入下一天
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
                    //隐藏UI主面板
                    UIManager.Instance.SetMainPanelActive(false);
                    //刷新僵尸秀的僵尸
                    ZombieManager.Instance.UpdateZombie(5);
                    //摄像机移动到右侧，观察关卡僵尸，并移回左侧，切换到战斗状态
                    CameraController.Instance.StartMove(LevelStartCameraBackAction);
                    break;
                case LevelState.Fighting:
                    //显示UI
                    UIManager.Instance.SetMainPanelActive(true);
                    //20s后刷新僵尸
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
    /// 开始关卡
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
                //如果没在刷新僵尸，则刷新僵尸
                if (isUpdateZombie == false)
                {
                    //僵尸刷新的时间
                    float updateTime = Random.Range(15 - currStage / 2, 20 - currStage / 2);
                    //僵尸刷新的数量
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
        /// 关卡开始时摄像机回归后要执行的方法
        /// </summary>
    private void LevelStartCameraBackAction()
    {
        //让阳光开始创建
        SkySunManager.Instance.StartCreateSun(6);

        //开始显示UI特效
        UIManager.Instance.ShowLevelStartEF();

        ZombieManager.Instance.ClearZombie();

        CurrLevelState = LevelState.Fighting;

        //关卡战斗开始时需要做的事情
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
    /// 增加关卡开始事件的监听者
    /// </summary>
    /// <param name=""></param>
    public void AddLevelStartActionListener(UnityAction action)
    {
        levelStartAction += action;
    }

    /// <summary>
    /// 当全部僵尸死亡时触发的事件
    /// </summary>
    private void OnAllZombieDeadAction()
    {
        //更新天数
        CurrLevel += 1;
        //执行一次之后，自己移除委托
        ZombieManager.Instance.RemoveAllZombieDeadAction(OnAllZombieDeadAction);
    }

    /// <summary>
    /// 游戏结束
    /// </summary>
    public void GameOver()
    {
        StopAllCoroutines();

        //效果
        AudioManager.Instance.PlayEFMusic(GameManager.Instance.GameConf.ZombieEat);
        AudioManager.Instance.PlayEFMusic(GameManager.Instance.GameConf.GameOver);


        isOver = true;
        //逻辑
        SkySunManager.Instance.StopCreateSun();
        ZombieManager.Instance.ClearZombie();


        //UI
        UIManager.Instance.GameOver();
    }

}