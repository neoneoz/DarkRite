using UnityEngine;
using System.Collections;

public class selfdelete : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        StartCoroutine("OnCompleteAttackAnimation");

        //if (GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Finished"))
        //{
        //    Debug.Log("deleting");
        //    Destroy(gameObject);
        //}
	}
    IEnumerator OnCompleteAttackAnimation()
    {
        while (GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
            yield return null;

        // TODO: Do something when animation did complete
        Destroy(gameObject);
    
    }
    public void destroy()
    {
        Destroy(gameObject);
    }
}
