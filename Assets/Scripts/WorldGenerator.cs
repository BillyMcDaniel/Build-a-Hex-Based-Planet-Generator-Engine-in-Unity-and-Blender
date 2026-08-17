using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class WorldGenerator : MonoBehaviour {

    [SerializeField] Material[] terrainMaterials;
    [SerializeField] List<CellData> cells = new List<CellData>();
    [Range(0, 20)] [SerializeField] int generationSpeed;
    [SerializeField] List<TerrainRule> rules = new List<TerrainRule>();

    void Start() {
        StartCoroutine(GenerateWorld());
    }

    void Update() {
        if(Keyboard.current.spaceKey.wasPressedThisFrame) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    IEnumerator GenerateWorld() {
        foreach(TerrainRule rule in rules) {
            List<CellData> seeds = CreateSeeds(rule);
            yield return GrowTerrain(rule, seeds);
        }
    }


    List<CellData> CreateSeeds(TerrainRule rule) {
        List<CellData> seeds = new List<CellData>();
        int availableCells = CountCells(rule.from);
        int targetSeeds = Random.Range(rule.seedRange.x, rule.seedRange.y + 1);
        targetSeeds = Mathf.Min(availableCells, targetSeeds);
        while(seeds.Count < targetSeeds) {
            int index = Random.Range(0, cells.Count);
            CellData cell = cells[index];
            if(cell.type == rule.from && !seeds.Contains(cell)) {
                seeds.Add(cell);
                int terrainIndex = (int)rule.to;
                cell.type = rule.to;
                cell.meshRenderer.material = terrainMaterials[terrainIndex];
            }
        }
        return seeds;
    }

    IEnumerator GrowTerrain(TerrainRule rule, List<CellData> seeds) {
        int sourceCount = CountCells(rule.from);
        int cellMax = Mathf.RoundToInt(sourceCount * rule.coverage);
        int cellCount = seeds.Count;
        List<Region> regions = CreateRegions(seeds);
        while(cellCount < cellMax && regions.Count > 0) {
            for(int regionIndex = regions.Count - 1; regionIndex >= 0; regionIndex--) {
                Region region = regions[regionIndex];
                if(GrowRegion(rule, region)) {
                    cellCount++;
                    if(generationSpeed > 0) {
                        if(cellCount % generationSpeed == 0) {
                            yield return null;
                        }
                    }
                }
                if(region.frontier.Count == 0) { regions.RemoveAt(regionIndex); }
                if(cellCount >= cellMax) { break; }
            }
        }
    }

    bool GrowRegion(TerrainRule rule, Region region) {
        int frontierIndex = Random.Range(0, region.frontier.Count);
        CellData frontierCell = region.frontier[frontierIndex];
        List<CellData> candidates = new List<CellData>();
        foreach(int index in frontierCell.neighbors) {
            CellData neighbor = cells[index];
            if(neighbor.type == rule.from) {
                candidates.Add(neighbor);
            }
        }
        if(candidates.Count == 0) {
            region.frontier.RemoveAt(frontierIndex);
            return false;
        }
        int candidateIndex = Random.Range(0, candidates.Count);
        CellData newCell = candidates[candidateIndex];
        newCell.type = rule.to;
        int terrainIndex = (int)rule.to;
        newCell.meshRenderer.material = terrainMaterials[terrainIndex];
        region.frontier.Add(newCell);
        return true;
    }

    List<Region> CreateRegions(List<CellData> seeds) {
        List<Region> regions = new List<Region>();
        foreach(CellData seed in seeds) {
            Region region = new Region();
            region.frontier.Add(seed);
            regions.Add(region);
        }
        return regions;
    }

    int CountCells(TerrainType type) {
        int count = 0;
        foreach(CellData cell in cells) {
            if(cell.type == type) { count++; }
        }
        return count;
    }

}
