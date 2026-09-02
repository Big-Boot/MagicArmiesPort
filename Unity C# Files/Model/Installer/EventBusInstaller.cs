using GameLogic.Scripts.Decoupling;
using GameLogic.Scripts.EventBus;

public class EventBusInstaller : AbstractInstaller
{
    #region Public Methods

    public override void Install(ServiceLocator serviceLocator)
    {
        serviceLocator.RegisterService<IEventBus>(new GameEventBus());
    }

    #endregion Public Methods
}