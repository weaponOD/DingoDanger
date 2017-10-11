using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zone : MonoBehaviour
{
    private enum Tier { MINOR, MODERATE, ELITE }

    private enum TYPE { SCARCE }

    [Header("Zone properties")]
    [SerializeField]
    private Tier tier = Tier.MINOR;

    [SerializeField]
    private TYPE zoneType = TYPE.SCARCE;

    [Header("Zone contents")]
    [SerializeField]
    private bool hasDock = false;

    [SerializeField]
    private bool hasTower = false;
}
