using UnityEngine;
using DG.Tweening;

public class BallController : MonoBehaviour
{
    public Transform center;
    public float outsideRadius = 2f;
    public float insideRadius = 1.3f;
    public float moveTime = 0.2f;

    [Header("Rotation")]
    public float speed = 360f;             
    public float maxSpeed = 1200f;        
    public float speedIncreaseRate = 0.5f; 

    float angle;
    float radius;
    bool isInside = false;

    Tweener rotationTween;

    void Start()
    {
        radius = outsideRadius;
        StartRotationTween();
    }

    void Update()
    {
        // Increase speed
        if (speed < maxSpeed)
        {
            speed += speedIncreaseRate * Time.deltaTime;
            RestartRotationTween(); 
        }

       
        if (Input.GetMouseButtonDown(0))
        {
            isInside = !isInside;
            float target = isInside ? insideRadius : outsideRadius;

            DOTween.To(() => radius,
                    r => radius = r,
                    target,
                    moveTime)
                .SetEase(Ease.OutBack);
        }

        float rad = angle * Mathf.Deg2Rad;

        transform.position = center.position +
                             new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0) * radius;
    }



    void StartRotationTween()
    {
        float duration = 360f / speed;

        rotationTween = DOTween.To(
                () => angle,
                x => angle = x,
                angle + 360f,
                duration
            )
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Restart);  
    }

    void RestartRotationTween()
    {
        if (rotationTween != null)
            rotationTween.Kill();

        StartRotationTween();
    }
}