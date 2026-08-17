using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour {

    [SerializeField] Material[] terrainMaterials;
    [SerializeField] List<CellData> cells = new List<CellData>();

    void Start() {
        SetSeeds();
    }

    void SetSeeds() {
        int targetSeeds = Random.Range(3, 9);
        List<CellData> seeds = new List<CellData>();
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

}
