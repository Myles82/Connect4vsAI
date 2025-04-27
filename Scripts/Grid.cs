using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField] int[,] grid = { { 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0 } };
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P)) {
            //updateGrid();
            PrintGrid();

        }
    }
    public void PrintGrid()
    {
        string output = "";

        for (int row = 0; row < grid.GetLength(0); row++)
        {
            for (int col = 0; col < grid.GetLength(1); col++)
            {
                output += grid[row, col] + " ";
            }
            output += "\n";
        }

        Debug.Log(output);
    }
    public int[,] updateGrid()
    {
        Array.Clear(grid, 0, grid.Length);
        Vector2 center = transform.position;
        Vector2 size = GetComponent<BoxCollider2D>().size;
        Collider2D[] hits = Physics2D.OverlapBoxAll(center,size,0f,LayerMask.GetMask("Chips"));

        foreach (Collider2D hit in hits)
        {
            
            if (hit.gameObject.CompareTag("chip"))
            {
                chipScript chip = hit.gameObject.GetComponent<chipScript>();
                var pos = chip.getGridPosition();
                updatePos(pos.Item1,pos.Item2,chip.playerNum);
                chip.Loose = false;
            }
        }
        return grid;
    }

    void OnDrawGizmos()
    {
        Vector2 center = transform.position;
        Vector2 size = GetComponent<BoxCollider2D>().size; 
        Vector2 halfSize = size / 2f;

        Vector3 topLeft = center + new Vector2(-halfSize.x, halfSize.y);
        Vector3 topRight = center + new Vector2(halfSize.x, halfSize.y);
        Vector3 bottomRight = center + new Vector2(halfSize.x, -halfSize.y);
        Vector3 bottomLeft = center + new Vector2(-halfSize.x, -halfSize.y);

        Gizmos.color = Color.green;

        Gizmos.DrawLine(topLeft, topRight);
        Gizmos.DrawLine(topRight, bottomRight);
        Gizmos.DrawLine(bottomRight, bottomLeft);
        Gizmos.DrawLine(bottomLeft, topLeft);
    }

    void updatePos(int row,int col,int num)
    {
        Debug.Log(row + " " + col);
        grid[row,col] = num; 
    }

    public bool isRoomInColumn(int col)
    {
        return grid[0, col-1] == 0;
    }
}
