using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingEntity : MonoBehaviour {


    //public float moveSpeed = 1f;
    //private float m_remainingMove = 0f;
    public int moveActionCost = 10;
    private Vector2 m_moveGoal;
    private SynchronizedActor m_syncActor;

    public class SyncMoveAction : SynchronizedActor.SyncAction
    {
        private Vector2 m_goal;
        private Transform m_mover;

        public SyncMoveAction(Transform mover, Vector2 moveGoal)
        {
            m_goal = moveGoal;
            m_mover = mover;
        }
        public override void Execute()
        {
            Debug.Log("Execute Move Action for " + m_mover.name);
            Vector3 realPosition = new Vector3(m_goal.x + xz_offset, m_mover.position.y, m_goal.y + xz_offset);
            LeanTween.move(m_mover.gameObject, realPosition, 0.25f);
        }
    }

    //position in 2d map. in drawable world x = x but map y =z;
    private const float xz_offset = 0.5f;

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
            return m_moveGoal;
        }
        set
        {
            m_moveGoal = value;            
        }
    }

	// Use this for initialization
	void Start () {
        
        m_syncActor = GetComponent<SynchronizedActor>();
        if (m_syncActor == null)
            m_syncActor = gameObject.AddComponent<SynchronizedActor>();

        resetGoal();
	}

    private void resetGoal()
    {
        m_moveGoal = MapPosition;
    }
	
	// Update is called once per frame
	void Update () {
        Debug.DrawLine(transform.position, new Vector3(m_moveGoal.x+xz_offset, 0f, m_moveGoal.y+xz_offset), Color.blue);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="direction">as map direction not unity direction</param>
    public void Move(Vector2 direction)
    {        
        //float move = m_remainingMove + moveSpeed;
        //m_remainingMove = move - (Mathf.Abs(move));

        Debug.Log("Position on Map: " + MapPosition +", Position on World: "+transform.position);

        Vector2 d = Vector2.zero;
        float absX = Mathf.Abs(direction.x);
        float absY = Mathf.Abs(direction.y);
        
        if (absX > absY)
        {
            d = new Vector2(Mathf.Sign(direction.x),0f);
        }
        else if (absX < absY)
        {
            d = new Vector2(0f, Mathf.Sign(direction.y));
        }
        
        Debug.Log("Move Direction: " + d);

        MoveGoal += d;        
        m_syncActor.AssignAction(new SyncMoveAction(transform, MoveGoal));
    }
}
