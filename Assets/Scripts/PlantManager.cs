using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantManager : MonoBehaviour
{
    public static PlantManager Instance;

    public enum PlantType
    {
        //太阳花
        SunFlower,
        //豌豆射手
        Peashooter,
        //坚果墙
        WallNut
    }

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        
    }

    public GameObject GetPlantByType(PlantType type)
    {
        switch(type)
        {
            case PlantType.SunFlower:
                return GameManager.Instance.GameConf.SunFlower;
            case PlantType.Peashooter:
                return GameManager.Instance.GameConf.Peashooter;
            case PlantType.WallNut:
                return GameManager.Instance.GameConf.Wallnut;
        }
        return null;
    }
}
