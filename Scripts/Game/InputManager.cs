using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager {

    float lastMouseDown;

    public List<Vector2Int> inputChoices;
    public UIManager.Card selectedCard;
    public ShiblitzMove selectedMove;

    public InputManager()
    {

    }

    public void showInput()
    {
        inputChoices = selectedMove.getCastableLocations(Game.getPlayer().position);
        foreach(Vector2Int v in inputChoices)
        {
            Game.getDungeonBoard().gui.SetTile((Vector3Int)v, selectedMove.isCantrip ? ShiblitzTile.inputHighlightCantrip : ShiblitzTile.inputHighlight);
        }
    }

    public bool selectMove(ShiblitzMove move)
    {
        if (Game.getState() == Game.State.GETTING_PLAYER_INPUT && Game.getPlayer().mana >= move.manaCost)
        {
            clearInputChoices();
            selectedMove = move;
            showInput();
            return true;
        }
        return false;
    }

    public bool selectCard(UIManager.Card card)
    {
        selectedCard = card;
        ShiblitzMove move = Game.getUIManager().getMove(card);
        if (Game.getState() == Game.State.GETTING_PLAYER_INPUT && Game.getPlayer().mana >= move.manaCost)
        {
            clearInputChoices();
            selectedMove = move;
            showInput();
            return true;
        }
        return false;
    }

    public void update()
    {
        if (Input.GetMouseButtonDown(0) && Input.touchCount < 2)
        {
            lastMouseDown = Time.time;
        }

        if (selectedMove != null && Game.instance.state == Game.State.GETTING_PLAYER_INPUT && Input.GetMouseButtonDown(0))
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2Int touchLocation = new Vector2Int(Mathf.FloorToInt(pos.x), Mathf.FloorToInt(pos.y));
            foreach (Vector2Int v in inputChoices)
            {
                if (v == touchLocation)
                {
                    if(selectedMove.isCantrip){
                        selectedMove.setCastLocation(v);
                        selectedMove.performMove();
                        clearInputChoices();
                        Game.getUIManager().drawIntoSlot(Game.getInputManager().selectedCard);
                    }
                    else {
                        selectedMove.setCastLocation(v);
                        Game.QueueMove(selectedMove);
                        clearInputChoices();
                        Game.getUIManager().drawIntoSlot(Game.getInputManager().selectedCard);
                        Game.finishState(Game.State.GETTING_PLAYER_INPUT);
                    }
                }
            }
        }
    }

    public void clearInputChoices()
    {
        selectedMove = null;
        if (inputChoices == null)
            return;
        foreach(Vector2Int v in inputChoices)
        {
            Game.getDungeonBoard().gui.SetTile((Vector3Int) v, null);
        }
        inputChoices = null;
    }
}
