using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoldCounter : MonoBehaviour
{
    [SerializeField] ParticleSystem goldEffect = null;
    [SerializeField] Text goldText;
    ParticleSystem.EmissionModule goldEffectEmission;
    ParticleSystem.Burst burst;
  
    public void SetParticleAmount(int score)
    {
        goldText.text = score.ToString();
        goldEffectEmission = goldEffect.emission;
        goldEffectEmission.enabled = true;
        //goldEffect.gravityModifier = Mathf.Clamp(6 + (score * 0.1f), 3, 9);
        goldEffectEmission.SetBursts(new ParticleSystem.Burst[] { new ParticleSystem.Burst(0, Mathf.Clamp(score, 0, 20)) });
    }
}
