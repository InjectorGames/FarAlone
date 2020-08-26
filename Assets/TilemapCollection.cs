using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace InjectorGames.FarAlone.TileManager
{
    public class TilemapCollection : MonoBehaviour
    {
        static Dictionary<TilemapType, Tilemap> tilemaps;

        TilemapCollection(Dictionary<TilemapType, Tilemap> _tilemaps)
        {
            tilemaps = _tilemaps;
        }


        public static TilemapCollection Initialize()
        {
            tilemaps = new Dictionary<TilemapType, Tilemap>();
            tilemaps.Add(TilemapType.GroundTilemap, GameObject.Find("GroundTilemap").GetComponent<Tilemap>());
            tilemaps.Add(TilemapType.WaterTilemap, GameObject.Find("WaterTilemap").GetComponent<Tilemap>());
            tilemaps.Add(TilemapType.StructureTilemap, GameObject.Find("StructureTilemap").GetComponent<Tilemap>());
            tilemaps.Add(TilemapType.EntityTilemap, GameObject.Find("EntityTilemap").GetComponent<Tilemap>());

            return new TilemapCollection(tilemaps);
        }

        public Tilemap GetTilemap(TilemapType typeKey)
        {
            return tilemaps[typeKey];
        }



    }
}