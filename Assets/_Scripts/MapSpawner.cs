using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class MapSpawner : SingletonClass<MapSpawner>
{
 
    public Camera _camera;
    public GameObject _floor;
    public GameObject _SideWall;
    public GameObject _wall;
    public GameObject Enemy;
    public int _borderSize;
    public Level[] Levels;
    private int _boardHeight=1;
    private int _boardWidth=1;
    public _Tile[,] FullTiles;
    [HideInInspector] public int _completionInt=0;

    // Start is called before the first frame update
    void Start()
    {
        PlaceTiles();
        SpawnSideWall();
        CameraSetup();
        PlacePlayerPosition();
    }
    private void PlaceTiles()
    {
        GameObject _floorParent= new GameObject("FloorParent");
        int _randomLevel = Random.Range(0, Levels.Length);
        _boardHeight = Levels[_randomLevel]._height;
        _boardWidth = Levels[_randomLevel]._width;
        FullTiles= new _Tile[_boardHeight,_boardWidth];
        int _totalTileSize = Levels[_randomLevel]._tilesData.Count;
        int _tempWidthIterator = 0;
        for (int i = 0; i < _boardHeight; i++)
        {
            _tempWidthIterator = 0;
            for (int j =  _totalTileSize-_boardWidth; j <  _totalTileSize; j++)
            {
                if (Levels[_randomLevel]._tilesData[j] == 0)
                {
                    GameObject G = Instantiate(_floor);
                    G.transform.position = new Vector3(_tempWidthIterator, 0, i);
                    StoreTile(G,_floorParent,_tempWidthIterator,i);
                    _completionInt++;
                }
                else if(Levels[_randomLevel]._tilesData[j]==1)
                {
                    GameObject G = Instantiate(_wall);
                    G.transform.position = new Vector3(_tempWidthIterator, 0.5f, i);
                    StoreTile(G,_floorParent,_tempWidthIterator,i);
                }
                else if(Levels[_randomLevel]._tilesData[j]>=2)
                {
                    GameObject G = Instantiate(_floor);
                    G.transform.position = new Vector3(_tempWidthIterator, 0, i);
                    StoreTile(G,_floorParent,_tempWidthIterator,i);
                    GameObject _enemy = Instantiate(Enemy);
                    _enemy.transform.position = new Vector3(_tempWidthIterator, 0.5f, i);
                    _enemy.GetComponent<Enemy>().PlaceEnemy(_tempWidthIterator,i,Levels[_randomLevel]._tilesData[j]);
                    _completionInt++;
                }
                _tempWidthIterator++;
            }
            _totalTileSize = _totalTileSize - _boardWidth;
        }

        LevelManager.Instance._pointsToWin = _completionInt;
    }

    public void PlacePlayerPosition()
    {
        playerController.Instance.transform.position = new Vector3(_boardWidth / 2, 0.5f, 0f);
    }
    
    public void StoreTile(GameObject G,GameObject _floorParent,int _tempWidthIterator,int i)
    {
        G.GetComponent<_Tile>()._TileX = _tempWidthIterator;
        G.GetComponent<_Tile>()._TileY = i;
        FullTiles[i,_tempWidthIterator]=(G.GetComponent<_Tile>());
        G.transform.parent = _floorParent.transform;
    }

    public void SpawnSideWall()
    {
        GameObject _sideWalls= new GameObject("SideWalls");
        for (int i = 0; i < _boardWidth; i++)
        {
            GameObject _sideWallDown = Instantiate(_SideWall);
            _sideWallDown.transform.position=new Vector3(i,0.5f,-1);
            GameObject _sideWallUp = Instantiate(_SideWall);
            _sideWallUp.transform.position=new Vector3(i,0.5f,_boardHeight);
            _sideWallDown.transform.parent = _sideWalls.transform;
            _sideWallUp.transform.parent = _sideWalls.transform;

        }
        for (int i = 0; i < _boardHeight; i++)
        {
            GameObject _sideWallLeft = Instantiate(_SideWall);
            _sideWallLeft.transform.position=new Vector3(-1,0.5f,i);
            GameObject _sideWallRight = Instantiate(_SideWall);
            _sideWallRight.transform.position=new Vector3(_boardWidth,0.5f,i);
            _sideWallLeft.transform.parent = _sideWalls.transform;
            _sideWallRight.transform.parent = _sideWalls.transform;
        }
    }
    public void CameraSetup()
    {
        float _height;
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            _height = 10;
        }
        else
        {
            _height = 15;
        }
        _camera.transform.position=new Vector3((float)(_boardWidth-1)/2,_height,(float)(_boardHeight-1)/2);
        _camera.orthographicSize = (_boardWidth > _boardHeight) ? _boardWidth+_borderSize : _boardHeight+_borderSize;
    }
}
