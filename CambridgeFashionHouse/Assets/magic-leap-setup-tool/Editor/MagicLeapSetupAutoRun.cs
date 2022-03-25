#region

using System;
using MagicLeapSetupTool.Editor.Setup;
using MagicLeapSetupTool.Editor.Utilities;
using UnityEditor;
using UnityEngine;

#endregion

namespace MagicLeapSetupTool.Editor
{
    /// <summary>
    /// Manages the apply all action and runs through all of the configuration steps.
    /// </summary>
    public static class MagicLeapSetupAutoRun
    {
        #region EDITOR PREFS

        private const string MAGICLEAP_AUTO_SETUP_PREF = "MAGICLEAP-AUTO-SETUP";

        #endregion

        #region DEBUG TEXT

        private const string CHANGING_BUILD_PLATFORM_DEBUG = "Setting Build Platform To Lumin...";
        private const string INSTALLING_LUMIN_SDK_DEBUG = "Installing Magic Leap Plug-in...";
        private const string ENABLING_LUMIN_SDK_DEBUG = "Enabling Magic Leap Plug-in...";
        private const string UPDATING_MANIFEST_DEBUG = "Updating Magic Leap Manifest...";
        private const string IMPORTING_LUMIN_UNITYPACKAGE_DEBUG = "Importing Magic Leap UnityPackage...";
        private const string UPDATING_COLORSPACE_DEBUG = "Changing Color Space to Recommended Setting [Linear]...";

        private const string CHANGING_GRAPHICS_API_DEBUG = "Updating Graphics API To Include [OpenGLCore] (Auto Api = false)...";

        #endregion

        #region TEXT AND LABELS

        internal const string APPLY_ALL_PROMPT_TITLE = "Configure all settings";

        internal const string APPLY_ALL_PROMPT_MESSAGE = "This will update the project to the recommended settings for Magic leap EXCEPT FOR SETTING A DEVELOPMENT CERTIFICATE. Would you like to continue?";

        internal const string APPLY_ALL_PROMPT_OK = "Continue";
        internal const string APPLY_ALL_PROMPT_CANCEL = "Cancel";
        internal const string APPLY_ALL_PROMPT_ALT = "Setup Development Certificate";

        internal const string APPLY_ALL_PROMPT_NOTHING_TO_DO_MESSAGE = "All settings are configured. There is no need to run utility";

        internal const string APPLY_ALL_PROMPT_NOTHING_TO_DO_OK = "Close";

        internal const string APPLY_ALL_PROMPT_MISSING_CERT_MESSAGE = "All settings are configured except the developer certificate. Would you like to set it now?";

        internal const string APPLY_ALL_PROMPT_MISSING_CERT_OK = "Set Certificate";
        internal const string APPLY_ALL_PROMPT_MISSING_CERT_CANCEL = "Cancel";

        #endregion

        #region ENUMS

        internal enum ApplyAllState
        {
            SetSdkPath,
            SwitchBuildTarget,
            InstallLumin,
            EnableXrPackage,
            UpdateManifest,
            ChangeColorSpace,
            ImportSdkUnityPackage,
            ChangeGraphicsApi,
            Done
        }

        #endregion

        private static ApplyAllState _currentApplyAllState = ApplyAllState.Done;

        internal static bool _allAutoStepsComplete =>
            UpdateGraphicsApiSetupStep.HasCorrectGraphicConfiguration
            && PlayerSettings.colorSpace == ColorSpace.Linear
            && ImportMagicLeapSdkSetupStep.HasMagicLeapSdkInstalled
            && UpdateManifestSetupStep.ManifestIsUpdated
            && SetupData.HasRootSDKPath
            && EnablePluginSetupStep.LuminSettingEnabled
            && HasLuminInstalled
            && ImportMagicLeapSdkSetupStep.HasCompatibleMagicLeapSdk
            && BuildTargetSetupStep.CorrectBuildTarget;

        private static SetSdkFolderSetupStep _setSdkFolderSetupStep = new SetSdkFolderSetupStep();
        private static BuildTargetSetupStep _buildTargetSetupStep = new BuildTargetSetupStep();
        private static EnablePluginSetupStep _enablePluginSetupStep = new EnablePluginSetupStep();
        private static UpdateManifestSetupStep _updateManifestSetupStep = new UpdateManifestSetupStep();
        private static SetCertificateSetupStep _setCertificateSetupStep = new SetCertificateSetupStep();
        private static ImportMagicLeapSdkSetupStep _importMagicLeapSdkSetupStep = new ImportMagicLeapSdkSetupStep();
        private static ColorSpaceSetupStep _colorSpaceSetupStep = new ColorSpaceSetupStep();
        private static UpdateGraphicsApiSetupStep _updateGraphicsApiSetupStep = new UpdateGraphicsApiSetupStep();
        public static bool HasLuminInstalled
        {
            get
            {
#if MAGICLEAP
                return true;
#else
                return  false;
#endif
            }
        }

        internal static ApplyAllState CurrentApplyAllState
        {
            get => _currentApplyAllState;
            set
            {
                EditorPrefs.SetString(MAGICLEAP_AUTO_SETUP_PREF, value.ToString());
                _currentApplyAllState = value;
            }
        }

        public static void CheckLastAutoSetupState()
        {
            if (Enum.TryParse(EditorPrefs.GetString(MAGICLEAP_AUTO_SETUP_PREF), true, out ApplyAllState value))
                CurrentApplyAllState = value;
            else
                _currentApplyAllState = ApplyAllState.Done;
        }

        internal static void Stop()
        {
            CurrentApplyAllState = ApplyAllState.Done;
        }

        internal static void RunApplyAll()
        {
            if (!_allAutoStepsComplete)
            {
                var dialogComplex = EditorUtility.DisplayDialogComplex(APPLY_ALL_PROMPT_TITLE, APPLY_ALL_PROMPT_MESSAGE,
                    APPLY_ALL_PROMPT_OK, APPLY_ALL_PROMPT_CANCEL, APPLY_ALL_PROMPT_ALT);

                switch (dialogComplex)
                {
                    case 0: //Continue
                        CurrentApplyAllState = ApplyAllState.SwitchBuildTarget;
                        break;
                    case 1: //Stop
                        CurrentApplyAllState = ApplyAllState.Done;
                        break;
                    case 2: //Go to documentation
                        Help.BrowseURL(MagicLeapSetupWindow.Get_CERTIFICATE_URL);
                        CurrentApplyAllState = ApplyAllState.Done;
                        break;
                }
            }
            else if (!SetCertificateSetupStep.ValidCertificatePath)
            {
                var dialogComplex = EditorUtility.DisplayDialogComplex(APPLY_ALL_PROMPT_TITLE,
                    APPLY_ALL_PROMPT_MISSING_CERT_MESSAGE,
                    APPLY_ALL_PROMPT_MISSING_CERT_OK, APPLY_ALL_PROMPT_MISSING_CERT_CANCEL, APPLY_ALL_PROMPT_ALT);

                switch (dialogComplex)
                {
                    case 0: //Continue
                        _setCertificateSetupStep.Execute();
                        break;
                    case 1: //Stop
                        CurrentApplyAllState = ApplyAllState.Done;
                        break;
                    case 2: //Go to documentation
                        Help.BrowseURL(MagicLeapSetupWindow.SETUP_ENVIRONMENT_URL);
                        CurrentApplyAllState = ApplyAllState.Done;
                        break;
                }
            }
            else if (SetCertificateSetupStep.ValidCertificatePath)
            {
                EditorUtility.DisplayDialog(APPLY_ALL_PROMPT_TITLE, APPLY_ALL_PROMPT_NOTHING_TO_DO_MESSAGE,
                    APPLY_ALL_PROMPT_NOTHING_TO_DO_OK);
            }
        }


        internal static void Tick()
        {
            var loading = AssetDatabase.IsAssetImportWorkerProcess() || EditorApplication.isCompiling ||
                           EditorApplication.isUpdating;
            if (CurrentApplyAllState != ApplyAllState.Done && !loading) ApplyAll();
        }

        private static void ApplyAll()
        {
            if (!MagicLeapLuminPackageUtility.HasRootSDKPath)
            {
                CurrentApplyAllState = ApplyAllState.SetSdkPath;
            }
       
            switch (CurrentApplyAllState)
            {
                case ApplyAllState.SetSdkPath:
                    if (!MagicLeapLuminPackageUtility.HasRootSDKPath)
                    {
                        _setSdkFolderSetupStep.Execute();
                        AssetDatabase.Refresh();
                    }
                    else
                    {
                        CurrentApplyAllState = ApplyAllState.SwitchBuildTarget;
                    }
                    break;
                case ApplyAllState.SwitchBuildTarget:
                    if (!MagicLeapLuminPackageUtility.HasRootSDKPath)
                    {
                        break;
                    }
                    if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.Lumin)
                    {
                        Debug.Log(CHANGING_BUILD_PLATFORM_DEBUG);
                        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Lumin, BuildTarget.Lumin);
                    }

                    CurrentApplyAllState = ApplyAllState.InstallLumin;
                    break;
                case ApplyAllState.InstallLumin:
                    if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.Lumin)
                    {
                        Debug.Log(INSTALLING_LUMIN_SDK_DEBUG);
                        if (ImportMagicLeapSdkSetupStep.HasCompatibleMagicLeapSdk)
                        {
                            _importMagicLeapSdkSetupStep.Execute();
                            Debug.Log(IMPORTING_LUMIN_UNITYPACKAGE_DEBUG);
                            CurrentApplyAllState = ApplyAllState.EnableXrPackage;
                        }
                        else
                        {
                            //TODO: Automate
                            Debug.LogError("Magic Leap SDK Conflict. Cannot resolve automatically.");
                            Stop();
                        }

                    }
                    break;
                case ApplyAllState.EnableXrPackage:
                    if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.Lumin && HasLuminInstalled)
                    {
                        if (!EnablePluginSetupStep.LuminSettingEnabled)
                        {
                            Debug.Log(ENABLING_LUMIN_SDK_DEBUG);
                            _enablePluginSetupStep.Execute();
                            ImportMagicLeapSdkSetupStep.CheckForMagicLeapSdkPackage();
                        }

                        CurrentApplyAllState = ApplyAllState.UpdateManifest;
                    }

                    break;
                case ApplyAllState.UpdateManifest:
                    if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.Lumin
                        && HasLuminInstalled
                        && EnablePluginSetupStep.LuminSettingEnabled)
                    {
                        if (!UpdateManifestSetupStep.ManifestIsUpdated)
                        {
                            Debug.Log(UPDATING_MANIFEST_DEBUG);
                            _updateManifestSetupStep.Execute();
                        }

                        CurrentApplyAllState = ApplyAllState.ChangeColorSpace;
                    }

                    break;

                case ApplyAllState.ChangeColorSpace:
                    if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.Lumin
                        && HasLuminInstalled
                        && EnablePluginSetupStep.LuminSettingEnabled
                        && UpdateManifestSetupStep.ManifestIsUpdated)
                    {
                        if (PlayerSettings.colorSpace != ColorSpace.Linear)
                        {
                            Debug.Log(UPDATING_COLORSPACE_DEBUG);
                            PlayerSettings.colorSpace = ColorSpace.Linear;
                        }

                        CurrentApplyAllState = ApplyAllState.ChangeGraphicsApi;
                    }

                    break;

                case ApplyAllState.ChangeGraphicsApi:
                    if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.Lumin
                        && HasLuminInstalled
                        && EnablePluginSetupStep.LuminSettingEnabled
                        && UpdateManifestSetupStep.ManifestIsUpdated
                        && PlayerSettings.colorSpace == ColorSpace.Linear)
                    {
                        if (!UpdateGraphicsApiSetupStep.HasCorrectGraphicConfiguration)
                        {
                            Debug.Log(CHANGING_GRAPHICS_API_DEBUG);
                            _updateGraphicsApiSetupStep.Execute();
                        }

                        CurrentApplyAllState = ApplyAllState.Done;
                    }

                    break;
                case ApplyAllState.Done:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}