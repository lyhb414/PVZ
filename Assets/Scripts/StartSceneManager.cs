using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneManager : MonoBehaviour
{
    public void GoEndless()
    {
        //清理对象池
        PoolManager.Instance.Clear();
        //播放音效
        AudioManager.Instance.PlayEFMusic(GameManager.Instance.GameConf.ButtonClick);

        Invoke("DoGoEndless", 0.5f);
    }
    private void DoGoEndless()
    {
        SceneManager.LoadScene("Endless");
    }

    public void Quit()
    {
        Application.Quit();
    }

}
