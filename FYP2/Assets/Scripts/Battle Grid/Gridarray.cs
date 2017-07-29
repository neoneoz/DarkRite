using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[System.Serializable]
public class Gridarray : MonoBehaviour
    {
        
        // Use this for initialization
        public GameObject sGrid , target_anim;
        public Tiled2Unity.TiledMap stage;
        public int width, height;
        public float tilesize;
        public GameObject[,] gridmesh;
        public bool Generate = false;
        public Sprite[] gridtextures;
        struct Point
        {
            public int x, y;
            public Point(int px, int py)
            {
                x = px;
                y = py;
            }
        }
        public List<GameObject> target_list;




        void Start()
        {
            GenerateGrid();
            if(gridmesh == null)
            Debug.Log("fuc");
  
            for (int i = 0; i < 10; i++)
            {
                GameObject targetanim = (GameObject)Instantiate(target_anim, Vector3.zero, Quaternion.identity);
                targetanim.gameObject.SetActive(false);
                target_list.Add(targetanim);
            }
            DerenderGrids();
        }

        // Update is called once per frame
        void Update()
        {
            //height = stage.NumTilesHigh;
            //width = stage.NumTilesWide;
        }

        void GenerateGrid()
        {
            if(!Generate)
            {
                gridmesh = new GameObject[width, height];
                for (int i = 0; i < transform.childCount; ++i)
                {
                    transform.GetChild(i).GetComponent<SpriteRenderer>().enabled = false;
                    Vector2 index = transform.GetChild(i).GetComponent<BGrid>().index;
                    gridmesh[(int)index.x, (int)index.y] = transform.GetChild(i).gameObject;

                }
                return;
            }


            gridmesh = new GameObject[width, height];
            GameObject layerstage = stage.transform.GetChild(1).gameObject;
            //int totalwidth = width * tilesize;
            //int totalheaight = height * tilesize;
            Vector3 offset = new Vector3(tilesize/2f,-tilesize/2f,0);
            foreach(Transform node in layerstage.transform)
            {
                GameObject grid = (GameObject)Instantiate(sGrid);
                Vector2 pos = node.GetComponent<Tiled2Unity.RectangleObject>().TmxPosition;
                Vector3 worldpos = node.transform.position+offset; 
                grid.GetComponent<BGrid>().SetBGridPos(worldpos);
                grid.transform.SetParent(gameObject.transform);
                int x =(int)(pos.x / tilesize) ; int y =(int)(pos.y / tilesize) ;
                grid.GetComponent<BGrid>().SetBGridIndex(new Vector2(x,y));
                gridmesh[x,y]=grid;
               // Debug.Log(gridmesh[x, y].transform.position);
            }

        }

        public GameObject GetGridAt(int x, int y)
        {
            //Debug.Log(new Vector2(x,y));
            if (x >= width || y >= height || x<0 || y<0)
                return null;

            return this.gridmesh[x, y];
        }
        public GameObject GetGridAt(Vector2 index)
        {
            if (index.x >= width || index.y >= height || index.x < 0 || index.y < 0)//if out of bounds , return an impossible grid. change this later>
                return null;

            return this.gridmesh[(int)index.x,(int)index.y];
        }
        public Vector3 GetGridPosAt(int x, int y)
        {
            return this.gridmesh[x, y].GetComponent<Transform>().position;
        }
        public BGrid GetRandomEmptyGrid()
        {
            int x,y;
            do
            {
                x = Random.Range(0, width - 1);
                y = Random.Range(0, height - 1);
            }
            while (gridmesh[x, y].GetComponent<BGrid>().unit != null);
            return gridmesh[x, y].GetComponent<BGrid>();
        }

        public List<BGrid> Search(BGrid start, System.Func<BGrid, BGrid, bool> addTile)
        {
            List<BGrid> retValue = new List<BGrid>();
            retValue.Add(start);
      
            ClearSearch();
            Queue<BGrid> checkNext = new Queue<BGrid>();
            Queue<BGrid> checkNow = new Queue<BGrid>();

            start.distance = 0;
            checkNow.Enqueue(start);


            while (checkNow.Count > 0)
            {
                BGrid t = checkNow.Dequeue();
                for (int i = 0; i < 4; ++i)
                {
                    Vector2 cindex = (t.index + dirs[i]);
                    GameObject newtile = GetGridAt((int)cindex.x, (int)cindex.y);
                    if (newtile == null)
                        continue;

                    BGrid next = newtile.GetComponent<BGrid>();
                    if (next.distance <= t.distance + 1)
                        continue;

                    if (addTile(t, next))
                    {
                        next.distance = t.distance + 1;
                        next.prev = t;
                        checkNext.Enqueue(next);
                        retValue.Add(next);
                    }
                    if (checkNow.Count == 0)
                        SwapReference(ref checkNow, ref checkNext);

                }
            }
            

            return retValue;
        }

        void ClearSearch()
        {

            for (int i = 0; i < transform.childCount; ++i)
            {
                BGrid t = transform.GetChild(i).GetComponent<BGrid>();
                t.prev = null;
                t.distance = int.MaxValue;
            }

        }
        void SwapReference(ref Queue<BGrid> a, ref Queue<BGrid> b)
        {
            Queue<BGrid> temp = a;
            a = b;
            b = temp;
        }

        public void SetGridState(List<BGrid> tiles, BGrid.Gridstate state)
        {
            for (int i = tiles.Count - 1; i >= 0; --i)
            {
                switch(state)
                {
                    case BGrid.Gridstate.MOVE:
                    tiles[i].gridstate = BGrid.Gridstate.MOVE;
                    break;
                    case BGrid.Gridstate.ATTACK:
                    tiles[i].gridstate = BGrid.Gridstate.ATTACK;
                    break;
                }
                UpdateGridSprites();
            }
        }
        public void SetGridState(BGrid tile, BGrid.Gridstate state)
        {
            switch (state)
            {
                case BGrid.Gridstate.MOVE:
                    tile.gridstate = BGrid.Gridstate.MOVE;
                    break;
                case BGrid.Gridstate.ATTACK:
                    tile.gridstate = BGrid.Gridstate.ATTACK;
                    break;
                //case BGrid.Gridstate.HIT:
                //    tile.gridstate = BGrid.Gridstate.HIT;
                    //break;
            }
            UpdateGridSprites();
        }
        
    
        public void RenderPathForGrid(BGrid destination, bool reset = false)
        {

            int distance = destination.distance;
            BGrid tile;
            tile = destination;
            if(reset)
            {
                destination.gridstate = BGrid.Gridstate.MOVE;
                destination.GetComponent<SpriteRenderer>().sprite = gridtextures[0];
                while (distance != 0)
                {
                    tile = tile.prev;
                    tile.gridstate = BGrid.Gridstate.MOVE;
                    tile.GetComponent<SpriteRenderer>().sprite = gridtextures[0];
                    distance = tile.distance;
                }
                return;
            } 

            destination.gridstate = BGrid.Gridstate.PATH;
            destination.GetComponent<SpriteRenderer>().sprite = gridtextures[1];
            while (distance!=0)
            {
                tile = tile.prev;
                tile.gridstate = BGrid.Gridstate.PATH;
                tile.GetComponent<SpriteRenderer>().sprite = gridtextures[1];
                distance = tile.distance;
            }
        }

        public void SetGridTrget(BGrid target,bool reset = false)
        {
            if(reset)
            {
                target.GetComponent<SpriteRenderer>().enabled = true;
                target.gridstate = BGrid.Gridstate.ATTACK;
                target_anim.gameObject.SetActive(false);
                //target_list[0].SetActive(false);
                for (int i = 0; i < target_list.Count; i++)
                {
                    target_list[i].SetActive(false);
                }
       
                return;
            }
            target.gridstate = BGrid.Gridstate.TARGET;
            target_list[0].gameObject.SetActive(true);
            target_list[0].gameObject.transform.position = target.transform.position;
            target.GetComponent<SpriteRenderer>().enabled = false;

        }

        public void DerenderGrids()//derenders the entire grid
        {
            for (int i = width-1; i >= 0; --i)
            {
                for (int x = height-1; x >= 0; --x)
                {
                    gridmesh[i, x].GetComponent<SpriteRenderer>().enabled = false;
                    gridmesh[i, x].GetComponent<BGrid>().gridstate = BGrid.Gridstate.INACTIVE;
                }
            }
        }
        public void UpdateGridSprites()
        {
            for (int i = width-1; i >= 0; --i)
            {
                for (int x = height-1; x >= 0; --x)
                {
                    gridmesh[i, x].GetComponent<SpriteRenderer>().enabled = true;
                    switch (gridmesh[i, x].GetComponent<BGrid>().gridstate)
                    {
                        case BGrid.Gridstate.INACTIVE:
                            gridmesh[i, x].GetComponent<SpriteRenderer>().enabled = false;
                            gridmesh[i, x].GetComponent<SpriteRenderer>().sprite = gridtextures[0];
                            break;

                        case BGrid.Gridstate.MOVE:
                            gridmesh[i, x].GetComponent<SpriteRenderer>().sprite = gridtextures[(int)BGrid.Gridstate.MOVE];
                            break;
                        case BGrid.Gridstate.PATH:
                            gridmesh[i, x].GetComponent<SpriteRenderer>().sprite = gridtextures[(int)BGrid.Gridstate.PATH];
                            break;
                        case BGrid.Gridstate.TARGET:
                            gridmesh[i, x].GetComponent<SpriteRenderer>().enabled = false;//play the animation, no need to render image;
                            break;
                        case BGrid.Gridstate.ATTACK:
                            gridmesh[i, x].GetComponent<SpriteRenderer>().sprite = gridtextures[(int)BGrid.Gridstate.ATTACK];
                            break;

                    }
                    
                }
            }
        }



        Vector2[] dirs = new Vector2[4]
        {
            new Vector2(0, 1),
            new Vector2(0, -1),
            new Vector2(1, 0),
            new Vector2(-1, 0)
        };


    }

