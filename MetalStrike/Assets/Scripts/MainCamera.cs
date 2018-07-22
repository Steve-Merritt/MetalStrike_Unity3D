using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    [SerializeField] float speed = 10;

	void Update ()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate((Vector3.forward + Vector3.right) * speed * Time.deltaTime, Space.World);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(-(Vector3.forward + Vector3.right) * speed * Time.deltaTime, Space.World);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(-(Vector3.right - Vector3.forward) * speed * Time.deltaTime, Space.World);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate((Vector3.right - Vector3.forward) * speed * Time.deltaTime, Space.World);
        }
    }
}
