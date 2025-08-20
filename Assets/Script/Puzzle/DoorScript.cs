using UnityEngine;
using UnityEngine.Tilemaps;

public class DoorScript : MonoBehaviour
{
    public Tilemap tileMap;
    public Vector3Int position;
    public int buttonType; // 1 - AND button, 2 - OR button, 3 - NAND button, 4 - NOR button
    public GameObject[] buttons;
    private bool all;
    private bool any;
    private Tile tile;

    private void Start()
    {
        tile = (Tile)tileMap.GetTile(position);
    }

    private void Update()
    {
        any = false;
        all = true;
        foreach (var button in buttons)
            if (button.GetComponent<ButtonScript>().getState())
                any = true;
            else
                all = false;

        switch (buttonType)
        {
            case 1:
                if (all)
                    OpenDoors();
                else
                    CloseDoors();
                break;
            case 2:
                if (any)
                    OpenDoors();
                else
                    CloseDoors();
                break;
            case 3:
                if (!all)
                    OpenDoors();
                else
                    CloseDoors();
                break;
            case 4:
                if (!any)
                    OpenDoors();
                else
                    CloseDoors();
                break;
        }
    }

    private void OpenDoors()
    {
        tileMap.SetTile(position, null);
    }

    private void CloseDoors()
    {
        tileMap.SetTile(position, tile);
    }
}