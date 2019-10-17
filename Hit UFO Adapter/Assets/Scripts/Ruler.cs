using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class Ruler
{
    private int round;
    private Random random;
    private Controller controller;
    private IActionManager actionManager;
    
    private int generated;
    
    private static int[] nUFO =
    {
        2, 3, 5, 6, 7, 8, 9, 10, 11, 12,
    };

    private static int[] nTotalUFO =
    {
        10, 20, 30, 40, 50, 60, 80, 100, 120, 150,
    };

    
    private static int[] score =
    {
        1, 3, 5, 8,
    };

    private static float[] speed =
    {
        0.3f, 0.5f, 0.8f, 1f,
    };

    private static float[] scale =
    {
        1.2f, 1f, 0.8f, 0.5f,
    };

    public Ruler(int round, Controller controller, IActionManager actionManager)
    {
        this.round = round;
        this.random = new Random();
        this.controller = controller;
        this.generated = 0;
        this.actionManager = actionManager;
    }

    public float NextFloat(float low = -1f, float high = 1f)
    {
        return ((float) random.NextDouble()) * (high - low) + low;
    }

    public int getUFOCount()
    {
        return nUFO[round - 1];
    }

    public GameObject generateUFO()
    {
        if (generated == nTotalUFO[round - 1])
        {
            generated = 0;
            controller.NextTrial();
            return null;
        }
        ++generated;
        // Randomly choose a color
        Array colors = Enum.GetValues(typeof(UFOFactory.Color));
        int index = random.Next(colors.Length);
        UFOFactory.Color color = (UFOFactory.Color) colors.GetValue(index);
        GameObject UFO = UFOFactory.Instance.getUFO(color);

        // Randomly assign position
        Vector3 position = new Vector3(NextFloat() * round / 4, NextFloat() * round / 4, 0);
        UFO.transform.position = position;

        // Assign scale
        UFO.transform.localScale = new Vector3(scale[index], scale[index], scale[index]);

        // Assign speed
        UFOBehaviour behaviour = UFO.GetComponent<UFOBehaviour>();
        float UFOspeed = speed[index] * round / 5;

        // Assign score
        behaviour.score = score[index] * round;
        
        // Randomly assign direction
        Vector3 direction = new Vector3(NextFloat() * round, NextFloat() * round, 0);

        // Randomly assign rotation
        Quaternion rotation = Quaternion.Euler(NextFloat() * 360f, NextFloat() * 360f, NextFloat() * 360f);
        UFO.transform.rotation = rotation;
        
        // Fly!
        actionManager.FlyAway(UFO, direction, UFOspeed, controller);
        
        return UFO;
    }
}