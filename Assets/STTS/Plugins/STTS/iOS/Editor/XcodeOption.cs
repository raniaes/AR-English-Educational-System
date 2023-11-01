using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;

public static class XcodeOption
{
    [PostProcessBuild(999)]
    public static void OnPostProcessBuild(BuildTarget buildTarget, string path)
    {
        if (buildTarget == BuildTarget.iOS)
        {
            //{
            //    string projectPath = path = "/Unity-iPhone.xcodeproj/project.pbxproj";

            //    PBXProject pbxProject = new PBXProject();
            //    pbxProject.ReadFromFile(projectPath);

            //    string target = pbxProject.TargetGuidByName("Unity-iPhone");
            //    pbxProject.SetBuildProperty(target, "ENABLE_BITCODE", "NO");

            //    pbxProject.WriteToFile(projectPath);
            //}

            string infoPlistPath = path + "/Info.plist";

            PlistDocument plistDoc = new PlistDocument();
            plistDoc.ReadFromFile(infoPlistPath);
            if (plistDoc.root != null)
            {
                plistDoc.root.SetString("NSMicrophoneUsageDescription", "Need to Access Microphone");
                plistDoc.root.SetString("NSSpeechRecognitionUsageDescription", "Need to Access Speech Recognition");
                plistDoc.WriteToFile(infoPlistPath);
            }
            else
            {
                Debug.LogError("ERROR: Cannot open " + infoPlistPath);
            }
        }
    }
}


// Reference: https://sanctacrux.tistory.com/670