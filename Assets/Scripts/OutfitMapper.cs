using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BonePair
{
    public Transform parent;
    public Transform child;
}

public class OutfitMapper : MonoBehaviour
{
    [SerializeField] Transform root;
    [SerializeField] BonePair[] pairs;

    void Start()
    {
        transform.parent = root;
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var pair in pairs)
        {
            pair.child.position = pair.parent.position;
            pair.child.rotation = pair.parent.rotation;
            pair.child.localScale = pair.parent.localScale;
        }
    }
}