#region

using MagicLeapSetupTool.Editor.Setup;
using MagicLeapSetupTool.Editor.Utilities;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;

#endregion

namespace MagicLeapSetupTool.Editor
{
    /// <summary>
    /// This class controls when to show the Magic Leap setup window.
    /// </summary>
    [InitializeOnLoad]
    public static class AutoRunner
    {
        /// <summary>
        /// Is true when the Magic Leap XR package is installed
        /// </summary>
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

        static AutoRunner()
        {
            //Prevent the window from opening when launching Unity in command line mode
            if (Application.isBatchMode)
                return;

            EditorApplication.update += OnEditorApplicationUpdate;
            EditorApplication.quitting += OnQuit;
            Events.registeringPackages += SetupData.RegisteringPackagesEventHandler;
        }

        private static void OnQuit()
        {
            EditorApplication.quitting -= OnQuit;
            EditorPrefs.SetBool(EditorKeyUtility.WindowClosedEditorPrefKey, false);
            Events.registeringPackages -= SetupData.RegisteringPackagesEventHandler;
        }

       

        private static void OnEditorApplicationUpdate()
        {
            //TODO: move the line below to only get called when the user manually removes a local copy of the Magic Leap package.
            //SetupData.UpdateDefineSymbols();

            //Do not reload information when the editor is not idle.
            if (AssetDatabase.IsAssetImportWorkerProcess()
                || EditorApplication.isCompiling
                || EditorApplication.isUpdating)
            {
                return;
            }

            var autoShow = EditorPrefs.GetBool(EditorKeyUtility.AutoShowEditorPrefKey, true);
            if (!SetupData.HasRootSDKPathInEditorPrefs
                || !HasLuminInstalled
                || !BuildTargetSetupStep.CorrectBuildTarget
                || !ImportMagicLeapSdkSetupStep.HasCompatibleMagicLeapSdk)
            {
                autoShow = true;
                EditorPrefs.SetBool(EditorKeyUtility.AutoShowEditorPrefKey, true);
            }

            EditorApplication.update -= OnEditorApplicationUpdate;
            if (!autoShow) return;

            MagicLeapSetupWindow.ForceOpen();
        }
    }
}