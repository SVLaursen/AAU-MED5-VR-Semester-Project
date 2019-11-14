using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCan : MonoBehaviour
{
    [SerializeField] private TrashType trashToAccept;
    [SerializeField] private GameObject rejectIcon;
    [SerializeField] private GameObject acceptIcon;

    private void Start()
    {
        rejectIcon?.SetActive(false);
        acceptIcon?.SetActive(false);
    }
    
    private void OnCollisionEnter(Collision other)
    {
        var trashObject = other.gameObject.GetComponent<TrashObject>();
        Debug.Log(other.gameObject.GetComponent<TrashObject>().TrashType);

        if (trashObject.TrashType == trashToAccept)
        {
            trashObject.AcceptObject(); //Accept the trash
            StartCoroutine(ShowAcceptThenDissappear());
        }
        else
        {
            //Spew it back in their face, this is not correct
            trashObject.RejectObject();
            StartCoroutine(ShowRejectThenDissappear());
        }
    }

    private IEnumerator ShowRejectThenDissappear()
    {
        rejectIcon?.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        rejectIcon?.SetActive(false);
    }

    private IEnumerator ShowAcceptThenDissappear()
    {
        acceptIcon?.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        acceptIcon?.SetActive(false);
    }
}
