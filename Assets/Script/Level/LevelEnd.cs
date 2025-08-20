using System;
using DG.Tweening;
using Script.Animation;
using UnityEngine;

public class LevelEnd : MonoBehaviour
{
    public string lvl;
    private PlayerAnimation _playerAnimation;

    private void Start()
    {
        var player = GameObject.FindWithTag("Player").GetComponent<Transform>();
        _playerAnimation = new PlayerAnimation(player.GetComponent<SpriteRenderer>(), player.transform);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            if (AreKeysCollected())
                _playerAnimation.AnimateEntering(transform).OnComplete(EndLevel);
    }

    public static event Action OnLevelFinished;

    public bool AreKeysCollected()
    {
        var key1 = PlayerPrefs.GetInt("keylvl" + lvl + "number1");
        var key2 = PlayerPrefs.GetInt("keylvl" + lvl + "number2");
        var key3 = PlayerPrefs.GetInt("keylvl" + lvl + "number3");
        return key1 == 1 && key2 == 1 && key3 == 1;
    }

    private void EndLevel()
    {
        OnLevelFinished?.Invoke();
    }
}