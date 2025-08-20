using UnityEngine;

public class CollectableScript : MonoBehaviour
{
    public string collectableID;

    //ID naming: [ObjectName]lvl[levelNumber]number[numberOfObjectOnLevel]
    //Examples: featherlvl1number2
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
        }
    }
}