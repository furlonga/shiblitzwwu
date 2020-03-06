using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameDriver : MonoBehaviour
{

    Game game;

    // Awake is called before Start (Im pretty sure...)
    void Awake()
    {
        Game.instance = new Game();
        game = Game.instance;
        Game.instance.Start();
    }

    private void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
        game.update();
    }

    private void FixedUpdate()
    {
        game.fixedUpdate();
    }
}
