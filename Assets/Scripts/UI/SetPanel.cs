using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SetPanel : MonoBehaviour
{
    public void Show(bool isShow)
    {
        gameObject.SetActive(isShow);
        //如果显示出来，则游戏暂停
        if(isShow)
        {
            Time.timeScale = 0;
        }
        else
        {
            AudioManager.Instance.PlayEFMusic(GameManager.Instance.GameConf.ButtonClick);
            Time.timeScale = 1;
        }
    }

    /// <summary>
    /// 返回首页
    /// </summary>
    public void BackMainScene()
    {
        //清理对象池
        PoolManager.Instance.Clear();
        //播放音效
        AudioManager.Instance.PlayEFMusic(GameManager.Instance.GameConf.ButtonClick);
        Time.timeScale = 1;
        Invoke("DoBackMainScene", 0.5f);
    }
    private void DoBackMainScene()
    {
        SceneManager.LoadScene("Start");
    }

    public void Quit()
    {
        AudioManager.Instance.PlayEFMusic(GameManager.Instance.GameConf.ButtonClick);
        Application.Quit();
    }
}
