using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sun : MonoBehaviour
{
    //最终下落的终点PosY
    private float down_targetPosY;

    //鼠标是否点击阳光
    private bool onclick=false;

    //是否来自天空生成
    private bool isFromSky;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        
    }

    void Update()
    {
        if(!isFromSky)
        {
            Invoke("DestroySun", 5);
            return;
        }

        if(transform.position.y<=down_targetPosY)
        {
            Invoke("DestroySun", 5);
            return;
        }
        if(onclick==false)
            transform.Translate(Vector3.down * Time.deltaTime);
    }

    /// <summary>
    /// 鼠标点击阳光时，增加GameManager中的SunNum
    /// 并销毁自身
    /// </summary>
    private void OnMouseDown()
    {
        if(transform.position.y>3.7f)
        {
            return;
        }
        onclick = true;
        PlayerManager.Instance.SunNum += 25;
        Vector3 sunNum_worldPos = Camera.main.ScreenToWorldPoint(UIManager.Instance.GetSunNumTextPos());
        sunNum_worldPos = new Vector3(sunNum_worldPos.x, sunNum_worldPos.y, 0);
        FlyAnimation(sunNum_worldPos);
        AudioManager.Instance.PlayEFMusic(GameManager.Instance.GameConf.SunClick);
    }

    private void InitForAll()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        transform.localScale = new Vector2(1, 1);
        spriteRenderer.color = new Color(1, 1, 1, 1);
    }

    /// <summary>
    /// 阳光来自天空的初始化
    /// </summary>
    public void InitForSky(float down_targetPosY, float createPosX, float createPosY)
    {
        InitForAll();
        this.down_targetPosY = down_targetPosY;
        transform.position = new Vector2(createPosX, createPosY);
        isFromSky = true;
    }

    /// <summary>
    /// 阳光来自太阳花的初始化
    /// </summary>
    public void InitForSunFlower(Vector2 pos)
    {
        InitForAll();
        transform.position = pos;
        isFromSky = false;
        StartCoroutine(DoJump());
    }
    private IEnumerator DoJump()
    {
        bool isLeft = Random.Range(0, 2) == 0;
        Vector3 startPos = transform.position;
        float deltaX=0.005f;
        if(isLeft)
        {
            deltaX = -deltaX;
        }

        float speed = 0;

        while(transform.position.y<=startPos.y+1.0f&& onclick == false)
        {
            yield return new WaitForSeconds(0.001f);
            speed += 0.002f;
            transform.Translate(new Vector3(deltaX, 0.01f+speed, 0));
        }
        while (transform.position.y > startPos.y&& onclick == false)
        {
            yield return new WaitForSeconds(0.001f);
            speed -= 0.002f;
            transform.Translate(new Vector3(deltaX, -0.01f+speed, 0));
        }
    

    }

    /// <summary>
    /// 飞行动画
    /// </summary>
    private void FlyAnimation(Vector3 pos)
    {
        transform.localScale = new Vector2(0.7f, 0.7f);
        spriteRenderer.color = new Color(1, 1, 1, 0.6f);
        StartCoroutine(DoFly(pos));
    }
    private IEnumerator DoFly(Vector3 pos)
    {
        Vector3 direction = (pos - transform.position).normalized;
        while(Vector3.Distance(pos,transform.position)>0.1f)
        {
            yield return new WaitForSeconds(0.001f);
            if(Vector3.Distance(pos, transform.position) > 0.5f)
                transform.Translate(direction*0.15f);
            else
                transform.Translate(direction * 0.02f);
        }
        DestroySun();
    }

    /// <summary>
    /// 销毁自身
    /// </summary>
    private void DestroySun()
    {
        //取消自身全部协程和延迟调用
        StopAllCoroutines();
        CancelInvoke();
        onclick = false;
        //放进缓存池，不做真实销毁
        PoolManager.Instance.PushObj(GameManager.Instance.GameConf.Sun, gameObject);
    }
}
