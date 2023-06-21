using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Check : MonoBehaviour
{
    [SerializeField] private GameObject[] blood;
    [SerializeField] private Transform[] spawn;

    private float _time = 3;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _time -= Time.deltaTime;
        if (_time <= 0)
        {
            Instantiate(blood[Random.Range(0, blood.Length)], spawn[Random.Range(0, spawn.Length)].position, Quaternion.identity);
            _time = 3;
        }
    }
}
