using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    private const float kGunRadius = 0.3f;

    private Transform m_playerTrasform;
    private Transform m_gunSpriteLoc;
    private Transform m_gunNull;
    private Vector3 kOffsetTweak;
    [SerializeField] private Transform m_projectileRef;

    private const float kBulletSpread = 10.0f;
    private const float kFireRate = 8.0f;
    private const float kBulletSpeed = 50.0f;
    private float m_fireCoolDown = 0;
    private GameObject m_bulletPrefab;

    private Sprite m_gunSprite;
    private Sprite m_gunSpriteFlash;
    private int m_flashFrameCooldown = 0;

    private const int kMaxBullets = 10;
    private int m_remainingBullets;
    private float m_regenCooldown = 0;

    public void ResetAmmo()
    {
        m_remainingBullets = kMaxBullets;
        UiManager.Get().UpdateAmmoUi(m_remainingBullets);
    }

    private void Awake()
    {
        var p = GameManager.Get().GetPlayer();
        m_playerTrasform = p.transform;

        if (!m_projectileRef)
            Debug.LogError("ASSIGN THE PROJECTILE ADN GUN REF!!!");

        m_gunSpriteLoc = m_projectileRef.parent;
        m_gunNull = m_gunSpriteLoc.parent;
        m_bulletPrefab = Resources.Load<GameObject>("bullet");

        m_gunSprite = Resources.Load<Sprite>("revolver");
        m_gunSpriteFlash = Resources.Load<Sprite>("revolverFlash");

        kOffsetTweak = new Vector3(0, -.3f, 0);
        m_remainingBullets = 10;
    }

    private void Update()
    {
        if (Time.timeScale != 0)
        {
            UpdateGunPostion();
            UpdateFiring();
            UpdateRegen();

            bool doFlip = GameManager.Get().GetMouseWorldPos().x > m_playerTrasform.position.x;
            m_gunSpriteLoc.localScale = doFlip ? new Vector3(1, -1, 1) : Vector3.one;
            m_gunNull.localPosition = doFlip ? new Vector3(0, 0, 0) : Vector3.zero;
        }
    }

    private bool IsFiring()
    {
        return Input.GetKey(KeyCode.Mouse0);
    }

    private void UpdateRegen()
    {
        if (IsFiring() || m_remainingBullets >= kMaxBullets)
        {
            m_regenCooldown = 0;
            return;
        }
        
        m_regenCooldown += Time.deltaTime;
        if(m_regenCooldown >  .3f)
        {
            m_regenCooldown = 0;
            m_remainingBullets++;
            UiManager.Get().UpdateAmmoUi(m_remainingBullets);
            FXManager.Get().PlaySFX("sfx/Gun Shot 1", 5, 0.3F);
        }
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
        Vector3 targetPosition = shootDir * kGunRadius + m_playerTrasform.position + kOffsetTweak;
        transform.position = Vector3.Lerp(transform.position, targetPosition, 20 * Time.deltaTime);

        // face gun toward shoot diretion
        float rot_z = Mathf.Atan2(shootDir.y, shootDir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);

        // ease gun back to center after kick
        m_gunSpriteLoc.localPosition = Vector3.Lerp(m_gunSpriteLoc.localPosition, Vector3.zero, 3 * Time.deltaTime);
        m_gunNull.localRotation = Quaternion.Slerp(m_gunNull.localRotation, Quaternion.identity, 10 * Time.deltaTime);
       
    }

    private void UpdateFiring()
    {
        if (m_flashFrameCooldown == 2)
        {
            m_gunSpriteLoc.GetComponent<SpriteRenderer>().sprite = m_gunSprite;
        }
        else
        {
            m_flashFrameCooldown++;
        }

        // only shoot kFireRate times per second
        m_fireCoolDown += Time.deltaTime;
        if (IsFiring() && 
            m_fireCoolDown > (1.0f / kFireRate)&&
            m_remainingBullets > 0)
        {
            m_fireCoolDown = 0;
            m_gunSpriteLoc.localPosition = new Vector3(0, -0.2f, 0);

            // spawn bullet and send if flying
            Vector3 sprayDirection = Quaternion.Euler(0, 0, Random.Range(-kBulletSpread, kBulletSpread)) * GetShootDirection();
            float rot_z = Mathf.Atan2(sprayDirection.y, sprayDirection.x) * Mathf.Rad2Deg;
            GameObject bullet = Instantiate(m_bulletPrefab, m_projectileRef.position, Quaternion.Euler(0f, 0f, rot_z));
            bullet.GetComponent<Rigidbody2D>().velocity = sprayDirection * kBulletSpeed;

            CameraManager.Get().DoShake(0.1f);
            m_gunSpriteLoc.GetComponent<SpriteRenderer>().sprite = m_gunSpriteFlash;
            m_flashFrameCooldown = 0;

            m_gunNull.localRotation = Quaternion.Euler(0, 0, 10);

            FXManager.Get().PlaySFX("sfx/Gun Shot 1", Random.Range(-0.2f, 0.2f), 0.3F);
            m_remainingBullets--;
            UiManager.Get().UpdateAmmoUi(m_remainingBullets);
        }

    }
}
