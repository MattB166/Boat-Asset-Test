using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Manages the movement of the ocean surface. This is done by creating a mesh and manipulating the vertices to create waves. The waves are created using a combination of sine waves, which are defined by the octaves array. Each octave has a scale, frequency, speed, and height, which are used to calculate the y position of each vertex. The UVs are also generated to create a tiling effect on the water texture.
/// </summary>
public class PlayerOcean : MonoBehaviour
{
    [Serializable]
    public struct Octave
    {
        public Vector2 scale; //size of area
        public Vector2 frequency; //need a value for frequency 
        public Vector2 speed; //speed of the wave, if alternate is true, speed will be used as a multiplier for time, otherwise it will be added to the perlin noise coordinates
        public float height; //amplitude of the wave
        public bool alternate;
    }

    public WaterSettings settings;

    public int dimensions = 10;

    protected MeshFilter meshFilter;
    protected Mesh mesh;    
    public Octave[] octaves;
    public float UVScale;

    // Start is called before the first frame update
    void Start()
    {
        //assign settings to variables

        for (int i = 0; i < octaves.Length; i++)
        {
            octaves[i].frequency.x = settings.frequency / 10; //remove magic numbers eventually. but had to divide to counter for the difference in scale between the large water plane and the smaller boat. 
            octaves[i].frequency.y = settings.frequency / 10;
            octaves[i].height = settings.amplitude;
            octaves[i].speed.x = settings.speed;
            octaves[i].speed.y = settings.speed;
        }

        mesh = new Mesh();
        mesh.name = gameObject.name + " Mesh";

        mesh.vertices = GenerateVerts();

        mesh.triangles = GenerateTris();

        mesh.RecalculateBounds();  

        meshFilter = gameObject.AddComponent<MeshFilter>();
        meshFilter.mesh = mesh;
        
        Vector3 centre = mesh.bounds.center;
        Debug.Log("Mesh centre: " + centre);
        Debug.Log("Boat centre: " + gameObject.transform.parent.position);

        //amend local position to centre the mesh on the boat.
        
        mesh.RecalculateBounds();


        //lock rotation and position of the water plane
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }


    private Vector3[] GenerateVerts()
    {
        var verts = new Vector3[(dimensions + 1) * (dimensions +1)];

        for(int x = 0; x <= dimensions; x++)
        {
            for(int z = 0; z <= dimensions; z++)
            {
                verts[index(x,z)] = new Vector3(x, 0, z);
            }
        }
        return verts;
    }

    private int index(int x, int z)
    {
        return x * (dimensions + 1) + z;
    }
    private int[] GenerateTris()
    {
        var tries = new int[mesh.vertices.Length * 6];

        for (int x = 0; x < dimensions; x++)
        {
            for (int z = 0; z < dimensions; z++)
            {
                tries[index(x, z) * 6 + 0] = index(x, z);
                tries[index(x, z) * 6 + 1] = index(x + 1, z + 1);
                tries[index(x, z) * 6 + 2] = index(x + 1, z);
                tries[index(x, z) * 6 + 3] = index(x, z);
                tries[index(x, z) * 6 + 4] = index(x, z + 1);
                tries[index(x, z) * 6 + 5] = index(x + 1, z + 1);

            }
        }
        return tries;
    }

    private Vector2[] GenerateUVs()
    {
        var uvs = new Vector2[mesh.vertices.Length];
        for (int x = 0; x <= dimensions; x++)
        {
            for (int z = 0; z <= dimensions; z++)
            {
                var vec = new Vector2((x / UVScale) % 2, (z / UVScale) % 2);
                uvs[index(x, z)] = new Vector2(vec.x > 1 ? 2 - vec.x : vec.x, vec.y > 1 ? 2 - vec.y : vec.y);
            }
        }

        return uvs;
    }

    // Update is called once per frame
    void Update()
    {
        var verts = mesh.vertices;

        for (int x = 0; x <= dimensions; x++)
        {
            for (int z = 0; z <= dimensions; z++)
            {
                var y = 0f;
                for(int o = 0; o < octaves.Length; o++)
                {
                  Octave octave = octaves[o];
                  float phase = (x*octave.frequency.x) - (Time.time * octave.speed.x);

                  y += MathF.Sin(phase) * octave.height;
                }
                verts[index(x, z)] = new Vector3(x, y, z);

                
            }
        }
        mesh.vertices = verts;
        mesh.RecalculateNormals();
        transform.rotation = Quaternion.Euler(0, 0, 0);

       
    }

    private void LateUpdate()
    {
        Vector3 pos = transform.position;
        pos.y = 0;
        transform.position = pos;
    }

    //methods to attain normals and height at a given position, for use in the boat script, so can apply appropriate forces to the boat based on the wave movement.
    public float GetHeight(Vector3 localPos)
    {
        float y = 0f;
        for (int o = 0; o < octaves.Length; o++)
        {
            Octave octave = octaves[o];
            float phase = (localPos.x * octave.frequency.x) - (Time.time * octave.speed.x);
            y += MathF.Sin(phase) * octave.height;

        }
        return y;
    }

    public Vector3 GetNormal(Vector3 localPos)
    {
        float delta = 0.01f; // small offset for numerical derivative
        float hL = GetHeight(localPos - new Vector3(delta, 0, 0));
        float hR = GetHeight(localPos + new Vector3(delta, 0, 0));
        float hD = GetHeight(localPos - new Vector3(0, 0, delta));
        float hU = GetHeight(localPos + new Vector3(0, 0, delta));

        Vector3 normal = new Vector3(hL - hR, 2 * delta, hD - hU);
        return normal.normalized;
    }
}
