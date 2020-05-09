using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace InjectorGames.FarAlone.WorldGeneration
{
    public class WorldGeneration : MonoBehaviour
    {
        [Header("Tilemaps")]

        [SerializeField]
        List<Tilemap> tileMaps = new List<Tilemap>();

        [Header("Camera Settings")]

        [SerializeField]
        int cameraPosX;
        [SerializeField]
        int cameraPosY;
        [SerializeField]
        int screenHeightInUnits;
        [SerializeField]
        int screenWidthInUnits;
        [SerializeField]
        enum Border
        {
            Up,
            Bottom,
            Left,
            Right
        }

        [Header("Biomes")]

        [SerializeField]
        int biomesCount;

        enum Biome {Forest = 1}


        const int distanceFromEmptyCell = 3;
        const int biomeWidth = 6;
        const int biomeHeight = 6;
        

        void Awake() 
        {   

            SetBiomesCount();
            UpdateCamera();
            InstantiateTilemaps();
        }

        bool IsCellEmpty(Vector3Int position)
        {
            if(tileMaps[0].HasTile(position) || tileMaps[1].HasTile(position) || tileMaps[2].HasTile(position))
                return false;
            return true;
        }

        void SetBiomesCount()
        {
            biomesCount = Biome.GetNames(typeof(Biome)).Length;
        }

        void InstantiateTilemaps()
        {
            GameObject[] tileMapsObj = GameObject.FindGameObjectsWithTag("Tilemap");
            foreach(GameObject obj in tileMapsObj)
            {
                tileMaps.Add(obj.GetComponent<Tilemap>());
            }
        }

        void Update() 
        {
            UpdateCamera();
            CheckBorders();
            
        }

        void CheckBorders()
        {
            Check_DownBorder();
            Check_UpBorder();
            Check_LeftBorder();
            Check_RightBorder();
        }

        void Check_DownBorder()
        {
            Vector3Int leftBottomAngle = new Vector3Int(cameraPosX - (screenWidthInUnits / 2), cameraPosY - (screenHeightInUnits / 2), 0);

            for(int x = 0; x <= screenWidthInUnits; ++x)
            {
                Vector3Int checkPos = new Vector3Int(leftBottomAngle.x + x, leftBottomAngle.y, 0);
                
                if(IsCellEmpty(checkPos))
                {
                    Vector3Int tempBiomeCenter = new Vector3Int(checkPos.x, checkPos.y - distanceFromEmptyCell, 0);
                    
                    GenerateBiome(tempBiomeCenter, Border.Bottom);
                }
            }
        }

        void Check_UpBorder()
        {
            Vector3Int leftUpAngle = new Vector3Int(cameraPosX - (screenWidthInUnits / 2), cameraPosY + (screenHeightInUnits / 2), 0);
            
            for(int x = 0; x <= screenWidthInUnits; ++x)
            {
                Vector3Int checkPos = new Vector3Int(leftUpAngle.x + x, leftUpAngle.y, 0);

                if(IsCellEmpty(checkPos))
                {
                    Vector3Int tempBiomeCenter = new Vector3Int(checkPos.x, checkPos.y + distanceFromEmptyCell, 0);

                    GenerateBiome(tempBiomeCenter, Border.Up);
                }
            }
        }

        void Check_LeftBorder()
        {
            Vector3Int leftBottomAngle = new Vector3Int(cameraPosX - (screenWidthInUnits / 2), cameraPosY - (screenHeightInUnits / 2), 0);

            for(int y = 0; y <= screenHeightInUnits; ++y)
            {
                Vector3Int checkPos = new Vector3Int (leftBottomAngle.x, leftBottomAngle.y + y, 0);

                if(IsCellEmpty(checkPos))
                {
                    Vector3Int tempBiomeCenter = new Vector3Int(checkPos.x - distanceFromEmptyCell, checkPos.y, 0);
                    
                    GenerateBiome(tempBiomeCenter, Border.Left);
                }
            }
        }

        void Check_RightBorder()
        {
            Vector3Int rightBottomAngle = new Vector3Int(cameraPosX + (screenWidthInUnits / 2), cameraPosY - (screenHeightInUnits / 2), 0);
            
            for(int y = 0; y <= screenHeightInUnits; ++y)
            {
                Vector3Int checkPos = new Vector3Int (rightBottomAngle.x, rightBottomAngle.y + y, 0);

                if(IsCellEmpty(checkPos))
                {
                    Vector3Int tempBiomeCenter = new Vector3Int(checkPos.x + distanceFromEmptyCell, checkPos.y, 0);

                    GenerateBiome(tempBiomeCenter, Border.Right);
                }
            }
        }

        void GenerateBiome(Vector3Int tempBiomeCenter, Border side)
        {
            Biome biome = (Biome)Random.Range(1, biomesCount);

            switch(biome)
            {
                case Biome.Forest:
                {
                    GenerateForest(tempBiomeCenter, side);
                }break;
                default:return;
            }
        }

        void GenerateForest(Vector3Int tempBiomeCenter, Border side)
        {
            const int biomeChangeValue = 15;

            int x, y;
            switch(side)
            {
                case Border.Up:
                {
                    x = 0;
                    y = biomeChangeValue;
                }break;
                case Border.Bottom:
                {
                    x = 0;
                    y = -biomeChangeValue;
                }break;
                case Border.Left:
                {
                    x = -biomeChangeValue;
                    y = 0;
                }break;
                case Border.Right:
                {
                    x = biomeChangeValue;
                    y = 0;
                }break;
                default: return;    
            }

            Vector3Int forestCenter = new Vector3Int (tempBiomeCenter.x + x, tempBiomeCenter.y + y, 0);

            int lakeChance = 10;

            int forestWidth = (distanceFromEmptyCell + biomeChangeValue) * 2;
            int forestHeight = (distanceFromEmptyCell + biomeChangeValue) * 2;

            Vector3Int forestBottomLeft = new Vector3Int(forestCenter.x - (forestWidth / 2), forestCenter.y - (forestHeight / 2), 0);

            for(int _y = 0; _y <= forestHeight; ++_y)
            {
                for(int _x = 0; _x <= forestWidth; ++_x)
                {
                    Vector3Int checkPos = new Vector3Int(forestBottomLeft.x + _x, forestBottomLeft.y + _y, 0);

                    if(!(IsCellEmpty(checkPos)))
                        continue;
                    
                    int randomizer = Random.Range(0, 5000);

                    if(randomizer < lakeChance)
                    {
                        GenerateLakes(checkPos, side);
                    }
                    else
                    {
                        GenerateGrass(checkPos);
                    }
                }
            }
        }

        void GenerateGrass(Vector3Int position)
        {
            Tilemap tilemap = tileMaps[1];
            Tile tile = Resources.Load<Tile>("Tiles/Grass/Grass");

            tilemap.SetTile(position, tile);
        }

        void GenerateLakes(Vector3Int position, Border side)
        {
            Tilemap waterTilemap = tileMaps[0];
            Tilemap grassTilemap = tileMaps[1];
            Tile waterTile = Resources.Load<Tile>("Tiles/Water");
            Tile grassTile = Resources.Load<Tile>("Tiles/Grass/Grass");

            int waterSpawnChance = 900;

            int lakeWidth = Random.Range(5, 8);
            int lakeHeight = Random.Range(5, 8);

            switch(side)
            {
                case Border.Bottom:
                {
                    Vector3Int leftBottomAngle = new Vector3Int(position.x - (lakeWidth / 2), position.y - (lakeHeight / 2), 0);

                    for(int _y = 0; _y <= lakeHeight; ++_y)
                    {
                        for(int _x = 0; _x <= lakeWidth; ++_x)
                        {
                            Vector3Int checkPos = new Vector3Int(leftBottomAngle.x + _x, leftBottomAngle.y + _y, 0);
                            if(!(IsCellEmpty(checkPos)))
                                continue;

                            if(IsBorder(_x, _y, lakeWidth, lakeHeight))
                            {
                                int randomizer = Random.Range(0, 1000);

                                if(randomizer > waterSpawnChance)
                                {
                                    grassTilemap.SetTile(checkPos, grassTile);
                                }
                                else
                                {
                                    waterTilemap.SetTile(checkPos, waterTile);
                                }
                            }
                            else
                            {
                                waterTilemap.SetTile(checkPos, waterTile);
                            }
                        }
                    }
                    LakeBorderFix(leftBottomAngle, lakeHeight, lakeWidth);
                }break;
                default: break;
            }
        }

        bool IsBorder(int _x, int _y, int maxX, int maxY)
        {
            if(_x == 0 || _y == 0 || _x == maxX || _y == maxY)
                return true;
            return false;
        }

        void LakeBorderFix(Vector3Int leftBottomAngle, int lakeHeight, int lakeWidth)
        {
            Tilemap waterTilemap = tileMaps[0];
            Tilemap grassTilemap = tileMaps[1];

            Tile waterTile = Resources.Load<Tile>("Tiles/Water");

            Tile grassSideTile = Resources.Load<Tile>("Tiles/Grass/GrassSide");
            Tile grassLeftSideTile = Resources.Load<Tile>("Tiles/Grass/GrassSideLeft");
            Tile grassRightSideTile = Resources.Load<Tile>("Tiles/Grass/GrassSideRight");

            Tile grassTopLeftTile = Resources.Load<Tile>("Tiles/Grass/GrassTopLeft");
            Tile grassTopTile = Resources.Load<Tile>("Tiles/Grass/GrassTop");
            Tile grassTopRightTile = Resources.Load<Tile>("Tiles/Grass/GrassTopRight");

            for(int y = 0; y <= lakeHeight; ++y)
            {
                for(int x = 0; x <= lakeWidth; ++x)
                {
                    Vector3Int checkPos = new Vector3Int(leftBottomAngle.x + x, leftBottomAngle.y + y, 0);
                    Vector3Int checkBelowPos = new Vector3Int(checkPos.x, checkPos.y - 1, 0);
                    Vector3Int checkAbovePos = new Vector3Int(checkPos.x, checkPos.y + 1, 0);

                    if(IsWater(checkPos))
                    {
                        if(IsGrass(checkBelowPos))
                        {
                            grassTilemap.SetTile(checkBelowPos, null);
                            grassTilemap.SetTile(checkBelowPos, grassTopTile);
                        }
                        if(IsGrass(checkAbovePos))
                        {
                            grassTilemap.SetTile(checkAbovePos, null);
                            grassTilemap.SetTile(checkAbovePos, grassSideTile);
                            
                        }
                    }
                    else if(IsGrass(checkPos))
                    {
                        if(IsWater(checkBelowPos))
                        {
                            grassTilemap.SetTile(checkAbovePos, null);
                            grassTilemap.SetTile(checkAbovePos, grassSideTile);

                        }
                        if(IsWater(checkAbovePos))
                        {
                            grassTilemap.SetTile(checkBelowPos, null);
                            grassTilemap.SetTile(checkBelowPos, grassTopTile);
                            
                        }
                    }

                }
            }
        }

        bool IsGrass(Vector3Int grassPos)
        {
            Tilemap grassTilemap = tileMaps[1];
            Tile grassTile = Resources.Load<Tile>("Tiles/Grass/Grass");

            if(grassTilemap.GetTile(grassPos) == grassTile)
                return true;
            return false;
        }

        bool IsWater(Vector3Int waterPos)
        {
            Tilemap waterTilemap = tileMaps[0];
            Tile waterTile = Resources.Load<Tile>("Tiles/water");

            if(waterTilemap.GetTile(waterPos) == waterTile)
                return true;
            return false;
        }


        void UpdateCamera()
        {
            cameraPosX = Mathf.RoundToInt(Camera.main.transform.position.x);
            cameraPosY = Mathf.RoundToInt(Camera.main.transform.position.y); 
            screenHeightInUnits = Mathf.RoundToInt(Camera.main.orthographicSize * 2);
            screenWidthInUnits = Mathf.RoundToInt(screenHeightInUnits * Screen.width / Screen.height);
        }


    }
}
