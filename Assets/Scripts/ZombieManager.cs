using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ZombieManager : MonoBehaviour
{
    public static ZombieManager Instance;
    private List<Zombie> zombies = new List<Zombie>();
    private int currOrderNum = 0;

    //������ʬ��������С��X����
    private float createMaxX = 8.6f;
    private float createMinX = 7.36f;

    //���н�ʬ������ʱ�¼�
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
    /// ���½�ʬ
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
    /// �������н�ʬ
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
    /// ��ȡһ�����X���꣬���ڴ�����ʬ
    /// </summary>
    private float GetCreatePosXRandom()
    {
        return Random.Range(createMinX, createMaxX);
    }

    /// <summary>
    /// ������ʬ
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
    /// ��ȡһ����������Ľ�ʬ����ʬ����
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
    /// Ϊ�ؿ�������������н�ʬ����ʱ���¼�
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
    /// �������������Ч
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
    /// 5�����һ�Σ���һ���������������Ч
    /// </summary>
    private void Groan()
    {
        //���˾Ϳ���??????
        //StartCoroutine(DoGroan());
    }

    IEnumerator DoGroan()
    {
        while(true)
        {
            //�н�ʬ�Ž������
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