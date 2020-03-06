using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShiblitzPlayerBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ShiblitzPlayer p = new ShiblitzPlayer(gameObject);
        Game.attachPlayer(p);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
