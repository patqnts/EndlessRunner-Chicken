using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EggScript : MonoBehaviour
{
    // Start is called before the first frame update
    public EggScore egg;
    void Start()
    {
         egg = FindObjectOfType<EggScore>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            egg.CollectEgg();
            Destroy(gameObject);
        }
    }
}
