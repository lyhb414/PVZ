using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WallNumState
{
    Idol,
    State1,
    State2
}

public class WallNut : PlantBase
{
    public override float MaxHp
    {
        get
        {
            return 4000;
        }
    }


    private WallNumState state;

    public WallNumState State { get => state; 
        set
        {
            state = value;
            switch (state)
            {
                case WallNumState.Idol:
                    animator.Play("WallNut", 0, 0);
                    break;
                case WallNumState.State1:
                    animator.Play("WallNut_1", 0, animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
                    break;
                case WallNumState.State2:
                    animator.Play("WallNut_2", 0, animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
                    break;
            }


        }
    }

    void Start()
    {
        
    }


    void Update()
    {
        
    }

    public override void Hurt(float hurtValue)
    {
        base.Hurt(hurtValue);
        if (MaxHp * (1.0f / 3) < Hp && Hp < MaxHp * (2.0f / 3))
        {
            State = WallNumState.State1;
        }
        if (MaxHp * (1.0f / 3) >= Hp && Hp > 0) 
        {
            State = WallNumState.State2;
        }
    }

    protected override void OnInitForPlant()
    {
        State = WallNumState.Idol;
    }
}
