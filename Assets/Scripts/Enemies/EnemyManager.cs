using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : SystemSingleton<EnemyManager>
{
    public Transform[] SpawnLocations;
    private GameManager _gm;
    public GameObject GargoylePrefab;

    public int MaxGargoyles = 25;
    // Start is called before the first frame update
    void Start()
    {
        _gm = GameManager.Get();
        StartCoroutine(SpawnGargoyles());
    }

    IEnumerator SpawnGargoyles()
    {
        for (int i = 0; i < MaxGargoyles; i++)
        {
            Instantiate(GargoylePrefab, SpawnLocations[0].position, Quaternion.identity);
            yield return new WaitForSeconds(.5f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
