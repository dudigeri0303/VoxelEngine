using Microsoft.Xna.Framework.Input;
using System;
using System.ComponentModel.DataAnnotations;

namespace OurCraft.Tools
{
    //TODO: Make it a singleon class
    public class Globals
    {
        //Terrain Gen Values
        private int _chunkCount;
        private int _chunkSize;
        private int _maxChunkHeight;
        private int _waterLevel;
        private float _frequency;
        private float _fractalGain;
        private int _octaves;
        private float _fractalLacunarity;

        //MinMaxValues
        public (int, int) ChunkCountMinMax { get; } = (1, 32);
        public (int, int) MaxChunkHeightMinMax { get; } = (10, 300);
        public (int, int) WaterLevelMinMax { get; } = (0, 100);
        public (float, float) FrequencyMinMax { get; } = (0.00099f, 0.5f);
        public (float, float) FractalGainMinMax { get; } = (0.0f, 1.0f);
        public (int, int) OctavesMinMax { get; } = (1, 8);
        public (float, float) FractalLacunarityMinMax { get; } = (1.5f , 3.5f);

        //Strting values
        public  int StartingChunkCountForNormalScene { get; } = 4;
        public int StartingChunkCountForLayeredScene { get; } = 10;
        public int StartingChunkSize { get; } = 32;
        public int StartingMaxChunkHeightForNormalScene { get; } = 32;
        public int StartingMaxChunkHeightForLayeredScene { get; } = 130;
        public int StartingWaterLevelForNormalScene { get; } = 5;
        public int StartingWaterLevelForLayeredScene { get; } = 20;
        public float StartingFrequencyForNormalScene { get; } = 0.03f;
        public float StartingFrequencyForLayeredScene { get; } = 0.005f;

        private static Globals _instance;
        public static Globals Instance
        {
            get 
            {
                if(_instance == null)
                    _instance = new Globals();
                return _instance;
            }
        }

        private Globals() 
        {
            this._chunkCount = this.StartingChunkCountForNormalScene;
            this._chunkSize = this.StartingChunkSize;
            this._maxChunkHeight = this.StartingMaxChunkHeightForNormalScene;
            this._frequency = this.StartingFrequencyForNormalScene;
            this._waterLevel = this.StartingWaterLevelForNormalScene;
            this._fractalGain = 0.5f;
            this._octaves = 2;
            this._fractalLacunarity = 2.0f;
        }

        public int WindowWidth => 1920;
        public int WindowHeight => 1200;

        public int CubeSize => 2;
        public int SquareSize => 10;

        public int ChunkCount
        {
            get => this._chunkCount;
            set
            {
                if (value > this.ChunkCountMinMax.Item1 && value <= this.ChunkCountMinMax.Item2)
                    _chunkCount = value;
            }
        }

        public int ChunkSize => this._chunkSize;
        
        public int MaxChunkHeight 
        {
            get => this._maxChunkHeight;
            set 
            {
                if (value >= this.MaxChunkHeightMinMax.Item1 && value <= this.MaxChunkHeightMinMax.Item2)
                    this._maxChunkHeight = value;
            }
        }

        public int WaterLevel
        {
            get => this._waterLevel;
            set 
            {
                if (value >= this.WaterLevelMinMax.Item1 && value <= this.WaterLevelMinMax.Item2)
                    this._waterLevel = value;
            }
        }

        public float Frequency
        {
            get => this._frequency;
            set
            {
                if (value >= this.FrequencyMinMax.Item1 && value <= this.FrequencyMinMax.Item2)
                    this._frequency = value;
            }
        }

        public float FractalGain
        {
            get => this._fractalGain;
            set 
            {
                if (value >= this.FractalGainMinMax.Item1 && value <= this.FractalGainMinMax.Item2)
                    this._fractalGain = value;
            }
        }

        public int Octaves
        {
            get => this._octaves;
            set 
            {
                if (value >= this.OctavesMinMax.Item1 && value <= this.OctavesMinMax.Item2)
                    this._octaves = value;
            }
        }

        public float FractalLacunarity 
        {
            get => this._fractalLacunarity;
            set 
            {
                if(value >= this.FractalLacunarityMinMax.Item1 && value <= this.FractalLacunarityMinMax.Item2)
                    this._fractalLacunarity = value;
            } 
        }

        public int HeightMapSize => this._chunkCount * this._chunkSize;
        public int HeightMapGridSize => (HeightMapSize / 10) + 1;

        //Water values
        private int WaterSurfaceSize => (HeightMapSize + 1) * CubeSize;
        private int WaterSurfaceChunkSize => 10;
        public int WaterSurfaceChunkCount => WaterSurfaceSize / WaterSurfaceChunkSize + 1;

        //Cube values
        public int CubeCount { get; set; } = 0;

    }
}
