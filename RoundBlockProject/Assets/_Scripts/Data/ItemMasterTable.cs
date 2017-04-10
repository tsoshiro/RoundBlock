using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMasterTable : MasterTableBase<ItemMaster> {
	public void Load() {
		Load(convertClassToFilePath (this.GetType ().Name));
	}
}

public class ItemMaster : MasterBase
{
	public float TIME_PASSED_LESS_THAN { get; private set; }
	public float RATE_ADD_BLOCK { get; private set; }
	public float RATE_WIDE { get; private set; }
	public float SHOT { get; private set; }
	public float SUPER_SHOT { get; private set; }
	public float FEVER { get; private set; }
	public float TIME { get; private set; }
	public float TOTAL { get; private set; }
}