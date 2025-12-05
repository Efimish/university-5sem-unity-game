using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenuScript : MonoBehaviour
{

    public void Update()
    {
        if (Input.anyKeyDown)
        {
            OpenLevelsList();
        }
    }
    public void PlayCurrentLevel()
    {
        
    }

    public void OpenLevelsList()
    {
        SceneManager.LoadScene(1);
    }
}
