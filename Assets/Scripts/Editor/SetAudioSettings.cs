using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Editor
{
    public class SetAudioSettings : EditorWindow
    {
        #region Fields
        private int compression = 196;
        private AudioImporterFormat format;
        private bool gapless;
        private bool hardware;
        private AudioImporterLoadType loadType;
        private bool mono;
        private bool threeD;
        #endregion

        #region Methods
        [MenuItem("Tools/Bulk Audio Settings")]
        private static void Init()
        {
            GetWindow(typeof(SetAudioSettings));
        }

        private void OnGUI()
        {
            format = (AudioImporterFormat)EditorGUILayout.EnumPopup("Audio Format", format);
            threeD = EditorGUILayout.Toggle("3D sound", threeD);
            mono = EditorGUILayout.Toggle("Force to mono", mono);
            loadType = (AudioImporterLoadType)EditorGUILayout.EnumPopup("Load Type", loadType);
            hardware = EditorGUILayout.Toggle("Hardware decoding", hardware);
            gapless = EditorGUILayout.Toggle("Gapless Looping", gapless);
            compression = EditorGUILayout.IntSlider("Compression (kbps)", compression, 0, 256);

            if (GUILayout.Button("Set Settings"))
            {
                foreach (var o in Selection.GetFiltered(typeof(AudioClip), SelectionMode.DeepAssets))
                {
                    var path = AssetDatabase.GetAssetPath(o);
                    var audioImporter = AssetImporter.GetAtPath(path) as AudioImporter;
                    audioImporter.format = format;
                    audioImporter.threeD = threeD;
                    audioImporter.forceToMono = mono;
                    audioImporter.loadType = loadType;
                    audioImporter.hardware = hardware;
                    audioImporter.compressionBitrate = compression * 1000;
                    AssetDatabase.ImportAsset(path);
                }
            }
        }
        #endregion
    }
}