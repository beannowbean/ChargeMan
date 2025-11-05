using UnityEngine;
using UnityEngine.SceneManagement;

public class StageButton : MonoBehaviour
{
    public void ChangeToTutorial() // 미구현
    {

    }    
    public void ChangeToBoss1()
    {
        SceneManager.LoadScene("Boss1");
    }
    public void ChangeToBoss2() // 미구현
    {
        SceneManager.LoadScene("Boss2");
    }
}
