using ScriptableObjects.GameEvents;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ObstacleManager : MonoBehaviour
{
    public IntEvent OnScoreChanged;

    public Transform center;
    public GameObject obstaclePrefab;
    public GameObject scoreItemPrefab;

    public float insideRadius = 1.3f;
    public float outsideRadius = 2f;

    public int scoreStep = 100;
    private int lastScoreStep = 0;

    public float spawnAnimationTime = 0.2f;
    public float removeAnimTime = 0.25f;

    List<GameObject> obstacles = new List<GameObject>();
    GameObject currentScoreItem;


    void Start()
    {
        for (int i = 0; i < 4; i++)
            SpawnNewObstacle();
        RefreshScoreItem();
    }


    void OnEnable()
    {
        OnScoreChanged.OnEventRaised += OnScoreEvent;
    }

    void OnDisable()
    {
        OnScoreChanged.OnEventRaised -= OnScoreEvent;
    }


    void OnScoreEvent(int score)
    {
        int step = score / scoreStep;

        if (step > lastScoreStep)
        {
            lastScoreStep = step;
            SpawnNewObstacle();
        }

        RefreshObstacles();
        RefreshScoreItem();
    }


    void RefreshObstacles()
    {
        if (obstacles.Count == 0)
            return;

        int removeCount = Mathf.CeilToInt(obstacles.Count * 0.4f);
        List<GameObject> toRemove = new List<GameObject>();

        for (int i = 0; i < removeCount; i++)
        {
            int index = Random.Range(0, obstacles.Count);
            toRemove.Add(obstacles[index]);
            obstacles.RemoveAt(index);
        }

        foreach (var ob in toRemove)
            RemoveWithAnimation(ob);

        for (int i = 0; i < removeCount; i++)
            SpawnNewObstacle();
    }


    void SpawnNewObstacle()
    {
        bool inside = Random.value > 0.5f;
        float radius = inside ? insideRadius : outsideRadius;

        float angle = Random.Range(0f, 360f);
        float rad = angle * Mathf.Deg2Rad;

        Vector3 pos = center.position + new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0) * radius;

        GameObject ob = Instantiate(obstaclePrefab, pos, Quaternion.identity);
        obstacles.Add(ob);

        var firstScale = ob.transform.localScale;
        ob.transform.localScale = Vector3.zero;

        ob.transform.up = (pos - center.position).normalized;

        ob.transform.DOScale(firstScale, spawnAnimationTime).SetEase(Ease.OutBack);
    }


    void RefreshScoreItem()
    {
        if (currentScoreItem != null)
            RemoveScoreItem(currentScoreItem);

        SpawnScoreItem();
    }


    void SpawnScoreItem()
    {
        bool inside = Random.value > 0.5f;
        float radius = inside ? insideRadius : outsideRadius;

        float angle = Random.Range(0f, 360f);
        float rad = angle * Mathf.Deg2Rad;

        Vector3 pos = center.position + new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0) * radius;

        currentScoreItem = Instantiate(scoreItemPrefab, pos, Quaternion.identity);

        var firstScale = currentScoreItem.transform.localScale;
        currentScoreItem.transform.localScale = Vector3.zero;

        currentScoreItem.transform.up = (pos - center.position).normalized;

        currentScoreItem.transform.DOScale(firstScale, spawnAnimationTime)
            .SetEase(Ease.OutBack);

        currentScoreItem.transform.DORotate(new Vector3(0, 0, 360f), 1.4f, RotateMode.FastBeyond360)
            .SetLoops(-1)
            .SetEase(Ease.Linear);
    }


    void RemoveWithAnimation(GameObject ob)
    {
        ob.transform.DOScale(Vector3.zero, removeAnimTime)
            .SetEase(Ease.InBack)
            .OnComplete(() =>
            {
                if (ob != null)
                    Destroy(ob);
            });
    }


    void RemoveScoreItem(GameObject ob)
    {
        ob.transform.DOScale(Vector3.zero, removeAnimTime)
            .SetEase(Ease.InBack)
            .OnComplete(() =>
            {
                if (ob != null)
                    Destroy(ob);
            });
    }
}
