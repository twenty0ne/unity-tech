using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WidgetMainIcon : UIWidget
{
	public Action<WidgetMainIcon> onClick = null;

	public void OnClicked()
	{
		if (onClick != null)
			onClick(this);
	}
}
