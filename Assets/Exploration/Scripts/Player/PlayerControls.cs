using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour {

    private MovingEntity m_movingEntity;
    private SynchronizedActor m_actor;
    private bool m_myTurn;

    // Use this for initialization
    void Start () {
        m_movingEntity = GetComponent<MovingEntity>();
        m_actor = GetComponent<SynchronizedActor>();
        m_actor.OnTurnStatusChange += actor_OnTurnStatusChange;
	}

    private void actor_OnTurnStatusChange(bool hasTurn)
    {
        if (hasTurn)
            Debug.Log("Player gained turn");
        else
        {
            Debug.Log("Player Turn End");
        }
        m_myTurn = hasTurn;
    }

    // Update is called once per frame
    void Update () {
        if (m_myTurn)
        {
            Vector2 move = Vector2.zero;
            if (Input.GetButtonDown("Horizontal"))
            {
                float h = Input.GetAxis("Horizontal");
                move.x = Mathf.Sign(h);
            }
            if (Input.GetButtonDown("Vertical"))
            {
                float h = Input.GetAxis("Vertical");

                move.y = Mathf.Sign(h);
            }

            if (move != Vector2.zero)
            {
                Debug.Log("Move: " + move);
                MovingEntity.MoveResult result = m_movingEntity.Move(move);
                switch (result.Result)
                {
                    case MovingEntity.MoveResult.ResultValue.Ok:
                        Synchronizer.Continue(m_actor, m_movingEntity.moveActionCost);
                        break;
                    case MovingEntity.MoveResult.ResultValue.TileBlocked:
                        Debug.Log("Tile blocked!");
                        break;
                    case MovingEntity.MoveResult.ResultValue.TileOccupied:
                        Debug.Log("Tile Occupied! Maybe attack?! ");
                        Entity entity = result.Cell.GetEntity();
                        if (entity.GetType() == typeof(Monster))
                        {
                            Debug.Log(name+" Attacking "+ entity.Actor.name);
                        }
                        break;
                }

                
            }
            
        }
	}
}
