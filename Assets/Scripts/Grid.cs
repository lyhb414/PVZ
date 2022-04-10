using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid
{
    /// <summary>
    /// �����:(0,1)(1,1)...
    /// </summary>
    public Vector2 point;

    /// <summary>
    /// ��������
    /// </summary>
    public Vector2 position;

    /// <summary>
    /// �ø��Ƿ���ֲ��
    /// </summary>
    public bool havePlant;

    private PlantBase currPlantBase;

    public Grid(Vector2 point, Vector2 position, bool havePlant)
    {
        this.point = point;
        this.position = position;
        this.havePlant = havePlant;
    }

    public PlantBase CurrPlantBase { get => currPlantBase;
        set
        {
            currPlantBase = value;
            if(currPlantBase==null)
            {
                havePlant = false;
            }
            else
            {
                havePlant = true;
            }
        }

    }
}
