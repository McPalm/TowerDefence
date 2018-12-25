using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bob : MonoBehaviour
{
    public float origDelay;
    public float targetDelay;

    public Vector3 offset;


	// Use this for initialization
	void Start () {
        StartCoroutine(Routine());
	}
	
	IEnumerator Routine()
    {
        yield return null;
        yield return new WaitForSeconds(1f + (transform.position.x * .3f)%(origDelay+targetDelay));
        var origin = transform.localPosition;
        var target = transform.localPosition + offset;


        while(true)
        {
            transform.localPosition = target;
            yield return new WaitForSeconds(targetDelay);
            transform.localPosition = origin;
            yield return new WaitForSeconds(origDelay);
        }
    }
}
