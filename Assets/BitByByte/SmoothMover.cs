using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// Attach component to gameobject and call from other scripts to initiate smooth translation and rotation actions
/// 
/// </summary>
public class SmoothMover : MonoBehaviour {

    private List<Coroutine> m_runningRoutines;
    private bool m_moving = false;
    private bool m_rotating = false;

    public bool isActive
    {
        get
        {
            return m_moving || m_rotating;
        }
    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void AbortAll()
    {
        if (isActive)
            StopAllCoroutines();
    }
    public void Move(Vector3 position, float speed)
    {
        StartCoroutine( moveToPosition( position, speed, false ) );
        m_moving = true;
    }
    public void Rotate(Vector3 rotationEuler, float speed)
    {
        StartCoroutine( rotateToEuler( rotationEuler, speed ) );
        m_rotating = true;
    }
    private IEnumerator moveToPosition(Vector3 targetPos, float speed, bool move2D)
    {
        float distance = 0;
        do
        {
            Vector3 position = Vector3.Lerp( transform.position, targetPos, Time.deltaTime * speed );
            if ( move2D )
            {
                position.z = transform.position.z;                
                distance = Vector2.Distance( targetPos, position );
            }
            else {
                distance = Vector3.Distance( targetPos, position );
            }
            transform.position = position;            
            yield return null;
        } while ( distance > 0.1f );
    }

    internal void UiMoveDirection(Vector3 velocity, Vector3 acceleration, float duration)
    {
        StartCoroutine(cr_MoveUiUiDirection(velocity, acceleration, duration));
    }

    private IEnumerator cr_MoveUiUiDirection(Vector3 velocity, Vector3 acceleration, float duration)
    {
        while (duration > 0)
        {
            Vector3 p = (transform as RectTransform).position;
            (transform as RectTransform).position = p + velocity * Time.deltaTime;
            yield return null;
            velocity += acceleration * Time.deltaTime;
            duration -= Time.deltaTime;
        }
    }

    private IEnumerator rotateToEuler(Vector3 rotation, float speed)
    {
        float angleDif = 0;
        Quaternion target = Quaternion.Euler(rotation);
        do
        {
            Quaternion.RotateTowards( transform.rotation, target, Time.deltaTime * speed );
            angleDif = Quaternion.Angle( transform.rotation, target );
            yield return null;
        } while ( angleDif > 0.1f );
    }

    internal void Move2D( Vector3 position, float speed )
    {
        StartCoroutine( moveToPosition( position, speed, true ) );
        m_moving = true;
    }

    //internal void setZoomCallback( Delegate callback )
    //{        
    //    throw new NotImplementedException();
    //}
}
