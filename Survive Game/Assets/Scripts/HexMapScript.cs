using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexMapScript : MonoBehaviour {
    public Transform HexPrefab;

    public int GridWeight = 11;
    public int GridHeight = 11;

    float HexWidth = 1;
    float HexHeight = 1.20f;
    public float Gap = 0.0f;

    Vector3 startPos;

    void Start()
    {
        AddGap();
        CalcStartPos();
        CreateGrid();
    }

    void AddGap()
    {
        HexWidth += HexWidth * Gap;
        HexHeight += HexHeight * Gap;
    }

    void CalcStartPos()
    {
        float Offset = 0;
        // if odd number 
        if (GridHeight / 2 % 2 != 0)
        {
            Offset = HexWidth / 2;
        }
        // Adds offset to the Hexs X pos to make hexes fit together
        float x = -HexWidth * (GridWeight / 2) - Offset;
        float z = HexHeight * 0.75f * (GridHeight / 2);

        startPos = new Vector3(x, 0, z);
    }

    Vector3 CalcWorldPos(Vector2 gridPos)
    {
        float Offset = 0;
        // If odd number
        if (gridPos.y % 2 != 0)
        {
            Offset = HexWidth / 2;
        }
        float x = startPos.x + gridPos.x * HexWidth + Offset;
        float z = startPos.z - gridPos.y * HexHeight * 0.75f;

        return new Vector3(x, 0, z);
    }

    void CreateGrid()
    {
        for (int y = 0; y < GridHeight; y++)
        {
            for (int x = 0; x < GridWeight; x++)
            {
                Transform Hex = Instantiate(HexPrefab) as Transform;
                Vector2 GridPos = new Vector2(x, y);
                Hex.position = CalcWorldPos(GridPos);
                Hex.parent = this.transform;
                Hex.name = "Hexagon" + x + "|" + y;
            }
        }
    }
}