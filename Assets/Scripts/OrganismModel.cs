using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum OrganismType
{
    Green,
    Red,
    Blue
}

public class OrganismModel : MonoBehaviour
{
    public OrganismType type;

    public bool CanEatType(OrganismType otherType)
    {
        return (type == OrganismType.Green && otherType == OrganismType.Blue)
             || (type == OrganismType.Red && otherType == OrganismType.Green)
             || (type == OrganismType.Blue && otherType == OrganismType.Red);
    }

}
