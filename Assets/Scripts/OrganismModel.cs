using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum OrganismType
{
    Green,
    Red,
    Blue
}

// green - virginia creeper
// red - butterfly weed
// blue - long leaf pine

public class OrganismModel : MonoBehaviour
{
    public OrganismType type;

    public float weatherModifier(OrganismType weather)
    {
      if (weather == 'sunny')
      {
        if (type == OrganismType.Red)
        {
          return 0.03f;
        }
        else if (type == OrganismType.Blue)
        {
          return 0.02f;
        }
        else if (type == OrganismType.Green)
        {
          return 0.01f;
        }
      }

      if (weather == 'cloudy')
      {
        return -0.02f;
      }

      if (weather == 'rainy')
      {
        if (type == OrganismType.Blue)
        {
          return 0.03f;
        }
        else if (type == OrganismType.Green)
        {
          return 0.02f;
        }
        else if (type == OrganismType.Red)
        {
          return 0.01f;
        }
      }

      if (weather == 'frost')
      {
        if (type == OrganismType.Blue)
        {
          return 0.0;
        }

        return -0.05f;
      }
    }

    public bool CanEatType(OrganismType otherType)
    {
        return (type == OrganismType.Green && otherType == OrganismType.Blue)
             || (type == OrganismType.Red && otherType == OrganismType.Green)
             || (type == OrganismType.Blue && otherType == OrganismType.Red);
    }

    public float reproductionProbability(bool isWatering)
    {
      if (isWatering == true)
      {
          return 0.30f + weatherModifier;
      }

      return 0.05f + weatherModifier;
    }
}
