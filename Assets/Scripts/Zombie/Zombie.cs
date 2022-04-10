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
    //�ҵ�״̬
    private ZombieState state;

    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private Grid currGrid;

    //����ֵ
    private int hp;


    /// <summary>
    /// �ٶȣ�����������һ��
    /// </summary>
    private float speed = 6;
    //�Ƿ��ڹ���״̬
    private bool isAttackState;

    //�Ƿ��Ѿ�ʧȥͷ
    private bool isLostHead;

    //��������ÿ����ɵ�hp�˺�
    private float attackValue = 100;

    //��ʼ������������߶�������
    private string walkingAnimationStr;
    //������������
    private string attackAnimationStr;

    /// <summary>
    /// �޸�״̬����ֱ�Ӹı䶯��
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
                //��Ҫʧȥͷ
                isLostHead = true;
                walkingAnimationStr = "Zombie_LostHead";
                attackAnimationStr = "Zombie_LostHeadAttack";
                //ͷ���䣬����һ��ͷ
                Zombie_Head zombie_Head = PoolManager.Instance.GetObj(GameManager.Instance.GameConf.Zombie_Head).GetComponent<Zombie_Head>();
                zombie_Head.Init(new Vector3(animator.transform.position.x + 1f, animator.transform.position.y, 0));
                zombie_Head.transform.SetParent(null);
                //״̬���
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
    /// �������
    /// </summary>
    private void CheckOrder(int orderNum)
    {
        //������һ�ţ�Խ����0 orderԽ��
        //��0�����400-499����4����С��0-99��
        int line = (int)CurrGrid.point.y;
        spriteRenderer.sortingOrder = (4 - line) * 100 + orderNum;
    }


    /// <summary>
    /// ��ʼ����������
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
    /// ״̬���
    /// </summary>
    private void CheckState()
    {
        switch (State)
        {
            case ZombieState.Idol:
                //�������߶���������Ҫ���ڵ�һ֡
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
    /// ״̬�л�
    /// </summary>
    private void FSM()
    {
        switch (state)
        {
            case ZombieState.Idol:
                State = ZombieState.Walk;
                break;
            case ZombieState.Walk:
                //һֱ�����ߣ�����ֲ��ṥ������������������
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
    /// ��ȡһ�����񣬾����ڵڼ��ų���
    /// </summary>
    /// <param name="verticalNum"></param>
    private void GetGridByVerticalNum(int verticalNum)
    {
        CurrGrid = GridManager.Instance.GetGridByVerticalNum(verticalNum);
        transform.position = new Vector3(transform.position.x, CurrGrid.position.y);
    }

    /// <summary>
    /// �ƶ�
    /// </summary>
    private void Move()
    {
        //�����ǰ����Ϊ�գ������ƶ����
        if (CurrGrid == null) return;
        CurrGrid= GridManager.Instance.GetGridByWorldPos(transform.position);
        //�����ǰ������ֲ�����ֲ������ߣ����Ҿ���ܽ�
        if (CurrGrid.havePlant
            &&CurrGrid.CurrPlantBase.transform.position.x<transform.position.x
            &&transform.position.x- CurrGrid.CurrPlantBase.transform.position.x<0.3f)
        {
            //����ֲ��
            State = ZombieState.Attack;
            return;
        }

        //�����������ߵ����񣬲������Ѿ�Խ������
        if (currGrid.point.x == 0 && currGrid.position.x - transform.position.x > 1.2f) 
        {
            //�����յ㣺����
            Vector2 pos = transform.position;
            Vector2 target = new Vector2(-9.33f, -1.33f);
            Vector2 dir = (target - pos).normalized*3f;
            transform.Translate(dir * Time.deltaTime / speed);

            //��������յ�ܽ�����ζ����Ϸ����
            if(Vector2.Distance(target,pos)<0.05f)
            {
                //������Ϸ����
                LevelManager.Instance.GameOver();
                return;
            }
            return;

        }

        transform.Translate(new Vector2(-1.33f, 0) * Time.deltaTime / speed);
    }

    /// <summary>
    /// ����ֲ��
    /// </summary>
    /// <param name="plant"></param>
    private void Attack(PlantBase plant)
    {
        isAttackState = true;
        //ֲ������߼�
        StartCoroutine(DoHurtPlant(plant));
    }

    /// <summary>
    /// �����˺���ֲ��
    /// </summary>
    /// <returns></returns>
    IEnumerator DoHurtPlant(PlantBase plant)
    {
        int num = 0;
        //ֲ��hp����0���Ѫ
        while (plant != null && plant.Hp > 0) 
        {
            if(num==5)
            {
                num = 0;
            }
            //���Ž�ʬ��ֲ�����Ч
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
    /// ��������
    /// </summary>
    public void Hurt(int attackValue)
    {
        Hp -= attackValue;
        if (gameObject.activeSelf)
            StartCoroutine(ColorEF(0.2f,new Color(0.4f,0.4f,0.4f),0.05f,null));
    }

    /// <summary>
    /// ����
    /// </summary>
    public void Dead()
    {
        StopAllCoroutines();
        currGrid = null;
        isAttackState = false;

        //���߽�ʬ������ ������
        ZombieManager.Instance.RemoveZombie(this);
        //���Լ��Ž������
        PoolManager.Instance.PushObj(GameManager.Instance.GameConf.Zombie, gameObject);
    }

    /// <summary>
    /// ��ɫЧ��
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
