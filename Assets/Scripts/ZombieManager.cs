using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ZombieManager : MonoBehaviour
{
    public static ZombieManager Instance;
    private List<Zombie> zombies = new List<Zombie>();
    private int currOrderNum = 0;

    //创建僵尸的最大和最小的X坐标
    private float createMaxX = 8.6f;
    private float createMinX = 7.36f;

    //所有僵尸都死亡时事件
    private UnityAction allZombieDeadAction;

    public int CurrOrderNum { get => currOrderNum;
        set
        {
            currOrderNum = value;
            if(value>50)
            {
                currOrderNum = 0;
            }
        }

    }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Groan();
    }

    /// <summary>
    /// 更新僵尸
    /// </summary>
    /// <param name="zombieNum"></param>
    public void UpdateZombie(int zombieNum)
    {
        RandomPlayGroan();
        for (int i=0;i<zombieNum;i++)
        {
            CreateZombie(Random.Range(0, 5));
        }
    }


    /// <summary>
    /// 清理所有僵尸
    /// </summary>
    public void ClearZombie()
    {
        while(zombies.Count>0)
        {
            zombies[0].Dead();
        }
    }

    private void Update()
    {

    }

    /// <summary>
    /// 获取一个随机X坐标，用于创建僵尸
    /// </summary>
    private float GetCreatePosXRandom()
    {
        return Random.Range(createMinX, createMaxX);
    }

    /// <summary>
    /// 创建僵尸
    /// </summary>
    private void CreateZombie(int lineNum)
    {
        Zombie zombie = PoolManager.Instance.GetObj(GameManager.Instance.GameConf.Zombie).GetComponent<Zombie>();
        AddZombie(zombie);

        zombie.transform.SetParent(transform);
        zombie.Init(lineNum,CurrOrderNum, new Vector2(GetCreatePosXRandom(), 0));
        CurrOrderNum++;
    }

    public void AddZombie(Zombie zombie)
    {
        zombies.Add(zombie);
    }

    public void RemoveZombie(Zombie zombie)
    {
        zombies.Remove(zombie);
        CheckAllZombieDeadForLevel();
    }

    /// <summary>
    /// 获取一个距离最近的僵尸（僵尸在左）
    /// </summary>
    /// <returns></returns>
    public Zombie GetZombieByLineMinDistance(int lineNum,Vector3 pos)
    {
        Zombie zombie = null;
        float dis = 1 << 30;
        for (int i = 0; i < zombies.Count; i++)
        {
            if (zombies[i].CurrGrid.point.y == lineNum
                && Vector2.Distance(pos, zombies[i].transform.position) < dis
                && zombies[i].transform.position.x > pos.x) 
            {
                dis = Vector2.Distance(pos, zombies[i].transform.position);
                zombie = zombies[i];
            }
        }
        return zombie;
    }

    public void ZombieStartMove()
    {
        for(int i=0;i<zombies.Count;i++)
        {
            zombies[i].StartMove();
        }
    }

    /// <summary>
    /// 为关卡管理器检查所有僵尸死亡时的事件
    /// </summary>
    private void CheckAllZombieDeadForLevel()
    {
        if(zombies.Count==0)
        {
            if(allZombieDeadAction!=null)
            {
                allZombieDeadAction();
            }
        }
    }

    public void AddAllZombieDeadAction(UnityAction action)
    {
        allZombieDeadAction += action;
    }

    public void RemoveAllZombieDeadAction(UnityAction action)
    {
        allZombieDeadAction -= action;
    }

    /// <summary>
    /// 随机播放呻吟音效
    /// </summary>
    private void RandomPlayGroan()
    {
        int rand = Random.Range(0, 12);
        switch (rand)
        {
            case 6:
                AudioManager.Instance.PlayEFMusic(GameManager.Instance.GameConf.ZombieGroan1);
                break;
            case 7:
                AudioManager.Instance.PlayEFMusic(GameManager.Instance.GameConf.ZombieGroan2);
                break;
            case 8:
                AudioManager.Instance.PlayEFMusic(GameManager.Instance.GameConf.ZombieGroan3);
                break;
            case 9:
                AudioManager.Instance.PlayEFMusic(GameManager.Instance.GameConf.ZombieGroan4);
                break;
            case 10:
                AudioManager.Instance.PlayEFMusic(GameManager.Instance.GameConf.ZombieGroan5);
                break;
            case 11:
                AudioManager.Instance.PlayEFMusic(GameManager.Instance.GameConf.ZombieGroan6);
                break;
            default:
                break;
        }
    }


    /// <summary>
    /// 5秒进行一次，有一定概率随机播放音效
    /// </summary>
    private void Groan()
    {
        //加了就卡死??????
        //StartCoroutine(DoGroan());
    }

    IEnumerator DoGroan()
    {
        while(true)
        {
            //有僵尸才进行随机
            if(zombies.Count>0)
            {
                if(Random.Range(0,10)>6)
                    AudioManager.Instance.PlayEFMusic(GameManager.Instance.GameConf.ZombieGroan1);
                //int rand = Random.Range(0, 12);
                //switch (rand)
                //{
                //    case 6:
                //        AudioManager.Instance.PlayEFMusic(GameManager.Instance.GameConf.ZombieGroan1);
                //        break;
                //    case 7:
                //        AudioManager.Instance.PlayEFMusic(GameManager.Instance.GameConf.ZombieGroan2);
                //        break;
                //    case 8:
                //        AudioManager.Instance.PlayEFMusic(GameManager.Instance.GameConf.ZombieGroan3);
                //        break;
                //    case 9:
                //        AudioManager.Instance.PlayEFMusic(GameManager.Instance.GameConf.ZombieGroan4);
                //        break;
                //    case 10:
                //        AudioManager.Instance.PlayEFMusic(GameManager.Instance.GameConf.ZombieGroan5);
                //        break;
                //    case 11:
                //        AudioManager.Instance.PlayEFMusic(GameManager.Instance.GameConf.ZombieGroan6);
                //        break;
                //    default:
                //        break;
                //}
                yield return new WaitForSeconds(5);
            }
        }

    }
}