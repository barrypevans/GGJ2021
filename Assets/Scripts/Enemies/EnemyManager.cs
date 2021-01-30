using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemyManager : SystemSingleton<EnemyManager>
{
    public Transform[] SpawnLocations;
    private GameManager _gm;
    public GameObject GargoylePrefab;
    public Transform PlayerPosition;

    public Transform Target1;
    public Transform Target2;

    private bool _isPlayerSet = false;

    public int MaxGargoyles = 10;
    private List<GameObject> _gargoyles = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        GargoylePrefab = Resources.Load<GameObject>("Gargoyle");
        _gm = GameManager.Get();
        StartCoroutine(SpawnGargoyles());
    }

    IEnumerator SpawnGargoyles()
    {
        for (int i = 0; i < MaxGargoyles; i++)
        {
            var gargoyle = Instantiate(GargoylePrefab, SpawnLocations[0].position, Quaternion.identity);
            gargoyle.GetComponent<Gargoyle>().SetTarget(Target1);
            gargoyle.GetComponent<Gargoyle>().Direction = -1;
            _gargoyles.Add(gargoyle);
            yield return new WaitForSeconds(.5f);
        }
        for (int i = 0; i < MaxGargoyles; i++)
        {
            var gargoyle = Instantiate(GargoylePrefab, SpawnLocations[0].position, Quaternion.identity);
            gargoyle.GetComponent<Gargoyle>().SetTarget(Target2);
            _gargoyles.Add(gargoyle);
            yield return new WaitForSeconds(.5f);
        }
        StartCoroutine(GargoyleAttack());
    }

    IEnumerator GargoyleAttack()
    {
        while (true)
        {
            var eligibleGargoyles = _gargoyles.Where(a => 
                a.GetComponent<Gargoyle>().State == Gargoyle.EnemyState.InPosition).ToList();
            var gargoyle = eligibleGargoyles[Random.Range(0, eligibleGargoyles.Count)];
            gargoyle.GetComponent<Gargoyle>().Attack();
            yield return new WaitForSeconds(Random.Range(1, 4));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!_isPlayerSet)
        {
            _gm.GetPlayer().transform.position = PlayerPosition.position;
            _isPlayerSet = true;
        }
    }
}
