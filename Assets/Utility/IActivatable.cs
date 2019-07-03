using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IActivatable
{
	void OnButtonActivated();
	void OnButtonDeactivated();

	void OnPlateActivated();
	void OnPlateExited();
}
