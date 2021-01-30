using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    private const float kGunRadius = 1.3f;

    private Transform m_playerTrasform;
    private Transform m_gunSprite;
    [SerializeField]
    private Transform m_projectileRef;

    private void Awake()
    {
        var p = GameManager.Get().GetPlayer();
        m_playerTrasform = p.transform;

        if (!m_projectileRef)
            Debug.LogError("ASSIGN THE PROJECTILE ADN GUN REF!!!");

        m_gunSprite = m_projectileRef.parent;
    }

    private void Update()
    {
        Vector3 mousePosWs = GameManager.Get().GetMouseWorldPos();
        Vector3 shootDir = (mousePosWs - m_playerTrasform.position).normalized;

        // Add some drag to the gun
        Vector3 targetPosition = shootDir * kGunRadius + m_playerTrasform.position;
        transform.position = Vector3.Lerp(transform.position, targetPosition, 10 * Time.deltaTime);

        // face gun toward shoot diretion
        float rot_z = Mathf.Atan2(shootDir.y, shootDir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
    }
}
