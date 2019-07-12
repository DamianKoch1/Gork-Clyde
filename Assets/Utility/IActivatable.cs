using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IActivatable
{
	/// <summary>
	/// Is called when activating button that targets this object
	/// </summary>
	void OnButtonActivated();
	
	/// <summary>
	/// Is called when deactivating button that targets this object
	/// </summary>
	void OnButtonDeactivated();

	/// <summary>
	/// Is called when activating plate that targets this object
	/// </summary>
	void OnPlateActivated();
	
	/// <summary>
	/// Is called when deactivating plate that targets this object
	/// </summary>
	void OnPlateExited();
}
