using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private new Rigidbody2D rigidbody;
    private SpriteRenderer spriteRenderer;

    //�ӵ��˺�
    private int attackValue;

    //�Ƿ���� 
    private bool isHit ;

    

    public void Init(int attackValue,Vector2 pos)
    {
        transform.position = pos;
        rigidbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        rigidbody.AddForce(Vector2.right*300);
        this.attackValue = attackValue;

        isHit = false;
        rigidbody.gravityScale = 0;
        //�޸ĳ������ӵ�ͼƬ
        spriteRenderer.sprite = GameManager.Instance.GameConf.Bullet1Nor;
    }

    void Update()
    {
        if (isHit) return;
        if(transform.position.x>7.7f)
        {
            Destroy();
            return;
        }

        transform.Rotate(new Vector3(0, 0, -5));
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (isHit) return;

        if(coll.tag=="Zombie")
        {
            isHit = true;

            //���Ž�ʬ���㶹��������Ч
            AudioManager.Instance.PlayEFMusic(GameManager.Instance.GameConf.ZombieHurtForPea);

            //����˺�
            coll.GetComponentInParent<Zombie>().Hurt(attackValue);

            //�޸ĳ��ӵ�����ͼƬ
            spriteRenderer.sprite = GameManager.Instance.GameConf.Bullet1Hit;

            //��ͣ�����˶�
            rigidbody.velocity = Vector2.zero;

            //����
            rigidbody.gravityScale = 1;

            //��������
            Invoke("Destroy", 0.5f);
        }
    }

    private void Destroy()
    {
        //ȡ���ӳٵ���
        CancelInvoke();
        //���Լ��Ž������
        PoolManager.Instance.PushObj(GameManager.Instance.GameConf.Bullet1,gameObject);
    }
}
