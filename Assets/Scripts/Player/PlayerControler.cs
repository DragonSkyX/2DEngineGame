using UnityEngine;
using System.Collections;

public class PlayerControler : MonoBehaviour
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
    public float[] AngleTX = new float[3];
    public float[] AngleRX = new float[3];
    public float[] AngleLX = new float[3];
    public float slopeFriction = 0.5f;
    public LayerMask Ground;
    public float Speed = 5f;
    public float BFX,BCX,BBX,TFX,TCX,TBX,RFX,RCX,RBX,LFX,LCX,LBX;
    public float RotationTime = 0.02f;
    public float minSpeed;
    private RaycastHit2D[] hit = new RaycastHit2D[4];




    public float Acel;
    public float Desac;

    void Awake()
    {

        // Chama os componentes!

        anim = GetComponent<Animator>();
        box = GetComponent<BoxCollider2D>();


    }


   

    float Check_Angle()
    {

        /**
         * Cálculo do angulo personagem. O Angulo interfere na velocidade de movimentação, além da maneira que o 
         * personagem ficará no sistema. Sistema baseado nos jogos do SONIC.
         * 
         * Se o jogador estiver no chão. Velocidade * cossseno(Angulo);
         * 
         *
         *Sistema de Sensores por RaycastHit2D. Vai confirmar se cada lado está sendo acertado por algo. Vai virar uma função a parte depois.
         * 
         *SBX[0] 
         *SBX[1] 
         *SBX[2] 
         *STX[0] 
         *STX[1] 
         *STX[2] 
         *SRX[0] 
         *SRX[1] 
         *SRX[2] 
         *SLX[0] 
         *SLX[1] 
         *SLX[2] 
         *
         * 
         * **/

        float boxX = box.size.x;
        float boxY = box.size.y;

        RaycastHit2D[] SBX0 = new RaycastHit2D[2];
        RaycastHit2D[] SBX1 = new RaycastHit2D[2];
        RaycastHit2D[] SBX2 = new RaycastHit2D[2];
        RaycastHit2D[] STX0 = new RaycastHit2D[2];
        RaycastHit2D[] STX1 = new RaycastHit2D[2];
        RaycastHit2D[] STX2 = new RaycastHit2D[2];
        RaycastHit2D[] SRX0 = new RaycastHit2D[2];
        RaycastHit2D[] SRX1 = new RaycastHit2D[2];
        RaycastHit2D[] SRX2 = new RaycastHit2D[2];
        RaycastHit2D[] SLX0 = new RaycastHit2D[2];
        RaycastHit2D[] SLX1 = new RaycastHit2D[2];
        RaycastHit2D[] SLX2 = new RaycastHit2D[2];

        BFX = Physics2D.RaycastNonAlloc(new Vector2(transform.position.x + boxX, transform.position.y), -Vector2.up, SBX0);
        BCX = Physics2D.RaycastNonAlloc(transform.position, -Vector2.up, SBX1); 
        BBX = Physics2D.RaycastNonAlloc(new Vector2(transform.position.x - boxX, transform.position.y), -Vector2.up, SBX2); 
        TFX = Physics2D.RaycastNonAlloc(new Vector2(transform.position.x + boxX, transform.position.y), Vector2.up, STX0); 
        TCX = Physics2D.RaycastNonAlloc(transform.position, Vector2.up, STX1); 
        TBX = Physics2D.RaycastNonAlloc(new Vector2(transform.position.x - boxX, transform.position.y), Vector2.up, STX2); 
        RFX = Physics2D.RaycastNonAlloc(new Vector2(transform.position.x + boxY, transform.position.y), Vector2.right, SRX0);   
        RCX = Physics2D.RaycastNonAlloc(transform.position, Vector2.right, SRX1); 
        RBX = Physics2D.RaycastNonAlloc(new Vector2(transform.position.x - boxY, transform.position.y), Vector2.right, SRX2); 
        LFX = Physics2D.RaycastNonAlloc(new Vector2(transform.position.x + boxY, transform.position.y), -Vector2.right, SLX0);  
        LCX = Physics2D.RaycastNonAlloc(transform.position, -Vector2.right, SLX1); 
        LBX = Physics2D.RaycastNonAlloc(new Vector2(transform.position.x - boxY, transform.position.y), -Vector2.right, SLX2);


        AngleBX[0]  = (Mathf.Atan2(SBX0[1].normal.x, SBX0[1].normal.y) * Mathf.Rad2Deg);
        AngleBX[1]  = (Mathf.Atan2(SBX1[1].normal.x, SBX1[1].normal.y) * Mathf.Rad2Deg);
        AngleBX[2]  = (Mathf.Atan2(SBX2[1].normal.x, SBX2[1].normal.y) * Mathf.Rad2Deg);
        AngleTX[0]  = (Mathf.Atan2(STX0[1].normal.x, STX0[1].normal.y) * Mathf.Rad2Deg);
        AngleTX[1]  = (Mathf.Atan2(STX1[1].normal.x, STX1[1].normal.y) * Mathf.Rad2Deg);
        AngleTX[2]  = (Mathf.Atan2(STX2[1].normal.x, STX2[1].normal.y) * Mathf.Rad2Deg);
        AngleRX[0]  = (Mathf.Atan2(SRX0[1].normal.x, SRX0[1].normal.y) * Mathf.Rad2Deg);
        AngleRX[1]  = (Mathf.Atan2(SRX1[1].normal.x, SRX1[1].normal.y) * Mathf.Rad2Deg);
        AngleRX[2]  = (Mathf.Atan2(SRX2[1].normal.x, SRX2[1].normal.y) * Mathf.Rad2Deg);
        AngleLX[0]  = (Mathf.Atan2(SLX0[1].normal.x, SLX0[1].normal.y) * Mathf.Rad2Deg);
        AngleLX[1]  = (Mathf.Atan2(SLX1[1].normal.x, SLX1[1].normal.y) * Mathf.Rad2Deg);
        AngleLX[2]  = (Mathf.Atan2(SLX2[1].normal.x, SLX2[1].normal.y) * Mathf.Rad2Deg);




        /**Se H for maior que 1, que dizer que acertou algo! Com isso, o angulo é calculado.
            * 
            * Mathf.Atan2 Retorna o angulo em radianos, referente ao Y/X
            * Mathf.Rad2Deg Conversão simples de Graus para radianos.
            * 
            * Transform. Euler Angles  Função para passar o objeto para a rotação do objeto em si.
           **/

        /** Movimento no sentido Anti-Horário **/


        if (AngleBX[0] > AngleBX[1] && AngleBX[0] < 0)
        {

            float tempAngle = ((AngleBX[2] + AngleBX[1]) / 2) * -1;
            StartCoroutine(Rotate(tempAngle));


        }


        if (AngleBX[0] > AngleBX[1] && AngleBX[0] > 0)
        {
            float tempAngle = ((AngleBX[2] + AngleBX[1]) / 2) * -1;
            StartCoroutine(Rotate(tempAngle));
        }

    


        transform.eulerAngles = new Vector3(transform.rotation.x, transform.rotation.y, AngleBX[1] * -1);

        return AngleBX[1];










    }

    

    RaycastHit2D[] HitPlatformer() {


        hit[0] = Physics2D.Raycast(transform.position, Vector2.down, 1f, 1 << 12);
        hit[1] = Physics2D.Raycast(transform.position, Vector2.up, 1f, 1 << 12);
        hit[2] = Physics2D.Raycast(transform.position, Vector2.left, 1f, 1 << 12);
        hit[3] = Physics2D.Raycast(transform.position, Vector2.right, 1f, 1 << 12);
        return hit;

    }



    void Check_Ground()
    {

        HitPlatformer();


        if (hit[0].collider != null)
        {
            grounded = true;
        }

        else if (hit[0].collider == null && (hit[2].collider != null || hit[2].collider != null) && AngleBX[1] >= 80)
        {

            grounded = true;

        }
        else {
            grounded = false;
        }

        Debug.Log(hit[0].distance);

        if (Input.GetButtonDown("Jump") && grounded)
        {
            jump = true;

        }
    }
        
    

    void DebugLOG()
    {

        Debug.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y - 1f), Color.green, 2f, false);
        Debug.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y + 1f), Color.green, 2f, false);
        Debug.DrawLine(transform.position, new Vector2(transform.position.x + 0.5f, transform.position.y), Color.red, 2f, false);
        Debug.DrawLine(transform.position, new Vector2(transform.position.x - 0.5f, transform.position.y), Color.red, 2f, false);






    }

    void Update()
    {

        DebugLOG();
        Check_Ground();





    }

    /*Bounds CalculateGroundCheckBounds()
    {
        Vector2 center = new Vector2(transform.position.x + box.offset.x, box.bounds.min.y);
        Vector2 size = new Vector2(box.bounds.max.x - box.bounds.min.x, 0.5f);

        return new Bounds(center, size);
    }*/

    void Movement()
    {

        // Armazena o eixo horizontal.


        // Velocidade da animação, baseado no Exio Horizontal;
       // anim.SetFloat("Speed", Mathf.Abs(Direction()));

        //Compara se o Jogador está no Chão. Se estiver, faça!

        if (grounded)
        {



            HitPlatformer();
            Debug.Log(hit[0].collider);
            if (hit[0].collider != null && AngleBX[1] < 85)
            { 
                Aceleration();
                transform.Translate(new Vector2(1, 0) * Direction() * (Mathf.Abs(Mathf.Cos(Check_Angle()))) * Time.deltaTime * Speed);
            }

            if (hit[0].collider != null && AngleBX[1] > 85)
            {
                Aceleration();
                transform.Translate(new Vector2(1, 0) * Direction() * (Mathf.Abs(Mathf.Cos(Check_Angle()))) * Time.deltaTime * Speed);
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


    float Direction() {
        float direction = Input.GetAxis("Horizontal");
        return direction;

    }


    void Aceleration() {



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

            transform.eulerAngles = new Vector3(transform.rotation.x, transform.rotation.y, initialAngle);
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
        HitPlatformer();
        
        while (true)
             {

            if (transform.position.y >= maxJump)
            {
                jump = false;
                StopAllCoroutines();
        }

            if(jump) {
                transform.Translate(new Vector2(Direction()* Time.smoothDeltaTime, 1 * jumpSpeed * Time.smoothDeltaTime));

            }
          

            yield return new WaitForEndOfFrame();
        }

    }
    
     

        void FixedUpdate()
    {


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