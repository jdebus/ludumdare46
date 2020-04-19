using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    public bool isPlaced = false;


    public GameObject placedVisual;
    public GameObject itemVisual;


    public AudioClip placeSound;
    public AudioClip snapSound;

    Item item;

    private void Start()
    {
        item = GetComponent<Item>();
    }

    public void Place()
    {
        isPlaced = true;
        if (placeSound != null)
            AudioSource.PlayClipAtPoint(placeSound, Camera.main.transform.position);
    }

    public void Snap()
    {
        if (snapSound != null)
            AudioSource.PlayClipAtPoint(snapSound, Camera.main.transform.position);
    }

    private void OnValidate()
    {
        UpdateVisual();
    }

    void UpdateVisual()
    {
        placedVisual.SetActive(isPlaced);
        itemVisual.SetActive(!isPlaced);
    }

    private void Update()
    {
        UpdateVisual();
        item.canBePickedUp = !isPlaced;
    }
}
