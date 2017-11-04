using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCheck : MonoBehaviour {

    public GameObject itemText;

    // Use this for initialization
    void Start () {
        
    }

    void OnTriggerStay(Collider other)
    {
        itemText.gameObject.SetActive(true);
    }

    void OnTriggerExit(Collider other)
    {
        itemText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update () {
        
    }
}