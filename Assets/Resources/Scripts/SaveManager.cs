using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using InjectorGames.FarAlone.WorldGeneration;
using InjectorGames.FarAlone.TileManager;

namespace InjectorGames.FarAlone.FileManager
{
    [System.Serializable]
    public class WorldData
    {
        //public int loadInfo;
        public bool loadInfo;

        public WorldData(World world)
        {
            loadInfo = world.IsLoad;
        }
    }

    [System.Serializable]
    public struct Serializable_Vector2Int
    {
        public int x;
        public int y;

        public Vector2Int GetPos()
        {
            return new Vector2Int(x, y);
        }

        public Serializable_Vector2Int(Vector2Int pos)
        {
            x = pos.x;
            y = pos.y;
        }


    }

    [System.Serializable]
    public class Serializable_Chunk
    {
        public ChunkType ser_type;
        public Biome ser_chunk_biome;
        public Serializable_Vector2Int ser_center_pos;

        public TileType[ , ] ser_terrain_heatMap;

        public Serializable_Chunk(Chunk chunk)
        {
            ser_type = chunk.type;
            ser_chunk_biome = chunk.chunk_biome;
            ser_terrain_heatMap = chunk.Terrain_heatMap;

            ser_center_pos = new Serializable_Vector2Int(chunk.center_pos);

        }

        public Chunk GetChunk()
        {
            return new Chunk(ser_center_pos.GetPos(), ser_chunk_biome, ser_type, ser_terrain_heatMap);
        }

    }


    public class SaveManager : MonoBehaviour
    {

        static World world;

        #region WorldLoad

        public static void Set_WorldLoad()
        {
            Debug.Log("Saving the world load data!");

            FileStream file = new FileStream(Application.persistentDataPath + "/LoadInfo.dat", FileMode.Create);

            try
            {
                if(world == null)
                {
                    world = FindObjectOfType(typeof(World)) as World;
                }

                BinaryFormatter formatter = new BinaryFormatter();

                WorldData data = new WorldData(world);

                formatter.Serialize(file, data);

                Debug.Log("The world data has succesfully serialized!");
            }
            catch (SerializationException exc)
            {
                Debug.LogError("There was an issue with serializing World's load info: " + exc.Message);
            }
            finally
            {
                file.Close();
            }
        }

        public static bool Check_WorldLoad()
        {
            Debug.Log("Loading the world load data!");

            WorldData data;

            if(File.Exists(Application.persistentDataPath + "/LoadInfo.dat"))
            {
                FileStream file = new FileStream(Application.persistentDataPath + "/LoadInfo.dat", FileMode.Open);
                try
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    data = formatter.Deserialize(file) as WorldData;

                    return data.loadInfo;
                }
                catch (SerializationException exc)
                {
                    Debug.LogError("Error with loading the world load data: " + exc.Message);
                    return false;
                    //return 0;
                }
                finally
                {
                    file.Close();
                }

            }
            else
            {
                Debug.Log("Loading file doesn't exist!");
                return false;
                //return 0;
            }
        }

        #endregion

        #region ChunkSerialization

        public static void Save_Chunk(Chunk serialize_chunk)
        {
            Debug.Log("Saving a chunk with world position: " + serialize_chunk.center_pos.x + "." + serialize_chunk.center_pos.y);

            string chunks_path = Application.persistentDataPath + "/Chunks";

            FileStream file; 

            if(serialize_chunk.type == ChunkType.Static)
            {
                file = new FileStream(chunks_path + "/Static/chk_" + serialize_chunk.center_pos.x + "_" + serialize_chunk.center_pos.y + ".chnk", 
                FileMode.OpenOrCreate);
            }
            else
            {
                file = new FileStream(chunks_path + "/Dynamic/chk_" + serialize_chunk.center_pos.x + "_" + serialize_chunk.center_pos.y + ".chnk",
                FileMode.OpenOrCreate);
                
            }

            try
            {
                BinaryFormatter formatter = new BinaryFormatter();

                Serializable_Chunk ser_chunk_data = new Serializable_Chunk(serialize_chunk);

                formatter.Serialize(file, ser_chunk_data);

                Debug.Log("Chunk with world position: " + serialize_chunk.center_pos.x + "." + serialize_chunk.center_pos.y + " has been succesfully saved");
            }
            catch(SerializationException exc)
            {
                Debug.LogError("Error in saving chunk with world position: " + serialize_chunk.center_pos.x + "." + serialize_chunk.center_pos.y + " :" + exc.Message);
            }
            finally
            {
                file.Close();
            }



        }

        public static Chunk Load_Chunk(int chunk_x, int chunk_y, ChunkType type)
        {
            Debug.Log("Loading a chunk with position: " + chunk_x + "." + chunk_y);

            string chunks_path = Application.persistentDataPath + "/Chunks";

            Serializable_Chunk ser_chunk;

            FileStream file;

            if(type == ChunkType.Static)
            {
                file = new FileStream(chunks_path + "/Static/chk_" + chunk_x + "_" + chunk_y + ".chnk", FileMode.Open);
            }
            else
            {
                file = new FileStream(chunks_path + "/Dynamic/chk_" + chunk_x + "_" + chunk_y + ".chnk", FileMode.Open);
            }

            try
            {
                BinaryFormatter formatter = new BinaryFormatter();

                ser_chunk = formatter.Deserialize(file) as Serializable_Chunk;

                return ser_chunk.GetChunk();
            }
            catch(SerializationException exc)
            {
                Debug.LogError("Error in loading chunk with world position: " + chunk_x + "." + chunk_y + " :" + exc.Message);

                return new Chunk(new Vector2Int(0, 0), Biome.Forest, ChunkType.Static, new TileType[world.IntDiameter, world.IntDiameter]);
            }
            finally
            {
                file.Close();
            }
        }

        #region Bools

        public static bool IsChunk_Exist(int chunk_x, int chunk_y)
        {
            string chunk_path = Application.persistentDataPath + "/Chunks";

            if(File.Exists(chunk_path + "/Static/chk_" + chunk_x + "_" + chunk_y + ".chnk") ||
            File.Exists(chunk_path + "/Dynamic/chk_" + chunk_x + "_" + chunk_y + ".chnk"))
            {
                return true;
            }
            else
            {
                return false;
            }
                
        }

        public static bool IsChunk_Static(int chunk_x, int chunk_y)
        {
            if(File.Exists(Application.persistentDataPath + "/Chunks/Static/chk_" + chunk_x + "_" + chunk_y + ".chnk"))
            {
                return true;
            }
            return false;
        }

        #endregion
        #endregion
    
    
    
    }

}
