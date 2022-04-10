using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum ZombieState
{
    Idol,
    Walk,
    Attack,
    Dead
}

public class Zombie : MonoBehaviour
{
    //我的状态
    private ZombieState state;

    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private Grid currGrid;

    //生命值
    private int hp;


    /// <summary>
    /// 速度，决定几秒走一格
    /// </summary>
    private float speed = 6;
    //是否在攻击状态
    private bool isAttackState;

    //是否已经失去头
    private bool isLostHead;

    //攻击力：每秒造成的hp伤害
    private float attackValue = 100;

    //初始随机产生的行走动画名称
    private string walkingAnimationStr;
    //攻击动画名称
    private string attackAnimationStr;

    /// <summary>
    /// 修改状态，会直接改变动画
    /// </summary>
    public ZombieState State
    {
        get => state;
        set
        {
            state = value;
            CheckState();
        }
    }

    public Grid CurrGrid { get => currGrid; set => currGrid = value; }
    public int Hp { get => hp; 
        set
        {
            hp = value;
            if (hp <= 90 && !isLostHead) 
            {
                //需要失去头
                isLostHead = true;
                walkingAnimationStr = "Zombie_LostHead";
                attackAnimationStr = "Zombie_LostHeadAttack";
                //头掉落，创建一个头
                Zombie_Head zombie_Head = PoolManager.Instance.GetObj(GameManager.Instance.GameConf.Zombie_Head).GetComponent<Zombie_Head>();
                zombie_Head.Init(new Vector3(animator.transform.position.x + 1f, animator.transform.position.y, 0));
                zombie_Head.transform.SetParent(null);
                //状态检测
                CheckState();
            }
            if(hp<=0)
            {
                State = ZombieState.Dead;
            }

        }

    }

    public void Init(int lineNum,int orderNum,Vector2 pos)
    {
        transform.position = pos;
        InitAnimationName();
        Find();
        GetGridByVerticalNum(lineNum);
        CheckOrder(orderNum);

        
        hp = 270;
        isLostHead = false;
        State = ZombieState.Idol;
        spriteRenderer.color = Color.white;
    }

    /// <summary>
    /// 检查排序
    /// </summary>
    private void CheckOrder(int orderNum)
    {
        //我在哪一排，越靠近0 order越大
        //第0排最大（400-499）第4排最小（0-99）
        int line = (int)CurrGrid.point.y;
        spriteRenderer.sortingOrder = (4 - line) * 100 + orderNum;
    }


    /// <summary>
    /// 初始化动画名称
    /// </summary>
    private void InitAnimationName()
    {
        int rangeWalk = Random.Range(1, 4);
        switch (rangeWalk)
        {
            case 1:
                walkingAnimationStr = "Zombie_Walk1";
                break;
            case 2:
                walkingAnimationStr = "Zombie_Walk2";
                break;
            case 3:
                walkingAnimationStr = "Zombie_Walk3";
                break;
        }
        attackAnimationStr = "Zombie_Attack";
    }
    private void Find()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();
        
    }


    void Update()
    {
        FSM();
    }

    /// <summary>
    /// 状态检测
    /// </summary>
    private void CheckState()
    {
        switch (State)
        {
            case ZombieState.Idol:
                //播放行走动画，但是要卡在第一帧
                animator.Play(walkingAnimationStr, 0, 0);
                animator.speed = 0;
                break;
            case ZombieState.Walk:
                animator.Play(walkingAnimationStr, 0, animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
                animator.speed = 1;
                break;
            case ZombieState.Attack:
                animator.Play(attackAnimationStr, 0, animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
                animator.speed = 1;
                break;
            case ZombieState.Dead:
                Dead();
                break;

        }
    }

    /// <summary>
    /// 状态切换
    /// </summary>
    private void FSM()
    {
        switch (state)
        {
            case ZombieState.Idol:
                State = ZombieState.Walk;
                break;
            case ZombieState.Walk:
                //一直向左走，遇到植物会攻击，攻击结束继续走
                Move();
                break;
            case ZombieState.Attack:
                if (isAttackState)
                    break;
                Attack(CurrGrid.CurrPlantBase);
                break;
            case ZombieState.Dead:
                break;

        }
    }


    /// <summary>
    /// 获取一个网格，决定在第几排出现
    /// </summary>
    /// <param name="verticalNum"></param>
    private void GetGridByVerticalNum(int verticalNum)
    {
        CurrGrid = GridManager.Instance.GetGridByVerticalNum(verticalNum);
        transform.position = new Vector3(transform.position.x, CurrGrid.position.y);
    }

    /// <summary>
    /// 移动
    /// </summary>
    private void Move()
    {
        //如果当前网格为空，跳过移动检测
        if (CurrGrid == null) return;
        CurrGrid= GridManager.Instance.GetGridByWorldPos(transform.position);
        //如果当前网格有植物，并且植物在左边，并且距离很近
        if (CurrGrid.havePlant
            &&CurrGrid.CurrPlantBase.transform.position.x<transform.position.x
            &&transform.position.x- CurrGrid.CurrPlantBase.transform.position.x<0.3f)
        {
            //攻击植物
            State = ZombieState.Attack;
            return;
        }

        //如果我在最左边的网格，并且我已经越过了它
        if (currGrid.point.x == 0 && currGrid.position.x - transform.position.x > 1.2f) 
        {
            //走向终点：房子
            Vector2 pos = transform.position;
            Vector2 target = new Vector2(-9.33f, -1.33f);
            Vector2 dir = (target - pos).normalized*3f;
            transform.Translate(dir * Time.deltaTime / speed);

            //如果我离终点很近，意味着游戏结束
            if(Vector2.Distance(target,pos)<0.05f)
            {
                //触发游戏结束
                LevelManager.Instance.GameOver();
                return;
            }
            return;

        }

        transform.Translate(new Vector2(-1.33f, 0) * Time.deltaTime / speed);
    }

    /// <summary>
    /// 攻击植物
    /// </summary>
    /// <param name="plant"></param>
    private void Attack(PlantBase plant)
    {
        isAttackState = true;
        //植物相关逻辑
        StartCoroutine(DoHurtPlant(plant));
    }

    /// <summary>
    /// 附加伤害给植物
    /// </summary>
    /// <returns></returns>
    IEnumerator DoHurtPlant(PlantBase plant)
    {
        int num = 0;
        //植物hp大于0则扣血
        while (plant != null && plant.Hp > 0) 
        {
            if(num==5)
            {
                num = 0;
            }
            //播放僵尸吃植物的音效
            if(num==0)
            {
                AudioManager.Instance.PlayEFMusic(GameManager.Instance.GameConf.ZombieEat);
            }
            num++;

            plant.Hurt(attackValue*0.2f);
            yield return new WaitForSeconds(0.2f);
        }
        isAttackState = false;
        State = ZombieState.Walk;
    }

    /// <summary>
    /// 自身受伤
    /// </summary>
    public void Hurt(int attackValue)
    {
        Hp -= attackValue;
        if (gameObject.activeSelf)
            StartCoroutine(ColorEF(0.2f,new Color(0.4f,0.4f,0.4f),0.05f,null));
    }

    /// <summary>
    /// 死亡
    /// </summary>
    public void Dead()
    {
        StopAllCoroutines();
        currGrid = null;
        isAttackState = false;

        //告诉僵尸管理器 我死了
        ZombieManager.Instance.RemoveZombie(this);
        //把自己放进缓存池
        PoolManager.Instance.PushObj(GameManager.Instance.GameConf.Zombie, gameObject);
    }

    /// <summary>
    /// 颜色效果
    /// </summary>
    /// <returns></returns>
    protected IEnumerator ColorEF(float wantTime, Color targetColor, float delayTime, UnityAction func)
    {
        float currTime = 0;
        float lerp;
        while (currTime < wantTime)
        {
            lerp = currTime / wantTime;
            currTime += delayTime;
            yield return new WaitForSeconds(delayTime);
            spriteRenderer.color = Color.Lerp(Color.white, targetColor, lerp);
        }
        spriteRenderer.color = Color.white;
        if (func != null)
        {
            func();
        }
    }

    public void StartMove()
    {
        State = ZombieState.Walk;
    }
}
