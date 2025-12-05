using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class ButtonChoseLevel : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    public void Select(int numberInBuild)
    {
        SceneManager.LoadScene(numberInBuild);
    }
}
