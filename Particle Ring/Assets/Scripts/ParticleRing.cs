using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using ExtensionMethods;

public enum Direction
{
    Clockwise,
    Anticlockwise,
    Spread
}

public class ParticleRing : MonoBehaviour
{
    public Direction direction = Direction.Clockwise;
    private ParticleSystem particleSystem; 
    private ParticleSystem.Particle[] particles;
    public int count = 10000;
    public float size = 0.03f;
    public float radius = 8f;
    public float spread = 0.1f;
    private System.Random random;
    
    // Start is called before the first frame update
    void Start()
    {
        particles = new ParticleSystem.Particle[count];
        particleSystem = this.GetComponent<ParticleSystem>();
        random = new System.Random();

        ParticleSystem.MainModule main = particleSystem.main;
        main.startSpeed = 0;
        main.startSize = size;
        main.loop = false;
        main.maxParticles = count;

        particleSystem.Emit(count);
        particleSystem.GetParticles(particles);
        
        Spread();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < count; i++)
        {
            Vector3 position = particles[i].position;
            position -= this.transform.position;
            float norm = position.magnitude;
            position /= norm;
            
            if (direction == Direction.Anticlockwise) position = new Vector3(-position.y, position.x, position.z);
            
            else if (direction == Direction.Clockwise) position = new Vector3(position.y, -position.x, position.z);
            
            float speed = Random.Range(1f, 5f);
            
            Vector3 worldPosition = particles[i].position + Time.deltaTime * speed * position;
            
            if (worldPosition.magnitude > norm && direction != Direction.Spread)
            {
                worldPosition *= norm / worldPosition.magnitude;
            }

            particles[i].position = worldPosition;
        }

        particleSystem.SetParticles(particles, particles.Length);
    }

    
    void Spread()
    {
        var length = particles.Length;
        for (var i = 0; i < length; ++i)
        {
            float r = random.Gaussian(radius, spread);
            float angle = Random.Range(0, 2 * Mathf.PI);
            float x = Mathf.Cos(angle) * r;
            float y = Mathf.Sin(angle) * r;
            particles[i].position = new Vector3(transform.position.x + x, this.transform.position.y + y);
        }
        
        particleSystem.SetParticles(particles, particles.Length);
    }
}