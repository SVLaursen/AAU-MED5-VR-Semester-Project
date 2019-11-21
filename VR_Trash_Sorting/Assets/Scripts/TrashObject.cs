using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus;

[RequireComponent(typeof(Rigidbody), typeof(OVRGrabbable))]
public class TrashObject : MonoBehaviour
{
    [SerializeField] private TrashType trashType;
    [SerializeField] private GameObject popUp;
    [SerializeField] private int pointsGiven;
    [SerializeField] private int pointsTaken;

    private Rigidbody _rigidbody;
    private Vector3 _originalPosition;

    public TrashType TrashType => trashType;

    private void Start()
    {
        _originalPosition = transform.position;
        _rigidbody = GetComponent<Rigidbody>();
        popUp.SetActive(false);
        
        Manager.Instance.TrashObjects.Add(this);
    }

    public void RejectObject()
    {
        Manager.Instance.updateScore(-pointsTaken);
        Manager.Instance.CountTrashError(trashType);
        
        transform.position = _originalPosition;
        _rigidbody.velocity = Vector3.zero;
    }

    public void AcceptObject()
    {
        Manager.Instance.DisableTrashObject(this);
        Manager.Instance.updateScore(pointsGiven);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
            transform.position = _originalPosition;
        else if (other.gameObject.CompareTag("Hands"))
            popUp.SetActive(true);
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Hands"))
            popUp.SetActive(false);
    }
}

public enum TrashType
{
    Default, Paper, Plastic, Metal, BioWaste, ResidualWaste, Glass
}
