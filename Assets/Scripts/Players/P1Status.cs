﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P1Status : MonoBehaviour {

    public bool frozen = false;
    public bool shrank = false;
    public bool blown = false;
    public float duration = 5f;

    public float originalSpeed;
    private IEnumerator curUnshrink;
    private IEnumerator curUnfreeze;

    private void Start()
    {
        originalSpeed = gameObject.GetComponent<p1_movement>().speed;
    }

    //void FixedUpdate()
    //{
    //    if (shrank)
    //    {
    //        if (curUnshrink != null) {
    //            StopCoroutine(curUnshrink);
    //        }
    //        curUnshrink = Unshrink();
    //        StartCoroutine(Unshrink());
    //        // Debug.Log("Waiting for unshrink!");
    //    }

    //    if (frozen)
    //    {
    //        if (curUnfreeze != null)
    //        {
    //           StopCoroutine(curUnfreeze);
    //        }
    //        curUnfreeze = Unfreeze();
    //        StartCoroutine(Unfreeze());
    //        // Debug.Log("Waiting for unfreeze!");
    //    }
    //
    //}

    //IEnumerator waitForStatusEnd()
    //{
    //    yield return new WaitForSeconds(duraiton);

    //    Debug.Log("Effect debuffed!!");
    //}

    public void Shrink(float ratio)
    {
        shrank = true;

        // reduce speed after being shrunk
        gameObject.GetComponent<p1_movement>().speed = gameObject.GetComponent<p1_movement>().speed * ratio;

        if (curUnshrink != null)
        {
            StopCoroutine(curUnshrink);
        }
        curUnshrink = Unshrink();
        StartCoroutine(curUnshrink);
        // Debug.Log("Waiting for unshrink!");
    }

    IEnumerator Unshrink()
    {
        yield return new WaitForSeconds(duration);

        // restore original size after being unshrink
        while (transform.localScale.x < 15)
        {
            transform.localScale = transform.localScale + new Vector3(1f, 1f, 1f) * 3f * Time.deltaTime;
            transform.position = new Vector3(transform.position.x,
                                             transform.localScale.y / 2,
                                             transform.position.z);
            yield return null;
        }

        // restore original speed after being unshrink
        if (!frozen)
        {
            gameObject.GetComponent<p1_movement>().speed = originalSpeed;
        }

        shrank = false;
        // Debug.Log("Unshrank!!");
    }

    public void Freeze()
    {
        frozen = true;

        // make speed 0 after being frozen
        gameObject.GetComponent<p1_movement>().speed = 0;

        if (curUnfreeze != null)
        {
            StopCoroutine(curUnfreeze);
        }
        curUnfreeze = Unfreeze();
        StartCoroutine(curUnfreeze);
        // Debug.Log("Waiting for unfreeze!");
    }

    IEnumerator Unfreeze()
    {
        yield return new WaitForSeconds(duration);

        // restore original speed after being unfrozen
        gameObject.GetComponent<p1_movement>().speed = originalSpeed;

        frozen = false;
        //Debug.Log("Unfrozen!!");
    }

    public void BlowAway(float seconds)
    {
        blown = true;
        float curSpeed = gameObject.GetComponent<p1_movement>().speed;
        gameObject.GetComponent<p1_movement>().speed = 0;

        StartCoroutine(UnBlown(seconds, curSpeed));
    }

    IEnumerator UnBlown(float seconds, float curSpeed)
    {
        yield return new WaitForSeconds(seconds);
        blown = false;

        // restore original speed after being unfrozen
        if (frozen)
        {
        }
        else if (shrank)
        {
            gameObject.GetComponent<p1_movement>().speed = curSpeed;
        }
        else
        {
            gameObject.GetComponent<p1_movement>().speed = originalSpeed;
        }

    }

    public void Fall()
    {
        if (shrank)
        {
            StopCoroutine(Unshrink());
            shrank = false;
        }

        gameObject.GetComponent<p1_movement>().speed = 0;
        gameObject.GetComponent<PlayerHealth>().Die();
        gameObject.GetComponent<p1_movement>().speed = originalSpeed;
    }

}
