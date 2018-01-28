using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{

    private MovingEntity m_movingEntity;
    private AttackingEntity m_attackingEntity;
    private SynchronizedActor m_actor;
    private LevelBuilder m_levelBuilder;
    private bool m_myTurn;
    private GameObject m_crosshair;

    // Use this for initialization
    void Start()
    {
        m_movingEntity = GetComponent<MovingEntity>();
        m_attackingEntity = GetComponent<AttackingEntity>();
        m_actor = GetComponent<SynchronizedActor>();
        m_levelBuilder = FindObjectOfType<LevelBuilder>();
        m_actor.OnTurnStatusChange += actor_OnTurnStatusChange;
    }
    public event Action OnPlayerMovedToEndEvent = null;
    public event Action OnPlayerMovedToObjectiveEvent = null;
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
    void Update()
    {
        if (m_myTurn)
        {
            
            World.Cell mouseOverCell = GetMouseOverCell();
            if (mouseOverCell != null && mouseOverCell.ContainsEntity)
            {
                int x, y;
                Entity ent = mouseOverCell.GetEntity();
                ent.GetPosition(out x, out y);
                
                if (!"Player".Equals(ent.GetType().ToString()))
                    SetCrosshair(x, y);
                else
                    HideCrosshair();

                if (Input.GetMouseButtonDown(0))
                {
                    AttackingEntity.AttackResult res = CombatSolver.Fight(m_actor, mouseOverCell.GetEntity().Actor);

                    if (res != null && res.Weapon != null)
                    {
                        Synchronizer.Continue(m_actor, res.Weapon.TimeCost);
                        return;
                    }
                    else if (res.Result == AttackingEntity.AttackResult.ResultValue.NoEnergy)
                        Debug.Log(res.Weapon.Name+" Out of energy");

                }
            }
            else
            {
                HideCrosshair();
            }

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
                //Debug.Log("Move: " + move);
                MovingEntity.MoveResult result = m_movingEntity.Move(move);
                switch (result.Result)
                {
                    case MovingEntity.MoveResult.ResultValue.Ok:
                        Synchronizer.Continue(m_actor, m_movingEntity.moveActionCost);

                        if (m_levelBuilder.LevelType != LevelType.Ambush)
                        {

                            if (m_levelBuilder.EndPoint.X == result.Cell.X && m_levelBuilder.EndPoint.Y == result.Cell.Y)
                            {
                                if (OnPlayerMovedToEndEvent != null)
                                {
                                    OnPlayerMovedToEndEvent();
                                }
                            }

                            if(m_levelBuilder.ObjectivePoint.X == result.Cell.X && m_levelBuilder.ObjectivePoint.Y == result.Cell.Y)
                            {
                                if(OnPlayerMovedToObjectiveEvent != null)
                                {
                                    OnPlayerMovedToObjectiveEvent();
                                }
                            }

                        }
                        break;
                    case MovingEntity.MoveResult.ResultValue.TileBlocked:
                        //Debug.Log("Tile blocked!");
                        break;
                    case MovingEntity.MoveResult.ResultValue.TileOccupied:
                        AttackingEntity.AttackResult res = CombatSolver.Fight(m_actor, result.Cell.GetEntity().Actor);
                        Synchronizer.Continue(m_actor, res.Weapon.TimeCost);
                        break;
                }
            }
        }
    }

    private World.Cell GetMouseOverCell()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            int x = (int)(hit.point.x + 0.5);
            int y = (int)(hit.point.y + 0.5);
            int z = (int)(hit.point.z + 0.5);
            Vector3 loc = new Vector3(x, y, z);
            //Debug.Log(loc);
            World.Cell cell = World.Instance.GetGrid().GetCell((int)loc.x, (int)loc.z);
            return cell;
        }
        return null;
    }

    private GameObject SetCrosshair(int x, int y)
    {
        if (m_crosshair == null)
            m_crosshair = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        m_crosshair.transform.position = new Vector3(x, 2, y);
        m_crosshair.SetActive(true);
        return m_crosshair;
    }

    private void HideCrosshair()
    {
        if (m_crosshair != null)
            m_crosshair.SetActive(false);
    }
}

