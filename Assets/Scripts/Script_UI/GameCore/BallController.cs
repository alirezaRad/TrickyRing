using UnityEngine;
using DG.Tweening;

public class BallController : MonoBehaviour
{
    public Transform center;
    public float outsideRadius = 2f;
    public float insideRadius = 1.3f;
    public float moveTime = 0.2f;
    public float angle;
    public float speed = 360f;

    bool isInside = false;
    float radius;

    void Start()
    {
        radius = outsideRadius;

        DOTween.To(() => angle,
                x => angle = x,
                360f,
                360f/speed)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Incremental);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isInside = !isInside;
            float target = isInside ? insideRadius : outsideRadius;

            DOTween.To(() => radius,
                    r => radius = r,
                    target,
                    moveTime)
                .SetEase(Ease.OutCubic);
        }

        float rad = angle * Mathf.Deg2Rad;

        transform.position = center.position +
                             new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0) * radius;
    }
}