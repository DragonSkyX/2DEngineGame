using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    [HideInInspector]
    public bool facingRight = true;         // Determina o lado que o jogador está olhando
    [HideInInspector]
    public bool jump = false;               // Determina se o jogador pode pular!

    private BoxCollider2D coll;
    public float moveForce = 365f;          // Força adicionada ao jogador para se mover da direita para a esquerda e vice versa.
    public float maxSpeed = 5f;            
    public AudioClip[] jumpClips;          
    public float jumpForce = 1000f;           
    public bool grounded = false;
    public Transform checkground;          
    private Animator anim;                 
    private RaycastHit2D hitDown;
    private float distanceDown;
    private BoxCollider2D box;
    public float Angle;
    private RaycastHit2D hitBFront;
    public float slopeFriction= 0.5f;
    private Rigidbody2D body;
    public LayerMask Ground;
  


    void Awake()
    {

        // Chama os componentes!

        anim = GetComponent<Animator>();
        box = GetComponent<BoxCollider2D>();
        body = GetComponent<Rigidbody2D>();
       
    }




    void Check_Angle()
    {

        /**
         * Cálculo do angulo personagem. O Angulo interfere na velocidade de movimentação, além da maneira que o 
         * personagem ficará no sistema. Sistema baseado nos jogos do SONIC.
         * 
         * Se o jogador estiver no chão. Velocidade * cossseno(Angulo);
           Se o jogador estiver pulando. Velocidade * Seno(Angulo);
         * 
         * **/

        RaycastHit2D[] hits = new RaycastHit2D[2];
        int h = Physics2D.RaycastNonAlloc(transform.position, -Vector2.up, hits); //Lança um raio para baixo!
        if (h > 1)
        {
            /**Se H for maior que 1, que dizer que acertou algo! Com isso, o angulo é calculado.
             * 
             * Mathf.Atan2 Retorna o angulo em radianos, referente ao Y/X
             * Mathf.Rad2Deg Conversão simples de Graus para radianos.
             * 
             * Transform. Euler Angles  Função para passar o objeto para a rotação do objeto em si.
            **/




            Angle = Mathf.Abs(Mathf.Atan2(hits[1].normal.x, hits[1].normal.y) * Mathf.Rad2Deg);
            transform.eulerAngles = new Vector3 (transform.rotation.x, transform.rotation.y, Angle);

            
            
        }
    }

        void Check_Ground() {

        Bounds groundCheckBounds = CalculateGroundCheckBounds();
        grounded = Physics2D.BoxCast(groundCheckBounds.center, groundCheckBounds.size, 0, Vector2.right, 0, Ground);



        // Se o botão de pulo for pressionado, e o jogador estiver no "CHÃO", libera o pulo.
        if (Input.GetButtonDown("Jump") && grounded)
        {
            jump = true;
           
        }

    }


    void DebugLOG (){

        Debug.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y - 1f), Color.green, 2f, false);
        Debug.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y + 1f), Color.green, 2f, false);
        Debug.DrawLine(transform.position, new Vector2(transform.position.x + 0.5f, transform.position.y), Color.red, 2f, false);
        Debug.DrawLine(transform.position, new Vector2(transform.position.x - 0.5f, transform.position.y), Color.red, 2f, false);
       
        




    }

    void Update(){

        DebugLOG();
        Check_Ground();
       




    }

    Bounds CalculateGroundCheckBounds()
    {
        Vector2 center = new Vector2(transform.position.x + box.offset.x, box.bounds.min.y);
        Vector2 size = new Vector2(box.bounds.max.x - box.bounds.min.x, 0.5f);

        return new Bounds(center, size);
    }

    void Movement() {

        // Armazena o eixo horizontal.
        float h = Input.GetAxis("Horizontal");

        // Velocidade da animação, baseado no Exio Horizontal;
        anim.SetFloat("Speed", Mathf.Abs(h));

        //Compara se o Jogador está no Chão. Se estiver, faça!

        if (grounded) {

            Check_Angle();

            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 2f, 1 << 12);

            /* if (hit.collider != null && Angle > 0.3f && Angle < 0.6f)
             {

                 // Aplica força contrária a força da superficie
                 // Quanto maior a fricção, maior a força contrária?
                 body.velocity = new Vector2(body.velocity.x - (hit.normal.x * slopeFriction), body.velocity.y);
                 Vector3 pos = transform.position;
                 pos.y += -hit.normal.x * Mathf.Abs(body.velocity.x) * Time.deltaTime * (body.velocity.x - hit.normal.x > 0 ? 1 : -1);
                 transform.position = pos;
             }*/
            if (hit.collider != null) { 

                // Se jogador mudou de de direção e ainda não alcançou a velocidade máxima
                if (h * body.velocity.x < maxSpeed )
                // Adiciona força ao jogador e compara o angulo que ele está.
                body.AddForce(Vector2.right * h * (moveForce * Mathf.Cos(Angle)));

            // Se a velocidade do jogador estiver acima da velocidade máxima
            if (Mathf.Abs(body.velocity.x) > maxSpeed)
            {
                // ... set the player's velocity to the maxSpeed in the x axis.
                body.velocity = new Vector2(Mathf.Sign(body.velocity.x) * maxSpeed *Mathf.Cos(Angle), Mathf.Sign(body.velocity.y)* Mathf.Sin(Angle));
            }
        // Se o jogador pressiona para a esquerda, e ele está na direita.
        if (h > 0 && !facingRight)
            
            Flip();
       // Se for o contráruio da função acima.
        else if (h < 0 && facingRight)
          
            Flip();

            }

            // Se o jogador conseguir pular
        if (jump){
            // Aciona a animação de pulo!
            anim.SetTrigger("Jump");

            /* Executa um som aleatório
            int i = Random.Range(0, jumpClips.Length);
            AudioSource.PlayClipAtPoint(jumpClips[i], transform.position);*/

            // Adiciona uma força vertical ao jogador!
            body.AddForce(new Vector2(body.velocity.x*h*Mathf.Cos(Angle), jumpForce));

            //Jogador não pode pular de novo, a não ser que esteja no CHÃO!
            jump = false;
        }

            
        
                
            }

       
    }


    void FixedUpdate()
    {


        Movement();

    }


    void Flip()
    {
       // Troca os lados do Jogador
        facingRight = !facingRight;

        // Inverte a imagem, invertendo a posição de escala. Inverte o colisor inferior também...

        
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
       
    }


}