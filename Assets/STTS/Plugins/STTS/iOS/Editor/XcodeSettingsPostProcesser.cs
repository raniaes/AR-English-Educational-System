using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using System.IO;

public class XcodeSettingsPostProcesser
{
    [PostProcessBuildAttribute (0)]
    public static void OnPostprocessBuild (BuildTarget buildTarget, string pathToBuiltProject)
    {
        // it only works for iOS Platform
        if (buildTarget != BuildTarget.iOS)
            return;

        // Initialize PbxProject
        var projectPath = pathToBuiltProject + "/Unity-iPhone.xcodeproj/project.pbxproj";
        PBXProject pbxProject = new PBXProject();
        pbxProject.ReadFromFile(projectPath);
        //string targetGuid = pbxProject.TargetGuidByName("Unity-iPhone");
        string targetGuid = pbxProject.GetUnityFrameworkTargetGuid();

        // Add Frameworks
        pbxProject.AddFrameworkToProject(targetGuid, "Speech.framework", true);

        // Apply Settings
        File.WriteAllText(projectPath, pbxProject.WriteToString());
    }
}




// Reference: https://sanctacrux.tistory.com/670