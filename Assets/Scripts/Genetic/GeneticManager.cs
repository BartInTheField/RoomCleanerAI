using System.Collections.Generic;
using System.Linq;
using UnityAtoms.BaseAtoms;
using UnityEngine;

public class GeneticManager : MonoBehaviour
{
    [SerializeField] private RoomManager roomManager;
    [SerializeField] private Vector2 spawnLocation;
    [SerializeField] private VoidEvent doSpawnRobotOn;
    [SerializeField] private GameObject individualObject;
    [SerializeField] private Room room;

    [SerializeField] private int initalPopulationCount = 1;
    [SerializeField] private int geneCount = 100;
    [SerializeField] private float mutationRate = 0.15f;

    private List<AIIndividual> population = new List<AIIndividual>();
    private Move[] bestGenes;
    private int generation = 0;
    private float bestFitness = 0f;
    private int bestGeneration = 0;
    private int bestCleaned = 0;

    private void OnEnable()
    {
        doSpawnRobotOn.Register(StartPopulation);
    }

    private void OnDisable()
    {
        doSpawnRobotOn.Unregister(StartPopulation);
    }

    private void Update()
    {
        DebugGUI.LogPersistent("Generation", $"Generation : {generation}");
        DebugGUI.LogPersistent("Best fitness", $"Best fitness : {bestFitness}");
        DebugGUI.LogPersistent("Best generation", $"Best generation : {bestGeneration}");
        DebugGUI.LogPersistent("Best cleaned", $"Best cleaned : {bestCleaned}");

        // If all the individuals took their steps
        if (population.All((individual => individual.IsFinished())))
        {

            foreach (AIIndividual individual in population)
            {
                if(individual.RoomClone.GetAmountOfTilesOfType(RoomTile.DirtyFloor) == 0)
                {
                    DebugGUI.LogPersistent("CleanedAll", $"Winner is! : {individual.name} from generation {generation}");
                    roomManager.StopTime();
                    return;
                }
            }

            NewGeneration();
        }
    }

    private void StartPopulation()
    {
        for (int i = 0; i < initalPopulationCount; i++)
        {
            GameObject initializedObject = Instantiate(individualObject, spawnLocation, Quaternion.identity);
            AIIndividual individual = initializedObject.GetComponent<AIIndividual>();

            initializedObject.transform.name = GetIndividualName(population);

            // Random genes because we have no clue about the room yet
            individual.Genes = CreateRandomGenes();

            // Give copy of the room because they can not clean the same room
            individual.RoomClone = CloneRoom();
            
            // Individual is ready!
            individual.Initialize();

            population.Add(individual);
        }
    }

    private string GetIndividualName(List<AIIndividual> currentPopulation)
    {
        return "Individual " + (currentPopulation.Count + 1);
    }

    private Room CloneRoom()
    {
        var roomClone = Instantiate(room);
        roomClone.content = room.content.ConvertAll(tiles => tiles.ConvertAll(tile => (tile.Item1, tile.Item2)));

        return roomClone;
    }

    private Move[] CreateRandomGenes()
    {
        Move[] moves = new Move[geneCount];

        for (int i = 0; i < geneCount; i++)
        {
            moves[i] = (Move)Random.Range(0, System.Enum.GetNames(typeof(Move)).Length);
        }

        return moves;
    }

    private void NewGeneration()
    {
        // Go calculate the fitness
        CalculateFitness();

        //Order population based on fitness
        IOrderedEnumerable<AIIndividual> orderedPopulation = population.Select(value => value).OrderByDescending(individual => individual.Fitness);
        // Find the fittest individual
        AIIndividual fittest = orderedPopulation.ElementAt(0);
        // Find the second fittest individual 
        AIIndividual secondFittest = orderedPopulation.ElementAt(1);

        if (bestGenes == null || bestFitness < fittest.Fitness)
        {
            bestGenes = fittest.Genes;
            bestFitness = fittest.Fitness;
            bestGeneration = generation;
            bestCleaned = fittest.cleaned;
        }

        // Create new population
        List<AIIndividual> newPopulation = new List<AIIndividual>();

        for (int i = 0; i < population.Count; i++)
        {
            GameObject initializedObject = Instantiate(individualObject, spawnLocation, Quaternion.identity);
            AIIndividual child = initializedObject.GetComponent<AIIndividual>();

            initializedObject.transform.name = GetIndividualName(newPopulation);

            // Crossover fittest with second fittest
            child.Genes = bestGenes.Equals(fittest.Genes) ? fittest.CrossOver(secondFittest) : fittest.CrossOver(bestGenes);

            // Mutate 
            child.Mutate(mutationRate);

            // Give copy of the room because they can not clean the same room
            child.RoomClone = CloneRoom();

            // Child is ready!
            child.Initialize();

            // Add child to new population
            newPopulation.Add(child);
        }

        foreach (AIIndividual individual in population)
        {
            Destroy(individual.gameObject);
        }
        population.Clear();
        population = newPopulation;

        generation++;
    }

    private void CalculateFitness()
    {
        foreach (AIIndividual individual in population)
        {
            individual.CalculateFitness();
        }
    }
}
