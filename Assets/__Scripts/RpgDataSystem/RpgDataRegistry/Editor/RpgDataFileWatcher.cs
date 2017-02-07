using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;

namespace SphericalCow
{
	/// <summary>
	/// 	A editor utility class that processes events involving RPG Data Assets changing
	/// 	(like when such files are created, moved, renamed, or deleted)
	/// </summary>
	public class RpgDataFileWatcher 
	{
		private const string RpgDataProjectPath = "Assets/_DataAssets/RpgSystem";
		
		private static FileSystemWatcher rpgFileWatcher = null;
		private static bool fileWasChanged = false;
		private static bool fileWasCreated = false;
		private static bool fileWasDeleted = false;
		private static bool fileWasRenamed = false;
		
		
		
		/// <summary>
		/// 	Setup the File Watcher
		/// </summary>
		[InitializeOnLoadMethod]
		static void Setup()
		{
			//Debug.Log("Starting RPG File Watcher");
			
			RpgDataFileWatcher.fileWasChanged = false;
			RpgDataFileWatcher.fileWasCreated = false;
			RpgDataFileWatcher.fileWasDeleted = false;
			RpgDataFileWatcher.fileWasRenamed = false;
			
			if(RpgDataFileWatcher.rpgFileWatcher == null)
			{
				#if UNITY_EDITOR_OSX
					System.Environment.SetEnvironmentVariable("MONO_MANAGED_WATCHER", "enabled");
				#endif
				
				string currentPath = Path.GetFullPath(RpgDataFileWatcher.RpgDataProjectPath);
				RpgDataFileWatcher.rpgFileWatcher = new FileSystemWatcher(currentPath);
				
				RpgDataFileWatcher.rpgFileWatcher.Changed += OnFileChanged;
				RpgDataFileWatcher.rpgFileWatcher.Created += OnFileCreated;
				RpgDataFileWatcher.rpgFileWatcher.Deleted += OnFileDeleted;
				RpgDataFileWatcher.rpgFileWatcher.Renamed += OnFileRenamed;
				
				RpgDataFileWatcher.rpgFileWatcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
															   | NotifyFilters.FileName | NotifyFilters.DirectoryName;
				RpgDataFileWatcher.rpgFileWatcher.Filter = "*.asset";
				RpgDataFileWatcher.rpgFileWatcher.IncludeSubdirectories = true;
				RpgDataFileWatcher.rpgFileWatcher.EnableRaisingEvents = true;
				
				//Debug.Log("RPG File Watcher ready");
			}
			
			EditorApplication.update += OnEditorApplicationUpdate;
		}
		
		
		
		
		/// <summary>
		///		Raises the file changed event. Do not call Unity classes here
		/// </summary>
		private static void OnFileChanged(object sender, FileSystemEventArgs e)
		{
			RpgDataFileWatcher.fileWasChanged = true;
		}
		
		/// <summary>
		///		Raises the file created event. Do not call Unity classes here
		/// </summary>
		private static void OnFileCreated(object sender, FileSystemEventArgs e)
		{
			RpgDataFileWatcher.fileWasCreated = true;
		}
		
		/// <summary>
		///		Raises the file deleted event. Do not call Unity classes here
		/// </summary>
		private static void OnFileDeleted(object sender, FileSystemEventArgs e)
		{
			RpgDataFileWatcher.fileWasDeleted = true;
		}
		
		/// <summary>
		///		Raises the file renamed event. Do not call Unity classes here
		/// </summary>
		private static void OnFileRenamed(object sender, FileSystemEventArgs e)
		{
			RpgDataFileWatcher.fileWasRenamed = true;
		}
		
		
		
		
		
		
		/// <summary>
		/// 	Unity Editor update routine for this class (must be added as a delegate to work, see Setup())
		/// </summary>
		static void OnEditorApplicationUpdate ()
		{
			// note that this is called very often (100/sec)
			
			if (RpgDataFileWatcher.fileWasChanged)
			{
				RpgDataFileWatcher.fileWasChanged = false;
				//Debug.Log("RPG File Watcher detected file change");
				
				// You would run your file-change event here
				
			}
			
			if (RpgDataFileWatcher.fileWasCreated)
			{
				RpgDataFileWatcher.fileWasCreated = false;
				//Debug.Log("RPG File Watcher detected file creation");
				
				// You would run your file-creation event here
				
			}
			
			if (RpgDataFileWatcher.fileWasDeleted)
			{
				RpgDataFileWatcher.fileWasDeleted = false;
				Debug.Log("RPG File Watcher detected file deletion");
				
				// You would run your file-deletion event here
				RpgDataFileWatcher.CleanNullReferencesFromRpgRegistry();
			}
			
			if (RpgDataFileWatcher.fileWasRenamed)
			{
				RpgDataFileWatcher.fileWasRenamed = false;
				//Debug.Log("RPG File Watcher detected file renaming");
				
				// You would run your file-renaming event here
				
			}
			
		}
		
		
		
		
		
		/// <summary>
		/// 	Shrinks the data arrays in the RpgDataRegistry if they have null references
		/// </summary>
		private static void CleanNullReferencesFromRpgRegistry()
		{
			// Find the RpgDataRegistry
			RpgDataRegistry registry = RpgDataAssetUtility.FindRpgDataRegistry();
			
			// Clean the arrays in the registry of null references
			registry.CleanMissingReferences();
		}
		
		

		
		
		
		/// <summary>
		/// 	Shrinks the data arrays in the RpgDataRegistry if they have null references
		/// </summary>
		[MenuItem("Tasks/SphericalCow/RPG Data System/Clean Registry of Null References")]
		private static void MenuCallToCleanRegistry()
		{
			RpgDataFileWatcher.CleanNullReferencesFromRpgRegistry();
		}
	}
}
