using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointCharacterSpawner : MonoBehaviour
{
    public GameObject waypointRoot;
    public GameObject characterRoot;
    public GameObject characterList;
    public int pedestriansToSpawn;

    private int pedestriansToSpawnLast;
    public int pedestrianCount = 0;

    public double spawnDelayRatio = 1.0;
    public double spawnDelay = 0.0;
    private double lastSpawn = 0.0;

    private int count = 0;

    void Start()
    {
        if (characterRoot == null)
        {
            Debug.LogError("characterRoot missing");
        }

        if (characterList.transform.childCount == 0)
        {
            Debug.LogError("characterList has no character");
        }

        if (pedestriansToSpawn < 1) {
            pedestriansToSpawn = 1;
            pedestriansToSpawnLast = 1;
        }

        int countStart = characterRoot.transform.childCount;
        while (countStart < pedestriansToSpawn)
        {
            Spawn(true);
            countStart++;
        }
        pedestriansToSpawnLast = pedestriansToSpawn;
    }

    void Update()
    {
        double time = UnixTime.GetTime();

        if (pedestriansToSpawn > 0)
        {

            if (pedestriansToSpawn != pedestriansToSpawnLast)
            {
                int countStart = characterRoot.transform.childCount;
                while (countStart < pedestriansToSpawn)
                {
                    Spawn(true);
                    countStart++;
                }
                while (countStart > pedestriansToSpawn)
                {
                    Remove();
                    countStart--;
                }
                pedestriansToSpawnLast = pedestriansToSpawn;
            }

            spawnDelay = spawnDelayRatio / (pedestriansToSpawn / 20.0);
            if (lastSpawn + spawnDelay < time)
            {
                lastSpawn = time;
                if (characterRoot.transform.childCount < pedestriansToSpawn)
                {
                    Spawn();
                }
            }
            if (characterRoot.transform.childCount > pedestriansToSpawn)
            {
                Remove();
            }
        }


        pedestrianCount = characterRoot.transform.childCount;
    }

    public void Remove()
    {
        if (characterRoot.transform.childCount <= 1) {
            return;
        }
        int randomInt = Random.Range(0, characterRoot.transform.childCount);
        Transform t = characterRoot.transform.GetChild(randomInt);
        Destroy(t.gameObject);
    }

    public void Spawn(bool randomWaypoint = false)
    {
        List<Transform> starts = new List<Transform>();
        foreach (Transform child in this.waypointRoot.transform) {
            if (child.GetComponent<Waypoint>().isStart) {
                starts.Add(child);
            }
        }

        GameObject character = Instantiate(characterList.transform.GetChild(Random.Range(0, characterList.transform.childCount)).gameObject);
        character.name = character.name.Replace("(Clone)", "") + count;
        character.transform.parent = characterRoot.transform;
        if (character.GetComponent<WaypointNavigator>() == null) {
            WaypointNavigator wn = character.AddComponent(typeof(WaypointNavigator)) as WaypointNavigator;
            //wn.setController();
        }

        Waypoint wp;
        Waypoint wp2;
        Vector3 startPosition;

        if (randomWaypoint) {
            Waypoint[] wps = getWaypoints();
            wp = wps[Random.Range(0, wps.Length)];

            if (wp.isEnd == false) { 
                var lst = wp.getAllNextWaypoints();
                wp2 = lst[Random.Range(0, lst.Count)];

                if (wp2 == null)
                {
                    Debug.LogWarning(wp.name + "has not end");
                }
            } else {
                wp2 = wp;
            }
            startPosition = GetRandomVector3Between(wp.transform.position, wp2.transform.position) + getRandomVector3(new Vector3(-1, .01f, -1), new Vector3(1, .01f, 1));
        }
        else
        {
            Transform start = starts[Random.Range(0, starts.Count)];
            startPosition = start.position;

            wp = start.GetComponent<Waypoint>();

            var lst = wp.getAllNextWaypoints();
            wp2 = lst[Random.Range(0, lst.Count)];
        }

        if (wp2 == null) {
            Debug.Log(wp.name + "has not end");
        }

        character.transform.position = startPosition;
        character.GetComponent<WaypointNavigator>().currentWaypoint = wp2;

        character.GetComponents<Animator>()[0].enabled = true;
        foreach (MonoBehaviour monoBehaviour in character.GetComponents<MonoBehaviour>())
        {
            monoBehaviour.enabled = true;
        }

        count++;
    }

    public Vector3 getRandomVector3(Vector3 min, Vector3 max)
    {
        return new Vector3(Random.Range(min.x, max.x), Random.Range(min.y, max.y), Random.Range(min.z, max.z));
    }

    public Vector3 GetRandomVector3Between(Vector3 min, Vector3 max)
    {
        return min + Random.Range(0.0f, 1.0f) * (max - min);
    }

    public Waypoint [] getWaypoints()
    {
        return waypointRoot.transform.GetComponentsInChildren<Waypoint>();
    }
}

