using System;
using UnityEngine;
using System.Collections;

public interface IActionable
{
    event Action OnActionTaken;
}
