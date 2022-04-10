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

    //�Ƿ���Թ���
    private bool canAttack ;

    //�������
    private float attackCD = 1.4f;

    //�ҵĹ�����
    private int attackValue = 20;

    //�����ӵ���ƫ��������
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
        //����Ҫ����
        InvokeRepeating("Attack", 0.4f, 0.2f);
    }

    /// <summary>
    /// �������� ѭ�����
    /// </summary>
    private void Attack()
    {
        if(canAttack==false)
        {
            return;
        }

        //�ӽ�ʬ������ ��ȡһ�������ұߵ���������Ľ�ʬ
        Zombie zombie = ZombieManager.Instance.GetZombieByLineMinDistance((int)currGrid.point.y, transform.position);


        //��û�н�ʬ������
        if (zombie == null)
        {
            return; 
        }
        //����ʬ���ڲ�ƺ�ϣ�����
        if (zombie.CurrGrid.point.x == 8 && Vector2.Distance(zombie.transform.position, zombie.CurrGrid.position) > 2f)
        {
            return;
        }

        //��ʼ����
        //ʵ����һ���ӵ�
        Bullet bullet = PoolManager.Instance.GetObj(GameManager.Instance.GameConf.Bullet1).GetComponent<Bullet>();
        bullet.transform.SetParent(transform);
        bullet.Init(attackValue, transform.position + createBulletOffsetPos);
        canAttack = false;
        StartCD();
    }

    /// <summary>
    /// ��ʼ����CD
    /// </summary>
    private void StartCD()
    {
        StartCoroutine(CalCD());
    }

    /// <summary>
    /// ����CD
    /// </summary>
    /// <returns></returns>
    IEnumerator CalCD()
    {
        yield return new WaitForSeconds(attackCD);
        canAttack = true;
    }
}
