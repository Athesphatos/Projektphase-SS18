#define mouse
#define controller
#undef mouse

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shootSomething : MonoBehaviour
{

    public GameObject projectile;
    public Vector2 velocity;
    bool canShoot = true;
    public Vector2 offset = new Vector2(0.4f, 0.1f);
    public float cooldown = 5f;

    public Transform FirePoint;
    public LayerMask notToHit;
    public Rigidbody2D rb;
    public float FireForce = 200f;
    public float angle = 45f;



    //-------------------------------------------------------------------------U P D A T E------------------------------------------------------------------------------------
    void Update()
    {
#if mouse
        //______________M O U S E___________I N P U T___________
        RaycastM();

        if (Input.GetKeyDown(KeyCode.Mouse0) && canShoot == true)
        {
            ShootM();

            StartCoroutine(CanShoot());
        }
#elif controller
        //_________C O N T R O L L E R______I N P U T_____________
        RaycastC();

        if (Input.GetButtonDown("AButton") && canShoot == true)
        {
            ShootCN();
            StartCoroutine(CanShoot());
        }

        if (Input.GetButtonDown("BButton") && canShoot == true)
        { 
            ShootCT(); 
            StartCoroutine(CanShoot());
        }
#endif        
    }

    //-------------------------------F U N K T I O N S---------------------------
    //___________________________________________________________________________

    IEnumerator CanShoot()
    {
        canShoot = false;
        yield return new WaitForSeconds(cooldown);
        canShoot = true;
    }

    //_________________________________________________M O U S E____________________________________________________________________________________________________
    void RaycastM()
    {
        Vector2 mousePosition = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
        Vector2 FirePointPosition = new Vector2(FirePoint.position.x, FirePoint.position.y);
        RaycastHit2D hit = Physics2D.Raycast(FirePointPosition, mousePosition - FirePointPosition, 100, notToHit);
        Debug.DrawLine(FirePointPosition, (mousePosition - FirePointPosition) * 100);

    }

    void ShootM()
    {
        Vector2 mousePosition = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
        Vector2 FirePointPosition = new Vector2(FirePoint.position.x, FirePoint.position.y);

        // Vector (x,y) um phi rotiert:
        // xRot = x *cos(phi)  -  y *sin(phi)
        // yRot = x *sin(phi)  +  y *cos(phi)

        GameObject go1 = (GameObject)Instantiate(projectile, FirePointPosition, Quaternion.identity);
        go1.GetComponent<Rigidbody2D>().velocity = new Vector2(mousePosition.x - FirePointPosition.x, mousePosition.y - FirePointPosition.y).normalized * (FireForce * Time.deltaTime);
        Debug.Log(mousePosition);

        double xRotDown = (mousePosition.x - FirePointPosition.x) * Mathf.Cos(-angle) - (mousePosition.y - FirePointPosition.y) * Mathf.Sin(-angle);
        double yRotDown = (mousePosition.x - FirePointPosition.x) * Mathf.Sin(-angle) + (mousePosition.y - FirePointPosition.y) * Mathf.Cos(-angle);
        GameObject go2 = (GameObject)Instantiate(projectile, FirePointPosition, Quaternion.identity);
        go2.GetComponent<Rigidbody2D>().velocity = new Vector2((float)xRotDown, (float)yRotDown).normalized * (FireForce * Time.deltaTime);


        double xRotUp = (mousePosition.x - FirePointPosition.x) * Mathf.Cos(angle) - (mousePosition.y - FirePointPosition.y) * Mathf.Sin(angle);
        double yRotUp = (mousePosition.x - FirePointPosition.x) * Mathf.Sin(angle) + (mousePosition.y - FirePointPosition.y) * Mathf.Cos(angle);
        GameObject go3 = (GameObject)Instantiate(projectile, FirePointPosition, Quaternion.identity);
        go3.GetComponent<Rigidbody2D>().velocity = new Vector2((float)xRotUp, (float)yRotUp).normalized * (FireForce * Time.deltaTime);

        //Console Test
        Debug.Log("Cos: " + Mathf.Cos(angle));
        Debug.Log("CosGrad: " + Mathf.Acos(angle));
        Debug.Log("Sin: " + Mathf.Sin(angle));
        Debug.Log("SinGrad: " + Mathf.Asin(angle));

        //Rückstoß
        BackforceM();
    }

    void BackforceM()
    {
        Vector2 mousePosition = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
        Vector2 FirePointPosition = new Vector2(FirePoint.position.x, FirePoint.position.y);
        Debug.Log("Backforce!");
        rb.GetComponent<Rigidbody2D>().velocity = new Vector2((mousePosition.x - FirePointPosition.x) * -1, (mousePosition.y - FirePointPosition.y) * -1).normalized * (FireForce * Time.deltaTime);


    }
    //________________________________C O N T R O L L E R____________________________________________________________________________________________________________________________
    void RaycastC()
    {
        float XAxis = Input.GetAxis("LeftJoystickHorizontal");
        float YAxis = Input.GetAxis("LeftJoystickVertical");
        Vector2 axisPosition = new Vector2(XAxis, -YAxis);
        Vector2 FirePointPosition = new Vector2(FirePoint.position.x, FirePoint.position.y);
        RaycastHit2D hit = Physics2D.Raycast(FirePointPosition, axisPosition - FirePointPosition, 100, notToHit);
        Debug.DrawLine(FirePointPosition, axisPosition * 100);
    }

    // normaler Schuss
    void ShootCN()
    { 
        float XAxis = Input.GetAxis("LeftJoystickHorizontal");
        float YAxis = Input.GetAxis("LeftJoystickVertical");

        Vector2 FirePointPosition = new Vector2(FirePoint.position.x, FirePoint.position.y);

        GameObject go1 = (GameObject)Instantiate(projectile, FirePointPosition, FirePoint.rotation );
        go1.GetComponent<Rigidbody2D>().velocity = new Vector2(XAxis, -YAxis).normalized* (FireForce* Time.deltaTime);


        //Rückstoß
        BackforceC();
    }

    // Dreifachschuss gestreut
    void ShootCT()
    {
        float XAxis = Input.GetAxis("LeftJoystickHorizontal");
        float YAxis = Input.GetAxis("LeftJoystickVertical");

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
        float XAxis = Input.GetAxis("LeftJoystickHorizontal");
        float YAxis = Input.GetAxis("LeftJoystickVertical");

        rb.GetComponent<Rigidbody2D>().velocity = new Vector2(XAxis * -1, (-YAxis) * -1).normalized * (FireForce * Time.deltaTime);
    }




} 
