using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneManager : MonoBehaviour
{
    public void GoEndless()
    {
        //��������
        PoolManager.Instance.Clear();
        //������Ч
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
