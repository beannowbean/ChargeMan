using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleButton : MonoBehaviour
{
    public void ChangeToPlayScene()
    {
        SceneManager.LoadScene("Boss1");
    }

    public void ChangeToSelectScene()
    {
        SceneManager.LoadScene("StageSelect");
    }
}
