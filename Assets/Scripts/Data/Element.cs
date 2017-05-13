using UnityEngine;
using System.Collections;
using System.Runtime.Serialization;

public class Element
{
    /// <summary>
    /// just a id
    /// </summary>
    public int id;
    /// <summary>
    /// just a name
    /// </summary>
    public string name;
    /// <summary>
    /// gas solid liquid etc.
    /// </summary>
    public int state;
    /// <summary>
    /// to write
    /// </summary>
    public int color;
    /// <summary>
    /// can ion in some liquid?
    /// </summary>
    public int ionization;
    /// <summary>
    /// can solute in water?
    /// </summary>
    public int solubility;
    public float amount;
    public float count;

    public Element Copy()
    {
        return new Element()
        {
            id = id,
            name = name,
            state = state,
            color = color,
            ionization = ionization,
            solubility = solubility,
            amount = amount,
            count = count,
        };
    }
}
