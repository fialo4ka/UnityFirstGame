using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private CubePos nullCube = new CubePos(0, 1, 0);
    public float cubeChangePlaceSpeed = 0.5f;
    public Transform cubeToPlace;
    public GameObject cubeToCreate, allCubes;
    private Rigidbody allCubesRb;
    private bool isLoose;
    private Coroutine showCubePlace;


    private List<Vector3> allCubesPositions = new List<Vector3>
    {
        new Vector3(0,0,0),
        new Vector3(0,0,1),
        new Vector3(-1,0,1),
        new Vector3(-1,0,0),
        new Vector3(-1,0,-1),
        new Vector3(0,0,-1),
        new Vector3(1,0,-1),
        new Vector3(1,0,0),
        new Vector3(1,0,1),

        new Vector3(0,1,0),
    };

    private void Start()
    {
        allCubesRb = allCubes.GetComponent<Rigidbody>();
        showCubePlace = StartCoroutine(ShowCubePlace());
    }

    private void Update()
    {
        if (cubeToPlace == null)
        {
            return;
        }
        if (Input.GetMouseButtonDown(0) || Input.touchCount > 0)
        {
#if !UNITY_EDITOR
            if (Input.GetTouch(0).phase != TouchPhase.Began)
	        {
                return;
	        }
#endif
            var newCube = Instantiate(cubeToCreate, cubeToPlace.position, Quaternion.identity);
            newCube.transform.SetParent(allCubes.transform);
            nullCube.setVector(cubeToPlace.position);
            allCubesPositions.Add(nullCube.getVector());
            allCubesRb.isKinematic = true;
            allCubesRb.isKinematic = false;
            SpawnPositions();
        }

        if (!isLoose && allCubesRb.velocity.magnitude > 0.1f)
        {
            Destroy(cubeToPlace.gameObject);
            isLoose = true;
            StopCoroutine(showCubePlace);
        }
    }

    IEnumerator ShowCubePlace()
    {
        while (true)
        {
            SpawnPositions();
            yield return new WaitForSeconds(cubeChangePlaceSpeed);
        }
    }

    private void SpawnPositions()
    {
        var positions = new List<Vector3>();
        if (IsPositionEmpty(new Vector3(nullCube.x + 1, nullCube.y, nullCube.z)) && nullCube.x + 1 != cubeToPlace.position.x)
        {
            positions.Add(new Vector3(nullCube.x + 1, nullCube.y, nullCube.z));
        }
        if (IsPositionEmpty(new Vector3(nullCube.x - 1, nullCube.y, nullCube.z)) && nullCube.x - 1 != cubeToPlace.position.x)
        {
            positions.Add(new Vector3(nullCube.x - 1, nullCube.y, nullCube.z));
        }
        if (IsPositionEmpty(new Vector3(nullCube.x, nullCube.y + 1, nullCube.z)) && nullCube.y + 1 != cubeToPlace.position.y)
        {
            positions.Add(new Vector3(nullCube.x, nullCube.y + 1, nullCube.z));
        }
        if (IsPositionEmpty(new Vector3(nullCube.x, nullCube.y - 1, nullCube.z)) && nullCube.y - 1 != cubeToPlace.position.y)
        {
            positions.Add(new Vector3(nullCube.x, nullCube.y - 1, nullCube.z));
        }
        if (IsPositionEmpty(new Vector3(nullCube.x, nullCube.y, nullCube.z + 1)) && nullCube.z + 1 != cubeToPlace.position.z)
        {
            positions.Add(new Vector3(nullCube.x, nullCube.y, nullCube.z + 1));
        }
        if (IsPositionEmpty(new Vector3(nullCube.x, nullCube.y, nullCube.z - 1)) && nullCube.z - 1 != cubeToPlace.position.z)
        {
            positions.Add(new Vector3(nullCube.x, nullCube.y, nullCube.z - 1));
        }
        cubeToPlace.position = positions[UnityEngine.Random.Range(0, positions.Count)];
    }

    private bool IsPositionEmpty(Vector3 position)
    {
        if (position.y <= 0)
        {
            return false;
        }
        return !allCubesPositions.Exists(element => element.x == position.x && element.y == position.y && element.z == position.z);
    }
}

class CubePos
{
    public int x, y, z;
    public CubePos(int x, int y, int z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public Vector3 getVector()
    {
        return new Vector3(x, y, z);
    }

    public void setVector(Vector3 pos)
    {
        x = Convert.ToInt32(pos.x);
        y = Convert.ToInt32(pos.y);
        z = Convert.ToInt32(pos.z);
    }
}