using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionsScript : MonoBehaviour
{
    public void ReturnToMenu(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}