using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// simulate loading
public class SceneLoading : MonoBehaviour 
{
	private enum LoadingStep
	{
		None,
		Login,
		Config,
		Data,
		AssetBundle,
		Audio,
		Scene,
	}

	public string initMenu = null;

	private LoadingStep _loadingStep = LoadingStep.None;

	private void Awake()
	{
		if (string.IsNullOrEmpty(initMenu) == false)
		{
			UIManager.Instance.OpenMenu(initMenu);
		}
	}

	void Start () 
	{
		
	}

	void Update () 
	{
		if (_loadingStep == LoadingStep.Login)
		{
			
		}
	}
}
