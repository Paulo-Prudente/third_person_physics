using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam;

    public string playerState = "idle";
    public bool isDashing = false;

    public float speed = 10.0f;
    public float speedLimit = 39.0f;

    public float turnSmoothTime = 0.1f;
    float turnSmoorthVelocity;

    public Vector3 direction;

    //contador pro dash
    public float waitCounter=0f;


    //ataque
    public bool attacking = false;
    public bool podeAtacar = true;
    public GameObject hitboxAtaque;


    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        direction = new Vector3(horizontal, 0f, vertical).normalized;


        if (direction.magnitude >= 0.1f)
        {
            playerState = "walking";

            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoorthVelocity, turnSmoothTime);

            transform.rotation = Quaternion.Euler(0f, angle, 0f);



            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);



            //acelera
            if (speed <= speedLimit)
                speed += 1.5f;
        }
        else
        {
            playerState = "idle";

            //desacelera
            speed = 10.0f;
        }

        print(playerState);

        if (!isDashing)
            controller.Move(Vector3.down * speed / 1.2f * Time.deltaTime);


        /////////////////////////////////////
        //DASH!

        if (Input.GetKeyDown("space"))
        {
            isDashing = true;

            speed += 50;
            waitCounter = 1;
        }

        if (waitCounter > 0.9f && waitCounter < 1.4f)
        {
            waitCounter += Time.deltaTime;
        }

        if (waitCounter >= 1.4f)
        {
            speed = 10;
            waitCounter = 0;

            isDashing = false;
        }
        //////////////////////////////////////
        ///




        //////////////////////////////////////
        //ATTACK!

        if (Input.GetMouseButtonDown(0))
        {
            attacking = true;
        }

        //////////////////////////////////////

    }

    private void FixedUpdate()
    {
        //ATAQUE
        if (attacking && podeAtacar)
        {
            attacking = false;

            StartCoroutine(TaxaDeLancamento(1.5f));

            StartCoroutine(AttackWithStartupTime(0.3f, 1));
        }
    }

    private IEnumerator TaxaDeLancamento(float qtdDeSegundos)
    {
        podeAtacar = false;
        yield return new WaitForSeconds(qtdDeSegundos);
        podeAtacar = true;
    }

    private IEnumerator AttackWithStartupTime(float qtdDeSegundos, int qualJogador)
    {
        yield return new WaitForSeconds(qtdDeSegundos);

        GameObject hitboxAtaqueInstanciado = Instantiate(hitboxAtaque, transform.position + Vector3.Cross(transform.right, new Vector3(0, 6, -5)), Quaternion.identity) as GameObject;
    }


}
