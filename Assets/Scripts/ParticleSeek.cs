using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParticleSeek : MonoBehaviour
{
    [SerializeField] Slider _galaxyShape;
    [SerializeField] Slider _galaxyForce;
    [SerializeField] Slider _galaxySpread;

    public Transform target;
    public float force = 10.0f;

    public bool perlinBool = true;

    ParticleSystem ps;

    // Start is called before the first frame update
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        var shape = ps.shape;
        
        if(_galaxyShape.value != 1)
        {
            shape.arcSpread = 1.0f / _galaxyShape.value;
        }
        else
        {
            shape.arcSpread = 0;
        }

        force = _galaxyForce.value;

        var velocity = ps.velocityOverLifetime;
        velocity.radial = _galaxySpread.value;

        ParticleSystem.Particle[] particles = new ParticleSystem.Particle[ps.particleCount];
        ps.GetParticles(particles);

        for(int i=0; i<particles.Length; i++)
        {
            ParticleSystem.Particle p = particles[i];

            Vector3 particleWorldPosition;
            if (ps.main.simulationSpace == ParticleSystemSimulationSpace.Local)
            {
                particleWorldPosition = transform.TransformPoint(p.position);
            }
            else if(ps.main.simulationSpace == ParticleSystemSimulationSpace.Custom)
            {
                particleWorldPosition = ps.main.customSimulationSpace.TransformPoint(p.position);
            }
            else
            {
                particleWorldPosition = p.position;
            }
            Vector3 directionToTarget = (target.position - particleWorldPosition).normalized;
            
            Vector3 seekForce;
            if (perlinBool)
            {
                seekForce = (directionToTarget * force * (Mathf.PerlinNoise(p.position.x, p.position.y) + 0.5f)) * Time.deltaTime;
            }
            else
            {
                seekForce = (directionToTarget * force) * Time.deltaTime;
            }
            
            p.velocity += seekForce;

            particles[i] = p;
        }

        ps.SetParticles(particles, particles.Length);
    }

    public void TogglePerlinNoise()
    {
        perlinBool = !perlinBool;
        Debug.Log("Perlin Bool: "+perlinBool);
    }
}
