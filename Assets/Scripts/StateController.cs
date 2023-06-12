using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public static class StateController
{
    public static int lives = 1;
    public static int currentHealth = 100;
    public static List<GameObject> items = new List<GameObject>();
    public static List<GameObject> medals = new List<GameObject>();
    public static List<int> LevelPlayerPassed = new List<int>();
}
