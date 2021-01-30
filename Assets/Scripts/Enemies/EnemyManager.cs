using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemyManager : SystemSingleton<EnemyManager>
{
    private Transform[] SpawnLocations;
    private Transform[] TargetLocations;

    private GameObject m_batPrefab;
    public Transform PlayerPosition;
    public bool Debug;
    private int BatsPerWave = 10;
    private List<GameObject> _bats = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        m_batPrefab = Resources.Load<GameObject>("Gargoyle");

        if (Debug)
        {
            SetLocations();
            StartFirstWave();
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

    public void StartFirstWave()
    {
        StartCoroutine(SpawnBats());
    }

    IEnumerator SpawnBats()
    {
        StartCoroutine(BatAttack());
        for (int j = 0; j < 50; j++) {
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

    }

    IEnumerator BatAttack()
    {
        while (true)
        {
            var eligibleGargoyles = _bats.Where(a => 
                a.GetComponent<Bat>().State == Bat.EnemyState.InPosition &&
                a.activeSelf).ToList();
            if (eligibleGargoyles.Count == 0) yield return new WaitForSeconds(Random.Range(1, 4));
            else
            {
                var gargoyle = eligibleGargoyles[Random.Range(0, eligibleGargoyles.Count)];
                gargoyle.GetComponent<Bat>().Attack();
                yield return new WaitForSeconds(Random.Range(1, 4));
            }
        }
    }
}
