using UnityEngine;
using System.Collections;


public class Player_Movement : MonoBehaviour
/*Cria um controlador para o personagem. **/


{
    [HideInInspector]
    public bool facingRight = true;         // Determina o lado que o jogador está olhando
    [HideInInspector]
    public bool jump = false;               // Determina se o jogador pode pular!

    private BoxCollider2D coll;
    public float moveForce = 365f;          // Força adicionada ao jogador para se mover da direita para a esquerda e vice versa.
    public float maxSpeed = 5f;
    public AudioClip[] jumpClips;
    public float jumpSpeed = 10f;
    public float MaxJump = 100f;
    public bool grounded = false;
    public Transform checkground;
    private Animator anim;
    private RaycastHit2D hitDown;
    private float distanceDown;
    private BoxCollider2D box;
    public float[] AngleBX = new float[3];
    public float[] AngleLX = new float[3];
    public float slopeFriction = 0.5f;
    public LayerMask Ground;
    public float Speed = 5f;
    public float BFX, BCX, BBX, LFX, LCX, LBX;
    public float RotationTime = 0.02f;
    public float minSpeed;
    RaycastHit2D[] SBX0 = new RaycastHit2D[2];
    RaycastHit2D[] SBX1 = new RaycastHit2D[2];
    RaycastHit2D[] SBX2 = new RaycastHit2D[2];
    RaycastHit2D[] LBX0 = new RaycastHit2D[2];
    RaycastHit2D[] LBX1 = new RaycastHit2D[2];
    RaycastHit2D[] LBX2 = new RaycastHit2D[2];
    public float Acel;
    public float Desac;

    void Awake()
    {

        // Chama os componentes!

        anim = GetComponent<Animator>();
        box = GetComponent<BoxCollider2D>();
        


        
        
    }

    void UpdateRaycast() {
        float boxX = box.size.x;
        float boxY = box.size.y;

        BFX = Physics2D.RaycastNonAlloc(new Vector2(transform.position.x + boxX, transform.position.y), -Vector2.up, SBX0);
        BBX = Physics2D.RaycastNonAlloc(new Vector2(transform.position.x - boxX, transform.position.y), -Vector2.up, SBX2);
        BCX = Physics2D.RaycastNonAlloc(new Vector2(transform.position.x, transform.position.y), -Vector2.up, SBX1);
        LFX = Physics2D.RaycastNonAlloc(new Vector2(transform.position.x, transform.position.y + boxY), -Vector2.right, LBX0);
        LBX = Physics2D.RaycastNonAlloc(new Vector2(transform.position.x, transform.position.y - boxY), -Vector2.right, LBX2);
        LCX = Physics2D.RaycastNonAlloc(new Vector2(transform.position.x, transform.position.y), -Vector2.right, LBX1);

    }

    void Check_Angle()
    {


        /* Verifica o Angulo da plataforma*/



        AngleBX[0] = Mathf.Atan2(SBX0[1].normal.x, SBX0[1].normal.y) * Mathf.Rad2Deg;
        AngleBX[1] = Mathf.Atan2(SBX1[1].normal.x, SBX1[1].normal.y) * Mathf.Rad2Deg;
        AngleBX[2] = Mathf.Atan2(SBX2[1].normal.x, SBX2[1].normal.y) * Mathf.Rad2Deg;
        AngleLX[0] = Mathf.Atan2(LBX0[1].normal.x, LBX0[1].normal.y) * Mathf.Rad2Deg;
        AngleLX[1] = Mathf.Atan2(LBX1[1].normal.x, LBX1[1].normal.y) * Mathf.Rad2Deg;
        AngleLX[2] = Mathf.Atan2(LBX2[1].normal.x, LBX2[1].normal.y) * Mathf.Rad2Deg;






        Debug.Log(SBX1[1].collider + "Colisor SBX1");
        Debug.Log(SBX1[1].distance + "Colisor SBX1");

        if (SBX1[1].distance < 1){ 

        transform.rotation = Quaternion.Euler(new Vector3 (transform.localRotation.x, transform.localRotation.y, AngleBX[1]-1));
        }
      



    }



  

       
     

    /** Sistema de detecção de movimento baseado em, Raycast2D**/

       

    void Check_Ground()
    {

 


        if (SBX1[1].collider != null && SBX1[1].distance <1.5f)
        {
            grounded = true;
        }

        else
        {
            grounded = true;
        }

       

        if (Input.GetButtonDown("Jump") && grounded)
        {
            jump = true;

        }
    }




    void Update()
    {

      
        Check_Ground();
        UpdateRaycast();



    }


    void Movement()
    {

        // Armazena o eixo horizontal.


        // Velocidade da animação, baseado no Exio Horizontal;
        // anim.SetFloat("Speed", Mathf.Abs(Direction()));

        //Compara se o Jogador está no Chão. Se estiver, faça!

        if (grounded)
        {
            
            /**Trabalhar o movimento em todos os sentidos do angulo **/
          
            if (SBX1[1].collider != null)

            {
                
                Aceleration();
                transform.Translate(new Vector2(1,0 )* Direction() *Speed *Mathf.Abs(Mathf.Cos(AngleBX[1])) * Time.deltaTime);
                Debug.Log(Mathf.Abs(Mathf.Cos(AngleBX[1])) + "C" + Mathf.Abs(Mathf.Sin(AngleBX[1])) + "S");

                Debug.Log(transform.eulerAngles);
            }












            // Se o jogador conseguir pular
            if (jump)
            {
                // Aciona a animação de pulo!
                anim.SetTrigger("Jump");



                // Aciona a Rotina de PULO.

                StartCoroutine(Jump());

            }


            /* // Se o jogador pressiona para a esquerda, e ele está na direita.
             if (h > 0 && !facingRight)

                 Flip();
             // Se for o contráruio da função acima.
             else if (h < 0 && facingRight)

                 Flip();*/


        }


    }


    float Direction()
    {
        float direction = Input.GetAxis("Horizontal");
        return direction;

    }


    void Aceleration()
    {



        if ((Direction() == 1 || Direction() == -1) && Mathf.Abs(Speed) < maxSpeed)
        {
            Speed += Acel;
        }



        // Se a velocidade do jogador estiver abaixo da velocidade minima


        if ((Direction() == 0 && Mathf.Abs(Speed) > minSpeed))
        {

            Speed -= Acel;
        }



    }

    IEnumerator Rotate(float tempAngle)
    {

        float initialAngle = AngleBX[1];


        for (int i = 0; i < tempAngle; i++)
        {

            
            Quaternion.Euler(new Vector3(transform.localRotation.x, transform.localRotation.y, initialAngle));
            transform.rotation =  Quaternion.Euler(new Vector3(transform.localRotation.x, transform.localRotation.y, initialAngle));
            initialAngle += 0.1f;
            yield return new WaitForSeconds(RotationTime);




        }
    }

    IEnumerator gravitySystem()
    {

        if (!grounded && !jump)
        {
            transform.Translate(new Vector2(Direction() * Mathf.Abs(Mathf.Cos(Speed)) * Time.smoothDeltaTime, -1 * jumpSpeed * Time.smoothDeltaTime));
            // Vector2.Lerp(transform.position, checkground.position, 0.1f * Time.smoothDeltaTime);
            if (grounded)
            {
                StopAllCoroutines();
            }
        }

        yield return new WaitForEndOfFrame();
    }





    IEnumerator Jump()
    {

        float maxJump = transform.position.y + 5.0f;
      

        while (true)
        {

            if (transform.position.y >= maxJump)
            {
                jump = false;
                StopAllCoroutines();
            }

            if (jump)
            {
                transform.Translate(new Vector2(Direction() * Time.smoothDeltaTime, 1 * jumpSpeed * Time.smoothDeltaTime));

            }


            yield return new WaitForEndOfFrame();
        }

    }



    void FixedUpdate()
    {

        Check_Angle();
        Movement();
        StartCoroutine(gravitySystem());
    }


    /*  void Flip()
      {
          // Troca os lados do Jogador
          facingRight = !facingRight;

          // Inverte a imagem, invertendo a posição de escala. Inverte o colisor inferior também...


          Vector3 theScale = transform.localScale;
          theScale.x *= -1;
          transform.localScale = theScale;

      }*/


}