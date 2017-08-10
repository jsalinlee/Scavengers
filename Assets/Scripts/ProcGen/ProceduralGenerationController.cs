using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProceduralGenerationController : MonoBehaviour {
    public Vector2 mapSize;

    // Some of these are hidden because they represent deprecated and/or non functioning features
    [HideInInspector]
    public int minBuildings;
    [HideInInspector]
    public int maxBuildings;
    [HideInInspector]
    public int minCosmetics;
    [HideInInspector]
    public int maxCosmetics;
    public bool generateOnStart;
    public bool generateTiles;
    [HideInInspector]
    public bool generateCosmetics;
    [HideInInspector]
    public bool generateBuildings;
    private List<GameObject> generatedBuildings;
    private List<GameObject> generatedCosmetics;
    [HideInInspector]
    public GameObject[] buildingLibrary;
    [HideInInspector]
    public Sprite[] cosmeticLibrary;
    public Sprite[] highDensityGrass;
    public Sprite[] lowDensityGrass;
    public Sprite baseTile;
    public RawImage uiCurrentMap; // reference to the UI raw image to draw the noise map to
    void Start()
    {
        // Generate the base tileset
        if (generateTiles)
        {
            for (int x = 0; x < mapSize.x; x += 2)
            {
                for (int y = 0; y < mapSize.y; y += 2)
                {
                    GameObject tile = GenerateSprite(baseTile, new Vector2(x, y), "Tileset", false);
                    tile.name = x.ToString() + " " + y.ToString();
                    tile.transform.parent = gameObject.transform;
                }
            }
        }
        generatedBuildings = new List<GameObject>();
        generatedCosmetics = new List<GameObject>();
        if (generateOnStart) 
        {
            GenerateFromNoiseMap();
        }
    }
    // Generate new set of game objects
    public void Generate()
    {
        Clear();
        SeedRandom();
        if (generateBuildings)
        {
            int numBuildings = Random.Range(minBuildings, maxBuildings + 1);
            Debug.Log("generating " + numBuildings + " buildings");
            for (int i = 0; i < numBuildings; i++)
            {

                int index = Random.Range(0, buildingLibrary.Length);
                GameObject newObj = Instantiate(buildingLibrary[index], RandomFloatPosition(mapSize), new Quaternion(0, 0, 0, 0));
                generatedBuildings.Add(newObj);
            } 
        }
        if(generateCosmetics)
        {
            int numCosmetics = Random.Range(minCosmetics, maxCosmetics + 1);
            Debug.Log("generating " + numCosmetics + " cosmetics");
            for(int i = 0; i < numCosmetics; i++)
            {
                int index = Random.Range(0, cosmeticLibrary.Length);
                generatedCosmetics.Add(GenerateSprite(cosmeticLibrary[index], RandomIntPosition(mapSize), "Environment Cosmetics", false));
            }
        }
        GetComponent<AudioSource>().Play();
    }
    // Generate cosmetics from a noisemap. Places a random cosmetic at half-unit intervals based on the float value at that position in the noisemap. 0 = empty, 1 = place cosmetic
    public void GenerateFromNoiseMap()
    {
        Clear();
        float[,] noiseMap = Noise.GenerateNoiseMapArray((int)mapSize.x * 4, (int)mapSize.y * 4, 35, .597f);
        float posX = 0, posY = 0;
        for (int x = 0; x < noiseMap.GetLength(0); x++)
        {
            for(int y = 0; y < noiseMap.GetLength(1); y++)
            {
                if(Random.Range(0.0f, 100.0f) > 40.0f) // chance of nothing at all to keep it interesting
                {
                    var value = noiseMap[x, y];
                    if (value > .9f) //high density?
                    {
                        int index = Random.Range(0, highDensityGrass.Length);
                        bool flipX = Random.value > 0.5f ? true : false;
                        generatedCosmetics.Add(GenerateSprite(highDensityGrass[index], new Vector3(posX, posY, 0), "Environment Cosmetics", flipX));
                    }
                    else if (value > .8f) //low density?
                    {
                        int index = Random.Range(0, lowDensityGrass.Length);
                        bool flipX = Random.value > 0.5f ? true : false;
                        generatedCosmetics.Add(GenerateSprite(lowDensityGrass[index], new Vector3(posX, posY, 0), "Environment Cosmetics", flipX));
                    }
                    else // random chance to have low density grass anyway
                    {
                        if (Random.Range(0.0f, 100.0f) < .5f)
                        {
                            int index = Random.Range(0, lowDensityGrass.Length);
                            bool flipX = Random.value > 0.5f ? true : false;
                            generatedCosmetics.Add(GenerateSprite(lowDensityGrass[index], new Vector3(posX, posY, 0), "Environment Cosmetics", flipX));
                        }
                        
                    }
                }
                posY += .25f;
            }
            posY = 0;
            posX += .25f;
        }
        SetUINoiseMap(noiseMap.GetLength(0), noiseMap.GetLength(1), noiseMap);
    }
    void SetUINoiseMap(int width, int height, float[,] noiseMap)
    {
        Texture2D texture = new Texture2D(width, height);
        //texture.filterMode = FilterMode.Point;
        Color[] colourMap = new Color[width * height];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                colourMap[y * width + x] = Color.Lerp(Color.black, Color.white, noiseMap[x, y]);
            }
        }
        texture.SetPixels(colourMap);
        texture.Apply();
        uiCurrentMap.texture = texture;
    }
    GameObject GenerateSprite(Sprite sprite, Vector3 pos, string sortingLayer, bool flipX)
    {
        GameObject newObj = new GameObject(sprite.name + " " + pos.ToString());

        SpriteRenderer spriteRenderer = newObj.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprite;
        spriteRenderer.sortingLayerName = sortingLayer;
        spriteRenderer.flipX = flipX;

        newObj.transform.position = pos;
        return newObj;
    }
    // Clear ALL generated objects in scene
    public void Clear()
    {
        if(generatedBuildings.Count > 0)
        {
            foreach (GameObject obj in generatedBuildings)
            {
                Destroy(obj);
            }
            generatedBuildings.Clear();
        }
        if(generatedCosmetics.Count > 0)
        {
            foreach(GameObject obj in generatedCosmetics)
            {
                Destroy(obj);
            }
            generatedCosmetics.Clear();
        }
    }
    // Generate new random position based on limits, Z is ALWAYS zero
    Vector3 RandomFloatPosition(Vector2 limit)
    {
        float x = Random.Range(0, Mathf.Abs(limit.x));
        float y = Random.Range(0, Mathf.Abs(limit.y));
        return new Vector3(x, y, 0);
    }
    // Generate new random position based on limits, Z is ALWAYS zero, CLAMPED TO INT
    Vector3 RandomIntPosition(Vector2 limit)
    {
        int x = Random.Range(0, Mathf.Abs((int)limit.x));
        int y = Random.Range(0, Mathf.Abs((int)limit.y));
        return new Vector3(x, y, 0);
    }
    void SeedRandom()
    {
        Random.InitState(System.DateTime.Now.Millisecond);
    }
}
