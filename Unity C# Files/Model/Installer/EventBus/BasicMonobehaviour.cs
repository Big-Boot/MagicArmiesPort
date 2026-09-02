using GameLogic.Scripts.Decoupling;
using GameLogic.Scripts.EventBus;
using UnityEngine;

public class BasicMonobehaviour : MonoBehaviour
{

    protected ServiceLocator _serviceLocator;
    protected ServiceLocator serviceLocator
    {
        get
        {
            if (_serviceLocator == null)
            {
                _serviceLocator = ServiceLocator.Instance;
            }
            return _serviceLocator;
        }
    }
    protected IEventBus eventBus { 
        get
        {
            if (_eventBus == null)
            {
                _eventBus = serviceLocator.GetService<IEventBus>();
            }
            return _eventBus;
        }
    }
    IEventBus _eventBus;
}