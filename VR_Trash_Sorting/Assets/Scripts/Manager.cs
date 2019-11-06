using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class Manager : MonoBehaviour
{
    #region Singleton Pattern
    public static Manager Instance { get; private set; }

    private void Awake()
    {
        if(Instance != null)
            Destroy(gameObject);

        Instance = this;
    }
    #endregion
    
    #region Delegate Pattern
    public delegate void UpdateScore(int point);
    public UpdateScore updateScore;
    #endregion

    [SerializeField] private Transform player;
    [SerializeField] private Canvas gameOverUI;
    
    private int _userId;
    private int _score;
    
    private List<TrashObject> _trashObjects;

    public Transform Player => player;

    private void Start()
    {
        _trashObjects.AddRange(Resources.FindObjectsOfTypeAll<TrashObject>());
        gameOverUI.enabled = false;

        //Assigning listeners
        updateScore += SetPoints;
        updateScore += WinCheck;
        updateScore += UpdateUI;
    }

    #region UI Calls
    //Set using UI input
    public void SetUsername(int id)
    {
        _userId = id;
    }
    
    //Called from UI button press
    public void SaveToFile()
    {
        var filePath = Application.dataPath + "/user_" + _userId + ".txt";

        if (!File.Exists(filePath))
            File.WriteAllText(filePath, "User:" + _userId + "\n");

        var content = "Score: " + _score;
        
        File.AppendAllText(filePath, content);
    }
    #endregion

    //Sets the current score 
    private void SetPoints(int points) => _score += points;

    //Check if the player is done with the game
    private void WinCheck(int input)
    {
        if (!CheckForCompletion()) return;
        Time.timeScale = 0;
        gameOverUI.enabled = true;
    }

    //Update the score seen in the UI 
    private void UpdateUI(int input)
    {
        //TODO: Update the UI 
    }

    //Disables a trash object after it hits the right trash can
    public void DisableTrashObject(TrashObject trashObject)
    {
        foreach (var trash in _trashObjects.Where(trash => trash == trashObject))
            trash.gameObject.SetActive(false);
    }

    private bool CheckForCompletion() => !_trashObjects.Any(trash => trash.gameObject.activeSelf);
}
