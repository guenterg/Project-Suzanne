﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    #region Variables
    [Header("Maze Variables")]
    [Range(3, 2000)] public int columsCount = 3;
    [Range(3, 2000)] public int rowsCount = 3;
    public static int cols, rows;

    [Header("Cell Variables")]
    public GameObject cellPrefab;
    public static List<Cell> grid = new List<Cell>();
    public static List<Cell> stack = new List<Cell>();
    public static List<Cell> correctPath = new List<Cell>();
    public static List<GameObject> deleteCell = new List<GameObject>();
    private Cell current;
    private Cell next;
    private int pathIndex = 0;
    private bool isPath = false;
    #endregion

    #region Methods
    private void Start()
    {
        SetupGrid();

        do
        {
            MazeLogic();
        } while (stack.Count > 0);
    }

    //Basic Methods
    private void SetupGrid()
    {
        cols = columsCount;
        rows = rowsCount;

        for (int q = 0; q < rows; q++)
        {
            for (int i = 0; i < cols; i++)
            {
                Cell cell = new Cell();
                cell.x = i;
                cell.z = q;

                grid.Add(cell);
            }
        }
        for (int i = 0; i < grid.Count; i++)
        {
            grid[i].prefab = Instantiate(cellPrefab, new Vector3(grid[i].x, 0, grid[i].z), Quaternion.identity, transform);
        }
        current = grid[0];
    }

    private void MazeLogic()
    {
        current.visited = true;
        //Step 1
        next = current.CheckNeighbors();
        current.neighbors = new List<Cell>();
        if (next != null)
        {
            next.visited = true;
            //Step 2
            stack.Add(current);
            //Step 3
            RemoveWalls(current, next);
            //Step 4
            current = next;
        }
        else if (stack.Count > 0)
        {
            current = stack[stack.Count - 1];
            stack.RemoveAt(stack.Count - 1);
        }
        else
        {
            foreach (GameObject go in deleteCell)
            {
                Destroy(go);
            }
            if (pathIndex < correctPath.Count - 1)
            {
                pathIndex++;
                Debug.Log(pathIndex);
            }
        }
    }
    private void RemoveWalls(Cell a, Cell b)
    {
        int x = a.x - b.x;
        if (x == 1)
        {
            a.walls[0] = false;
            b.walls[2] = false;
        }
        else if (x == -1)
        {
            a.walls[2] = false;
            b.walls[0] = false;
        }

        int z = a.z - b.z;
        if (z == 1)
        {
            a.walls[3] = false;
            b.walls[1] = false;
        }
        else if (z == -1)
        {
            a.walls[1] = false;
            b.walls[3] = false;
        }

        a.CalculateWalls();
        b.CalculateWalls();
    }

    private void CorrectPath()
    {
        if (isPath == false && current.x == cols - 1 && current.z == rows - 1)
        {
            for (int i = 0; i < stack.Count; i++)
            {
                correctPath.Add(stack[i]);
            }
            isPath = true;
        }
    }
    #endregion
}