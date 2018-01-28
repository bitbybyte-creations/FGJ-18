using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World
{
    private static event System.Action<World> OnWorldInitializedEvent;

    private bool _init = false;
    private static World _world;
    private Grid _grid;
    private int[,] _heatMap;

    private World()
    {

    }

    public static World GetInstance()
    {
        if (_world == null)
            _world = new World();
        return _world;
    }

    public static World Instance
    {
        get
        {
            return GetInstance();
        }
    }

    public World InitTest()
    {
        Map map = Loader.LoadMap();
        return Init(map);
    }

    public World Init(Map map)
    {
        _grid = new Grid(map);
        _init = true;
        return this;
    }
    public void InitMap(Map map)
    {
        _grid = new Grid(map);
    }

    public static void OnWorldInitialized(System.Action<World> action)
    {
        if (_world != null && _world._init)
        {
            action(_world);
        }
        else
        {
            OnWorldInitializedEvent += action;
        }
    }

    public void Draw()
    {
        foreach (Cell c in _grid.GetCells())
        {
            c.DrawTiles();
        }
        Debug.Log("World Initialized");
        if (OnWorldInitializedEvent != null)
        {
            OnWorldInitializedEvent(this);
        }
        int x = _grid.GetCells().GetLength(0);
        int y = _grid.GetCells().GetLength(1);
        CreateFloorCollider(x, y);
    }

    public void SetHeatMap(int[,] hate)
    {
        _heatMap = hate;
    }

    public int[,] GetHeatMap()
    {
        return _heatMap;
    }

    public Grid GetGrid()
    {
        return _grid;
    }

    public GameObject CreateFloorCollider(int x, int y)
    {
        Mesh m = new Mesh();
        m.name = "floor_collider";
        m.SetVertices(
            new List<Vector3> {
                new Vector3(-1, 0.01f, -1),
                new Vector3(-1, 0.01f, y),
                new Vector3(x, 0.01f, -1),
                new Vector3(x , 0.01f, y)
            });
        m.triangles = new int[] { 0, 1, 2, 2, 1, 3 };
        m.RecalculateNormals();

        GameObject plane = new GameObject("plane");
        MeshFilter mf = (MeshFilter)plane.AddComponent(typeof(MeshFilter));
        mf.mesh = m;
        //MeshRenderer rend = plane.AddComponent(typeof(MeshRenderer)) as MeshRenderer;
        MeshCollider col = plane.AddComponent(typeof(MeshCollider)) as MeshCollider;

        return plane;
    }

    public class Cell
    {
        public static readonly Cell NULL = new Cell();

        public readonly int X;
        public readonly int Y;
        private List<Item> _items;
        private Entity _entity;
        private List<Tile> _tiles;
        private bool _blocked;
        private bool _visible = false;

        public Cell()
        {
            _items = new List<Item>();
            _tiles = new List<Tile>();
            _blocked = true;
        }
        public Cell(int x, int y)
        {
            X = x;
            Y = y;
            _items = new List<Item>();
            _tiles = new List<Tile>();
            _blocked = true;
        }

        public void DrawTiles()
        {
            foreach (Tile t in _tiles)
            {
                t.Draw();
            }
        }

        public bool IsBlocked
        {
            get { return _blocked; }
            set
            {
                _blocked = value;
            }

        }

        public void SetVisible(bool visible)
        {
            _visible = visible;
        }

        public bool IsVisible()
        {
            return _entity != null && _visible;
        }

        public List<Tile> GetTiles()
        {
            return _tiles;
        }

        public List<Item> GetItems()
        {
            return _items;
        }

        public bool ContainsEntity
        {
            get
            {
                return _entity != null;
            }
        }

        public Entity GetEntity()
        {
            return _entity;
        }

        public void SetEntity(Entity e)
        {
            _entity = e;
        }

        public void AddTile(string type, int x, int y, int rot)
        {
            _tiles.Add(new Tile(type, new Vector3(x, 0, y), new Vector3(0, rot, 0)));
        }

        public override string ToString()
        {
            string output = "Cell:\nItems: "+_items.Count;            
            output += "\nTiles: "+_tiles.Count;
            output += "\nEntity: " + (_entity != null ? _entity.Actor.name : "null");
            output += "\nBlocked: " + IsBlocked;

            return output;
        }
    }

    public class Grid
    {
        Cell[,] _grid;
        
        public Grid(Map map)
        {
            _grid = new Cell[map.X, map.Y];

            for (int x = 0; x < map.X; x++)
            {
                for (int y = 0; y < map.Y; y++)
                {
                    Cell c = new Cell(x,y);
                    switch (map.GetTile(x, y))
                    {
                        case '#':
                            //wall
                            if('#'.Equals(map.GetTile(x + 1, y)) && '#'.Equals(map.GetTile(x - 1, y)))
                            {
                                if('_'.Equals(map.GetTile(x, y + 1)))
                                    c.AddTile((UnityEngine.Random.Range(0f, 1f) < 0.9f ? Tile.Set.WALL : Tile.Set.WALL_SEC), x, y, 180);
                                if('_'.Equals(map.GetTile(x, y - 1)))
                                    c.AddTile((UnityEngine.Random.Range(0f, 1f) < 0.9f ? Tile.Set.WALL : Tile.Set.WALL_SEC), x, y, 0);
                            }
                            else if ('#'.Equals(map.GetTile(x, y + 1)) && '#'.Equals(map.GetTile(x, y - 1)))
                            {
                                if ('_'.Equals(map.GetTile(x + 1, y)))
                                    c.AddTile((UnityEngine.Random.Range(0f, 1f) < 0.9f ? Tile.Set.WALL : Tile.Set.WALL_SEC), x, y, 270);
                                if ('_'.Equals(map.GetTile(x - 1, y)))
                                    c.AddTile((UnityEngine.Random.Range(0f, 1f) < 0.9f ? Tile.Set.WALL : Tile.Set.WALL_SEC), x, y, 90);
                            }

                            //concave
                            if('#'.Equals(map.GetTile(x + 1, y)))
                            {
                                if('#'.Equals(map.GetTile(x, y + 1)) && '_'.Equals(map.GetTile(x + 1, y + 1)))
                                    c.AddTile(Tile.Set.CORNER_CAVE, x, y, 270);
                                if ('#'.Equals(map.GetTile(x, y - 1)) && '_'.Equals(map.GetTile(x + 1, y - 1)))
                                    c.AddTile(Tile.Set.CORNER_CAVE, x, y, 0);
                            }
                            if ('#'.Equals(map.GetTile(x - 1, y)))
                            {
                                if ('#'.Equals(map.GetTile(x, y + 1)) && '_'.Equals(map.GetTile(x - 1, y + 1)))
                                    c.AddTile(Tile.Set.CORNER_CAVE, x, y, 180);
                                if ('#'.Equals(map.GetTile(x, y - 1)) && '_'.Equals(map.GetTile(x - 1, y - 1)))
                                    c.AddTile(Tile.Set.CORNER_CAVE, x, y, 90);
                            }

                            //convex
                            if ('#'.Equals(map.GetTile(x + 1, y)))
                            {
                                if ('#'.Equals(map.GetTile(x, y + 1)) &&
                                    '_'.Equals(map.GetTile(x - 1, y)) &&
                                    '_'.Equals(map.GetTile(x, y - 1)))
                                    c.AddTile(Tile.Set.CORNER_CVEX, x, y, 90);
                                else if ('#'.Equals(map.GetTile(x, y - 1)) &&
                                    '_'.Equals(map.GetTile(x - 1, y)) &&
                                    '_'.Equals(map.GetTile(x, y + 1)))
                                    c.AddTile(Tile.Set.CORNER_CVEX, x, y, 180);
                            }
                            else if('#'.Equals(map.GetTile(x - 1, y)))
                            {
                                if ('#'.Equals(map.GetTile(x, y - 1)) &&
                                    '_'.Equals(map.GetTile(x + 1, y)) &&
                                    '_'.Equals(map.GetTile(x, y + 1)))
                                    c.AddTile(Tile.Set.CORNER_CVEX, x, y, 270);
                                else if ('#'.Equals(map.GetTile(x, y + 1)) &&
                                    '_'.Equals(map.GetTile(x + 1, y)) &&
                                    '_'.Equals(map.GetTile(x, y - 1)))
                                    c.AddTile(Tile.Set.CORNER_CVEX, x, y, 0);
                            }

                            //sharp corner
                            if ('#'.Equals(map.GetTile(x + 1, y)) &&
                                '_'.Equals(map.GetTile(x - 1, y)) &&
                                '_'.Equals(map.GetTile(x, y + 1)) &&
                                '_'.Equals(map.GetTile(x, y - 1)))
                                c.AddTile(Tile.Set.CORNER_SHARP, x, y, 0);
                            if ('_'.Equals(map.GetTile(x + 1, y)) &&
                                '#'.Equals(map.GetTile(x - 1, y)) &&
                                '_'.Equals(map.GetTile(x, y + 1)) &&
                                '_'.Equals(map.GetTile(x, y - 1)))
                                c.AddTile(Tile.Set.CORNER_SHARP, x, y, 180);
                            if ('_'.Equals(map.GetTile(x + 1, y)) &&
                                '_'.Equals(map.GetTile(x - 1, y)) &&
                                '#'.Equals(map.GetTile(x, y + 1)) &&
                                '_'.Equals(map.GetTile(x, y - 1)))
                                c.AddTile(Tile.Set.CORNER_SHARP, x, y, 270);
                            if ('_'.Equals(map.GetTile(x + 1, y)) &&
                                '_'.Equals(map.GetTile(x - 1, y)) &&
                                '_'.Equals(map.GetTile(x, y + 1)) &&
                                '#'.Equals(map.GetTile(x, y - 1)))
                                c.AddTile(Tile.Set.CORNER_SHARP, x, y, 90);

                            //pillar
                            if ('_'.Equals(map.GetTile(x + 1, y)) &&
                                '_'.Equals(map.GetTile(x - 1, y)) &&
                                '_'.Equals(map.GetTile(x, y + 1)) &&
                                '_'.Equals(map.GetTile(x, y - 1)))
                            {
                                c.AddTile(Tile.Set.PILLAR, x, y, 0);
                                c.AddTile(Tile.Set.FLOOR, x, y, 0);
                            }
                            

                            break;
                        case '_':
                            c.AddTile(Tile.Set.FLOOR, x, y, 0);
                            c.IsBlocked = false;
                            break;
                    }
                    _grid[x, y] = c;
                }
            }
        }

        public Cell[,] GetCells()
        {
            return _grid;
        }

        public Cell GetCell(int x, int y)
        {
            if (x < 0 || y < 0 || x > _grid.GetLength(0) || y > _grid.GetLength(1))
                return Cell.NULL;
            return _grid[x, y];
        }

        /// <summary>
        /// Get four neighbours (left right up down)
        /// </summary>
        /// <param name="myCell"></param>
        /// <returns></returns>
        internal Cell[] GetNeighbours4(Cell myCell)
        {
            Cell[] result = new Cell[4];
            result[0] = GetCell(myCell.X - 1, myCell.Y);
            result[1] = GetCell(myCell.X + 1, myCell.Y);
            result[2] = GetCell(myCell.X, myCell.Y - 1);
            result[3] = GetCell(myCell.X, myCell.Y + 1);
            return result;
        }
    }

    public class Hack : MonoBehaviour
    {
        public static GameObject GET(string _type)
        {
            return Instantiate(Resources.Load(_type) as GameObject);
        }
    }

    public class Tile// : MonoBehaviour
    {
        public class Set
        {
            public static readonly string BASE = "Prefabs/Tilesets/";
            private static readonly string _default = "Concrete/";
            protected static string GetSet() { return BASE + _default; }


            public static string FLOOR { get { return GetSet() + "Floor"; } }
            public static string WALL = GetSet() + "Wall";
            public static string WALL_SEC = GetSet() + "WallSec";
            public static string PILLAR = GetSet() + "Pillar";
            public static string CORNER_CAVE = GetSet() + "CornerConcave";
            public static string CORNER_CVEX = GetSet() + "CornerConvex";
            public static string CORNER_SHARP = GetSet() + "CornerSharp";

        }

        //public class Concrete : Set
        //{
        //    protected static string GetSet() { return BASE + "Concrete/"; }
        //}


        private GameObject _go;
        private Vector3 _pos;
        private Vector3 _rot;
        private string _type;

        public Tile(string type, Vector3 pos, Vector3 rot)
        {
            _type = type;
            _pos = pos;
            _rot = rot;
        }

        public GameObject GetGO()
        {
            return _go;
        }

        public void Draw()
        {
            _go = Hack.GET(_type);
            _go.transform.position = _pos;
            _go.transform.Rotate(_rot);
        }
    }
}
