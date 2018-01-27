using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {
    
    void Start () {

        World.Instance.InitTest().Draw();

        int[,] hm = GenerateHeatMap();
        string s = "\n";
        for (int i = 0; i < hm.GetLength(0); i++)
        {
            for (int j = 0; j < hm.GetLength(1); j++)
            {

                s += hm[i, j] < 0 ? "#" : ""+hm[i, j];
            }
            s += "\n";
        }
        Debug.Log(s);
    }


    void Update () {

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                int x = (int)(hit.point.x + 0.5);
                int y = (int)(hit.point.y + 0.5);
                int z = (int)(hit.point.z + 0.5);
                Vector3 loc = new Vector3(x, y, z);
                Debug.Log(loc);
            }
        }
    }

    public int[,] GenerateHeatMap()
    {
        World w = World.Instance;
        World.Cell[,] c = w.GetGrid().GetCells();

        int[,] heatMap = new int[c.GetLength(0), c.GetLength(1)];
        for (int i = 0; i < c.GetLength(0); i++)
            for (int j = 0; j < c.GetLength(1); j++)
                heatMap[i, j] = -1;

        Queue<Pos> queue = new Queue<Pos>();
        Pos pos = new Pos(1, 1);
        queue.Enqueue(pos);
        heatMap[pos.x, pos.y] = 0; ;
        int xm = c.GetLength(0);
        int ym = c.GetLength(1);

        while (queue.Count > 0)
        {
            Pos cell = queue.Dequeue();
            Debug.Log(cell.x + " " + cell.y);
            int cost = heatMap[cell.x, cell.y] + 1;
            Debug.Log(cell.x + 1 < xm && !c[cell.x + 1, cell.y].IsBlocked && heatMap[cell.x + 1, cell.y] == -1);
            if (cell.x + 1 < xm && !c[cell.x + 1, cell.y].IsBlocked && heatMap[cell.x + 1, cell.y] == -1)
            {
                heatMap[cell.x + 1, cell.y] = cost;
                queue.Enqueue(new Pos(cell.x + 1, cell.y));
            }
            Debug.Log(cell.y + 1 < ym && !c[cell.x, cell.y + 1].IsBlocked && heatMap[cell.x, cell.y + 1] == -1);
            if (cell.y + 1 < ym && !c[cell.x, cell.y + 1].IsBlocked && heatMap[cell.x, cell.y + 1] == -1)
            {
                heatMap[cell.x, cell.y + 1] = cost;
                queue.Enqueue(new Pos(cell.x, cell.y + 1));
            }
            Debug.Log(!(cell.x - 1 < 0) && !c[cell.x - 1, cell.y].IsBlocked && heatMap[cell.x - 1, cell.y] == -1);
            if (!(cell.x - 1 < 0) && !c[cell.x - 1, cell.y].IsBlocked && heatMap[cell.x - 1, cell.y] == -1)
            {
                heatMap[cell.x - 1, cell.y] = cost;
                queue.Enqueue(new Pos(cell.x - 1, cell.y));
            }
            Debug.Log(!(cell.y - 1 < 0) && !c[cell.x, cell.y - 1].IsBlocked && heatMap[cell.x, cell.y - 1] == -1);
            if (!(cell.y - 1 < 0) && !c[cell.x, cell.y - 1].IsBlocked && heatMap[cell.x, cell.y - 1] == -1)
            {
                heatMap[cell.x, cell.y - 1] = cost;
                queue.Enqueue(new Pos(cell.x, cell.y - 1));
            }
        }

        return heatMap;
    }

    class Pos
    {
        public int x;
        public int y;

        public Pos(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
}


