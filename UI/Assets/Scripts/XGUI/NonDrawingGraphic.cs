// https://answers.unity.com/questions/1091618/ui-panel-without-image-component-as-raycast-target.html

using UnityEngine;
using UnityEngine.UI;

// providing a raycast target without drawing anything
public class NonDrawingGraphic : Graphic
{
	public override void SetMaterialDirty() { return; }
	public override void SetVerticesDirty() { return; }

	protected override void OnPopulateMesh(VertexHelper vh)
	{
		vh.Clear();
		return;
	}
}
