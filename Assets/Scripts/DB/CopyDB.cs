using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Diagnostics;

//Top of the script
#pragma warning disable 0414

public class CopyDB : MonoBehaviour {
	
	private SQLiteDB db = null;
	public string dbfilename;
	private string log;
	public int versionDB;
	public string next_scene;
	
	// Use this for initialization
	void Start(){
		if(!File.Exists(Application.persistentDataPath + "/" + dbfilename)) {
			print ("No existe la base de datos");
			StartCoroutine(CopiaDB());
		}else{
			print("Ya existe la base de datos");
			//Checando si es la version correcta
            if (!checkVersion(versionDB))
            {
                File.Delete(Application.persistentDataPath + "/" + dbfilename);
                //Copiando nueva base de datos
                StartCoroutine(CopiaDB());
            }
            else
                SceneManager.LoadSceneAsync(next_scene);
		}
	}
	
	bool checkVersion(int version)
	{
		SQLiteDB db = new SQLiteDB();
		string filename = Application.persistentDataPath + "/" + dbfilename;
		int ver = 0;
		try{
			db.Open(filename);
			SQLiteQuery qr = null;
			try{
				qr = new SQLiteQuery(db, "SELECT db_version FROM Version WHERE idVersion = 1");
				qr.Step();
				ver = qr.GetInteger("db_version");
				qr.Release();
			
			}catch(Exception e){
				print ("Error: " + e.ToString());
				if(qr != null){
					qr.Release();
					qr = null;
				}
			}
			
		}catch(Exception e){
			print ("Error: " + e.ToString());
			if(db != null){
				db.Close();
				db = null;
			}
		}
		
		db.Close();
		db = null;
		print ("VersionDB_Streaming: " + version + " VersionDB_Persistent: " + ver);
		
		if(ver == version)
			return true;
		else
			return false;
	}
	
	IEnumerator CopiaDB () {
		db = new SQLiteDB();
			
			log = "";

			byte[] bytes = null;				
			
			
#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX
		string dbpath = "file://" + Application.streamingAssetsPath + "/" + dbfilename; log += "asset path is: " + dbpath;
		WWW www = new WWW(dbpath);
		yield return www;
		bytes = www.bytes;
#elif UNITY_WEBPLAYER
		string dbpath = "StreamingAssets/" + dbfilename;								log += "asset path is: " + dbpath;
		WWW www = new WWW(dbpath);
		yield return www;
		bytes = www.bytes;
#elif UNITY_IPHONE
		string dbpath = Application.dataPath + "/Raw/" + dbfilename;					log += "asset path is: " + dbpath;					
		try{	
			using ( FileStream fs = new FileStream(dbpath, FileMode.Open, FileAccess.Read, FileShare.Read) ){
				bytes = new byte[fs.Length];
				fs.Read(bytes,0,(int)fs.Length);
			}			
		} catch (Exception e){
			log += 	"\nTest Fail with Exception " + e.ToString();
			log += 	"\n";
		}
		yield return null;
#elif UNITY_ANDROID
		string dbpath = Application.streamingAssetsPath + "/" + dbfilename;	            log += "asset path is: " + dbpath;
		WWW www = new WWW(dbpath);
		yield return www;
		bytes = www.bytes;
#endif
		if ( bytes != null )
		{
			try{	
				
				string filename = Application.persistentDataPath + "/" + dbfilename;

				//
				//
				// copy database to real file into cache folder
				using( FileStream fs = new FileStream(filename, FileMode.Create, FileAccess.Write) )
				{
					fs.Write(bytes,0,bytes.Length);             log += "\nCopy database from streaminAssets to persistentDataPath: " + filename;
				}
				
#if UNITY_IPHONE
				iPhone.SetNoBackupFlag(filename);
#endif			
			} catch (Exception e){
				log += 	"\nTest Fail with Exception " + e.ToString();
				log += 	"\n\n Did you copy test.db into StreamingAssets ?\n";
			}
		}
        SceneManager.LoadSceneAsync(next_scene);
	}
}