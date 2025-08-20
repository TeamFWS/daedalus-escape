using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    public Sprite activeSprite;
    private Sprite baseSprite;
    private SpriteRenderer spriteRenderer;
    private int state;

    private void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        baseSprite = spriteRenderer.sprite;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" || collision.tag == "HeavyObject") state += 1;
        UpdateSprite();
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player" || collision.tag == "HeavyObject") state -= 1;
        UpdateSprite();
    }

    public bool getState()
    {
        return state > 0;
    }

    private void UpdateSprite()
    {
        if (state > 0)
            spriteRenderer.sprite = activeSprite;
        else
            spriteRenderer.sprite = baseSprite;
    }
}