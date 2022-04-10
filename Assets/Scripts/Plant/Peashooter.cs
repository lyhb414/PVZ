using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Peashooter : PlantBase
{
    public override float MaxHp
    {
        get
        {
            return 300;
        }
    }

    //是否可以攻击
    private bool canAttack ;

    //攻击间隔
    private float attackCD = 1.4f;

    //我的攻击力
    private int attackValue = 20;

    //创建子弹的偏移量坐标
    private Vector3 createBulletOffsetPos = new Vector2(0.547f, 0.399f);

    void Start()
    {
        
    }

    void Update()
    {
        
    }
    protected override void OnInitForPlant()
    {
        canAttack = true;
        //可能要攻击
        InvokeRepeating("Attack", 0.4f, 0.2f);
    }

    /// <summary>
    /// 攻击方法 循环检测
    /// </summary>
    private void Attack()
    {
        if(canAttack==false)
        {
            return;
        }

        //从僵尸管理器 获取一个在我右边的离我最近的僵尸
        Zombie zombie = ZombieManager.Instance.GetZombieByLineMinDistance((int)currGrid.point.y, transform.position);


        //若没有僵尸，返回
        if (zombie == null)
        {
            return; 
        }
        //若僵尸不在草坪上，返回
        if (zombie.CurrGrid.point.x == 8 && Vector2.Distance(zombie.transform.position, zombie.CurrGrid.position) > 2f)
        {
            return;
        }

        //开始攻击
        //实例化一个子弹
        Bullet bullet = PoolManager.Instance.GetObj(GameManager.Instance.GameConf.Bullet1).GetComponent<Bullet>();
        bullet.transform.SetParent(transform);
        bullet.Init(attackValue, transform.position + createBulletOffsetPos);
        canAttack = false;
        StartCD();
    }

    /// <summary>
    /// 开始进入CD
    /// </summary>
    private void StartCD()
    {
        StartCoroutine(CalCD());
    }

    /// <summary>
    /// 计算CD
    /// </summary>
    /// <returns></returns>
    IEnumerator CalCD()
    {
        yield return new WaitForSeconds(attackCD);
        canAttack = true;
    }
}
