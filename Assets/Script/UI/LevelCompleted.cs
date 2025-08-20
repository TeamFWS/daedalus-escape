using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelCompleted : MonoBehaviour
{
    [SerializeField] private GameObject window;
    [SerializeField] private GameObject nextLevelButton;
    private string _nextLevel;

    private void Start()
    {
        _nextLevel = SceneManager.GetActiveScene().name switch
        {
            "Overworld" => "Cave",
            "Cave" => "Overworld2",
            "Overworld2" => "Cave2",
            _ => "MainMenu"
        };

        if (_nextLevel == "MainMenu") nextLevelButton.SetActive(false);
        window.SetActive(false);
    }

    private void OnEnable()
    {
        LevelEnd.OnLevelFinished += Activate;
    }

    private void OnDisable()
    {
        LevelEnd.OnLevelFinished -= Activate;
    }

    private void Activate()
    {
        window.SetActive(true);
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(_nextLevel);
    }
}