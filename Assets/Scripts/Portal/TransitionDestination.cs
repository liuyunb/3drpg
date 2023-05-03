using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionDestination : MonoBehaviour
{
    public enum TransitionType { Enter,A,B,C,Home,HomeEnter};

    public TransitionType destination;
}
