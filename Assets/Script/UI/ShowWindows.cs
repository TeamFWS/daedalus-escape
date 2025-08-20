using DG.Tweening;
using Script.Animation;
using UnityEngine;

public class ShowWindows : MonoBehaviour
{
    [SerializeField] private GameObject options;
    [SerializeField] private GameObject map;
    [SerializeField] private GameObject toDisable;
    private GameObject _active;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M)) ActivateMap();
        if (Input.GetKeyDown(KeyCode.O)) ActivateOptions();
        if (!Input.GetKeyDown(KeyCode.Escape)) return;
        if (options.activeSelf || map.activeSelf)
            DeactivateAll();
        else
            ActivateOptions();
    }

    public void ActivateOptions()
    {
        ActivateWindow(options);
    }

    public void ActivateMap()
    {
        ActivateWindow(map);
    }

    private void ActivateWindow(GameObject objectToActivate)
    {
        if (objectToActivate.activeSelf) return;
        DeactivateAllImmediate();
        ScaleAnimation.ScaleX(objectToActivate);
        toDisable.SetActive(false);
        objectToActivate.SetActive(true);
        _active = objectToActivate;
    }

    public void DeactivateAll()
    {
        if (_active) ScaleAnimation.ScaleX(_active, 0.3f, true).OnComplete(DeactivateAllImmediate);
        else DeactivateAllImmediate();
    }

    private void DeactivateAllImmediate()
    {
        options.SetActive(false);
        map.SetActive(false);
        toDisable.SetActive(true);
    }
}