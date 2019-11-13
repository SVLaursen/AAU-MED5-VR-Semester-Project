using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
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
    [SerializeField] private InputField inputField;
    
    private int _userId;
    private int _score;
    
    private Dictionary<TrashType, int> failData = new Dictionary<TrashType, int>();

    private void Start()
    {
        gameOverUI.enabled = false;

        //Assigning listeners
        updateScore += SetPoints;
        updateScore += WinCheck;
        updateScore += UpdateUI;
    }

    #region UI Calls
    //Set using UI input
    public void SetUsername(string arg0)
    {
        Debug.Log("User ID set");
        _userId = int.Parse(arg0);
    }
    
    //Called from UI button press
    public void SaveToFile()
    {
        var filePath = Application.dataPath + "/user_" + _userId + ".txt";

        if (!File.Exists(filePath))
            File.WriteAllText(filePath, "User:" + _userId + "\n");

        var content = "Score: " + _score + "\n";
        
        File.AppendAllText(filePath, content);
        
        foreach (var data in failData.Select(entry => entry.Key.ToString() + ": " + entry.Value + "\n"))
            File.AppendAllText(filePath, data);
    }
    #endregion
    
    //Collects info on what objects has been
    public void CountTrashError(TrashType key) => failData[key] += 1;
    
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

    private bool CheckForCompletion() => !TrashObjects.Any(trash => trash.gameObject.activeSelf);
}
