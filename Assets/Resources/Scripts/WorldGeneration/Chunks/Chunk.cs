using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using InjectorGames.FarAlone.TileManager;
using InjectorGames.FarAlone.FileManager;

namespace InjectorGames.FarAlone.WorldGeneration
{
    public class Chunk : MonoBehaviour
    {
        World world = GameObject.Find("World generation").GetComponent<World>();

        const int birthLimit = 1;
        const int deathLimit = 3;
        
        public ChunkType type;
        public Biome chunk_biome;
        public Vector2Int center_pos;

        public bool IsVisible;
        public bool IsGenerated;

        public TileType[ , ] Terrain_heatMap;// = new TileType[world.chunk_IntDiameter, world.chunk_IntDiameter];

        public Chunk(Vector2Int _pos, Biome _biome, ChunkType _type, TileType[ , ] _ter_heatMap)
        {
            Terrain_heatMap = _ter_heatMap;
            center_pos = _pos;
            chunk_biome = _biome;
            type = _type;

            IsVisible = false;
            IsGenerated = false;
        }

        Vector2Int GetLeftCornerPos()
        {
            return new Vector2Int(center_pos.x - (world.IntDiameter / 2), center_pos.y - (world.IntDiameter / 2 ));
        }

        #region Heatmap

        public void GenerateTerrain_HeatMap()
        {
            for(int x = 0; x < world.IntDiameter; ++x)
            {
                for(int y = 0; y < world.IntDiameter; ++y)
                {
                    Terrain_heatMap[x, y] = GenerateTile_HeatMap(x, y);
                    if(Terrain_heatMap[x,y] == TileType.Error)
                    {
                        Debug.Log("Error has occured in creating chunk's heatmap");
                    }
                }
            }
        }

        public void SmoothTerrain_Heatmap()
        {
            TileType[ , ] fixedHeatmap = new TileType[world.IntDiameter, world.IntDiameter];

            int waterNeighbourCount; 

            BoundsInt neighbourPos = new BoundsInt(-1, -1, 0, 3, 3, 1);

            Chunk temp_chunk;

            foreach(Vector2Int bound in neighbourPos.allPositionsWithin)
            {
                Debug.Log("BoundPos x: " + bound.x + " y: " + bound.y);
            }

            for(int x = 0; x < world.IntDiameter; ++x)
            {
                for(int y = 0; y < world.IntDiameter; ++y)
                {
                    waterNeighbourCount = 0;

                    foreach(Vector2Int bound in neighbourPos.allPositionsWithin)
                    {
                        if(bound.x == 0 && bound.y == 0)
                        {
                            continue;
                        }
                        if((x + bound.x >= 0 && y + bound.y >=0) && (x + bound.x < world.IntDiameter && y + bound.y < world.IntDiameter))
                        {
                            if(IsWater(x + bound.x, y + bound.y))
                            {
                                waterNeighbourCount++;
                            }
                            else continue;
                        }
                        else if(x + bound.x == -1 && (y + bound.y >= 0 && y + bound.y < world.IntDiameter))
                        {
                            temp_chunk = GetChunk(new Vector2Int(center_pos.x - world.IntDiameter, center_pos.y));

                            if(temp_chunk.IsWater(world.IntDiameter - 1, y + bound.y))
                            {
                                waterNeighbourCount++;
                            }
                            else continue;
                        }
                        else if(x + bound.x == world.IntDiameter && (y + bound.y >= 0 && y + bound.y < world.IntDiameter))
                        {
                            temp_chunk = GetChunk(new Vector2Int(center_pos.x + world.IntDiameter, center_pos.y));

                            if(temp_chunk.IsWater(0, y + bound.y))
                            {
                                waterNeighbourCount++;
                            }
                            else continue;
                        }
                        else if((x + bound.x >= 0 && x + bound.x < world.IntDiameter) && y + bound.y == -1)
                        {
                            temp_chunk = GetChunk(new Vector2Int(center_pos.x, center_pos.y - world.IntDiameter));

                            if(temp_chunk.IsWater(x + bound.x, world.IntDiameter - 1))
                            {
                                waterNeighbourCount++;
                            }
                            else continue;
                        }
                        else if((x + bound.x >= 0 && x + bound.x < world.IntDiameter) && y + bound.y == world.IntDiameter)
                        {
                            temp_chunk = GetChunk(new Vector2Int(center_pos.x, center_pos.y + world.IntDiameter));

                            if(temp_chunk.IsWater(x + bound.x, 0))
                            {
                                waterNeighbourCount++;
                            }
                            else continue;
                        }
                        else if(x + bound.x == -1 && y + bound.y == -1)
                        {
                            temp_chunk = GetChunk(new Vector2Int(center_pos.x - world.IntDiameter, center_pos.y - world.IntDiameter));

                            if(temp_chunk.IsWater(world.IntDiameter - 1, world.IntDiameter - 1))
                            {
                                waterNeighbourCount++;
                            }
                            else continue;
                        }
                        else if(x + bound.x == -1 && y + bound.y == world.IntDiameter)
                        {
                            temp_chunk = GetChunk(new Vector2Int(center_pos.x - world.IntDiameter, center_pos.y + world.IntDiameter));

                            if(temp_chunk.IsWater(world.IntDiameter - 1, 0))
                            {
                                waterNeighbourCount++;
                            }
                            else continue;
                        }
                        else if(x + bound.x == world.IntDiameter && y + bound.y == world.IntDiameter)
                        {
                            temp_chunk = GetChunk(new Vector2Int(center_pos.x + world.IntDiameter, center_pos.y + world.IntDiameter));
                            
                            if(temp_chunk.IsWater(0, 0))
                            {
                                waterNeighbourCount++;
                            }
                            else continue;
                        } 
                        else if(x + bound.x == world.IntDiameter && y + bound.y == -1)
                        {
                            temp_chunk = GetChunk(new Vector2Int(center_pos.x + world.IntDiameter, center_pos.y - world.IntDiameter));

                            if(temp_chunk.IsWater(0, world.IntDiameter - 1))
                            {
                                waterNeighbourCount++;
                            }
                            else continue;
                        }
                    }

                    Debug.Log("WaterNeighbourCount: " + waterNeighbourCount );

                    if(Terrain_heatMap[x, y] == TileType.Grass)
                    {
                        if(waterNeighbourCount > birthLimit)
                        {
                            fixedHeatmap[x, y] = TileType.Water;
                        }
                    }
                    else if(Terrain_heatMap[x, y] == TileType.Water)
                    {
                        if(waterNeighbourCount < deathLimit)
                        {
                            fixedHeatmap[x, y] = TileType.Grass;
                        }
                    }
                }
            }

            Terrain_heatMap = fixedHeatmap;

            return;
        }


        TileType GenerateTile_HeatMap(int x, int y)
        {
            float tile_perlin = Mathf.PerlinNoise((float) (center_pos.x + x) * world.Seed, (float) (center_pos.y + y) * world.Seed);

            if(chunk_biome == Biome.Forest)
            {
                if(tile_perlin < 0.75f)
                {
                    return TileType.Grass;
                }
                else if(tile_perlin < 1f)
                {
                    return TileType.Water;
                }
            }
            else if(chunk_biome == Biome.Lake)
            {
                if(tile_perlin < 0.72f)
                {
                    return TileType.Water;
                }
                else if(tile_perlin < 1f)
                {
                    return TileType.Grass;
                }
            }
            return TileType.Error;
        }

        public void Terrain_Heatmap_FirstCorrection() // First correction is correcting ONLY grass and water tiles
        {
            bool aboveWater, belowWater, rightWater, leftWater;

            Chunk temp_chunk;

            for(int x = 0; x < world.IntDiameter; ++x)
            {
                for(int y = 0; y < world.IntDiameter; ++y)
                {
                    if(!IsGrass(x, y))
                        continue;

                    if(y == world.IntDiameter - 1)
                    {
                        temp_chunk = GetChunk(new Vector2Int(center_pos.x, center_pos.y + world.IntDiameter));

                        aboveWater = temp_chunk.IsWater(x, 0);

                    }
                    else
                    {
                        aboveWater = IsWater_Above(x, y);
                    }

                    if(aboveWater) // If there is a water above (8 of 12 left)
                    {
                        if(y == 0)
                        {
                            temp_chunk = GetChunk(new Vector2Int(center_pos.x, center_pos.y - world.IntDiameter));

                            belowWater = temp_chunk.IsWater(x, world.IntDiameter - 1);
                        }
                        else
                        {
                            belowWater = IsWater_Below(x, y);
                        }

                        if(belowWater) // If there is a water below (4 of 8 left)
                        {
                            if(x == world.IntDiameter - 1)
                            {
                                temp_chunk = GetChunk(new Vector2Int(center_pos.x + world.IntDiameter, center_pos.y));

                                rightWater = temp_chunk.IsWater(0, y);

                                
                            }
                            else
                            {
                                rightWater = IsWater_Right(x, y);
                            }

                            if(rightWater) // If there is a water at right (2 of 4 left)
                            {
                                if(x == 0)
                                {
                                    temp_chunk = GetChunk(new Vector2Int(center_pos.x - world.IntDiameter, center_pos.y));

                                    leftWater = temp_chunk.IsWater(world.IntDiameter - 1, y);

 

                                    
                                }
                                else
                                {
                                    leftWater = IsWater_Left(x, y);
                                }

                                if(leftWater) //If there is a water at left (1 of 5 left)
                                {
                                    Terrain_heatMap[x, y] = TileType.Grass_Island;
                                }
                                else //If there is not a water at left (1 of 5 left)
                                {
                                    Terrain_heatMap[x, y] = TileType.GrassBottom_Top_Right;
                                }
                            }
                            else // If there is not a water at right (2 of 4 left)
                            {
                                if(x == 0)
                                {
                                    temp_chunk = GetChunk(new Vector2Int(center_pos.x - world.IntDiameter, center_pos.y));

                                    leftWater = temp_chunk.IsWater(world.IntDiameter - 1, y);


                                    
                                }
                                else
                                {
                                    leftWater = IsWater_Left(x, y);
                                }

                                if(leftWater) // If there is a water at left (1 of 2 left)
                                {
                                    Terrain_heatMap[x, y] = TileType.GrassBottom_Top_Left;
                                }
                                else // If there is not a water at left (1 of 2 left)
                                {
                                    Terrain_heatMap[x, y] = TileType.GrassBottom_Top;
                                }
                            }
                        }
                        else // if there is not a water below (4 of 8 left)
                        {
                            if(x == world.IntDiameter - 1)
                            {
                                temp_chunk = GetChunk(new Vector2Int(center_pos.x + world.IntDiameter, center_pos.y));

                                rightWater = temp_chunk.IsWater(0, y);



                            }
                            else
                            {
                                rightWater = IsWater_Right(x, y);
                            }

                            if(rightWater) // If there is a water at right (2 of 4 left)
                            {
                                if(x == 0)
                                {
                                    temp_chunk = GetChunk(new Vector2Int(center_pos.x - world.IntDiameter, center_pos.y));

                                    leftWater = temp_chunk.IsWater(world.IntDiameter - 1, y);




                                }
                                else
                                {
                                    leftWater = IsWater_Left(x, y);
                                }

                                if(leftWater) //If there is a water at left (1 of 2 left)
                                {
                                    Terrain_heatMap[x, y] = TileType.GrassTop_Closed;
                                }
                                else // If there is not a water at left (1 of 2 left)
                                {
                                    Terrain_heatMap[x, y] = TileType.GrassTop_Right;
                                }
                            }
                            else // If there is not a water at right (2 of 4 left)
                            {
                                if(x == 0)
                                {
                                    temp_chunk = GetChunk(new Vector2Int(center_pos.x - world.IntDiameter, center_pos.y));

                                    leftWater = temp_chunk.IsWater(world.IntDiameter - 1, y);



                                }
                                else
                                {
                                    leftWater = IsWater_Left(x, y);
                                }

                                if(leftWater) // if there is a water at left (1 of 2 left)
                                {
                                    Terrain_heatMap[x, y] = TileType.GrassTop_Left;
                                }
                                else // If there is not a water at left (1 of 2 left)
                                {
                                    Terrain_heatMap[x, y] = TileType.GrassTop;
                                }
                            }

                        }
                    }
                    else // if there is not a water above (4 of 12 left)
                    {
                        if(y == 0)
                        {
                            temp_chunk = GetChunk(new Vector2Int(center_pos.x, center_pos.y - world.IntDiameter));

                            belowWater = temp_chunk.IsWater(x, world.IntDiameter - 1);


                        }
                        else
                        {
                            belowWater = IsWater_Below(x, y);
                        }

                        if(belowWater) // If there is a water below (4 of 4 left)
                        {
                            if(x == world.IntDiameter - 1)
                            {
                                temp_chunk = GetChunk(new Vector2Int(center_pos.x + world.IntDiameter, center_pos.y));

                                rightWater = temp_chunk.IsWater(0, y);


                            }
                            else
                            {
                                rightWater = IsWater_Right(x, y);
                            }

                            if(rightWater) // If there is a water at right (2 of 4 left)
                            {
                                if(x == 0)
                                {
                                    temp_chunk = GetChunk(new Vector2Int(center_pos.x - world.IntDiameter, center_pos.y));

                                    leftWater = temp_chunk.IsWater(world.IntDiameter - 1, y);



                                }
                                else
                                {
                                    leftWater = IsWater_Left(x, y);
                                }    

                                if(leftWater) // If there is a water at left (1 of 2 left)
                                {
                                    Terrain_heatMap[x, y] = TileType.GrassBottom_Double;
                                }
                                else // If there is not a water at left (1 of 2 left)
                                {
                                    Terrain_heatMap[x, y] = TileType.GrassBottom_Right;
                                }
    
                            }
                            else // If there is not a water at right (2 of 4 left)
                            {
                                if(x == 0)
                                {
                                    temp_chunk = GetChunk(new Vector2Int(center_pos.x - world.IntDiameter, center_pos.y));

                                    leftWater = temp_chunk.IsWater(world.IntDiameter - 1, y);



                                }
                                else
                                {
                                    leftWater = IsWater_Left(x, y);
                                }

                                if(leftWater) // If there is a water at left (1 of 2 left)
                                {
                                    Terrain_heatMap[x, y] = TileType.GrassBottom_Left;
                                }
                                else // If there is not a water at left (1 of 2 left)
                                {
                                    Terrain_heatMap[x, y] = TileType.GrassBottom;
                                }
                            }
                        }
                        else // If there is not a water below (0 of 4 left)
                        {
                            continue;
                        }
                    }


                }
            }
        }

        public void DeleteCorrections()
        {
            for(int x = 0; x < world.IntDiameter; x++)
            {
                for(int y = 0; y < world.IntDiameter; y++)
                {
                    if(Terrain_heatMap[x, y] > TileType.Grass && Terrain_heatMap[x, y] <= TileType.GrassBottom_Top_Left_Trans)
                    {
                        Terrain_heatMap[x, y] = TileType.Grass; 
                    }
                }
            }
        }

        Chunk GetChunk(Vector2Int chunk_pos)
        {
            for(int i = 0; i < world.world_chunks.Count; ++i)
            {
                if(world.world_chunks[i].center_pos == chunk_pos)
                    return world.world_chunks[i];
            }

            Chunk temp_chunk;

            if(SaveManager.IsChunk_Exist(chunk_pos.x, chunk_pos.y))
            {
                if(SaveManager.IsChunk_Static(chunk_pos.x, chunk_pos.y))
                {
                    temp_chunk = SaveManager.Load_Chunk(chunk_pos.x, chunk_pos.y, ChunkType.Static);
                }
                else
                {
                    temp_chunk = SaveManager.Load_Chunk(chunk_pos.x, chunk_pos.y, ChunkType.Dynamic);                  
                }

                temp_chunk.DeleteCorrections();

            }
            else
            {
                temp_chunk = world.CreateChunk(chunk_pos, ChunkType.Dynamic);

                temp_chunk.GenerateTerrain_HeatMap();

            }

            return temp_chunk;

        }
        #region Bools
        bool IsWater_Above(int cur_x, int cur_y)
        {
            return IsWater(cur_x, cur_y + 1);
        }

        bool IsWater_Below(int cur_x, int cur_y)
        {
            return IsWater(cur_x, cur_y - 1);
        }

        bool IsWater_Right(int cur_x, int cur_y)
        {
            return IsWater(cur_x + 1, cur_y);
        }

        bool IsWater_Left(int cur_x, int cur_y)
        {
            return IsWater(cur_x - 1, cur_y);
        }

        bool IsWater(int x, int y)
        {
            if(Terrain_heatMap[x, y] == TileType.Water)
                return true;
            return false;
        }

        bool IsGrass(int x, int y)
        {
            if(Terrain_heatMap[x, y] == TileType.Grass)
                return true;
            return false;
        }

        #endregion


        #endregion

        #region Chunk_Image

        public void Generate_Tiles()
        {
            Vector2Int leftCorner_pos = GetLeftCornerPos();

            Vector3Int tempTile_pos;

            TileInfo tileInfo;

            var tileCollection = TileCollection.Initialize();

            for(int x = 0; x < world.IntDiameter; ++x)
            {
                for(int y = 0; y < world.IntDiameter; ++y)
                {
                    tempTile_pos = new Vector3Int(x + leftCorner_pos.x , y + leftCorner_pos.y, 0);
                    

                    tileInfo = tileCollection.GetTileInfo(Terrain_heatMap[x, y]);

                    tileInfo.tilemap.SetTile(new Vector3Int(tempTile_pos.x, tempTile_pos.y, 0), tileInfo.tile);
                    
                }
            }
        }

        #endregion
        



    }
}