using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType 
{
    Normal,
    Obstacle,
    Breakable
}

[RequireComponent(typeof(SpriteRenderer))]
public class Tile : MonoBehaviour
{
    public int xIndex;
    public int yIndex;

    Board m_board;

    public TileType tileType = TileType.Normal;

    SpriteRenderer m_spriteRenderer;

    public int breakableValue = 0;
    public Sprite[] breakableSprites;

    public Color normalColor;

    // Start is called before the first frame update
    void Awake()
    {
        m_spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Init(int x, int y, Board board)
    {
        xIndex = x;
        yIndex = y;
        m_board = board;
        
        if(tileType == TileType.Breakable)
        {
            if (breakableSprites[breakableValue] != null) 
            {
                m_spriteRenderer.sprite = breakableSprites[breakableValue]; //change the sprite according to the breakableValue.
            }
        }


    }

    void OnMouseDown()
    {
        if (m_board != null)
        {
            m_board.ClickTile(this);
        }
    }

    void OnMouseEnter()
    {
        if (m_board != null)
        {
            m_board.DragToTile(this);
        }
    }

    void OnMouseUp()
    {
        if (m_board != null)
        {
            m_board.ReleaseTile();
        }
    }

    public void BreakTile()
    {
        if (tileType != TileType.Breakable) {
            return; // only run if breakable, otherwise we want to return immeadiately.
        }

        StartCoroutine(BreakTileRoutine());
    }

    IEnumerator BreakTileRoutine()
    {
        breakableValue--;
        breakableValue = Mathf.Clamp(breakableValue, 0, breakableValue);

        yield return new WaitForSeconds(0.25f);

        if (breakableSprites[breakableValue] != null) 
        {
            m_spriteRenderer.sprite = breakableSprites[breakableValue]; //change the sprite according to the breakableValue.
        }

        // if it gets all the way down to zero, then we want to become a normal tile
        if(breakableValue <= 0)
        {
            tileType = TileType.Normal;
            m_spriteRenderer.color = normalColor;

        }
    }
}
