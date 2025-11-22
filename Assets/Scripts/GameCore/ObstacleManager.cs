using ScriptableObjects.GameEvents;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class ObstacleManager : MonoBehaviour
{
    public IntEvent OnScoreChanged;
    public NullEvent OnBallStartMovingEvent;
    public NullEvent OnGameOverEvent;
    

    public Transform center;
    public GameObject obstaclePrefab;
    public GameObject scoreItemPrefab;

    public float insideRadius = 1.3f;
    public float outsideRadius = 2f;

    public int scoreStep = 100;
    private int lastScoreStep = 0;

    public float spawnAnimationTime = 0.2f;
    public float removeAnimTime = 0.25f;

    public float minDistanceBetweenObjects = 0.5f;

    List<GameObject> obstacles = new List<GameObject>();
    GameObject currentScoreItem;
    
    private bool isGameOver = false;

    
    
    private void OnBallStartMoving()
    {
        var seq = DOTween.Sequence();
        seq.AppendCallback(() =>
        {
            isGameOver = false;
            for (int i = 0; i < 4; i++)
                SpawnNewObstacle();
            RefreshScoreItem();
        }).SetDelay(0.5f);
    }


    void OnEnable()
    {
        OnScoreChanged.OnEventRaised += OnScoreEvent;
        OnBallStartMovingEvent.OnEventRaised += OnBallStartMoving;
        OnGameOverEvent.OnEventRaised += CleaningRing;
    }

    private void CleaningRing()
    {
        isGameOver = true;
        List<GameObject> toRemove = new List<GameObject>();

        for (int i = 0; i < obstacles.Count; i++)
        {
            toRemove.Add(obstacles[i]);
        }
        
        obstacles.Clear();
        
        foreach (var ob in toRemove)
                RemoveWithAnimation(ob);
        
        RemoveScoreItem(currentScoreItem);
      
    }

    void OnDisable()
    {
        OnScoreChanged.OnEventRaised -= OnScoreEvent;
        OnBallStartMovingEvent.OnEventRaised -= OnBallStartMoving;
        OnGameOverEvent.OnEventRaised -= CleaningRing;
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
        currentScoreItem.GetComponentInChildren<ParticleSystem>().Play();
        RefreshScoreItem();
    }

    void RefreshObstacles()
    {
        if(isGameOver) return;
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
        if(isGameOver) return;
        
        Vector3 pos = GetValidSpawnPosition();
        GameObject ob = Instantiate(obstaclePrefab, pos, Quaternion.identity);
        obstacles.Add(ob);

        var firstScale = ob.transform.localScale;
        ob.transform.localScale = Vector3.zero;
        ob.transform.up = (pos - center.position).normalized;
        ob.transform.DOScale(firstScale, spawnAnimationTime).SetEase(Ease.OutBack);
    }

    void RefreshScoreItem()
    {
        if(isGameOver) return;
        
        if (currentScoreItem != null)
            RemoveScoreItem(currentScoreItem);

        Vector3 pos = GetValidSpawnPosition();
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

    Vector3 GetValidSpawnPosition()
    {
        int attempts = 0;
        Vector3 pos = Vector3.zero;
        float radius = Random.value > 0.5f ? insideRadius * transform.parent.localScale.x : outsideRadius* transform.parent.localScale.x;

        while (attempts < 50)
        {
            float angle = Random.Range(0f, 360f);
            float rad = angle * Mathf.Deg2Rad;
            pos = center.position + new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0) * radius;

            bool tooClose = false;
            foreach (var ob in obstacles)
            {
                if (Vector3.Distance(pos, ob.transform.position) < minDistanceBetweenObjects)
                {
                    tooClose = true;
                    break;
                }
            }

            if (currentScoreItem != null && Vector3.Distance(pos, currentScoreItem.transform.position) < minDistanceBetweenObjects)
                tooClose = true;

            if (!tooClose)
                break;

            attempts++;
        }

        return pos;
    }

    void RemoveWithAnimation(GameObject ob)
    {
        DOTween.Kill(ob.transform);
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
        
        DOTween.Kill(ob.transform);
        ob.transform.DOScale(Vector3.zero, removeAnimTime)
            .SetEase(Ease.InBack)
            .OnComplete(() =>
            {
                
                if (ob != null)
                    Destroy(ob);
            });
    }
}
