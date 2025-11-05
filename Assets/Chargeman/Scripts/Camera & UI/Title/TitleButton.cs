using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleButton : MonoBehaviour
{
    public void ChangeToSelectScene()
    {
        Debug.Log("¹öÆ° Å¬¸¯µÊ");
        SceneManager.LoadScene("StageSelect");
    }
}
