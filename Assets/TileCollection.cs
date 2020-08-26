using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace InjectorGames.FarAlone.TileManager
{
    public struct TileInfo
    {
        public bool destroyable;
        public float durability;

        public bool crushable;
        //TODO: Particles

        public Tile tile;
        public Tilemap tilemap;

        public TileInfo(bool _destroy, float _dur, bool _crush, Tilemap _tilemap, Tile _tile /*, particles*/ )
        {
            destroyable = _destroy;
            durability = _dur;

            crushable = _crush;
            // Particles = particles;

            tile = _tile;
            tilemap = _tilemap;
        }

    }
    public class TileCollection : MonoBehaviour
    {
        public static Dictionary<TileType, TileInfo> tiles;

        TileCollection(Dictionary<TileType, TileInfo> _tiles)
        {
            tiles = _tiles;
        }

        public static TileCollection Initialize()
        {
            tiles = new Dictionary<TileType, TileInfo>();

            var tilemapCollection = TilemapCollection.Initialize();

            tiles.Add(TileType.Error, new TileInfo(false, 0, false, tilemapCollection.GetTilemap(TilemapType.GroundTilemap), null));

            tiles.Add(TileType.Grass, new TileInfo(false, 0, false, tilemapCollection.GetTilemap(TilemapType.GroundTilemap), Resources.Load<Tile>("Tiles/Grass/Grass")));

            tiles.Add(TileType.Grass_Island, new TileInfo(false, 0, false, tilemapCollection.GetTilemap(TilemapType.GroundTilemap), Resources.Load<Tile>("Tiles/Grass/Grass_Island")));

            tiles.Add(TileType.GrassBottom, new TileInfo(false, 0, false, tilemapCollection.GetTilemap(TilemapType.GroundTilemap), Resources.Load<Tile>("Tiles/Grass/GrassBottom/GrassBottom")));
            tiles.Add(TileType.GrassBottom_Left, new TileInfo(false, 0, false, tilemapCollection.GetTilemap(TilemapType.GroundTilemap), Resources.Load<Tile>("Tiles/Grass/GrassBottom/GrassBottom_Left")));
            tiles.Add(TileType.GrassBottom_Right, new TileInfo(false, 0, false, tilemapCollection.GetTilemap(TilemapType.GroundTilemap), Resources.Load<Tile>("Tiles/Grass/GrassBottom/GrassBottom_Right")));
            tiles.Add(TileType.GrassBottom_Double, new TileInfo(false, 0, false, tilemapCollection.GetTilemap(TilemapType.GroundTilemap), Resources.Load<Tile>("Tiles/Grass/GrassBottom/GrassBottom_Double")));

            tiles.Add(TileType.GrassTop, new TileInfo(false, 0, false, tilemapCollection.GetTilemap(TilemapType.GroundTilemap), Resources.Load<Tile>("Tiles/Grass/GrassTop/GrassTop")));
            tiles.Add(TileType.GrassTop_Left, new TileInfo(false, 0, false, tilemapCollection.GetTilemap(TilemapType.GroundTilemap), Resources.Load<Tile>("Tiles/Grass/GrassTop/GrassTop_Left")));
            tiles.Add(TileType.GrassTop_Right, new TileInfo(false, 0, false, tilemapCollection.GetTilemap(TilemapType.GroundTilemap), Resources.Load<Tile>("Tiles/Grass/GrassTop/GrassTop_Right")));
            tiles.Add(TileType.GrassTop_Closed, new TileInfo(false, 0, false, tilemapCollection.GetTilemap(TilemapType.GroundTilemap), Resources.Load<Tile>("Tiles/Grass/GrassTop/GrassTop_Closed")));

            tiles.Add(TileType.GrassBottom_Top, new TileInfo(false, 0, false, tilemapCollection.GetTilemap(TilemapType.GroundTilemap), Resources.Load<Tile>("Tiles/Grass/GrassBottom_Top/GrassBottom_Top")));
            tiles.Add(TileType.GrassBottom_Top_Left, new TileInfo(false, 0, false, tilemapCollection.GetTilemap(TilemapType.GroundTilemap), Resources.Load<Tile>("Tiles/Grass/GrassBottom_Top/GrassBottom_Top_Left")));
            tiles.Add(TileType.GrassBottom_Top_Right, new TileInfo(false, 0, false, tilemapCollection.GetTilemap(TilemapType.GroundTilemap), Resources.Load<Tile>("Tiles/Grass/GrassBottom_Top/GrassBottom_Top_Right")));

            tiles.Add(TileType.GrassTop_Trans_Left, new TileInfo(false, 0, false, tilemapCollection.GetTilemap(TilemapType.GroundTilemap), Resources.Load<Tile>("Tiles/Grass/Transitions/GrassTop Transitions/GrassTop_Transition_Left")));
            tiles.Add(TileType.GrassTop_Trans_Right, new TileInfo(false, 0, false, tilemapCollection.GetTilemap(TilemapType.GroundTilemap), Resources.Load<Tile>("Tiles/Grass/Transitions/GrassTop Transitions/GrassTop_Transition_Right")));
            tiles.Add(TileType.GrassTop_Trans_Double, new TileInfo(false, 0, false, tilemapCollection.GetTilemap(TilemapType.GroundTilemap), Resources.Load<Tile>("Tiles/Grass/Transitions/GrassTop Transitions/GrassTop_DoubleTransition")));

            tiles.Add(TileType.GrassBottom_Top_Trans_Left, new TileInfo(false, 0, false, tilemapCollection.GetTilemap(TilemapType.GroundTilemap), Resources.Load<Tile>("Tiles/Grass/Transitions/GrassBottom_Top Transitions/GrassBottom_Top NoSide Transitions/GrassBottom_Top_Transition_Left")));
            tiles.Add(TileType.GrassBottom_Top_Trans_Right, new TileInfo(false, 0, false, tilemapCollection.GetTilemap(TilemapType.GroundTilemap), Resources.Load<Tile>("Tiles/Grass/Transitions/GrassBottom_Top Transitions/GrassBottom_Top NoSide Transitions/GrassBottom_Top_Transition_Right")));
            tiles.Add(TileType.GrassBottom_Top_Trans_Double, new TileInfo(false, 0, false, tilemapCollection.GetTilemap(TilemapType.GroundTilemap), Resources.Load<Tile>("Tiles/Grass/Transitions/GrassBottom_Top Transitions/GrassBottom_Top NoSide Transitions/GrassBottom_Top_DoubleTransition")));

            tiles.Add(TileType.GrassBottom_Top_Left_Trans, new TileInfo(false, 0, false, tilemapCollection.GetTilemap(TilemapType.GroundTilemap), Resources.Load<Tile>("Tiles/Grass/Transitions/GrassBottom_Top Transitions/GrassBottom_Top Side Transitions/GrassBottom_Top_Left_Trans")));
            tiles.Add(TileType.GrassBottom_Top_Right_Trans, new TileInfo(false, 0, false, tilemapCollection.GetTilemap(TilemapType.GroundTilemap), Resources.Load<Tile>("Tiles/Grass/Transitions/GrassBottom_Top Transitions/GrassBottom_Top Side Transitions/GrassBottom_Top_Right_Transition")));

            tiles.Add(TileType.Water, new TileInfo(false, 0, false, tilemapCollection.GetTilemap(TilemapType.WaterTilemap), Resources.Load<Tile>("Tiles/Water")));

            tiles.Add(TileType.Bridge_Horizontal, new TileInfo(false, 0, false, tilemapCollection.GetTilemap(TilemapType.StructureTilemap), Resources.Load<Tile>("Tiles/Bridge/BridgeHorizontal/Bridge_Horizontal")));
            tiles.Add(TileType.Bridge_Horizontal_Left, new TileInfo(false, 0, false, tilemapCollection.GetTilemap(TilemapType.StructureTilemap), Resources.Load<Tile>("Tiles/Bridge/BridgeHorizontal/Bridge_Horizontal_Left")));
            tiles.Add(TileType.Bridge_Horizontal_Right, new TileInfo(false, 0, false, tilemapCollection.GetTilemap(TilemapType.StructureTilemap), Resources.Load<Tile>("Tiles/Bridge/BridgeHorizontal/Bridge_Horizontal_Right")));

            tiles.Add(TileType.Bridge_Vertical, new TileInfo(false, 0, false, tilemapCollection.GetTilemap(TilemapType.StructureTilemap), Resources.Load<Tile>("Tiles/Bridge/BridgeVertical/Bridge_Vertical")));
            tiles.Add(TileType.Bridge_Vertical_Top, new TileInfo(false, 0, false, tilemapCollection.GetTilemap(TilemapType.StructureTilemap), Resources.Load<Tile>("Tiles/Bridge/BridgeVertical/Bridge_Vertical_Top")));
            tiles.Add(TileType.Bridge_Vertical_Bottom, new TileInfo(false, 0, false, tilemapCollection.GetTilemap(TilemapType.StructureTilemap), Resources.Load<Tile>("Tiles/Bridge/BridgeVertical/Bridge_Vertical_Bottom")));

            tiles.Add(TileType.Rock, new TileInfo(false, 0, false, tilemapCollection.GetTilemap(TilemapType.EntityTilemap), Resources.Load<Tile>("Tiles/Entity/Rock")));




            return new TileCollection(tiles);
        }

        public TileInfo GetTileInfo(TileType keyType)
        {
            return tiles[keyType];
        }

    }
}
