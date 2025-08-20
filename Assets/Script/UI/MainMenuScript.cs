using DG.Tweening;
using Script.Animation;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField] private GameObject LevelsOverview;
    [SerializeField] private GameObject LoreOverview;
    [SerializeField] private GameObject LoreDetails;
    [SerializeField] private GameObject Options;
    private GameObject _active;

    private void Start()
    {
        DeactivateAllImmediate();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) DeactivateAll();
    }

    public void DeactivateAll()
    {
        if (_active) ScaleAnimation.ScaleX(_active, 0.3f, true).OnComplete(DeactivateAllImmediate);
        else DeactivateAllImmediate();
    }

    private void DeactivateAllImmediate()
    {
        LevelsOverview.SetActive(false);
        LoreOverview.SetActive(false);
        LoreDetails.SetActive(false);
        Options.SetActive(false);
    }

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void ActivateLevels()
    {
        Activate(LevelsOverview);
    }

    public void ActivateLore()
    {
        Activate(LoreOverview);
    }

    public void ActivateLoreDetails()
    {
        Activate(LoreDetails);
    }

    public void ActivateOptions()
    {
        Activate(Options);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    private void Activate(GameObject objectToActivate)
    {
        DeactivateAllImmediate();
        ScaleAnimation.ScaleX(objectToActivate);
        objectToActivate.SetActive(true);
        _active = objectToActivate;
    }
}