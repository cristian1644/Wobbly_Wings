using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Sprite[] sprites;
    private int spriteIndex;
    private Vector3 direction;
    public float gravity = -9.8f;
    public float strength = 5f;

    private float screenTop;    // Limite superiore dello schermo
    private float screenBottom; // Limite inferiore dello schermo

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        // Calcola i limiti dello schermo in unità mondo
        Camera mainCamera = Camera.main;
        screenTop = mainCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y;
        screenBottom = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y;

        InvokeRepeating(nameof(AnimateSprite), 0.15f, 0.15f);
    }

    private void OnEnable()
    {
        Vector3 position = transform.position;
        position.y = 0f;
        transform.position = position;
        direction = Vector3.zero;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            direction = Vector3.up * strength;
        }

        if(Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if(touch.phase == TouchPhase.Began)
            {
                direction = Vector3.up * strength;
            }
        }
        direction.y += gravity * Time.deltaTime;
        // Applica il movimento
        Vector3 position = transform.position + direction * Time.deltaTime;

        // Limita la posizione verticale del giocatore
        position.y = Mathf.Clamp(position.y, screenBottom, screenTop);

        // Aggiorna la posizione del giocatore
        transform.position = position;
    }

    private void AnimateSprite()
    {
        spriteIndex++;
        
        if(spriteIndex >= sprites.Length)
        {
            spriteIndex = 0;
        }
        spriteRenderer.sprite = sprites[spriteIndex];
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Obstacle")
        {
            FindObjectOfType<GameManager>().GameOver();
        }else if(other.gameObject.tag == "Scoring")
        {
            FindObjectOfType<GameManager>().IncreaseScore();
        }
    }

}
