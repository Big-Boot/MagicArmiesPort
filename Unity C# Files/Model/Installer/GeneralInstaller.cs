using GameLogic.Scripts.Decoupling;
using UnityEngine;

[DefaultExecutionOrder(-500)]
public abstract class GeneralInstaller : MonoBehaviour
{
    #region Private Fields

    [SerializeField] private AbstractInstaller[] _installers;

    #endregion Private Fields

    #region Protected Methods

    private void OnValidate()
    {
        _installers = GetComponentsInChildren<AbstractInstaller>();
    }

    protected abstract void DoInstallDependencies();

    protected abstract void DoStart();

    #endregion Protected Methods

    #region Private Methods

    private void Awake()
    {
        InstallDependencies();
    }

    private void InstallDependencies()
    {
        foreach (var installer in _installers) installer.Install(ServiceLocator.Instance);
        DoInstallDependencies();
    }

    private void Start()
    {
        DoStart();
    }

    #endregion Private Methods
}