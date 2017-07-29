using UnityEngine;
using System.Collections;

public class BattleCameraMovement : MonoBehaviour {

    public float speed = 0.1f;

    private void OnMouseDown()
    {
        if(Input.touches.Length == 2 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
            transform.Translate(transform.position.x - Input.GetAxis("horizontal") * speed, transform.position.y -  Input.GetAxis("vertical") * speed, 0);
        }
    }

}
