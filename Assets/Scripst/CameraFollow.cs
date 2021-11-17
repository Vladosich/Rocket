using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject Player;
    private Vector3 offset;
    void Start()
    {
        offset = transform.position - Player.transform.position;
    }


    void Update()
    {
        transform.position = Player.transform.position + offset;
    }
}
