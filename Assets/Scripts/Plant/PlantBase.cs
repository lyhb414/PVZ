using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// ֲ�����
/// </summary>
public abstract class PlantBase : MonoBehaviour
{
    protected Animator animator;
    protected SpriteRenderer spriteRenderer;
    /// <summary>
    /// ��ǰֲ�����ڵ�����
    /// </summary>
    protected Grid currGrid;
    /// <summary>
    /// ֲ������ֵ
    /// </summary>
    protected float hp;

    protected PlantManager.PlantType plantType;

    public float Hp { get => hp; }
    public abstract float MaxHp { get; }

    /// <summary>
    /// �κ�����µ�ͨ�ó�ʼ��
    /// </summary>
    protected void InitForAll(PlantManager.PlantType type)
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        plantType = type;
    }

    /// <summary>
    /// ����ֲ��ʱ�ĳ�ʼ��
    /// </summary>
    public void InitForCreate(bool inGrid, PlantManager.PlantType type,Vector2 pos)
    {
        InitForAll(type);
        transform.position = pos;
        animator.speed = 0;
        if (inGrid)
        {
            spriteRenderer.sortingOrder = -1;
            spriteRenderer.color = new Color(1, 1, 1, 0.6f);
        }
        else
        {
            spriteRenderer.color = new Color(1, 1, 1, 1);
            spriteRenderer.sortingOrder = 1;
        }
    }

    /// <summary>
    /// ��ֲֲ��ʱ�ĳ�ʼ��
    /// </summary>
    public void InitForPlant(Grid grid, PlantManager.PlantType type)
    {
        InitForAll(type);
        spriteRenderer.color = new Color(1, 1, 1, 1);
        hp = MaxHp;
        currGrid = grid;
        transform.position = grid.position;
        currGrid.CurrPlantBase = this;
        animator.speed = 1;
        spriteRenderer.sortingOrder = 0;
        OnInitForPlant();
    }

    /// <summary>
    /// ���˷���������ʬ����ʱ����
    /// </summary>
    public virtual void Hurt(float hurtValue)
    {
        hp -= hurtValue;
        //����
        StartCoroutine(ColorEF(0.2f,new Color(0.5f,0.5f,0.5f),0.05f,null));

        if(Hp<=0)
        {
            //����
            Dead();
        }
    }

    /// <summary>
    /// ��ɫЧ��
    /// </summary>
    /// <returns></returns>
    protected IEnumerator ColorEF(float wantTime,Color targetColor,float delayTime,UnityAction func)
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
        
        if(func!=null)
        {
            func();
        }
    }

    public void Dead()
    {
        if (currGrid != null)
        {
            currGrid.CurrPlantBase = null;
            currGrid = null;
        }

        StopAllCoroutines();
        CancelInvoke();

        PoolManager.Instance.PushObj(PlantManager.Instance.GetPlantByType(plantType), gameObject);
    }



    protected virtual void OnInitForPlant()
    {

    }
}
