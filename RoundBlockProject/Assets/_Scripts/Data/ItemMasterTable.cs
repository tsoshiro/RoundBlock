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
//	time_passed_less_than,rate_add_block,rate_hard,rate_wide,rate_shot,rate_supershot,rate_fever,rate_time,total
	public float TIME_PASSED_LESS_THAN { get; private set; }
	public float RATE_ADD_BLOCK { get; private set; }
	public float RATE_HARD { get; private set; }
	public float RATE_WIDE { get; private set; }
	public float RATE_SHOT { get; private set; }
	public float RATE_SUPERSHOT { get; private set; }
	public float RATE_FEVER { get; private set; }
	public float RATE_TIME { get; private set; }
	public float TOTAL { get; private set; }
}