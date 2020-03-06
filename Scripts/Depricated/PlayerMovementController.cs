using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovementController : MonoBehaviour
{
    public GameObject up;
    public GameObject down;
    public GameObject left;
    public GameObject right;

    
    public ShiblitzPlayer player;

    // Start is called before the first frame update
    void Start()
    {
        /*up.GetComponent<Button>().onClick.AddListener(upOnClick);
        down.GetComponent<Button>().onClick.AddListener(downOnClick);
        left.GetComponent<Button>().onClick.AddListener(leftOnClick);
        right.GetComponent<Button>().onClick.AddListener(rightOnClick);
        */
    }

    // Update is called once per frame
    void Update()
    {
    }
}
