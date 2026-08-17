using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class WorldGenerator : MonoBehaviour {

    [SerializeField] Material[] terrainMaterials;
    [SerializeField] List<CellData> cells = new List<CellData>();
    [SerializeField] float targetLandRatio = 0.29f;
    [Range(0, 20)] [SerializeField] int generationSpeed;

    List<CellData> seeds = new List<CellData>();

    void Start() {
        SetSeeds();
        StartCoroutine(GrowIslands());
    }

    void Update() {
        if(Keyboard.current.spaceKey.wasPressedThisFrame) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    void SetSeeds() {
        int targetSeeds = Random.Range(3, 9);
        while(seeds.Count < targetSeeds) {
            int index = Random.Range(0, cells.Count);
            CellData cell = cells[index];
            if(!seeds.Contains(cell)) {
                seeds.Add(cell);
                int terrainIndex = (int)TerrainType.Plains;
                cell.type = TerrainType.Plains;
                cell.meshRenderer.material = terrainMaterials[terrainIndex];
            }
        }
    }

    IEnumerator GrowIslands() {
        int landMax = Mathf.RoundToInt(cells.Count * targetLandRatio);
        int landCount = seeds.Count;
        List<Island> islands = CreateIslands();
        while(landCount < landMax && islands.Count > 0) {
            for(int islandIndex = islands.Count - 1; islandIndex >= 0; islandIndex--) {
                Island island = islands[islandIndex];
                if(GrowIsland(island)) {
                    landCount++;
                    if(generationSpeed > 0) {
                        if(landCount % generationSpeed == 0) {
                            yield return null;
                        }
                    }
                }
                if(island.frontier.Count == 0) { islands.RemoveAt(islandIndex); }
                if(landCount >= landMax) { break; }
            }
        }
    }

    bool GrowIsland(Island island) {
        int frontierIndex = Random.Range(0, island.frontier.Count);
        CellData frontierCell = island.frontier[frontierIndex];
        List<CellData> waterNeighbors = new List<CellData>();
        foreach(int index in frontierCell.neighbors) {
            CellData neighbor = cells[index];
            if(neighbor.type == TerrainType.Water) {
                waterNeighbors.Add(neighbor);
            }
        }
        if(waterNeighbors.Count == 0) {
            island.frontier.RemoveAt(frontierIndex);
            return false;
        }
        int waterIndex = Random.Range(0, waterNeighbors.Count);
        CellData newLand = waterNeighbors[waterIndex];
        newLand.type = TerrainType.Plains;
        int terrainIndex = (int)TerrainType.Plains;
        newLand.meshRenderer.material = terrainMaterials[terrainIndex];
        island.frontier.Add(newLand);
        return true;
    }

    List<Island> CreateIslands() {
        List<Island> islands = new List<Island>();
        foreach(CellData seed in seeds) {
            Island island = new Island();
            island.frontier.Add(seed);
            islands.Add(island);
        }
        return islands;
    }

}
