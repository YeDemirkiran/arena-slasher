using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class BonePair
{
    public Bone parent;
    public Bone child;

    public BonePair(Bone parent, Bone child)
    {
        this.parent = parent;
        this.child = child;
    }
}

[System.Serializable]
public class Bone
{
    // Hard coded. Add a bone type here if you need it
    public enum BoneType { Root,
        Spine0, Spine1, Spine2, 
        Neck, Head, Hair, 
        ShoulderR, ShoulderL,
        UpperArmR, UpperArmL,
        LowerArmR, LowerArmL,
        HandR, HandL,
        UpperLegR, UpperLegL,
        ShinR, ShinL,
        FootR, FootL,
        ToeR, ToeL,
    Sheath}

    public BoneType type;
    public Transform transform;
}

public class Outfit : MonoBehaviour
{
    [SerializeField] Bone[] bones;
    List<BonePair> pairs = new List<BonePair>();

    // Update is called once per frame
    void Update()
    {
        //if (GameManager.Instance.state != GameManager.GameState.Running) { return; }

        //foreach (var pair in pairs)
        //{
        //    pair.child.transform.position = pair.parent.transform.position;
        //    pair.child.transform.rotation = pair.parent.transform.rotation;
        //    //pair.child.transform.localScale = pair.parent.transform.localScale;
        //}
    }

    public void SetPairs(Bone[] parentBones)
    {
        // For faster loop
        List<Bone> _parentBones = parentBones.ToList();
        List<Bone> _childBones = bones.ToList();

        for (int i = _childBones.Count - 1; i >= 0; i--)
        {
            Bone child = _childBones[i];


            for (int b = _parentBones.Count - 1; b >= 0; b--)
            {
                Bone parent = _parentBones[b];

                if (parent.type == child.type)
                {
                    child.transform.parent = parent.transform;
                    child.transform.localPosition = Vector3.zero;
                    child.transform.localRotation = Quaternion.identity;

                    _parentBones.RemoveAt(b);
                    break;
                }
            }
        }
    }
}