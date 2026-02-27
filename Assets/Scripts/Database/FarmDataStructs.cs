using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CropAreaData
{
    public string cropName;       // Name of crop
    public Vector3 localPosition; // Local position relative to land
    public bool isPlanted;        // Is crop planted
}

[Serializable]
public class LandData
{
    public string landName;
    public bool isFertilized;
    public CropAreaData[] cropAreas;
}

[Serializable]
public class FarmData
{
    public LandData[] lands;
}

[Serializable]
public class PlayerStateData
{
    public int coins;
    //crops seed count
    public int BeanSeed;
    public int BeetrootSeed;
    public int BroccoliSeed;
    public int CabbageSeed;
    public int CarrotSeed;
    public int ChilliSeed;
    public int CornSeed;
    public int PepperSeed;
    public int PumkinSeed;
    public int TomatoSeed;
    public int WatermelonSeed;
    public int WheatSeed;
    //crops count
    public int Bean;
    public int Beetroot;
    public int Broccoli;
    public int Cabbage;
    public int Carrot;
    public int Chilli;
    public int Corn;
    public int Pepper;
    public int Pumkin;
    public int Tomato;
    public int Watermelon;
    public int Wheat;
    //trees count

    //fruits count

    //fishname
    public int Rohu;
    public int Hilsa;
    public int Tilapia;
    public int Catfish;
    public int Salmon;
    public int Tuna;
    public int Mackerel;
    public int Sardine;
    public int Cod;
    public int Carp;

    //animal items count
    public int Egg;
    public int Milk;
    public int Wool;
    public int Meat;
    public int Wood;

    //other items count those are in per kg
    public int Flour;
    public int Rice;
    public int Suger;

    //animal count
    public int Cow;
    public int Chicken;
    public int Sheep,Sheep2;
    public int Goat1,Goat2;
    public int Duck;

    //mission index
    public int CurrentMission1Index=0;
    public int CurrentMission2Index=0;
    public int CurrentMission3Index = 0;
    public int CurrentMission4Index = 0;

}
public class animalItemData
{
    
}
