using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items
{
    public class Detail : Item
    {
      public override ItemType Type => ItemType.Detail;

      public Detail()
      {
        name = "NAME";
        weight = 0.0f;

      }

      public Detail(string arg_name, float arg_weight)
      {
          name = arg_name;
          weight = arg_weight;
      }

    
    }
}
