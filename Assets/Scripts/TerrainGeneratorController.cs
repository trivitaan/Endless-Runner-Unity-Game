using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGeneratorController : MonoBehaviour
{
    [Header("Templates")]
    public List<TerrainTemplateController> terrainTemplates;
    public float terrainTemplateWidth;

    [Header("Generator Area")]
    public Camera gameCamera;
    public float areaStartOffset;
    public float areaEndOffset;

    [Header("Force Early Template")]
    public List<TerrainTemplateController> earlyTerrainTemplates;

    private List<GameObject> spawnedTerrain;

    private float LastGeneratedPositionX;
    private float LastRemovedPositionX;

    private const float debugLineHeight = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        spawnedTerrain = new List<GameObject>();
        
        LastGeneratedPositionX = GetHorizontalPositionStart();
        LastRemovedPositionX = LastGeneratedPositionX - terrainTemplateWidth;

        foreach(TerrainTemplateController terrain in earlyTerrainTemplates)
        {
            GenerateTerrain(LastGeneratedPositionX, terrain);
            LastGeneratedPositionX += terrainTemplateWidth;
        }

        while(LastGeneratedPositionX < GetHorizontalPositionEnd())
        {
            GenerateTerrain(LastGeneratedPositionX);
            LastGeneratedPositionX += terrainTemplateWidth;
        }
    }

    // Update is called once per frame
    void Update()
    {
        while(LastGeneratedPositionX < GetHorizontalPositionEnd())
        {
            GenerateTerrain(LastGeneratedPositionX);
            LastGeneratedPositionX += terrainTemplateWidth;
        }

        while(LastRemovedPositionX + terrainTemplateWidth < GetHorizontalPositionStart())
        {
            LastRemovedPositionX += terrainTemplateWidth;
            RemoveTerrain(LastRemovedPositionX);
        }
    }

    private float GetHorizontalPositionStart()
    {
        return gameCamera.ViewportToWorldPoint(new Vector2(0.0f, 0.0f)).x + areaStartOffset;
    }

    private float GetHorizontalPositionEnd()
    {
        return gameCamera.ViewportToWorldPoint(new Vector2(1.0f, 0.0f)).x + areaEndOffset;
    }

    private void GenerateTerrain(float posX, TerrainTemplateController forceTerrain = null)
    {
        GameObject newTerrain = null;

        if(forceTerrain == null)
        {
            newTerrain = Instantiate(terrainTemplates[Random.Range(0, terrainTemplates.Count)].gameObject, transform);
        }
        else
        {
            newTerrain = Instantiate(forceTerrain.gameObject, transform);
        }
        newTerrain.transform.position = new Vector2(posX, 0f);

        spawnedTerrain.Add(newTerrain);
    }

    private void RemoveTerrain(float posX)
    {
        GameObject terrainToRemove = null;

        foreach(GameObject item in spawnedTerrain)
        {
            if(item.transform.position.x == posX)
            {
                terrainToRemove = item;
                break;
            }
        }

        //after found
        if(terrainToRemove != null)
        {
            spawnedTerrain.Remove(terrainToRemove);
            Destroy(terrainToRemove);
        }
    }

    private void OnDrawGizmos()
    {
        Vector3 areaStartPosition = transform.position;
        Vector3 areaEndPosition = transform.position;

        areaStartPosition.x = GetHorizontalPositionStart(); 
        areaEndPosition.x = GetHorizontalPositionEnd(); 

        Debug.DrawLine(areaStartPosition + Vector3.up * debugLineHeight / 2, areaStartPosition + Vector3.down * debugLineHeight/2, Color.red);
        Debug.DrawLine(areaEndPosition + Vector3.up * debugLineHeight / 2, areaEndPosition + Vector3.down * debugLineHeight/2, Color.red);
    }
}
