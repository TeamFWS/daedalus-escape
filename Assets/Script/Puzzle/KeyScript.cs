using Script.Puzzle;
using UnityEngine;

public class KeyScript : MonoBehaviour
{
    public string collectableID;
    private PuzzleManager manager;

    //ID naming: [ObjectName]lvl[levelNumber]number[numberOfObjectOnLevel]
    //Examples:keylvl2number1
    private void Start()
    {
        if (PlayerPrefs.GetInt(collectableID) == 1) gameObject.SetActive(false);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerPrefs.SetInt(collectableID, 1);
            gameObject.SetActive(false);
            manager = GameObject.FindGameObjectsWithTag("PuzzleManager")[0].GetComponent<PuzzleManager>();
            manager.CurrentPuzzleFinished();
        }
    }
}