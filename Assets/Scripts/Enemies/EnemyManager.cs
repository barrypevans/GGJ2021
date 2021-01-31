using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using UnityEngine.Events;

public class EnemyManager : SystemSingleton<EnemyManager>
{
    private const int BatsPerWave = 10;
    private const int WerewolvesPerWave = 5;

    private Transform[] SpawnLocations;
    private Transform[] TargetLocations;

    public Transform PlayerPosition;
    public bool IsDebug;
    private List<GameObject> _bats = new List<GameObject>();
    private List<GameObject> _werewolves = new List<GameObject>();
    private bool _isWaveFullySpawned;
    private IEnumerator _spawningCoroutine;
    private IEnumerator _attackingCoroutine;

    private GameObject m_batPrefab;
    private GameObject m_werewolfPrefab;

    private int _wave = 0; // manage this in game manager?

    // Start is called before the first frame update
    void Start()
    {
        m_batPrefab = Resources.Load<GameObject>("Gargoyle");
        m_werewolfPrefab = Resources.Load<GameObject>("Werewolf");
        SetLocations();
        if (IsDebug)
        {
            StartWave();
        }
    }

    public List<GameObject> GetBats()
    {
        return _bats;
    }

    public List<GameObject> GetWerewolves()
    {
        return _werewolves;
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
        StartCoroutine(_spawningCoroutine);
    }

    IEnumerator SpawnWerewolves(int totalWerewolfCount)
    {
        _attackingCoroutine = WerewolfAttack();
        StartCoroutine(_attackingCoroutine);
        for (int j = 0; j < totalWerewolfCount / WerewolvesPerWave; j++)
        {
            int spawnLoc = Random.Range(0, SpawnLocations.Length);
            for (int i = 0; i < WerewolvesPerWave; i++)
            {
                var werewolf = Instantiate(m_werewolfPrefab, SpawnLocations[spawnLoc].position, Quaternion.identity);
                _werewolves.Add(werewolf);
                yield return new WaitForSeconds(.2f);
            }
        }
        _isWaveFullySpawned = true;
        yield return null;
    }

    IEnumerator WerewolfAttack()
    {
        while (true)
        {
            if (_isWaveFullySpawned && _werewolves.Count == 0)
            {
                // GameManager.Get().WaveCompleted();
                Debug.Log("Wave completed");
                yield return new WaitForSeconds(5f);
                StartWave(++_wave);
                StopCoroutine(_attackingCoroutine);
            }

            if (_werewolves.Count == 0) yield return new WaitForSeconds(Random.Range(1, 4));
            else
            {
                var werewolf = _werewolves[Random.Range(0, _werewolves.Count)];
                werewolf.GetComponent<Werewolf>().Attack();
                yield return new WaitForSeconds(Random.Range(1, 4));
            }
        }
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
        yield return null;
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
