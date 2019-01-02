using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopingLerp : MonoBehaviour {

    public Transform start;
    public Transform end;
    public float speed;

	// Use this for initialization
	void Start () {
        StartCoroutine(Routine());
	}
	

    IEnumerator Routine ()
    {
        var start = this.start.position;
        var end = this.end.position;
        transform.position = start;

        float distance = (start - end).magnitude;

        while (true)
        {
            for (float t = 0; t < 1f; t += Time.deltaTime * speed / distance)
            {
                transform.position = Vector3.Lerp(start, end, t);
                yield return null;
            }
        }
    }
}
