using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 植物基类
/// </summary>
public abstract class PlantBase : MonoBehaviour
{
    protected Animator animator;
    protected SpriteRenderer spriteRenderer;
    /// <summary>
    /// 当前植物所在的网格
    /// </summary>
    protected Grid currGrid;
    /// <summary>
    /// 植物生命值
    /// </summary>
    protected float hp;

    protected PlantManager.PlantType plantType;

    public float Hp { get => hp; }
    public abstract float MaxHp { get; }

    /// <summary>
    /// 任何情况下的通用初始化
    /// </summary>
    protected void InitForAll(PlantManager.PlantType type)
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        plantType = type;
    }

    /// <summary>
    /// 创建植物时的初始化
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
    /// 种植植物时的初始化
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
    /// 受伤方法，被僵尸攻击时调用
    /// </summary>
    public virtual void Hurt(float hurtValue)
    {
        hp -= hurtValue;
        //发光
        StartCoroutine(ColorEF(0.2f,new Color(0.5f,0.5f,0.5f),0.05f,null));

        if(Hp<=0)
        {
            //死亡
            Dead();
        }
    }

    /// <summary>
    /// 颜色效果
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
