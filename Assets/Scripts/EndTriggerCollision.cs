using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTriggerCollision : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("End Trigger was activated");
        GameManager.Instance.EndGame();
    }
}
