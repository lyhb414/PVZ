using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SetPanel : MonoBehaviour
{
    public void Show(bool isShow)
    {
        gameObject.SetActive(isShow);
        //�����ʾ����������Ϸ��ͣ
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
    /// ������ҳ
    /// </summary>
    public void BackMainScene()
    {
        //��������
        PoolManager.Instance.Clear();
        //������Ч
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
