using UnityEngine;

namespace Features.AbilitySystem
{
    internal interface IAbilityActivator
    {
        GameObject ViewGameObject { get; }
        float JumpHeight {get;}
    }
}
