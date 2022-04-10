using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameConf GameConf { get; private set; }

    private void Awake()
    {
        if(Instance==null)
        {
            Instance = this;
            GameConf = Resources.Load<GameConf>("GameConf");
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }


}
