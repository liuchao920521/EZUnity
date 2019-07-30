/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-04-04 16:14:44
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEditor;
using UnityEngine;

namespace EZhex1991.EZUnity.Builder
{
    [CustomEditor(typeof(EZPlayerBuilder)), CanEditMultipleObjects]
    public class EZPlayerBuilderEditor : Editor
    {
        protected EZPlayerBuilder playerBuilder;

        protected SerializedProperty m_ConfigButDontBuild;
        protected SerializedProperty m_BuildTarget;
        protected SerializedProperty m_BuildOptions;

        protected SerializedProperty m_BundleBuilder;

        protected SerializedProperty m_LocationPathName;
        protected SerializedProperty m_Scenes;

        protected SerializedProperty m_CompanyName;
        protected SerializedProperty m_ProductName;
        protected SerializedProperty m_BundleIdentifier;
        protected SerializedProperty m_BundleVersion;
        protected SerializedProperty m_BuildNumber;
        protected SerializedProperty m_Icon;

        protected SerializedProperty m_CopyList;

        protected void OnEnable()
        {
            playerBuilder = target as EZPlayerBuilder;
            m_ConfigButDontBuild = serializedObject.FindProperty("configButDontBuild");
            m_BuildTarget = serializedObject.FindProperty("buildTarget");
            m_BuildOptions = serializedObject.FindProperty("buildOptions");
            m_BundleBuilder = serializedObject.FindProperty("bundleBuilder");
            m_LocationPathName = serializedObject.FindProperty("locationPathName");
            m_Scenes = serializedObject.FindProperty("scenes");
            m_CompanyName = serializedObject.FindProperty("companyName");
            m_ProductName = serializedObject.FindProperty("productName");
            m_BundleIdentifier = serializedObject.FindProperty("bundleIdentifier");
            m_BundleVersion = serializedObject.FindProperty("bundleVersion");
            m_BuildNumber = serializedObject.FindProperty("buildNumber");
            m_Icon = serializedObject.FindProperty("icon");

            m_CopyList = serializedObject.FindProperty("copyList");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EZEditorGUIUtility.ScriptableObjectTitle(target as ScriptableObject, !serializedObject.isEditingMultipleObjects);

            if (!serializedObject.isEditingMultipleObjects)
            {
                EditorGUILayout.LabelField("Build", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(m_ConfigButDontBuild);
                EditorGUILayout.PropertyField(m_BuildTarget);
                if (GUILayout.Button("Build"))
                {
                    playerBuilder.Execute((BuildTarget)m_BuildTarget.intValue);
                    GUIUtility.ExitGUI();
                }
                EditorGUILayout.Space();
                DrawQuickBuildButtons();
                EditorGUILayout.Space();
            }

            EditorGUILayout.LabelField("Build Options", EditorStyles.boldLabel);
            m_BuildOptions.intValue = (int)(BuildOptions)EditorGUILayout.EnumFlagsField("Build Options", (BuildOptions)m_BuildOptions.intValue);
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(m_LocationPathName);
                if (GUILayout.Button("...", EditorStyles.miniButton, new GUILayoutOption[] { GUILayout.Width(30) }))
                {
                    string path = EditorUtility.SaveFolderPanel("Choose Output Folder", "", "");
                    if (!string.IsNullOrEmpty(path)) m_LocationPathName.stringValue = path;
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.PropertyField(m_BundleBuilder);
            EditorGUILayout.PropertyField(m_Scenes, true);
            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Player Settings", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(m_CompanyName);
            EditorGUILayout.PropertyField(m_ProductName);
            EditorGUILayout.PropertyField(m_BundleIdentifier);
            EditorGUILayout.PropertyField(m_BundleVersion);
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(m_BuildNumber);
                if (GUILayout.Button("+", EditorStyles.miniButton, new GUILayoutOption[] { GUILayout.Width(30) }))
                {
                    m_BuildNumber.intValue++;
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.PropertyField(m_Icon);
            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(m_CopyList);

            serializedObject.ApplyModifiedProperties();
        }
        public void DrawQuickBuildButtons()
        {
            if (GUILayout.Button("Android"))
            {
                playerBuilder.Execute(BuildTarget.Android);
                GUIUtility.ExitGUI();
            }
            if (GUILayout.Button("iOS"))
            {
                playerBuilder.Execute(BuildTarget.iOS);
                GUIUtility.ExitGUI();
            }
            {
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("Windows"))
                {
                    playerBuilder.Execute(BuildTarget.StandaloneWindows);
                    GUIUtility.ExitGUI();
                }
                if (GUILayout.Button("Windows64"))
                {
                    playerBuilder.Execute(BuildTarget.StandaloneWindows64);
                    GUIUtility.ExitGUI();
                }
                EditorGUILayout.EndHorizontal();
            }
            if (GUILayout.Button("OSX"))
            {
                playerBuilder.Execute(BuildTarget.StandaloneOSX);
                GUIUtility.ExitGUI();
            }
        }
    }
}
