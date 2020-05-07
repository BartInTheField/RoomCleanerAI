using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class AIIndividual : AI
{
    public Room RoomClone { get; set; }
    public Move[] Genes { get; set; }
    private int steps = -1;

    public int cleaned = 0;

    public float Fitness { get; private set; }

    private List<Vector2> positionsIHaveBeen = new List<Vector2>();
    private int timesIHaveBeenInTheSameSpot = 0;

    protected override void Awake()
    {
        base.Awake();
        canStart = false;
    }
    protected override void Update()
    {
        if (steps + 1 == Genes.Length)
            hasStopped = true;

        cleaned = RoomClone.GetAmountOfTilesOfType(RoomTile.CleanFloor);
        DebugGUI.LogPersistent(transform.name, transform.name + " Has cleand " + cleaned);

        base.Update();
    }

    public void Initialize()
    {
        positionsIHaveBeen.Add(transform.position);
        GetComponent<RobotCleaner>().SetRoom(RoomClone);
        canStart = true;
    }

    public bool IsFinished()
    {
        return hasStopped;
    }

    public Move[] CrossOver(AIIndividual otherParent)
    {
        Move[] childGenes = new Move[Genes.Length];
        for (int i = 0; i < Genes.Length; i++)
        {
            childGenes[i] = Random.value < 0.5 ? Genes[i] : otherParent.Genes[i];
        }

        return childGenes;
    }

    public Move[] CrossOver(Move[] genes)
    {
        Move[] childGenes = new Move[Genes.Length];
        for (int i = 0; i < Genes.Length; i++)
        {
            childGenes[i] = Random.value < 0.5 ? Genes[i] : genes[i];
        }

        return childGenes;
    }

    public void Mutate(float mutationRate)
    {
        for (int i = 0; i < Genes.Length; i++)
        {
            if (Random.value < mutationRate)
            {
                // Create random gene
                Genes[i] = (Move)Random.Range(0, System.Enum.GetNames(typeof(Move)).Length);
            }
        }
    }

    public float CalculateFitness()
    {
        Fitness = (cleaned * 1.5f) - (timesIHaveBeenInTheSameSpot * 0.1f); 

        return Fitness;
    }

    protected override Move MakeMoveDecision()
    {
        steps++;
        return Genes[steps];
    }

    protected override void AfterMove()
    {
        base.AfterMove();

        if (positionsIHaveBeen.Any(position => position.Equals(transform.position)))
            timesIHaveBeenInTheSameSpot++;

        positionsIHaveBeen.Add(transform.position);
    }
}
