using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[Serializable]
public class ItemColors
{
    [SerializeField] public Color _playerColor;
    [SerializeField] public Color _cachedTileColor;
    [SerializeField] public Color _fullTileColor;
    
}

public class playerController : SingletonClass<playerController>
{
    private const float _speed=1f;
    private const float _swipeThreshold=80f;
    private Vector3 _startPosition;
    private float _speedX = 0;
    private float _speedY = 0;
    public ItemColors GameColor;
    private List<_Tile> Tile=new List<_Tile>();

    public override void Awake()
    {
        GetComponent<Renderer>().material.color = GameColor._playerColor;
        base.Awake();
    }

    void FixedUpdate()
    {
        //Moving The Box According to swipe. speedX and SpeedY is changed according to swipe
        transform.position += new Vector3(_speedX,0,_speedY)*Time.deltaTime*5f ;
        SwipeController();
    }
    private void SwipeController()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            _startPosition = Input.GetTouch(0).deltaPosition;
        }
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            float _deltaX = Input.GetTouch(0).deltaPosition.x - _startPosition.x;
            float _deltaY = Input.GetTouch(0).deltaPosition.y - _startPosition.y;
            #region Swipe Right and Left Zone
            if (_deltaX < -_swipeThreshold || _deltaX > _swipeThreshold && _deltaX > _deltaY)
            {
                //  Debug.Log(_deltaX);
                if (_deltaX > _swipeThreshold)
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y, Mathf.Round(transform.position.z));
                    Debug.Log("I Just Moved My mouse Right");
                    if (IsReverserMovable((int) transform.position.x+1, (int) transform.position.z))
                    {
                        _speedX = _speed;
                        _speedY = 0;
                    }
                }
                else if (_deltaX < -_swipeThreshold)
                {
                    Debug.Log("I Just Moved My mouse Left");
                    transform.position = new Vector3(transform.position.x, transform.position.y,
                        Mathf.Round(transform.position.z));
                    if (IsReverserMovable((int) transform.position.x-1, (int) transform.position.z))
                    {
                        _speedX = -_speed;
                        _speedY = 0;
                    }
                }
            }
            #endregion
            #region Swipe Up and Down Zone
            else if (_deltaY < -_swipeThreshold || _deltaY > _swipeThreshold)
            {
                if (_deltaY > _swipeThreshold)
                {
                    Debug.Log("I Just Moved My mouse Up");
                    transform.position = new Vector3(Mathf.Round(transform.position.x), transform.position.y,
                        transform.position.z);
                    if (IsReverserMovable((int) transform.position.x, (int) transform.position.z + 1))
                    {
                        _speedX = 0;
                        _speedY = _speed;
                    }
                }
                else if (_deltaY < -_swipeThreshold)
                {
                    transform.position = new Vector3(Mathf.Round(transform.position.x), transform.position.y,
                        transform.position.z);
                    Debug.Log("I Just Moved My mouse Down");
                    if (IsReverserMovable((int) transform.position.x, (int) transform.position.z - 1))
                    {
                        _speedX = 0;
                        _speedY = -_speed;
                    }
                }
            }
            #endregion
        }
    }

    public bool IsReverserMovable(int TileX, int TileY)
    {
        foreach (var VARIABLE in Tile)
        {
            if (VARIABLE._TileX ==TileX && VARIABLE._TileY == TileY)
            {
                return false;
            }
        }
        return true;
    }

    public void OnTriggerEnter(Collider other)
    {
        //Painting The Floor it touches
        if (other.CompareTag("Enemy")){
            _speedX = 0;
            _speedY = 0;
            GetComponentInChildren<ParticleSystem>().Play();
            LevelManager.Instance.GameOver();
    }
        if (other.CompareTag("floor"))
        {
            if (Tile.Contains(other.GetComponent<_Tile>()))
            {
                //if i hit the tile that is already cached player will die
                _speedX = 0;
                _speedY = 0;
                GetComponentInChildren<ParticleSystem>().Play();
               LevelManager.Instance.GameOver();
            }
            
            Tile.Add(other.GetComponent<_Tile>());
            other.GetComponent<Renderer>().material.color = GameColor._cachedTileColor;
            other.GetComponent<Animator>().Play("FloorCacheAnimation");
        }

        if (other.CompareTag("completeTile"))
        {
            ColorAllPreviousTiles();
        }
        //Stopping at the wall it touches
        else if (other.CompareTag("Wall"))
        {
            ColorAllPreviousTiles();
            _speedX = 0;
            _speedY = 0;
            transform.position = new Vector3(Mathf.Round(transform.position.x), transform.position.y, Mathf.Round(transform.position.z));
        }
    }
    public void ColorAllPreviousTiles()
    {
        if (Tile != null)
        {
            foreach (var VARIABLE in Tile)
            {
                VARIABLE.GetComponent<Renderer>().material.color = GameColor._fullTileColor;
                VARIABLE.GetComponent<Animator>().Play("FloorCompleteAnimation");
                    LevelManager.Instance._pointsToWin--;

             //   VARIABLE.GetComponent<BoxCollider>().enabled = false;
                VARIABLE.tag = "completeTile";
            }
            //Tile.Clear();
        }
        foreach (var VARIABLE in Tile)
        {
            for (int i = 0; i < VARIABLE._TileY; i++)
            {
                _Tile _tile = MapSpawner.Instance.FullTiles[i, VARIABLE._TileX];
                
                if( _tile.GetComponent<Renderer>().material.color != GameColor._fullTileColor &&_tile.tag!="Wall"){
                    _tile.GetComponent<Renderer>().material.color = GameColor._fullTileColor;
                    _tile.tag = "completeTile";
                    if (_tile.GetComponent<Animator>())
                    {
                        _tile.GetComponent<Animator>().Play("FloorCompleteAnimation");
                    }
                    LevelManager.Instance._pointsToWin--;
                    Debug.Log(LevelManager.Instance._pointsToWin);
                }
            }
        }
        Tile.Clear();
    }
}
