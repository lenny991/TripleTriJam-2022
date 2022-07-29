using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTowardsMouse : MonoBehaviour
{
    Camera cam;

    private void Start()
    {
        cam = Camera.main;
    }

    private void FixedUpdate()
    {
        Vector2 mousePos = Input.mousePosition;
        Vector2 mouseInWorld = cam.ScreenToWorldPoint(mousePos);

        //COPIED CODE FROM EARLIER PROJECT DONT MIND THIS

        //Get the angle between the points
        float angle = AngleBetweenTwoPoints(transform.position, mouseInWorld);

        //Ta Daaa
        transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle + 90));
    }
    float AngleBetweenTwoPoints(Vector3 a, Vector3 b) =>
        Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
}
