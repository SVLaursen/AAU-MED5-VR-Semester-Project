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
    [SerializeField] private float resetDistance = 0.1f;

    [SerializeField] private AudioClip collisionSound;

    private AudioSource audioSource;
    
    private Rigidbody _rigidbody;
    private List<GameObject> _popUps = new List<GameObject>();
    private Vector3 _originalPosition;
    private bool _displaced;

    public TrashType TrashType => trashType;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()                  
    {
        _originalPosition = transform.position;
        _rigidbody = GetComponent<Rigidbody>();
        DeactivatePopUp();

        foreach (Transform child in transform)
            _popUps.Add(child.gameObject);
        
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
        audioSource.PlayOneShot(collisionSound);

        if (other.gameObject.CompareTag("Ground"))
            transform.position = _originalPosition;
    }

    private void OnTriggerEnter(Collider other)
    {
        ActivatePopUp();
        _displaced = false;
    }

    private void OnTriggerStay(Collider other)
    {
        ActivatePopUp();
        _displaced = false;
    }

    private void OnTriggerExit(Collider other)
    {
        DeactivatePopUp();
        _displaced = true;
        StartCoroutine(WaitThenReturn());
    }

    private void ActivatePopUp()
    {
        foreach(var pop in _popUps)
            pop.SetActive(true);
    }

    private void DeactivatePopUp()
    {
        foreach(var pop in _popUps)
            pop.SetActive(false);
    }

    private IEnumerator WaitThenReturn()
    {
        var distanceFromOrigin = Vector3.Distance(transform.position, _originalPosition);

        if (distanceFromOrigin > resetDistance)
        {
            yield return new WaitForSeconds(6f);

            if (!_displaced)
                yield return null;
            
            transform.position = _originalPosition;
            _rigidbody.velocity = Vector3.zero;
        }
        else yield return null;
    }
}

public enum TrashType
{
    Default, Paper, Plastic, Metal, BioWaste, ResidualWaste, Glass
}
