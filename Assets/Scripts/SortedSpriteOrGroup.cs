using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[ExecuteInEditMode]
public class SortedSpriteOrGroup : MonoBehaviour
{
    [SerializeField]
    SortType type;

    [SerializeField]
    float offset;

    SortingGroup group;
    SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        group = GetComponent<SortingGroup>();
    }

    void Update()
    {
        int order = (int)((transform.position.y + offset) * -100);
        if (type == SortType.Sprite && sr != null)
            sr.sortingOrder = order;
        else if (type == SortType.SortGroup & group != null)
            group.sortingOrder = order;
    }
}

public enum SortType
{
    Sprite,
    SortGroup
}
