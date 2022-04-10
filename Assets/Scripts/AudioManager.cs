using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// ������Ч����
    /// </summary>
    public void PlayEFMusic(AudioClip clip)
    {
        //�Ӷ���ػ�ȡһ����Ч����
        EFAudio efAudio = PoolManager.Instance.GetObj(GameManager.Instance.GameConf.EFAudio).GetComponent<EFAudio>();
        efAudio.Init(clip);
    }

}
