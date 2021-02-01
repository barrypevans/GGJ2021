using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AnimData
{
    public Sprite[] m_sprites;
    public int m_spriteCount;
    public float m_animSpeed = 1;
};

[RequireComponent(typeof(SpriteRenderer))]
public class Animateable : MonoBehaviour
{
    protected AnimData m_activeAnimData;
    protected SpriteRenderer m_spriteRenderer;
    protected float m_animFpsTimer = 0;
    
    protected int m_animIndex = 0;

    protected string m_variantId = "";

    virtual protected void Awake()
    {
        m_spriteRenderer = GetComponent<SpriteRenderer>();
    }

    virtual protected void Update()
    {
        UpdateAnims();
    }

    virtual protected void UpdateAnims()
    {
        if (null == m_activeAnimData) return;
        if (m_animFpsTimer >= .1 / m_activeAnimData.m_animSpeed)
        {
            m_animIndex = m_animIndex % m_activeAnimData.m_spriteCount;

            m_spriteRenderer.sprite = m_activeAnimData.m_sprites[m_animIndex];

            m_animIndex++;
            m_animFpsTimer = 0;
        }
        m_animFpsTimer += Time.deltaTime;
    }

    protected AnimData InitAnimData(string prefix, int spriteCount, float animSpeed = 1, bool invert = false)
    {
        AnimData data = new AnimData();
        data.m_animSpeed = animSpeed;
        data.m_spriteCount = spriteCount;
        data.m_sprites = new Sprite[spriteCount];
        for (int i = 0; i < data.m_spriteCount; ++i)
        {
            int index = i;
            if (invert)
                index = (data.m_spriteCount - 1) - i;
            data.m_sprites[index] = Resources.Load<Sprite>(prefix+ m_variantId + (index + 1).ToString());
        }
        return data;
    }
}
