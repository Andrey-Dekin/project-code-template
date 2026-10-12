
namespace WinterDrift.Features.PlayerFacade
{
    using System.Collections;
    using System.Collections.Generic;
    using DekDev.Extensions.Facade;
    using UnityEngine;

    /// <summary>
    /// Container for player facade.
    /// </summary>
    [CreateAssetMenu(fileName = nameof(PlayerFacadeContainer), menuName = "WinterDrift/Features/PlayerFacade/" + nameof(PlayerFacadeContainer))]
    public class PlayerFacadeContainer : AbstractFacadeContainer<PlayerFacade>
    {

    }
}
