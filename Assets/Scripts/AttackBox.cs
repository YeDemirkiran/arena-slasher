using System.Collections.Generic;
using UnityEngine;

public class AttackBox : MonoBehaviour
{
    public List<BotController> enemies { get; private set; } = new List<BotController>();

    void OnTriggerEnter(Collider other)
    { 
        if (other.transform.parent != transform.parent)
        {
            //Debug.Log("Enter " + other.name);

            enemies.Add(other.GetComponent<BotController>());

            //Debug.Log("Current List: ");

            //foreach (var item in enemies)
            //{
            //    Debug.Log(item);
            //}
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.transform.parent != transform.parent)
        {
            //Debug.Log("Exit " + other.name);

            enemies.Remove(other.GetComponent<BotController>());

            //Debug.Log("Current List: ");

            //foreach (var item in enemies)
            //{
            //    Debug.Log(item);
            //}
        }
    }
}