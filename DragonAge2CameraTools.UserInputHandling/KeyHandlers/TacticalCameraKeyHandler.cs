using System.Linq;
using DragonAge2CameraTools.GameManagement.Factories.Interfaces;
using DragonAge2CameraTools.GameManagement.Interfaces;
using DragonAge2CameraTools.UserInputHandling.Enums;
using DragonAge2CameraTools.UserInputHandling.Factories.Interfaces;
using DragonAge2CameraTools.UserInputHandling.KeyHandlers.Data;
using DragonAge2CameraTools.UserInputHandling.KeyHandlers.Enums;
using DragonAge2CameraTools.UserInputHandling.KeyHandlers.Interfaces;

namespace DragonAge2CameraTools.UserInputHandling.KeyHandlers
{
    /// <summary>
    /// A key handler that manages all other key handlers, ensuring that they
    /// work properly and don't conflict with each other. Handlers are turned off
    /// when a loading screen is present
    /// </summary>
    public class TacticalCameraKeyHandler : IKeyHandler
    {
        private readonly IGameValueService _gameValueService;
        private readonly IKeyAndMouseEventHandlerFactory _keyAndMouseEventHandlerFactory;
        private readonly ILoadingScreenMonitorFactory _loadingScreenMonitorFactory;
        private readonly TacticalCameraSettings _tacticalCameraSettings;
        private ILoadingScreenMonitor _loadingScreenMonitor;
        private IKeyHandler[] _keyHandlers;
        
        public bool IsHandlerEnabled
        {
            get => _keyHandlers != null && _keyHandlers.Any(x => x.IsHandlerEnabled);
            set
            {
                if (_keyHandlers == null)
                {
                    return;
                }
                
                foreach (IKeyHandler keyHandler in _keyHandlers)
                {
                    keyHandler.IsHandlerEnabled = value;
                }
            }
        }

        public TacticalCameraKeyHandler(
            IKeyAndMouseEventHandlerFactory keyAndMouseEventHandlerFactory,
            ILoadingScreenMonitorFactory loadingScreenMonitorFactory,
            IGameValueService gameValueService, 
            TacticalCameraSettings tacticalCameraSettings)
        {
            _gameValueService = gameValueService;
            _keyAndMouseEventHandlerFactory = keyAndMouseEventHandlerFactory;
            _loadingScreenMonitorFactory = loadingScreenMonitorFactory;
            _tacticalCameraSettings = tacticalCameraSettings;
            
            CreateKeyAndMouseHandlers();
        }
        
        public InputResult OnKeyDown(UserInputKey keyCode, ModifierKeys modifiers)
        {
            if (_keyHandlers == null)
            {
                return InputResult.Continue;
            }

            foreach (IKeyHandler keyHandler in _keyHandlers)
            {
                InputResult inputResult = keyHandler.OnKeyDown(keyCode, modifiers);
                if (inputResult != InputResult.Continue)
                {
                    return inputResult;
                }
            }

            return InputResult.Continue;
        }
        
        public InputResult OnKeyUp(UserInputKey keyCode)
        {
            if (_keyHandlers == null)
            {
                return InputResult.Continue;
            }

            foreach (IKeyHandler keyHandler in _keyHandlers)
            {
                InputResult inputResult = keyHandler.OnKeyUp(keyCode);
                if (inputResult != InputResult.Continue)
                {
                    return inputResult;
                }
            }

            return InputResult.Continue;
        }

        public void StopCurrentlyRunningKeyFunction()
        {
            if (_keyHandlers != null)
            {
                foreach (IKeyHandler keyHandler in _keyHandlers)
                {
                    keyHandler.StopCurrentlyRunningKeyFunction();
                }
            }
        }
        
        private void CreateKeyAndMouseHandlers()
        {
            if (!(_tacticalCameraSettings.ManualTacticalCameraEnabled || _tacticalCameraSettings.AutomaticTacticalCameraEnabled))
            {
                _keyHandlers = null;
            }
            
            TacticalCameraKeyBindings keyBindings = _tacticalCameraSettings.TacticalCameraKeyBindings;

            IAutoTacticalCameraKeyHandler autoTacticalCameraHandler = null;
            IManualTacticalCameraKeyHandler manualTacticalCameraHandler = null;
            IKeyHandler cameraMovementHandler = CreateCameraMovementHandler(keyBindings);
            IKeyHandler cameraHeightHandler = CreateCameraHeightHandler(keyBindings);

            if (_tacticalCameraSettings.ManualTacticalCameraEnabled)
            {
                manualTacticalCameraHandler = CreateManualTacticalCameraHandler(keyBindings);
            }

            if (_tacticalCameraSettings.AutomaticTacticalCameraEnabled)
            {
                autoTacticalCameraHandler = CreateAutoTacticalCameraHandler(keyBindings);
            }

            if (manualTacticalCameraHandler != null)
            {
                AssignCameraStateHandlerRelations(manualTacticalCameraHandler, autoTacticalCameraHandler, cameraMovementHandler, cameraHeightHandler);
            }

            if (autoTacticalCameraHandler != null)
            {
                AssignCameraStateHandlerRelations(autoTacticalCameraHandler, manualTacticalCameraHandler, cameraMovementHandler, cameraHeightHandler);
            }

            InitializeLoadingScreenMonitor(manualTacticalCameraHandler, autoTacticalCameraHandler, cameraMovementHandler, cameraHeightHandler);
            
            _keyHandlers = new[]
            {
                autoTacticalCameraHandler,
                manualTacticalCameraHandler,
                cameraMovementHandler,
                cameraHeightHandler,
            }
            .Where(keyHandler => keyHandler != null).ToArray();
        }

        private IKeyHandler CreateCameraMovementHandler(TacticalCameraKeyBindings keyBindings)
        {
            return _keyAndMouseEventHandlerFactory.CreateCameraMovementHandler
            (
                _gameValueService,
                new CameraMovementKeys
                {
                    CameraForwardKeys = keyBindings.CameraForwardKeys,
                    CameraBackKeys = keyBindings.CameraBackKeys,
                    CameraLeftKeys = keyBindings.CameraLeftKeys,
                    CameraRightKeys = keyBindings.CameraRightKeys,
                },
                _tacticalCameraSettings.HorizontalCameraSpeed
            );
        }

        private IKeyHandler CreateCameraHeightHandler(TacticalCameraKeyBindings keyBindings)
        {
            return _keyAndMouseEventHandlerFactory.CreateCameraHeightHandler
            (
                _gameValueService,
                new CameraHeightKeys
                {
                    CameraDownKeys = keyBindings.CameraDownKeys, 
                    CameraUpKeys = keyBindings.CameraUpKeys
                }, 
                _tacticalCameraSettings.VerticalCameraSpeed);
        }

        private IManualTacticalCameraKeyHandler CreateManualTacticalCameraHandler(TacticalCameraKeyBindings keyBindings)
        {
            return  _keyAndMouseEventHandlerFactory.CreateManualTacticalCameraHandler(
                _gameValueService,
                keyBindings.TacticalCameraToggleKeys
            );
        }

        private IAutoTacticalCameraKeyHandler CreateAutoTacticalCameraHandler(TacticalCameraKeyBindings keyBindings)
        {
            return _keyAndMouseEventHandlerFactory.CreateAutoTacticalCameraHandler(_gameValueService,
                new AutoTacticalCameraKeys
                {
                    ZoomInKeys = keyBindings.ZoomInKeys,
                    ZoomOutKeys = keyBindings.ZoomOutKeys,
                    ToggleKeys = keyBindings.TacticalCameraToggleKeys
                }, 
                _tacticalCameraSettings.AutomaticTacticalCameraThreshold);
        }

        /// <summary>
        /// Ensures that when the tactical camera state handler is enabled all related handlers
        /// will also be enabled, while the conflicting handler will be disabled.
        /// For instance, both manual and automatic tactical camera handlers require camera movement handler and
        /// camera height handler to be enabled. On the other hand you cannot have both manual and
        /// automatic camera handlers enabled, so when one is enabled the other one must be disabled
        /// </summary>
        /// <param name="tacticalCameraStateHandler">Handler for which we are handling the TacticalCameraStateChanged event</param>
        /// <param name="conflictingHandler">(optional) Handler that needs to be disabled while tacticalCameraStateHandler is enabled</param>
        /// <param name="relatedHandlers">Handlers that need to be enabled while tacticalCameraStateHandler is enabled</param>
        private static void AssignCameraStateHandlerRelations(
                ITacticalCameraStateHandler tacticalCameraStateHandler, 
                IKeyHandler conflictingHandler,
                params IKeyHandler[] relatedHandlers)
        {
            tacticalCameraStateHandler.TacticalCameraStateChanged += enabled =>
            {
                foreach (IKeyHandler relatedHandler in relatedHandlers)
                {
                    relatedHandler.IsHandlerEnabled = enabled;
                }

                if (conflictingHandler != null)
                {
                    conflictingHandler.IsHandlerEnabled = !enabled;
                }
            };
        }

        private void InitializeLoadingScreenMonitor(
            IManualTacticalCameraKeyHandler manualTacticalCameraHandler, 
            IAutoTacticalCameraKeyHandler autoTacticalCameraHandler,
            IKeyHandler cameraMovementHandler,
            IKeyHandler cameraHeightHandler)
        {
            _loadingScreenMonitor = _loadingScreenMonitorFactory.CreateLoadingScreenMonitor(_gameValueService);
            _loadingScreenMonitor.EnteredLoadingScreen += (sender, eventArgs) =>
            {
                if (manualTacticalCameraHandler != null)
                {
                    manualTacticalCameraHandler.IsHandlerEnabled = false;
                }

                if (autoTacticalCameraHandler != null)
                {
                    autoTacticalCameraHandler.IsHandlerEnabled = false;
                }
                cameraMovementHandler.IsHandlerEnabled = false;
                cameraHeightHandler.IsHandlerEnabled = false;
            };
            
            _loadingScreenMonitor.ExitLoadingScreen += (sender, eventArgs) =>
            {
                if (manualTacticalCameraHandler != null)
                {
                    manualTacticalCameraHandler.IsHandlerEnabled = true;
                }

                if (autoTacticalCameraHandler != null)
                {
                    autoTacticalCameraHandler.IsHandlerEnabled = true;
                }
            };

            _loadingScreenMonitor.StartMonitoringLoadingScreen();
        }

        public void Dispose()
        {
            if (_loadingScreenMonitor != null)
            {
                _loadingScreenMonitor.Dispose();
                _loadingScreenMonitor = null;
            }
            
            if (_keyHandlers == null)
            {
                return;
            }
            
            foreach (IKeyHandler keyHandler in _keyHandlers)
            {
                keyHandler.Dispose();
            }

            _keyHandlers = null;
        }
    }
}