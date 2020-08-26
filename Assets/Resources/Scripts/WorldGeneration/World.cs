using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using InjectorGames.FarAlone.FileManager;
using InjectorGames.FarAlone.TileManager;

namespace InjectorGames.FarAlone.WorldGeneration
{
    public class World : MonoBehaviour
    {     
       public static bool IsLoaded = false;

       public bool IsLoad
       {
           get
           {
               return IsLoaded;
           }
       } 

       //public int IsLoaded = 0;

       const float chunk_diameter = 25f;

       public const int chunk_IntDiameter = 25;

       public int IntDiameter
       {
           get
           {
               return chunk_IntDiameter;
           }
       }

       public const float seed = 2.3f;

       public float Seed
       {
           get
           {
               return seed;
           }
       }

       public List<Chunk> world_chunks = new List<Chunk>();

       [Header("Generation")]
       [SerializeField]
       public int chunkCount;

       [SerializeField]
       int CurIntChunkPosX;
       [SerializeField]
       int CurIntChunkPosY;

       [SerializeField]
       Vector2Int leftVisChunk;
       [SerializeField]
       Vector2Int rightVisChunk; 

       [SerializeField]
       Vector2 leftBottomPoint;
       [SerializeField]
       Vector2 rightTopPoint;


       [Header("Biome")]
       [SerializeField]
       float PerlinBiome;

       [Header("Camera")]
       [SerializeField]
       float cameraPosX;
       [SerializeField]
       float cameraPosY;

       [Header("Tilemaps")]
       [SerializeField]
       Tilemap groundTilemap;
       [SerializeField]
       Tilemap waterTilemap;
       [SerializeField]
       Tilemap structureTilemap;
       [SerializeField]
       Tilemap entityTilemap;

       [SerializeField]
       Tile tempTile;

       void Start()
       {
           InitializeTilemaps();

           if(SaveManager.Check_WorldLoad()) // Checking if the world was loaded before or not to deserialize static chunks into it's List
           {
               IsLoaded = SaveManager.Check_WorldLoad();

               List<Chunk> static_chunks = new List<Chunk>();

               Generate_StaticChunks(static_chunks);

               DeleteStaticChunks(static_chunks);

               //TODO: DELETE OBJECTS IN THE CHUNK

               return;
           }
           else
           {
               List<Chunk> static_chunks = new List<Chunk>();  // static chunks

               Generate_StaticChunks(static_chunks); 

               Generate_StaticChunks_Heatmap(static_chunks);

               Save_StaticChunks(static_chunks);

               //TODO: SAVE OBJECTS IN THE CHUNK

               //DoStaticChunks_FirstCorrection(static_chunks);

               //Save_StaticChunks(static_chunks);

               for(int i = 0; i < static_chunks.Count; ++i)
               {
                   static_chunks[i].Terrain_Heatmap_FirstCorrection();
                   SaveManager.Save_Chunk(static_chunks[i]);
                   static_chunks[i].DeleteCorrections();

               }

               IsLoaded = true;

               SaveManager.Set_WorldLoad();

               DeleteStaticChunks(static_chunks);
 
           }

       }

       void Update() 
       {
           UpdateCamera();

           Generate_VisibleChunks();   

           Set_ChunksVisibility();

           DeleteChunks_NonVis(); // Deleting non-visible chunks

           Generate_ChunkImage();

           UpdateChunkNumber();

       }

       void InitializeTilemaps()
       {
           TilemapCollection tilemapCollection = TilemapCollection.Initialize();

           groundTilemap = tilemapCollection.GetTilemap(TilemapType.GroundTilemap);
           waterTilemap = tilemapCollection.GetTilemap(TilemapType.WaterTilemap);
           structureTilemap = tilemapCollection.GetTilemap(TilemapType.StructureTilemap);
           entityTilemap = tilemapCollection.GetTilemap(TilemapType.EntityTilemap);
       }

       #region Chunk generation

       #region Static Chunks

       void Generate_StaticChunks(List <Chunk> static_chunks) // IN PROCESS
       {
           static_chunks.Add(CreateStaticChunk(new Vector2Int(-75, -25), Biome.Island, ChunkType.Static));
           static_chunks.Add(CreateStaticChunk(new Vector2Int(-75, 0), Biome.Island, ChunkType.Static));
           static_chunks.Add(CreateStaticChunk(new Vector2Int(-75, 25), Biome.Island, ChunkType.Static));

           static_chunks.Add(CreateStaticChunk(new Vector2Int(-50, -25), Biome.Island, ChunkType.Static));
           static_chunks.Add(CreateStaticChunk(new Vector2Int(-50, 0), Biome.Island, ChunkType.Static));
           static_chunks.Add(CreateStaticChunk(new Vector2Int(-50, 25), Biome.Island, ChunkType.Static));

           static_chunks.Add(CreateStaticChunk(new Vector2Int(-25, -25), Biome.Island, ChunkType.Static));
           static_chunks.Add(CreateStaticChunk(new Vector2Int(-25, 0), Biome.Island, ChunkType.Static));
           static_chunks.Add(CreateStaticChunk(new Vector2Int(-25, 25), Biome.Island, ChunkType.Static));

           static_chunks.Add(CreateStaticChunk(new Vector2Int(0, -25), Biome.Island, ChunkType.Static));
           static_chunks.Add(CreateStaticChunk(new Vector2Int(0, 0), Biome.Island, ChunkType.Static));
           static_chunks.Add(CreateStaticChunk(new Vector2Int(0, 25), Biome.Island, ChunkType.Static));           

           static_chunks.Add(CreateStaticChunk(new Vector2Int(25, -25), Biome.Island, ChunkType.Static));
           static_chunks.Add(CreateStaticChunk(new Vector2Int(25, 0), Biome.Island, ChunkType.Static));
           static_chunks.Add(CreateStaticChunk(new Vector2Int(25, 25), Biome.Island, ChunkType.Static));

           static_chunks.Add(CreateStaticChunk(new Vector2Int(50, -25), Biome.Island, ChunkType.Static)); 
           static_chunks.Add(CreateStaticChunk(new Vector2Int(50, 0), Biome.Island, ChunkType.Static));
           static_chunks.Add(CreateStaticChunk(new Vector2Int(50, 25), Biome.Island, ChunkType.Static));
       }

       Chunk CreateStaticChunk(Vector2Int intPosition, Biome biome, ChunkType type)
       {
           return new Chunk(intPosition, biome, type, new TileType[chunk_IntDiameter, chunk_IntDiameter]);
       }

       void DeleteStaticChunks(List <Chunk> static_chunks)
       {
           for(int i = 0; i < static_chunks.Count; ++i)
           {
               DeleteChunk(static_chunks[i]);
           }
       }

       void Generate_StaticChunks_Heatmap(List <Chunk> static_chunks)
       {
           for(int i = 0; i < static_chunks.Count; ++i)
           {
               Generate_StChunk_Heatmap(static_chunks[i]);
           }
       }

       void Generate_StChunk_Heatmap(Chunk st_chunk)
       {
           int leftAnglePos_X = st_chunk.center_pos.x - (chunk_IntDiameter / 2);
           int leftAnglePos_Y = st_chunk.center_pos.y - (chunk_IntDiameter / 2);

           Debug.Log("LeftX: " + leftAnglePos_X + " LeftY: " + leftAnglePos_Y);

           for(int x = 0; x < chunk_IntDiameter; ++x)
           {
               for(int y = 0; y < chunk_IntDiameter; ++y)
               {
                   if(groundTilemap.GetTile<Tile>(new Vector3Int(leftAnglePos_X + x, leftAnglePos_Y + y, 0)) != null)
                   {
                       st_chunk.Terrain_heatMap[x, y] = TileType.Grass;
                   }
                   else if(waterTilemap.GetTile<Tile>(new Vector3Int(leftAnglePos_X + x, leftAnglePos_Y + y, 0)) != null)
                   {
                       st_chunk.Terrain_heatMap[x, y] = TileType.Water; 
                   }
                   else
                   {
                       st_chunk.Terrain_heatMap[x, y] = TileType.Error;
                   }     
               }
           }
       }

       void DoStaticChunks_FirstCorrection(List <Chunk> static_chunks)
       {
           for(int i = 0; i < static_chunks.Count; ++i)
           {
               static_chunks[i].Terrain_Heatmap_FirstCorrection(); // Do first static chunk correction
           }
       }
       
       #endregion

       #region SaveAndLoad 
       void Save_StaticChunks(List <Chunk> static_chunks)
       {
           for(int i = 0; i < static_chunks.Count; ++i)
           {
               SaveManager.Save_Chunk(static_chunks[i]);
           }
       }

       void SaveListChunks()
       {
           for(int i = 0; i < world_chunks.Count; ++i)
           {
               SaveManager.Save_Chunk(world_chunks[i]);
           }
       }
       #endregion

       public Chunk CreateChunk(Vector2Int intPosition, ChunkType type) // SHOULD BE UPDATED
       {

            float biomePerlin = Mathf.PerlinNoise((float) intPosition.x * seed, (float) intPosition.y * seed);

            PerlinBiome = biomePerlin; // just to check in Unity Editor
                
            Biome chunkBiome = GetBiome(biomePerlin);


            Chunk cur_chunk = new Chunk(intPosition, chunkBiome, type, new TileType[chunk_IntDiameter, chunk_IntDiameter]);
            
            return cur_chunk;
       }

       void Generate_VisibleChunks()
       {

           //Vector2Int leftVisChunk = GetLeftVisChunk_Pos();

           leftVisChunk = GetLeftVisChunk_Pos();

           //Vector2Int rightVisChunk = GetRightVisChunk_Pos();

           rightVisChunk = GetRightVisChunk_Pos();

           Chunk temp_chunk;

           for(int x = leftVisChunk.x; x <= rightVisChunk.x; x += chunk_IntDiameter)
           {
               for(int y = leftVisChunk.y; y <= rightVisChunk.y; y += chunk_IntDiameter)
               {
                   if(!IsExistInList(x, y))
                   {                
                       if(SaveManager.IsChunk_Exist(x, y))
                       {
                            if(SaveManager.IsChunk_Static(x, y))
                            {
                                world_chunks.Add(SaveManager.Load_Chunk(x, y, ChunkType.Static));
                            }
                            else
                            {
                                world_chunks.Add(SaveManager.Load_Chunk(x, y, ChunkType.Dynamic));
                                    
                            }
                       }
                       else
                       {
                            temp_chunk = CreateChunk(new Vector2Int(x, y), ChunkType.Dynamic);
                            temp_chunk.GenerateTerrain_HeatMap(); // Generating the new added chunk heatmap
                            temp_chunk.SmoothTerrain_Heatmap(); // Generating lakes/rivers
                            temp_chunk.SmoothTerrain_Heatmap(); // Doubling the smoothness to make it more realistic
                            temp_chunk.SmoothTerrain_Heatmap(); // Tripling the smoothness to make it more realistic
                            //temp_chunk.SmoothTerrain_Heatmap(); // Quadrupling the smoothness to make it more realistic

                            temp_chunk.Terrain_Heatmap_FirstCorrection(); // Doing chunk's first correction
                            //TODO: Do second correction
                            //TODO: Generate structures (with NPC)
                            //TODO: Generate objects
                            //TODO: Generate spawners
                            world_chunks.Add(temp_chunk);           
                       }                      
                   }
                   else
                   {
                       continue;
                   }

               }
           }
       }

       Vector2Int GetLeftVisChunk_Pos()
       {
           //Vector2 leftBottomPoint = (Vector2) Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f, 0f));

           leftBottomPoint = (Vector2) Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f, 0f));

           return GetChunkByPoint(new Vector2(leftBottomPoint.x, leftBottomPoint.y));


           
       }

       Vector2Int GetRightVisChunk_Pos()
       {
           //Vector2 rightTopPoint = (Vector2) Camera.main.ViewportToWorldPoint(new Vector3(1f, 1f, 0f));

           rightTopPoint = (Vector2) Camera.main.ViewportToWorldPoint(new Vector3(1f, 1f, 0f));

           return GetChunkByPoint(new Vector2(rightTopPoint.x, rightTopPoint.y));
       }

       void Set_ChunksVisibility()
       {
           leftVisChunk = GetLeftVisChunk_Pos();
           rightVisChunk = GetRightVisChunk_Pos();

           for(int i = 0; i < world_chunks.Count; ++i)
           {
               if((world_chunks[i].center_pos.x >= leftVisChunk.x && world_chunks[i].center_pos.x <= rightVisChunk.x) &&
               (world_chunks[i].center_pos.y >= leftVisChunk.y && world_chunks[i].center_pos.y <= rightVisChunk.y))
               {
                   world_chunks[i].IsVisible = true;
               }
               else
               {
                   world_chunks[i].IsVisible = false;
               }
           }
       }

       void DeleteChunks_NonVis()
       {
           for(int i = 0; i < world_chunks.Count; ++i)
           {
               if(world_chunks[i].IsVisible)
               {
                   continue;
               }
               else
               {
                   SaveManager.Save_Chunk(world_chunks[i]);
                   DeleteChunk(world_chunks[i]);
               }
           }
       }

       void Generate_ChunkImage()
       {
           for(int i = 0; i < world_chunks.Count; ++i)
           {
               if(!world_chunks[i].IsGenerated)
               {
                   world_chunks[i].Generate_Tiles();
                   //world_chunks[i].Generate_Objects();
                   //TODO: Generate spawners
                   //TODO: Generate NPC
                   world_chunks[i].IsGenerated = true;
               }
           }
       }

       void DeleteChunk(Chunk chunk)
       {
           DeleteChunk_Tiles(chunk);
           DeleteChunk_Objects(chunk);
           if(IsExistInList(chunk))
           {
               DeleteChunk_FromArray(chunk);
           }
       }

       void DeleteChunk_Tiles(Chunk chunk)
       {
           Vector2Int leftCorner = new Vector2Int(chunk.center_pos.x - (chunk_IntDiameter / 2), chunk.center_pos.y - (chunk_IntDiameter / 2));

           for(int x = 0; x < chunk_IntDiameter; ++x)
           {
               for(int y = 0; y < chunk_IntDiameter; ++y)
               {
                   Vector3Int delete_pos = new Vector3Int(leftCorner.x + x, leftCorner.y + y, 0);

                   if(waterTilemap.GetTile<Tile>(delete_pos) != null)
                   {
                       waterTilemap.SetTile(delete_pos, null);
                   }
                   else if(groundTilemap.GetTile<Tile>(delete_pos) != null)
                   {
                       groundTilemap.SetTile(delete_pos, null);
                   }
                   else if(structureTilemap.GetTile<Tile>(delete_pos) != null)
                   {
                       structureTilemap.SetTile(delete_pos, null);
                   }
                   else if(entityTilemap.GetTile<Tile>(delete_pos) != null)
                   {
                       entityTilemap.SetTile(delete_pos, null);
                   }
                   else
                   {
                       Debug.LogError("Error in deleting tile at x:" + (leftCorner.x + x) + " y: " + (leftCorner.y + y) + 
                        " in the chunk with position (x:" + chunk.center_pos.x + ", y:" + chunk.center_pos.y);
                   }
               }
           }
       }

       void DeleteChunk_Objects(Chunk chunk)
       {
           //TODO: Delete chunk objects
       }

       public void DeleteChunk_FromArray(Chunk chunk)
       {
           for(int i = 0; i < world_chunks.Count; ++i)
           {
               if(chunk.center_pos == world_chunks[i].center_pos)
               {
                   world_chunks.RemoveAt(i);
                   return;
               }
           }
       }


       #endregion


       #region Gizmos

       void OnDrawGizmos() 
       {
           if(!IsWorldEmpty())
           {
               foreach(Chunk chunk in world_chunks)
               {
                   Gizmos.color = Color.white;
                   Gizmos.DrawWireCube((Vector2) chunk.center_pos, new Vector2(chunk_diameter, chunk_diameter));
               }

               foreach(Chunk chunk in world_chunks)
               {
                   Gizmos.color = Color.white;
                   Gizmos.DrawWireCube((Vector2) chunk.center_pos, new Vector2(chunk_diameter, chunk_diameter));

                   int leftAnglePos_X = chunk.center_pos.x - (chunk_IntDiameter / 2);
                   int leftAnglePos_Y = chunk.center_pos.y - (chunk_IntDiameter / 2);

                   for(int x = 0; x < chunk_IntDiameter; ++x)
                   {
                       for(int y = 0; y < chunk_IntDiameter; ++y)
                       {
                           if(chunk.Terrain_heatMap[x, y] == TileType.Water)
                           {
                               Gizmos.color = Color.blue;
                           }
                           else
                           {
                               Gizmos.color = Color.green;
                           }

                           

                           Gizmos.DrawWireCube((Vector2) new Vector2Int(leftAnglePos_X + x, leftAnglePos_Y + y), Vector2.one * .4f);
                       }
                   }
               }
           }
       }

       #endregion  

       #region Biome

       Biome GetBiome(float biomePerlin)
       {
           if(biomePerlin < 0.7f)
           {
               return Biome.Forest;
           }
           else if(biomePerlin < 1f)
           {
               return Biome.Lake;
           }
           else
           {
               throw new Exception("An issue has occured in calculation of perlin value");
           }
           
       }
       #endregion
        // SHOULD BE UPDATED

       #region Finding chunk position

       Vector2Int GetChunkByPoint(Vector2 point)
       {
           int diameterDoubler_X;

           int pointInt_X;

           float pointPosInZero_X;

           if(point.x >= 0)
           {
               pointInt_X = Mathf.FloorToInt(point.x);
           }
           else
           {
               pointInt_X = Mathf.CeilToInt(point.x);
           }

           diameterDoubler_X = pointInt_X / chunk_IntDiameter;

           pointPosInZero_X = point.x - ((float) diameterDoubler_X * chunk_diameter);

           if(pointPosInZero_X > 0)
           {
               if(pointPosInZero_X / chunk_diameter > 0.5f)
               {
                   diameterDoubler_X += 1;
               }
               else if(pointPosInZero_X / chunk_diameter == 0.5f)
               {
                   Debug.LogError("The point position in zero chunk divided by chunk diameter is equal to 0.5");
               }
           }
           else if(pointPosInZero_X < 0)
           {
               if(pointPosInZero_X / chunk_diameter < -0.5f)
               {
                   diameterDoubler_X -= 1;
               }
               else if(pointPosInZero_X / chunk_diameter == -0.5f)
               {
                   Debug.LogError("The point position in zero chunk divided by chunk diameter is equal to -0.5");
               }
           }

           int diameterDoubler_Y;

           int pointInt_Y;

           float pointPosInZero_Y;

           if(point.y >= 0)
           {
               pointInt_Y = Mathf.FloorToInt(point.y);
           }
           else
           {
               pointInt_Y = Mathf.CeilToInt(point.y);
           }

           diameterDoubler_Y = pointInt_Y / chunk_IntDiameter;

           pointPosInZero_Y = point.y - ((float) diameterDoubler_Y * chunk_diameter); 

           if(pointPosInZero_Y > 0)
           {
               if(pointPosInZero_Y / chunk_diameter > 0.5f)
               {
                   diameterDoubler_Y += 1;
               }
               else if(pointPosInZero_Y / chunk_diameter > 0.5f)
               {
                   Debug.LogError("The point position in zero chunk divided by chunk diameter is equal to 0.5");
               }
           }
           else if(pointPosInZero_Y < 0)
           {
               if(pointPosInZero_Y / chunk_diameter < -0.5f)
               {
                   diameterDoubler_Y -= 1;
               }
               else if(pointPosInZero_Y / chunk_diameter > 0.5f)
               {
                   Debug.LogError("The point position in zero chunk divided by chunk diameter is equal to -0.5");
               }
           }

           return new Vector2Int(diameterDoubler_X * chunk_IntDiameter, diameterDoubler_Y * chunk_IntDiameter);
       }

       Vector2Int GetCurIntChunkPos()
       {

           int chunkPosInt_x = GetCurChunkPosX();

           int chunkPosInt_y = GetCurChunkPosY();


           return new Vector2Int(chunkPosInt_x, chunkPosInt_y);
       }

       int GetCurChunkPosX()
       {
           int temp_x;
           if(cameraPosX >= 0)
           {
               temp_x = Mathf.FloorToInt(cameraPosX);
               temp_x = (temp_x / chunk_IntDiameter) * chunk_IntDiameter;
           }
           else
           {
               temp_x = Mathf.FloorToInt(cameraPosX) + 1;
               temp_x = (temp_x / chunk_IntDiameter) * chunk_IntDiameter;
           }

           return temp_x;
       }

       int GetCurChunkPosY()
       {
           int temp_y;

           if(cameraPosY >= 0)
           {
               temp_y = Mathf.FloorToInt(cameraPosY);
               temp_y = (temp_y / chunk_IntDiameter) * chunk_IntDiameter;
           }
           else
           {
               temp_y = Mathf.FloorToInt(cameraPosY) + 1;
               temp_y = (temp_y / chunk_IntDiameter) * chunk_IntDiameter;
           }

           return temp_y;

       }

       #endregion
  
       #region Bools 
       bool IsWorldEmpty()
       {
           if(world_chunks.Count == 0)
               return true;
           return false;   
       }

       bool IsChunk(Vector2Int position)
       {
            if(world_chunks.Count == 0)
                return false;
            for(int i = 0; i < world_chunks.Count; ++i)
            {
                if(world_chunks[i].center_pos == position)
                {
                    return true;
                }
            }
            return false;
       }

       bool IsExistInList(Chunk chunk)
       {
           for(int i = 0; i < world_chunks.Count; ++i)
           {
               if(chunk.center_pos == world_chunks[i].center_pos)
               {
                   return true;
               }
           }
           return false;
       }

       bool IsExistInList(int x, int y)
       {
           Vector2Int center_pos = new Vector2Int(x, y);
           for(int i = 0; i < world_chunks.Count; ++i)
           {
               if(center_pos == world_chunks[i].center_pos)
               {
                   return true;
               }
           }
           return false;
       }

       #endregion

       #region Camera
       void UpdateCamera()
       {
           cameraPosX = Camera.main.transform.position.x;
           cameraPosY = Camera.main.transform.position.y;
       }

       bool DoesCameraSee(Vector3 check_pos)
       {
           Vector3 cameraViewPos = Camera.main.WorldToViewportPoint(check_pos);

           if(cameraViewPos.x >= 0 && cameraViewPos.x <= 1 && cameraViewPos.y >= 0 && cameraViewPos.y <= 1)
           {
               return true;
           }
           return false;
       }

       #endregion

       public void UpdateChunkNumber()
       {
           chunkCount = world_chunks.Count;
       } 
    }
}
