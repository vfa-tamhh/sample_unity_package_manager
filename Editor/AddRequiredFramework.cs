#if UNITY_IOS
using System;
using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEditor.iOS.Xcode;
using UnityEditor.Callbacks;
using System.Collections;

public class AddRequiredFramework
{

	[PostProcessBuildAttribute (0)]
	public static void OnPostprocessBuild (BuildTarget buildTarget, string pathToBuiltProject)
	{
		// Stop processing if targe is NOT iOS
		if (buildTarget != BuildTarget.iOS)
			return;
		UpdateXcode(pathToBuiltProject);
	}

	static void UpdateXcode(string pathToBuiltProject) 
	{
		var projectPath = PBXProject.GetPBXProjectPath(pathToBuiltProject);

		if (!File.Exists(projectPath))
		{
			throw new Exception(string.Format("projectPath is null {0}", projectPath));
		}
			
		// Initialize PbxProject
		PBXProject pbxProject = new PBXProject();
		pbxProject.ReadFromFile(projectPath);

		string targetGuid = pbxProject.GetUnityMainTargetGuid();

		// Adding required framework
#if UNITY_2019_3_OR_NEWER
		pbxProject.AddFrameworkToProject(pbxProject.GetUnityFrameworkTargetGuid(), "UserNotifications.framework", false);
		pbxProject.AddFrameworkToProject(pbxProject.GetUnityFrameworkTargetGuid(), "WebKit.framework", false);
		pbxProject.AddFrameworkToProject(pbxProject.GetUnityFrameworkTargetGuid(), "AuthenticationServices.framework", false);
		pbxProject.AddFrameworkToProject(pbxProject.GetUnityFrameworkTargetGuid(), "Social.framework", false);
		pbxProject.AddFrameworkToProject(pbxProject.GetUnityFrameworkTargetGuid(), "Accounts.framework", false);
#else
		pbxProject.AddFrameworkToProject(targetGuid, "UserNotifications.framework", false);
		pbxProject.AddFrameworkToProject(targetGuid, "WebKit.framework", false);
		pbxProject.AddFrameworkToProject(targetGuid, "AuthenticationServices.framework", false);
		pbxProject.AddFrameworkToProject(targetGuid, "Social.framework", false);
		pbxProject.AddFrameworkToProject(targetGuid, "Accounts.framework", false);
#endif

		// Apply settings
		File.WriteAllText (projectPath, pbxProject.WriteToString());
	}
}
#endif
