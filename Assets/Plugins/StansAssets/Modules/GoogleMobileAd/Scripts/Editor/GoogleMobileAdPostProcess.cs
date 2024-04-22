//#define CODE_DISABLED
#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.Collections;
using SA.IOSDeploy;

public class GoogleMobileAdPostProcess  {
	

	[PostProcessBuild(49)]
	public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject) {


        #if UNITY_IPHONE && !CODE_DISABLED
        ISD_API.AddFramework(ISD_iOSFramework.StoreKit);
        ISD_API.AddFramework(ISD_iOSFramework.CoreTelephony);
        ISD_API.AddFramework(ISD_iOSFramework.AdSupport);
        ISD_API.AddFramework(ISD_iOSFramework.MessageUI);
        ISD_API.AddFramework(ISD_iOSFramework.EventKit);
        ISD_API.AddFramework(ISD_iOSFramework.EventKitUI);

        ISD_API.AddFlag("-ObjC", ISD_FlagType.LinkerFlag);
		#endif
	}

}
#endif
