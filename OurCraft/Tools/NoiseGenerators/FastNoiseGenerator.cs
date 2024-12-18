using System;

namespace OurCraft.Tools.NoiseGenerators
{
    public class FastNoiseGenerator
    {
        private FastNoiseLite _noise;
        private float _frequency = Globals.Instance.Frequency;
        private int _mapSize = Globals.Instance.HeightMapSize;
        private int _maxChunkHeight = Globals.Instance.MaxChunkHeight;

        private float _fractalGain = Globals.Instance.FractalGain;
        private int _octaves = Globals.Instance.Octaves;
        private float _fractalLacunarity = Globals.Instance.FractalLacunarity;

        public FastNoiseGenerator() 
        {
            this._noise = new FastNoiseLite();
            
            Random rnd = new Random();
            this._noise.SetNoiseType(FastNoiseLite.NoiseType.Perlin);
            this._noise.SetFrequency(this._frequency);
            this._noise.SetSeed(rnd.Next(500, 2000));

            this._noise.SetFractalOctaves(this._octaves);
            this._noise.SetFractalLacunarity(this._fractalLacunarity);
            this._noise.SetFractalGain(this._fractalGain);
        }

        public int[,] GenerateNoise()
        {
            int[,] noiseArray = new int[this._mapSize, this._mapSize];
            for (int i = 0; i < noiseArray.GetLength(0); i++) 
            {
                for (int j = 0; j < noiseArray.GetLength(1); j++) 
                {
                    float noiseValue = this._noise.GetNoise(i, j);
                    noiseValue = Math.Abs((int)Math.Floor(noiseValue * this._maxChunkHeight));
                    noiseArray[i, j] = (int)noiseValue;
                }
            }

            return noiseArray;
        }

        public void UpdateGeneratorValuesBasedOnGlobals()
        {
            this._frequency = Globals.Instance.Frequency;
            this._mapSize = Globals.Instance.HeightMapSize;
            this._maxChunkHeight = Globals.Instance.MaxChunkHeight;
            this._fractalGain = Globals.Instance.FractalGain;
            this._octaves = Globals.Instance.Octaves;
            this._fractalLacunarity = Globals.Instance.FractalLacunarity;

            Random rnd = new Random();
            this._noise.SetNoiseType(FastNoiseLite.NoiseType.Perlin);
            this._noise.SetFrequency(this._frequency);
            this._noise.SetSeed(rnd.Next(500, 2000));

            this._noise.SetFractalOctaves(this._octaves);
            this._noise.SetFractalLacunarity(this._fractalLacunarity);
            this._noise.SetFractalGain(this._fractalGain);
        }
    }
}
