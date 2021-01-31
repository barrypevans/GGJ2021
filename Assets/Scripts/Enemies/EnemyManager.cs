using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using UnityEngine.Events;

public class EnemyManager : SystemSingleton<EnemyManager>
{
    private Transform[] SpawnLocations;
    private Transform[] TargetLocations;

    private GameObject m_batPrefab;
    public Transform PlayerPosition;
    public bool IsDebug;
    private int BatsPerWave = 10;
    private List<GameObject> _bats = new List<GameObject>();
    private bool _isWaveFullySpawned;
    private IEnumerator _spawningCoroutine;
    private IEnumerator _attackingCoroutine;

    private int _wave = 0; // manage this in game manager?

    // Start is called before the first frame update
    void Start()
    {
        m_batPrefab = Resources.Load<GameObject>("Gargoyle");

        if (IsDebug)
        {
            SetLocations();
            StartWave();
        }
    }

    public List<GameObject> GetBats()
    {
        return _bats;
    }

    public void SetLocations()
    {
        SpawnLocations = GameObject.FindGameObjectsWithTag("SpawnPoint").Select(a=>a.transform).ToArray();
        var targets = GameObject.FindGameObjectsWithTag("TargetPoint");
        foreach (var target in targets)
            target.AddComponent<HoverTarget>();
        TargetLocations = targets.Select(a => a.transform).ToArray();
    }

    public void StartWave(int wave = 0) // change bat properties based on wave?
    {
        Debug.Log("Starting wave: " + wave);
        _isWaveFullySpawned = false;
        switch (wave)
        {
            case 0:
                _spawningCoroutine = SpawnBats(20);
                break;
            case 1:
                _spawningCoroutine = SpawnBats(40);
                break;
            case 2:
                _spawningCoroutine = SpawnBats(60);
                break;
        }
        string sceneName = "Wave" + wave.ToString();
        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        Scene scene = SceneManager.GetSceneByName(sceneName);

        UnityAction sceneLoadDelegate = ()=>
        {
            SceneManager.SetActiveScene(scene);
            SetLocations();
            StartCoroutine(_spawningCoroutine);
        };
        
        StartCoroutine(WaitForScene(scene, sceneLoadDelegate));
    }

    IEnumerator WaitForScene(Scene scene, UnityAction action)
    {
        while(!scene.isLoaded)
            yield return new WaitForEndOfFrame();
        action.Invoke();
    }

    IEnumerator SpawnBats(int totalBatCount)
    {
        _attackingCoroutine = BatAttack();
        StartCoroutine(_attackingCoroutine);
        for (int j = 0; j < totalBatCount / BatsPerWave; j++) {
            int spawnLoc = Random.Range(0, SpawnLocations.Length);
            int targetLoc = Random.Range(0, TargetLocations.Length);
            for (int i = 0; i < BatsPerWave; i++)
            {
                var bat = Instantiate(m_batPrefab, SpawnLocations[spawnLoc].position, Quaternion.identity);
                bat.GetComponent<Bat>().SetTarget(TargetLocations[targetLoc]);
                _bats.Add(bat);
                yield return new WaitForSeconds(.2f);
            }
        }
        _isWaveFullySpawned = true;
    }

    IEnumerator BatAttack()
    {
        while (true)
        {
            if (_isWaveFullySpawned && _bats.Count == 0)
            {
                // GameManager.Get().WaveCompleted();
                Debug.Log("Wave completed");
                yield return new WaitForSeconds(5f);
                StartWave(++_wave);
                StopCoroutine(_attackingCoroutine);
            }

            var eligibleBats = _bats.Where(a => 
                a.GetComponent<Bat>().State == Bat.EnemyState.InPosition &&
                a.activeSelf).ToList();
            if (eligibleBats.Count == 0) yield return new WaitForSeconds(Random.Range(1, 4));
            else
            {
                var bat = eligibleBats[Random.Range(0, eligibleBats.Count)];
                bat.GetComponent<Bat>().Attack();
                yield return new WaitForSeconds(Random.Range(1, 4));
            }
        }
    }
}
