using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    private string _username;
    private bool _pamphletUser;
    private int _score;

    //Set using UI input
    public void SetUsername(string username)
    {
        _username = username;
    }

    //Set using UI input
    public void SetUserType(bool pamphletUser)
    {
        _pamphletUser = pamphletUser;
    }

    public void DeductPoints(int pointsToDeduct)
    {
        _score -= pointsToDeduct;
    }

    public void AddPoints(int pointsToAdd)
    {
        _score += pointsToAdd;
    }

    //Called from game over button press
    public void SaveToFile()
    {
        
    }
}
