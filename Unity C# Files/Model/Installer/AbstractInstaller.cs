using GameLogic.Scripts.Decoupling;
using UnityEngine;

public abstract class AbstractInstaller : MonoBehaviour
{
    #region Public Methods

    public abstract void Install(ServiceLocator serviceLocator);

    #endregion Public Methods
}