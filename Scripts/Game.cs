using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;
using UnityEngine.UI;

public class Game
{
    public static Game instance;
    
    private Dungeon dungeon;
    private EnemyHandler enemyHandler;
    private DungeonBoard dungeonBoard;
    private ShiblitzPlayer player;
    private CameraController camera;
    private InputManager inputManager;
    private UIManager uiManager;
    private SpellManager spellManager;

    private List<ShiblitzMove> queuedMoves;
    private int moveQueuePointer = 0;

    private int timeInFixedFrames;
    private float timerStart;

    private Slider inputTimer;
    
    /*
     * Should look something like
     * Game starts -> ROUND_START
     * Do animations for showing input choices, taking damage over time, etc, then -> GETTING_PLAYER_INPUT
     * Once player gives round-ending input or the timer runs out -> PERFORMING_ACTIONS
     * animations should occur showing movement and attacks, etc, then we can cycle back to -> ROUND_START
     */
    public enum State { ROUND_START, GETTING_PLAYER_INPUT, PERFORMING_ACTIONS }
    public State state { get; set; }

    public Game()
    {
        enemyHandler = new EnemyHandler();
        dungeonBoard = new DungeonBoard();
        dungeon = new Dungeon(new Seed());
        camera = GameObject.Find("Main Camera").GetComponent<CameraController>();
        inputManager = new InputManager();
        inputTimer = GameObject.Find("InputTimer").GetComponent<Slider>();
        uiManager = new UIManager();
        spellManager = new SpellManager();
    }

    public static void attachPlayer(ShiblitzPlayer p)
    {
        instance.player = p;
        instance.player.setPosition(new Vector2Int(1, 1));
        instance.uiManager.setMaxStats();
        instance.uiManager.reflectPlayerStats();

    }
    public static DungeonBoard getDungeonBoard()
    {
        return instance.dungeonBoard;
    }

    public static Dungeon getDungeon()
    {
        return instance.dungeon;
    }

    public static EnemyHandler getEnemyHandler()
    {
        return instance.enemyHandler;
    }
    public static UIManager getUIManager()
    {
        return instance.uiManager;
    }

    public static ShiblitzPlayer getPlayer()
    {
        return instance.player;
    }

    public static CameraController getCamera()
    {
        return instance.camera;
    }

    public static InputManager getInputManager()
    {
        return instance.inputManager;
    }
    public static SpellManager getSpellManager() {
        return instance.spellManager;
    }

    public void Start()
    {
        instance.dungeonBoard.showFogOfWar();
        instance.dungeon.spawnEnemies();
        DungeonSector startingSector = instance.dungeon.getSector(new Vector2Int(1, 1));
        startingSector.reveal();
        camera.panAndZoomTo(startingSector);
        state = State.ROUND_START;
        queuedMoves = new List<ShiblitzMove>();
    }

    public void update()
    {
        switch (state)
        {
            case State.ROUND_START:       
                Game.getSpellManager().tick();
                Game.getEnemyHandler().tick();
                Spell s = Game.getSpellManager().getSpell(Game.getPlayer().position);
                if(s!= null) {
                    Game.getPlayer().takeDamage(s.damage);
                }
                Game.getDungeonBoard().occupiedSpaces = new List<Vector2Int>();
                timeInFixedFrames = 0;
                //inputManager.selectMove(new GodMove(Game.getPlayer()));
                foreach (Enemy e in enemyHandler.getAggroedEnemies())
                {
                    e.queueMove();
                }
                finishState(State.ROUND_START);
                break;
            case State.GETTING_PLAYER_INPUT:
                inputTimer.value = ((300.0f - timeInFixedFrames) / 300);
                if (timeInFixedFrames > 300)
                    finishState(State.GETTING_PLAYER_INPUT);
                // InputHandler can get input from the user while this state is active
                break;
            case State.PERFORMING_ACTIONS:
                if(queuedMoves.Count == 0)
                {
                    moveQueuePointer = 0;
                    queuedMoves = new List<ShiblitzMove>();
                    finishState(State.PERFORMING_ACTIONS);
                    break;
                }
                ShiblitzMove move = queuedMoves[moveQueuePointer];
                if (!move.started())
                {
                    move.performMove();
                }
                if(Game.getEnemyHandler().getAggroedEnemies().Count == 0)
                {
                    moveQueuePointer = 0;
                    queuedMoves = new List<ShiblitzMove>();
                    finishState(State.PERFORMING_ACTIONS);
                    break;
                }
                if(move.isFinished())
                {
                    moveQueuePointer++;
                }
                if (moveQueuePointer >= queuedMoves.Count)
                {
                    moveQueuePointer = 0;
                    queuedMoves = new List<ShiblitzMove>();
                    finishState(State.PERFORMING_ACTIONS);
                }
                break;
            default:
                Debug.Log("Game state is not listed in State enum");
                break;
        }
        inputManager.update();
    }

    public static State getState()
    {
        return instance.state;
    }

    public void fixedUpdate()
    {
        timeInFixedFrames++;
    }

    public static void finishState(State state)
    {
        if(instance.state != state)
        {
            Debug.Log("tried to finish a state that we were not on...");
        }
        else
        {
            switch (state)
            {
                case State.GETTING_PLAYER_INPUT:
                    instance.state = State.PERFORMING_ACTIONS;
                    instance.queuedMoves.Sort(delegate (ShiblitzMove m1, ShiblitzMove m2) { return m2.speed - m1.speed; });
                    getInputManager().clearInputChoices(); 
                    break;
                case State.PERFORMING_ACTIONS:
                    instance.state = State.ROUND_START;
                    foreach(Enemy e in Game.getEnemyHandler().getAggroedEnemies())
                    {
                        if(e.position == Game.getPlayer().position)
                        {
                            getPlayer().health--;
                        }
                    }
                    Game.getPlayer().gainMana(1);
                    getUIManager().reflectPlayerStats();
                    break;
                case State.ROUND_START:
                    instance.state = State.GETTING_PLAYER_INPUT;
                    break;
                default:
                    Debug.Log("unhandled state");
                    break;
            }
        }
    }

    public static void QueueMove(ShiblitzMove m)
    {
        instance.queuedMoves.Add(m);
    }
}
