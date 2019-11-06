using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCan : MonoBehaviour
{
    [SerializeField] private TrashType trashToAccept;

    private void OnCollisionEnter(Collision other)
    {
        var trashObject = other.gameObject.GetComponent<TrashObject>();

        if (trashObject.TrashType == trashToAccept)
        {
            //Accept the trash
            trashObject.AcceptObject();
        }
        else
        {
            //Spew it back in their face, this is not correct
            trashObject.RejectObject();
        }
    }
}
