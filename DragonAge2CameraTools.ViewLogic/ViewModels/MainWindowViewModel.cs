using System;
using DragonAge2CameraTools.UserInputHandling.Data;
using DragonAge2CameraTools.UserInputHandling.Enums;
using DragonAge2CameraTools.UserInputHandling.Interfaces;
using DragonAge2CameraTools.UserInputHandling.KeyHandlers.Data;
using DragonAge2CameraTools.ViewLogic.Commands;
using DragonAge2CameraTools.ViewLogic.Data;
using DragonAge2CameraTools.ViewLogic.Factories.Interfaces;
using DragonAge2CameraTools.ViewLogic.Interfaces;
using DragonAge2CameraTools.ViewLogic.Settings.Data;
using DragonAge2CameraTools.ViewLogic.Settings.Interfaces;
using DragonAge2CameraTools.ViewLogic.ViewModels.Base;

namespace DragonAge2CameraTools.ViewLogic.ViewModels
{
    public class MainWindowViewModel : ViewModelBase, IDisposable
    {
        private float _automaticCameraThresholdMinValue;
        private float _automaticCameraThresholdMaxValue = 15;
        private float _automaticCameraThresholdValue = 1;
        private bool _unlimitedZoomEnabled;
        private bool _manualTacticalCameraEnabled;
        private bool _automaticTacticalCameraEnabled;
        private float _horizontalCameraSpeed;
        private float _verticalCameraSpeed;
        private ViewModelKeyBindings _keyBindings;
        private GameProcessStatus _gameProcessStatus;

        private readonly IKeyAwaiter _keyAwaiter;
        private readonly IApplicationSettingsRepository _applicationSettingsRepository;
        private readonly ISettingsMapper _settingsMapper;
        private readonly ITacticalCameraService _tacticalCameraService;

        public MainWindowViewModel
        (
            IKeyAwaiter keyAwaiter, 
            IApplicationSettingsRepository applicationSettingsRepository, 
            ISettingsMapper settingsMapper, 
            ITacticalCameraServiceFactory tacticalCameraServiceFactory
        )
        {
            _keyAwaiter = keyAwaiter;
            _applicationSettingsRepository = applicationSettingsRepository;
            _settingsMapper = settingsMapper;

            ApplicationSettings settings = GetApplicationSettings();
            RestoreSettings(settings);

            TacticalCameraSettings tacticalCameraSettings = _settingsMapper.MapToTacticalCameraSettings(settings);
            _tacticalCameraService = tacticalCameraServiceFactory.CreateTacticalCameraService(tacticalCameraSettings);
            _tacticalCameraService.ProcessStatusChanged += OnTacticalCameraServiceOnProcessStatusChanged;
            _tacticalCameraService.WaitAndAttachToProcess();
        }

        public ViewModelKeyBindings KeyBindings
        {
            get => _keyBindings;
            private set => SetProperty(ref _keyBindings, value);
        }

        public float AutomaticCameraThresholdMinValue
        {
            get => _automaticCameraThresholdMinValue;
            set => _automaticCameraThresholdMinValue = value;
        }

        public float AutomaticCameraThresholdMaxValue
        {
            get => _automaticCameraThresholdMaxValue;
            set => SetProperty(ref _automaticCameraThresholdMaxValue, value);
        }

        public float AutomaticCameraThresholdValue
        {
            get => _automaticCameraThresholdValue;
            set => SetProperty(ref _automaticCameraThresholdValue, value);
        }

        public float HorizontalCameraSpeed
        {
            get => _horizontalCameraSpeed;
            set => SetProperty(ref _horizontalCameraSpeed, value);
        }
        
        public float VerticalCameraSpeed
        {
            get => _verticalCameraSpeed;
            set => SetProperty(ref _verticalCameraSpeed, value);
        }

        public bool UnlimitedZoomEnabled
        {
            get => _unlimitedZoomEnabled;
            set
            {
                if (!value)
                {
                    AutomaticCameraThresholdMaxValue = 1f;
                    if (_automaticCameraThresholdValue > _automaticCameraThresholdMaxValue)
                    {
                        AutomaticCameraThresholdValue = _automaticCameraThresholdMaxValue;
                    }
                }
                else
                {
                    AutomaticCameraThresholdMaxValue = 15f;
                }
                SetProperty(ref _unlimitedZoomEnabled, value);
            }
        }

        public bool ManualTacticalCameraEnabled
        {
            get => _manualTacticalCameraEnabled;
            set => SetProperty(ref _manualTacticalCameraEnabled, value);
        }

        public bool AutomaticTacticalCameraEnabled
        {
            get => _automaticTacticalCameraEnabled;
            set => SetProperty(ref _automaticTacticalCameraEnabled, value);
        }

        public GameProcessStatus GameProcessStatus
        {
            get => _gameProcessStatus;
            set => SetProperty(ref _gameProcessStatus, value);
        }

        public RelayCommand BindKeysCommand => new RelayCommand(async parameter =>
        {
            var bindingTarget = parameter as KeyBindingViewmodel;

            if (bindingTarget == null)
            {
                return;
            }

            bindingTarget.IsInBinding = true;
            UserInputKey pressedKey = await _keyAwaiter.WaitForKeyPress(true);

            if (pressedKey == UserInputKey.Escape)
            {
                bindingTarget.Key = null;
                bindingTarget.IsInBinding = false;
                return;
            }

            bindingTarget.Key = pressedKey;
            bindingTarget.IsInBinding = false;
        });
        
        public RelayCommand SaveSettingsCommand => new RelayCommand(_ =>
        {
            var settings = new ApplicationSettings
            {
                KeyBindingSettings = _settingsMapper.MapToKeyBindingSettings(KeyBindings),
                ManualTacticalCameraEnabled = _manualTacticalCameraEnabled,
                AutomaticTacticalCameraEnabled = _automaticTacticalCameraEnabled,
                AutomaticCameraThresholdValue = _automaticCameraThresholdValue,
                UnlimitedZoomEnabled = _unlimitedZoomEnabled,
                HorizontalCameraSpeed = _horizontalCameraSpeed,
                VerticalCameraSpeed = _verticalCameraSpeed
            };
            
            _applicationSettingsRepository.SaveApplicationSettings(settings);
            TacticalCameraSettings tacticalCameraSettings = _settingsMapper.MapToTacticalCameraSettings(settings);

            if (_gameProcessStatus == GameProcessStatus.Attached)
            {
                _tacticalCameraService.UpdateSettings(tacticalCameraSettings);
            }
        });
        
        public RelayCommand RestoreSavedSettingsCommand => new RelayCommand(_ =>
        {
            ApplicationSettings settings = GetApplicationSettings();
            RestoreSettings(settings);
        });

        private ApplicationSettings GetApplicationSettings()
        {
            ApplicationSettings settings = _applicationSettingsRepository.GetApplicationSettings();
            if (settings == null)
            {
                settings = _applicationSettingsRepository.GetDefaultSettings();
                _applicationSettingsRepository.SaveApplicationSettings(settings);
            }

            return settings;
        }
        
        private void RestoreSettings(ApplicationSettings settings)
        {
            KeyBindings = _settingsMapper.MapToViewModelKeyBindings(settings.KeyBindingSettings);
            UnlimitedZoomEnabled = settings.UnlimitedZoomEnabled;
            ManualTacticalCameraEnabled = settings.ManualTacticalCameraEnabled;
            AutomaticTacticalCameraEnabled = settings.AutomaticTacticalCameraEnabled;
            AutomaticCameraThresholdValue = settings.AutomaticCameraThresholdValue;
            HorizontalCameraSpeed = settings.HorizontalCameraSpeed;
            VerticalCameraSpeed = settings.VerticalCameraSpeed;
        }
        
        private void OnTacticalCameraServiceOnProcessStatusChanged(object sender, ProcessStatusChangeEventArgs e)
        {
            GameProcessStatus = e.NewStatus;
            if (e.NewStatus == GameProcessStatus.Attached)
            {
                _tacticalCameraService.EnableTacticalCameraTriggers();
            }
            else if (e.NewStatus == GameProcessStatus.ProcessNotAvailable)
            {
                _tacticalCameraService.WaitAndAttachToProcess();
            }
        }

        public void Dispose()
        {
            _tacticalCameraService?.Dispose();
        }
    }
}