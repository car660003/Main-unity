using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VegetableMainMessange{
	public static int money = 500;
}

public static class VegetableDetail_Static {

	public static string name = "VegetableDetail_Static name！";
	public static int growingTime = 20;
}

public static class pumpkin{//60
	public static int Count = 0;
	public static float growing_00To01_time = 10;//3分鐘
	public static float growing_01To02_time = 10;//12
	public static float growing_02To03_time = 10;//20
	public static float growing_03To04_time = 10;//25
	public static float hp = 200;
	public static float hpPerS = 2;//值越大，扣的hp越少(每秒)
	public static int buyingPrice = 60;//購買種子的價格
	public static int salePrice = 70;//作物賣出的價格
	public static int seedAmount = 0;//種子的數量
}

public static class cucumber{//27.5
	public static int Count = 0;
	public static float growing_00To01_time = 240;//4
	public static float growing_01To02_time = 210;//3.5
	public static float growing_02To03_time = 900;//15
	public static float growing_03To04_time = 300;//5
	public static float hp = 200;
	public static float hpPerS = 2;//值越大，扣的hp越少(每秒)
	public static int buyingPrice = 30;
	public static int salePrice = 40;
	public static int seedAmount = 0;
}

public static class carrot{//45
	public static int Count = 0;
	public static float growing_00To01_time = 450;//7.5
	public static float growing_01To02_time = 450;//7.5
	public static float growing_02To03_time = 900;//15
	public static float growing_03To04_time = 900;//15
	public static float hp = 200;
	public static float hpPerS = 2;//值越大，扣的hp越少(每秒)
	public static int buyingPrice = 50;
	public static int salePrice = 60;
	public static int seedAmount = 0;
}

public static class eggplant{//42
	public static int Count = 0;
	public static float growing_00To01_time = 180;//3
	public static float growing_01To02_time = 540;//9
	public static float growing_02To03_time = 900;//15
	public static float growing_03To04_time = 900;//15
	public static float hp = 200;
	public static float hpPerS = 2;//值越大，扣的hp越少(每秒)
	public static int buyingPrice = 40;
	public static int salePrice = 50;
	public static int seedAmount = 0;
}

public static class tomato{//59.5
	public static int Count = 0;
	public static float growing_00To01_time = 270;//4.5
	public static float growing_01To02_time = 600;//10
	public static float growing_02To03_time = 900;//15
	public static float growing_03To04_time = 1800;//30
	public static float hp = 200;
	public static float hpPerS = 2;//值越大，扣的hp越少(每秒)
	public static int buyingPrice = 70;
	public static int salePrice = 80;
	public static int seedAmount = 0;
}