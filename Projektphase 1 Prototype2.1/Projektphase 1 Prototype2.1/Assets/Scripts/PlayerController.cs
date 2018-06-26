using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public Transform FirePoint;
    public GameObject projectile;
    public LayerMask notToHit;
    bool canShoot = true;
    public float cooldown = 5f;
    public float FireForce = 200f;
    public float angle = 45f;
    public Rigidbody2D rb;

    public EPlayerID m_playerID;

    public enum EPlayerID
    {
        Player1 = 1,
        Player2,
        Player3,
        Player4
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        RaycastC();

        if (Input.GetButtonDown("Shoot" + (int)m_playerID) && canShoot == true)
        {
            ShootCN();
            StartCoroutine(CanShoot());
        }

        if (Input.GetButtonDown("BButton" + (int)m_playerID) && canShoot == true)
        {
            ShootCT();
            StartCoroutine(CanShoot());
        }

    }

    IEnumerator CanShoot()
    {
        canShoot = false;
        yield return new WaitForSeconds(cooldown);
        canShoot = true;
    }

    void RaycastC()
    {
        float XAxis = Input.GetAxis("LeftJoystickHorizontal" + (int)m_playerID);
        float YAxis = Input.GetAxis("LeftJoystickVertical" + (int)m_playerID);
        Vector2 axisPosition = new Vector2(XAxis, -YAxis);
        Vector2 FirePointPosition = new Vector2(FirePoint.position.x, FirePoint.position.y);
        RaycastHit2D hit = Physics2D.Raycast(FirePointPosition, axisPosition - FirePointPosition, 100, notToHit);
        Debug.DrawLine(FirePointPosition, axisPosition * 100);
    }

    // normaler Schuss
    void ShootCN()
    {
        float XAxis = Input.GetAxis("LeftJoystickHorizontal" + (int)m_playerID);
        float YAxis = Input.GetAxis("LeftJoystickVertical" + (int)m_playerID);

        Vector2 FirePointPosition = new Vector2(FirePoint.position.x, FirePoint.position.y);

        GameObject go1 = (GameObject)Instantiate(projectile, FirePointPosition, FirePoint.rotation);
        go1.GetComponent<Rigidbody2D>().velocity = new Vector2(XAxis, -YAxis).normalized * (FireForce * Time.deltaTime);


        //Rückstoß
        BackforceC();
    }

    // Dreifachschuss gestreut
    void ShootCT()
    {
        float XAxis = Input.GetAxis("LeftJoystickHorizontal" + (int)m_playerID);
        float YAxis = Input.GetAxis("LeftJoystickVertical" + (int)m_playerID);

        Vector2 FirePointPosition = new Vector2(FirePoint.position.x, FirePoint.position.y);

        GameObject go1 = (GameObject)Instantiate(projectile, FirePointPosition, FirePoint.rotation);
        go1.GetComponent<Rigidbody2D>().velocity = new Vector2(XAxis, -YAxis).normalized * (FireForce * Time.deltaTime);



        // Vector (x,y) um phi rotiert:
        // xRot = x *cos(phi)  -  y *sin(phi)
        // yRot = x *sin(phi)  +  y *cos(phi)

        float xRotDown = (XAxis) * Mathf.Cos(-angle) - (-YAxis) * Mathf.Sin(-angle);
        float yRotDown = (XAxis) * Mathf.Sin(-angle) + (-YAxis) * Mathf.Cos(-angle);
        GameObject go2 = (GameObject)Instantiate(projectile, FirePointPosition, FirePoint.rotation);
        go2.GetComponent<Rigidbody2D>().velocity = new Vector2(xRotDown, yRotDown).normalized * (FireForce * Time.deltaTime);


        float xRotUp = (XAxis) * Mathf.Cos(angle) - (-YAxis) * Mathf.Sin(angle);
        float yRotUp = (XAxis) * Mathf.Sin(angle) + (-YAxis) * Mathf.Cos(angle);
        GameObject go3 = (GameObject)Instantiate(projectile, FirePointPosition, FirePoint.rotation);
        go3.GetComponent<Rigidbody2D>().velocity = new Vector2(xRotUp, yRotUp).normalized * (FireForce * Time.deltaTime);


        //Rückstoß
        BackforceC();
    }

    void BackforceC()
    {
        float XAxis = Input.GetAxis("LeftJoystickHorizontal" + (int)m_playerID);
        float YAxis = Input.GetAxis("LeftJoystickVertical" + (int)m_playerID);

        rb.GetComponent<Rigidbody2D>().velocity = new Vector2(XAxis * -1, (-YAxis) * -1).normalized * (FireForce * Time.deltaTime);
    }
}
