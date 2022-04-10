using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid
{
    /// <summary>
    /// 坐标点:(0,1)(1,1)...
    /// </summary>
    public Vector2 point;

    /// <summary>
    /// 世界坐标
    /// </summary>
    public Vector2 position;

    /// <summary>
    /// 该格是否有植物
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
