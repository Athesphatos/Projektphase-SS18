using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunRotation : MonoBehaviour {

    public int rotationOffset = 90;

    public PlayerController pc;
    public int playerID;

    private void Start()
    {
        playerID = (int)GetComponentInParent<PlayerController>().m_playerID;
    }

    // Update is called once per frame
    void Update() {
        MouseDirection();
        ControllerDirection();
    }

    void MouseDirection() { 
        
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        difference.Normalize();

        float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotZ + rotationOffset); 
    }

    
    void ControllerDirection()
    {
        // Mathf.Atan2(-YAxis, XAxis) * Mathf.Rad2Deg definiert die Rotation.
        float XAxis = Input.GetAxis("LeftJoystickHorizontal" + playerID);
        float YAxis = Input.GetAxis("LeftJoystickVertical" +playerID);
        
        float rotZ = Mathf.Atan2(-YAxis, XAxis) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotZ + rotationOffset);


    }
    void Controllertest(){
            // Controller A-Test
        if (Input.GetAxis("LeftJoystickHorizontal" + playerID) > 0)
        {

                Debug.Log("L3 Rechts");
        }
        if (Input.GetAxis("LeftJoystickHorizontal" + playerID) < 0)
        {

                Debug.Log("L3 Links");
        }
        if (Input.GetAxis("LeftJoystickVertical" + playerID) > 0)
        {

                Debug.Log("L3 Unten");
        }
        if (Input.GetAxis("LeftJoystickVertical" + playerID) < 0)
        {

                Debug.Log("L3 Oben");
        }
    }
    
}
