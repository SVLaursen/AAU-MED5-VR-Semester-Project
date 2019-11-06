using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TrashObject : MonoBehaviour
{
    [SerializeField] private TrashType trashType;
    [SerializeField] private int pointsGiven;
    [SerializeField] private int pointsTaken;
    [SerializeField] private float forceToApply;
    [SerializeField] private float directionYOffset;

    private Rigidbody _rigidbody;

    public TrashType TrashType => trashType;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void RejectObject()
    {
        var player = Manager.Instance.Player.position;
        var destination = new Vector3(player.x, player.y + directionYOffset, player.z);
        var direction = Vector3.Normalize(destination - transform.position);
        
        Manager.Instance.DeductPoints(pointsTaken);
        _rigidbody.AddForce(direction * forceToApply);
    }

    public void AcceptObject()
    {
        Manager.Instance.AddPoints(pointsGiven);
        Manager.Instance.DisableTrashObject(this);
    }
}

public enum TrashType
{
    Default, Paper, Plastic, Metal, BioWaste, ResidualWaste, Glass
}
