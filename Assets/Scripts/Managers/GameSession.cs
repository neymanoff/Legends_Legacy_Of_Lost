using System.Collections.Generic;
using UnityEngine;

// Singleton to carry data between scenes
public class GameSession : MonoBehaviour
{
    public static GameSession Instance { get; private set; }

    // IDs or references of selected units
    public List<string> SelectedUnitIds = new List<string>();

    // Grid params
    public int GridRows = 5;
    public int GridColumns = 5;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}