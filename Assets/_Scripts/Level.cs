using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Scriptable Objects for creating levels
// Custom Editor is also created to make levels
[CreateAssetMenu(menuName = "Map/Level",fileName = "Level_")]
public class Level : ScriptableObject
{
    [SerializeField]
    public int _height=1;
    [SerializeField]
    public int _width=1;
    [SerializeField] public List<int> _tilesData = new List<int>();

}
