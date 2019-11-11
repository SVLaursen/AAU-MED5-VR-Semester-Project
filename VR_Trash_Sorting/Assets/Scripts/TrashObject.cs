using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus;

[RequireComponent(typeof(Rigidbody), typeof(OVRGrabbable))]
public class TrashObject : MonoBehaviour
{
    [SerializeField] private TrashType trashType;
    [SerializeField] private int pointsGiven;
    [SerializeField] private int pointsTaken;
    
    private Vector3 _originalPosition;

    public TrashType TrashType => trashType;

    private void Start()
    {
        _originalPosition = transform.position;
        Manager.Instance.TrashObjects.Add(this);
    }

    public void RejectObject()
    {
        Manager.Instance.updateScore(-pointsTaken);
        Manager.Instance.DisableTrashObject(this);
    }

    public void AcceptObject()
    {
        Manager.Instance.updateScore(pointsGiven);
        Manager.Instance.DisableTrashObject(this);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
            transform.position = _originalPosition;
    }
}

public enum TrashType
{
    Default, Paper, Plastic, Metal, BioWaste, ResidualWaste, Glass
}
