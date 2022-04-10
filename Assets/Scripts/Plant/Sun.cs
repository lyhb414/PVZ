using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sun : MonoBehaviour
{
    //����������յ�PosY
    private float down_targetPosY;

    //����Ƿ�������
    private bool onclick=false;

    //�Ƿ������������
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
    /// ���������ʱ������GameManager�е�SunNum
    /// ����������
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
    /// ����������յĳ�ʼ��
    /// </summary>
    public void InitForSky(float down_targetPosY, float createPosX, float createPosY)
    {
        InitForAll();
        this.down_targetPosY = down_targetPosY;
        transform.position = new Vector2(createPosX, createPosY);
        isFromSky = true;
    }

    /// <summary>
    /// ��������̫�����ĳ�ʼ��
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
    /// ���ж���
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
    /// ��������
    /// </summary>
    private void DestroySun()
    {
        //ȡ������ȫ��Э�̺��ӳٵ���
        StopAllCoroutines();
        CancelInvoke();
        onclick = false;
        //�Ž�����أ�������ʵ����
        PoolManager.Instance.PushObj(GameManager.Instance.GameConf.Sun, gameObject);
    }
}
