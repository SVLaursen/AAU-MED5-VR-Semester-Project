using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    [HideInInspector] public List<TrashObject> TrashObjects;
    
    #region Singleton Pattern
    public static Manager Instance { get; private set; }

    private void Awake()
    {
        if(Instance != null)
            Destroy(gameObject);

        Instance = this;
        TrashObjects = new List<TrashObject>();
        Time.timeScale = 1;
    }
    #endregion
    
    #region Delegate Pattern
    public delegate void UpdateScore(int point);
    public UpdateScore updateScore;
    #endregion

    [SerializeField] private Canvas gameOverUI;
    
    private string _userId;
    private int _score;
    
    private Dictionary<TrashType, int> _failData = new Dictionary<TrashType, int>()
    {
        {TrashType.Glass, 0},
        {TrashType.Metal, 0},
        {TrashType.Paper, 0},
        {TrashType.Plastic, 0},
        {TrashType.BioWaste, 0},
        {TrashType.ResidualWaste, 0}
    };

    private void Start()
    {
        gameOverUI.enabled = false;

        //Assigning listeners
        updateScore += SetPoints;
        updateScore += WinCheck;
        updateScore += UpdateUI;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.L))
            SceneManager.LoadScene(0);
    }

    #region UI Calls
    //Set using UI input
    public void SetUsername(string arg0)
    {
        Debug.Log("User ID set");
        _userId = arg0;
    }
    
    //Called from UI button press
    public void SaveToFile()
    {
        var filePath = Application.dataPath + "/user_" + _userId + ".txt";

        if (!File.Exists(filePath))
            File.WriteAllText(filePath, "User:" + _userId + "\n");

        var content = "Score: " + _score + "\n";
        
        File.AppendAllText(filePath, content);
        
        foreach (var data in _failData.Select(entry => entry.Key.ToString() + ": " + entry.Value.ToString() + "\n"))
            File.AppendAllText(filePath, data);
    }
    #endregion

    //Collects info on what objects has been
    public void CountTrashError(TrashType key) => _failData[key] += 1;
    
    //Sets the current score 
    private void SetPoints(int points)
    {
        Debug.Log("Set the score");
        _score += points;
    }

    //Check if the player is done with the game
    private void WinCheck(int input)
    {
        Debug.Log("Did win check");
        if (!CheckForCompletion()) return;
        Time.timeScale = 0;
        gameOverUI.enabled = true;
    }

    //Update the score seen in the UI 
    private void UpdateUI(int input)
    {
        //TODO: Update the UI 
        Debug.Log("Updated the UI");
    }

    //Disables a trash object after it hits the right trash can
    public void DisableTrashObject(TrashObject trashObject)
    {
        foreach (var trash in TrashObjects.Where(trash => trash == trashObject))
            trash.gameObject.SetActive(false);
    }

    private bool CheckForCompletion()
    {
        foreach (var item in TrashObjects)
        {
            Debug.Log(item.gameObject.activeSelf);
            if (item.gameObject.activeSelf == true) return false;
        }
        
        return true;
    }
}
