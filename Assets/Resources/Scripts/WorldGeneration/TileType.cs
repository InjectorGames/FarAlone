using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InjectorGames.FarAlone.TileManager
{
    public enum TileType
    {
        Error = -1,
        
        Grass, 

        Grass_Island, //grassBottom_Top_double

        GrassBottom,
        GrassBottom_Left,
        GrassBottom_Right,
        GrassBottom_Double,

        GrassTop,
        GrassTop_Left,
        GrassTop_Right,
        GrassTop_Closed, // grass Top double

        GrassBottom_Top,
        GrassBottom_Top_Left,
        GrassBottom_Top_Right,

        GrassTop_Trans_Left,
        GrassTop_Trans_Right,
        GrassTop_Trans_Double,

        GrassBottom_Top_Trans_Left,
        GrassBottom_Top_Trans_Right,
        GrassBottom_Top_Trans_Double,

        GrassBottom_Top_Right_Trans,
        GrassBottom_Top_Left_Trans,

        Water,

        Bridge_Horizontal,
        Bridge_Horizontal_Left,
        Bridge_Horizontal_Right,

        Bridge_Vertical,
        Bridge_Vertical_Top,
        Bridge_Vertical_Bottom,

        Rock
    }
}
