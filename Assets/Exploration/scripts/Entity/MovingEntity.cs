using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingEntity : MonoBehaviour {

    public class MoveResult
    {
        public enum ResultValue
        {
            Ok,
            TileBlocked,
            TileOccupied
        }
        private ResultValue m_result;
        private World.Cell m_cell;
        public int m_x, m_y;
        public MoveResult(ResultValue result, World.Cell cell, int x, int y)
        {
            m_result = result;
            m_cell = cell;
            m_x = x;
            m_y = y;
        }
        public World.Cell Cell
        {
            get
            {
                return m_cell;
            }
        }

        public ResultValue Result
        {
            get
            {
                return m_result;
            }
        }
        public void GetPosition(out int x, out int y)
        {
            x = m_x;
            y = m_y;
        }

    }
    
    //public float moveSpeed = 1f;
    //private float m_remainingMove = 0f;
    public int moveActionCost = 10;
    //private Vector2 m_moveGoal;
    private SynchronizedActor m_syncActor;
    private Vector3 m_lastPositionUpdate;
    private Quaternion m_lastRotationUpdate;

    public class SyncMoveAction : SynchronizedActor.SyncAction
    {
        private Vector2 m_goal;
        private MovingEntity m_mover;

        public SyncMoveAction(MovingEntity mover, Vector2 moveGoal)
        {
            m_goal = moveGoal;
            m_mover = mover;
        }
        public override void Execute()
        {
            //Debug.Log("Execute Move Action for " + m_mover.name);
            Vector3 realPosition = new Vector3(m_goal.x + xz_offset, m_mover.transform.position.y, m_goal.y + xz_offset);
            Vector3 dir = realPosition - m_mover.transform.position;
            Quaternion rot = Quaternion.LookRotation(dir, Vector3.up);
            //LeanTween.rotate(m_mover.gameObject, rot.eulerAngles, 0.10f);
            m_mover.UpdatePosition(realPosition);
            m_mover.UpdateRotation(rot);
            //LeanTween.move(m_mover.gameObject, realPosition, 0.25f);
            
        }
    }

    private void UpdatePosition(Vector3 pos)
    {
        m_lastPositionUpdate = pos;
    }
    private void UpdateRotation(Quaternion rot)
    {
        m_lastRotationUpdate = rot;
    }

    //position in 2d map. in drawable world x = x but map y =z;
    private const float xz_offset = 0f;

    public Vector2 MapPosition
    {
        get
        {
            return new Vector2(transform.position.x - xz_offset, transform.position.z - xz_offset);
        }
    }

    public Vector2 MoveGoal
    {
        get
        {
            return m_syncActor.Entity.GetPositionVector();
        }
        set
        {
            int x, y;
            m_syncActor.Entity.Move((int)value.x, (int)value.y);
        }
    }

	// Use this for initialization
	void Start () {
        m_lastRotationUpdate = transform.rotation;
        m_lastPositionUpdate = transform.position;
        m_syncActor = GetComponent<SynchronizedActor>();
        if (m_syncActor == null)
            m_syncActor = gameObject.AddComponent<SynchronizedActor>();
	}

    //private void resetGoal()
    //{
    //    m_moveGoal = MapPosition;
    //}
	
	// Update is called once per frame
	void Update () {
        Debug.DrawLine(transform.position, new Vector3(MoveGoal.x+xz_offset, 0f, MoveGoal.y+xz_offset), Color.blue);
        Vector3 deltaPos = m_lastPositionUpdate - transform.position;
        float maxSpeed = Time.deltaTime * 2;
        Vector3 translate = deltaPos * Time.deltaTime * 2;
        if (deltaPos.magnitude <= maxSpeed)
            translate = deltaPos;

        transform.position = transform.position + translate;
        //transform.position = m_lastPositionUpdate;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, m_lastRotationUpdate, 720 * Time.deltaTime);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="direction">as map direction not unity direction</param>
    public MoveResult Move(Vector2 direction)
    {        
        //float move = m_remainingMove + moveSpeed;
        //m_remainingMove = move - (Mathf.Abs(move));

        //Debug.Log("Position on Map: " + MapPosition +", Position on World: "+transform.position);
        //Debug.Log("Move Input Direction: " + direction);

        float absX = Mathf.Abs(direction.x);
        float absY = Mathf.Abs(direction.y);
        float rand = UnityEngine.Random.value;
        int x, y;

        if (absX > absY || (absX == absY && rand < 0.5f))
        {
            x = Math.Sign(direction.x);
            y = 0;
        }
        else
        {
            x = 0;
            y = Math.Sign(direction.y);
        }

        MoveResult result;        
        Vector2 targetPosition = MoveGoal + new Vector2(x, y);
        x = (int)targetPosition.x;
        y = (int)targetPosition.y;
        World.Cell cell = World.Instance.GetGrid().GetCell(x,y);
        //Debug.Log("Cell at " + targetPosition+":  " + cell.ToString());
        if (cell.IsBlocked)
        {
            //Debug.Log("Cell Blocked");
            result = new MoveResult(MoveResult.ResultValue.TileBlocked, cell, x, y);
        }
        else if (cell.ContainsEntity)
        {
            //Debug.Log("Cell Occupied");
            result = new MoveResult(MoveResult.ResultValue.TileOccupied, cell, x, y);
        }
        else
        {
            //Debug.Log("Move OK");
            result = new MoveResult(MoveResult.ResultValue.Ok, cell, x, y);
            MoveGoal = targetPosition;
            m_syncActor.AssignAction(new SyncMoveAction(this, targetPosition));
        }
        return result;
        
    }
}
