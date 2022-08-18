using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

    
public class Enemy : MonoBehaviour
{
    int _moveValue;
    private int height;

    void Update()
    {
        // float pingPong = Mathf.PingPong(Time.time * 1, 1);
        // transform.position = Vector3.Lerp(transform.position, pos, pingPong);
         transform.position = new Vector3(transform.position.x, 0.5f,
             Mathf.PingPong(Time.time * 1f, _moveValue)+height);
    }

    public void PlaceEnemy(int _TileX,int _TileY,int _TileMovementValue)
    {
        _moveValue = _TileMovementValue;
        height = _TileY;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("completeTile"))
        {
            GetComponentInChildren<ParticleSystem>().Play();
            Destroy(gameObject,0.5f);
        }
    }
}
