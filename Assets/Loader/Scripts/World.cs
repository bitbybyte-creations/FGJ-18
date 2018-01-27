using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World
{
    //private List<IEntity> _entities;
    private Grid _grid;

    public World()
    {
        //_entities = new List<IEntity>();

        Map map = Loader.LoadMap();
        _grid = new Grid(map);
    }

    public void Draw()
    {
        foreach(Cell c in _grid.GetCells())
        {
            c.DrawTiles();
        }
    }

    public class Cell
    {
        public static readonly Cell NULL = new Cell();

        private List<Item> _items;
        private IEntity _entity;
        private List<Tile> _tiles;
        private bool _blocked;
        //graphic objects?

        public Cell()
        {
            _items = new List<Item>();
            _tiles = new List<Tile>();
            _blocked = true;
        }

        public void DrawTiles()
        {
            foreach(Tile t in _tiles)
            {
                t.Draw();
            }
        }

        public bool IsBlocked()
        {
            return _blocked;
        }

        public void SetBlocked(bool blocked)
        {
            _blocked = blocked;
        }

        public List<Item> GetItems()
        {
            return _items;
        }

        public bool ContainsEntity()
        {
            return _items != null;
        }

        public IEntity GetEntity()
        {
            return _entity;
        }

        public void AddTile(string type, int x, int y, int rot)
        {
            _tiles.Add(new Tile(type, new Vector3(x, 0, y), new Vector3(0, rot, 0)));
        }
    }

    public class Grid
    {
        Cell[,] _grid;

        public Grid(Map map)
        {
            _grid = new Cell[map.X, map.Y];

            for(int x = 0; x < map.X; x++)
            {
                for (int y = 0; y < map.Y; y++)
                {
                    Cell c = new Cell();
                    switch (map.GetTile(x, y))
                    {
                        case '#':

                            if ('_'.Equals(map.GetTile(x - 1, y)))
                                c.AddTile(Tile.Set.WALL, x, y, 0);
                            if ('_'.Equals(map.GetTile(x - 1, y)))
                                c.AddTile(Tile.Set.WALL, x, y - 1, 0);
                            if ('_'.Equals(map.GetTile(x - 1, y)))
                                c.AddTile(Tile.Set.WALL, x + 1, y, 0);
                            if ('_'.Equals(map.GetTile(x - 1, y + 1)))
                                c.AddTile(Tile.Set.WALL, x, y, 0);
                            break;
                        case '_':
                            c.AddTile(Tile.Set.FLOOR, x, y, 0);
                            c.SetBlocked(false);
                            break;
                        case '7':
                            c.AddTile(Tile.Set.CORNER, x, y, 270);
                            break;
                        case '9':
                            c.AddTile(Tile.Set.CORNER, x, y, 0);
                            break;
                        case '3':
                            c.AddTile(Tile.Set.CORNER, x, y, 90);
                            break;
                        case '1':
                            c.AddTile(Tile.Set.CORNER, x, y, 180);
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
    }

    public class Hack : MonoBehaviour
    {
        public static GameObject GET(string _type)
        {
            return Instantiate(Resources.Load(_type) as GameObject); ;
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
            public static string CORNER = GetSet() + "Corner";

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

        public void Draw()
        {
            _go = Hack.GET(_type);
            _go.transform.position = _pos;
            _go.transform.Rotate(_rot);
        }
    }
}
