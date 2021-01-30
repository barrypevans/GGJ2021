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

    private const float kBulletSpread = 10.0f;
    private const float kFireRate = 10.0f;
    private const float kBulletSpeed = 50.0f;
    private float m_fireCoolDown = 0;
    private GameObject m_bulletPrefab;

    private void Awake()
    {
        var p = GameManager.Get().GetPlayer();
        m_playerTrasform = p.transform;

        if (!m_projectileRef)
            Debug.LogError("ASSIGN THE PROJECTILE ADN GUN REF!!!");

        m_gunSprite = m_projectileRef.parent;
        m_bulletPrefab = Resources.Load<GameObject>("bullet");
    }

    private void Update()
    {
        UpdateGunPostion();
        UpdateFiring();
    }

    private bool IsFiring()
    {
        return Input.GetKey(KeyCode.Mouse0);
    }

    private Vector3 GetShootDirection()
    {
        Vector3 mousePosWs = GameManager.Get().GetMouseWorldPos();
        return (mousePosWs - m_playerTrasform.position).normalized;
    }

    private void UpdateGunPostion()
    {
        Vector3 mousePosWs = GameManager.Get().GetMouseWorldPos();
        Vector3 shootDir = (mousePosWs - m_playerTrasform.position).normalized;

        // Add some drag to the gun
        Vector3 targetPosition = shootDir * kGunRadius + m_playerTrasform.position;
        transform.position = Vector3.Lerp(transform.position, targetPosition, 10 * Time.deltaTime);

        // face gun toward shoot diretion
        float rot_z = Mathf.Atan2(shootDir.y, shootDir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);

        // ease gun back to center after kick
        m_gunSprite.localPosition = Vector3.Lerp(m_gunSprite.localPosition, Vector3.zero, 3 * Time.deltaTime);
    }

    private void UpdateFiring()
    {
        // only shoot kFireRate times per second
        m_fireCoolDown += Time.deltaTime;
        if (IsFiring() && 
            m_fireCoolDown > (1.0f / kFireRate))
        {
            m_fireCoolDown = 0;
            m_gunSprite.localPosition = new Vector3(0, -0.7f, 0);

            // spawn bullet and send if flying
            Vector3 sprayDirection = Quaternion.Euler(0, 0, Random.Range(-kBulletSpread, kBulletSpread)) * GetShootDirection();
            GameObject bullet = Instantiate(m_bulletPrefab, m_projectileRef.position, Quaternion.identity);
            bullet.GetComponent<Rigidbody2D>().velocity = sprayDirection * kBulletSpeed;
            CameraManager.Get().DoShake(0.1f);
        }
    }
}
