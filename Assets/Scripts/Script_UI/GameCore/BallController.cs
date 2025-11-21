using DG.Tweening;
using UnityEngine;
using ScriptableObjects.GameEvents;

public class BallController : MonoBehaviour
{
    [SerializeField] private IntEvent OnScoreChanged;
    [SerializeField] private NullEvent OnGameEnded;
    [SerializeField] private NullEvent OnStartMoving;
    [SerializeField] private ParticleSystem dieParticle;

    [SerializeField] private Transform center;
    [SerializeField] private float outsideRadius = 2f;
    [SerializeField] private float insideRadius = 1.3f;
    [SerializeField] private float moveTime = 0.2f;

    [Header("Rotation")]
    [SerializeField] private float speed = 360f;       
    [SerializeField] private float maxSpeed = 1200f;  
    [SerializeField] private float speedIncreaseRate = 50f; 

    [SerializeField] private int increasedScoreByPickingStars = 1;

    [Header("Death Animation")]
    [SerializeField] private float dieScaleTime = 0.5f;
    [SerializeField] private float dieRotationTime = 0.5f;
    [SerializeField] private float dieFadeTime = 0.5f;

    private int _score = 0;
    private float angle;
    private float radius;
    private bool isInside = false;
    private bool canMove = false; 

    private SpriteRenderer spriteRenderer;
    private Vector3 _firstLocalScale;
    private float _firstSpeed ;

    private void Start()
    {
        _firstSpeed = speed;
        _firstLocalScale = transform.localScale;
        radius = outsideRadius;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        OnGameEnded.OnEventRaised += Die;
        OnStartMoving.OnEventRaised += StartMovement; // subscribe to start event
    }

    private void OnDisable()
    {
        OnGameEnded.OnEventRaised -= Die;
        OnStartMoving.OnEventRaised -= StartMovement;
    }

    private void StartMovement()
    {
        canMove = true;
    }

    private void Update()
    {
        if (!canMove)
            return;

        if (speed < maxSpeed)
            speed = Mathf.Min(speed + speedIncreaseRate * Time.deltaTime, maxSpeed);

        angle += speed * Time.deltaTime;
        angle %= 360f;

        if (Input.GetMouseButtonDown(0))
        {
            isInside = !isInside;
            float target = isInside ? insideRadius : outsideRadius;
            DOTween.To(() => radius, r => radius = r, target, moveTime)
                   .SetEase(Ease.OutBack);
        }

        float rad = angle * Mathf.Deg2Rad;
        transform.position = center.position + new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0) * radius;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!canMove)
            return;

        if (other.CompareTag("Obsticle"))
            OnGameEnded.Raise();
        else
        {
            _score += increasedScoreByPickingStars;
            OnScoreChanged.Raise(_score);
        }
    }

    private void Die()
    {
        speed = 0;

        if (dieParticle != null)
            dieParticle.Play();
        
        Sequence dieSequence = DOTween.Sequence();
        dieSequence.Append(transform.DOScale(Vector3.zero, dieScaleTime).SetEase(Ease.InBack));
        dieSequence.Join(transform.DORotate(new Vector3(0, 0, 720f), dieRotationTime, RotateMode.FastBeyond360));
        
        if (spriteRenderer != null)
            dieSequence.Join(spriteRenderer.DOFade(0f, dieFadeTime));

        dieSequence.OnComplete(() =>
        {
            float rad = 0f;
            transform.position = center.position + new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0) * radius;
            
            gameObject.SetActive(false);
            DOTween.Kill(transform);
            
            if (spriteRenderer != null)
                spriteRenderer.color = new Color(0,0,0,1);
            
            angle = 0f;
            radius = outsideRadius;
            transform.localScale = _firstLocalScale;
            isInside = false;
            canMove = false;
            speed = _firstSpeed;
            _score = 0;
        });
    }
}
