using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class Cart : MonoBehaviour
{
    [SerializeField] private GameObject[] people;
    [SerializeField] private Transform[] seats;
    

    private void Start()
    {
        foreach (Transform seat in seats)
        {
            AddPassenger(seat);
        }
    }

    private void AddPassenger(Transform seat)
    {
        int index = Random.Range(0, people.Length);
        GameObject passenger = Instantiate(people[index], seat);
        Animator animator = passenger.GetComponent<Animator>();

    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("StationEnter"))
        {
            transform.DORotate(other.transform.rotation.eulerAngles, 1f, RotateMode.Fast);
        }
    }
}
