using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie_Head : MonoBehaviour
{
    private Animator animator;
    private bool isOver;

    public void Init(Vector3 pos)
    {
        animator = GetComponent<Animator>();

        transform.position = pos;
        animator.speed = 1;
        isOver = false;
        animator.Play("Zombie_Head", 0, 0);

    }

    // Update is called once per frame
    void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1 && !isOver) 
        {
            //�������
            animator.speed = 0;
            isOver = true;
            Invoke("Destroy", 2);
        }
    }

    private void Destroy()
    {
        //ȡ���ӳٵ���
        CancelInvoke();
        //���Լ��Ž������
        PoolManager.Instance.PushObj(GameManager.Instance.GameConf.Zombie_Head, gameObject);
    }
}
