using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OurCraft.Graphics.WorldObjects;
using OurCraft.Tools;
using System.Collections.Generic;
using OurCraft.InputDevices;
using System;
using OurCraft.Graphics.Tools;
using OurCraft.Tools.NoiseGenerators;
using System.Linq;
using OurCraft.Graphics.DataTypes;
using System.ComponentModel.Design;
using System.Diagnostics;

namespace OurCraft.Graphics
{
    public enum SceneMode
    {
        Normal, Layered
    }


    public class Scene : IDisposable
    { 
        private Camera _camera;

        private int _chunkWidth = Globals.Instance.ChunkSize;
        private int _chunkDepth = Globals.Instance.ChunkSize;

        private ByteCubeType[][][] _cubeDataArray;
        private Texture2D _heightMapTexture;

        private List<Chunk> _chunks;
        private Water _water;

        private FastNoiseGenerator _noiseGenerator;

        private int _vertexCount;
        private int _indexCount;

        private SceneMode _sceneMode;

        private Random _random;

        private List<CubeType> _cubeTypes;

        public Camera Camera => _camera;
        public int VertexCount => _vertexCount;
        public int IndexCount => _indexCount;
        public FastNoiseGenerator NoiseGenerator => _noiseGenerator;
        public SceneMode SceneMode => _sceneMode;

        public Scene(Effect cubeShader, Effect waterShader, Camera camera, SceneMode sceneMode) 
        {
            this._camera = camera;
            this._noiseGenerator = new FastNoiseGenerator();
            this._sceneMode = sceneMode;
            
            this._random = new Random();
            this._cubeTypes = Enum.GetValues(typeof(CubeType)).Cast<CubeType>().ToList();

            int[,] heightMap = this._noiseGenerator.GenerateNoise();
            this._chunks = this.GenerateChunks(cubeShader, heightMap);

            this._water = new Water(waterShader);
        }

        public void Dispose()
        {
            this._water.Dispose();
            this._chunks.ForEach(c => c.Dispose());

            GC.SuppressFinalize(this);
        }

        #region HelperMethods

        private ByteCubeType CreateNewCubeData(byte value, bool isValidCube, bool isWater)
        {
            CubeType type;
            if (isWater && isValidCube) 
            {
                type = CubeType.WATER;
            }

            else if (isValidCube && !isWater)
            {
                int randValue = this._random.Next(0, 10);
                int randomIndex;

                if (randValue < 2) randomIndex = 1;
                else randomIndex = 0;

                type = this._cubeTypes[randomIndex];
            }
            else type = CubeType.DIRT;

            return new ByteCubeType()
            {
                Value = value,
                Type = type
            };
        }

        private void CreateAndFillCubesDataArray(int[,] heightMap)
        {
            this._cubeDataArray = new ByteCubeType[heightMap.GetLength(0)][][];
            for (int i = 0; i < this._cubeDataArray.Length; i++)
            {
                this._cubeDataArray[i] = new ByteCubeType[heightMap.GetLength(1)][];

                for (int j = 0; j < this._cubeDataArray[i].Length; j++)
                {
                    int height = heightMap[i, j];
                    this._cubeDataArray[i][j] = new ByteCubeType[Globals.Instance.MaxChunkHeight];

                    for (int k = 0; k < this._cubeDataArray[i][j].Length; k++)
                    {
                        if (k < height)  
                            this._cubeDataArray[i][j][k] = this.CreateNewCubeData(0x00000001, true, false);
                        else
                            this._cubeDataArray[i][j][k] = this.CreateNewCubeData(0x00000000, false, false);
                    }
                }
            }
        }

        private void AddExtraLayerToCubesDataArray(int[,] heightMap)
        {
            for (int j = 0; j < this._cubeDataArray.Length; j++)
            {
                for (int k = 0; k < this._cubeDataArray[j].Length; k++)
                {
                    int height = heightMap[j, k];
                    int currentHeight = this._cubeDataArray[j][k].Count(d => d.Value == 0x00000001);

                    if (height > currentHeight)
                    {
                        for (int l = currentHeight; l < height; l++)
                        {    
                            this._cubeDataArray[j][k][l] = this.CreateNewCubeData(0x00000001, true, false);
                        }
                    }
                }
            }
        }

        private void AddWaterDataToCubesDataArray() 
        {
            for (int j = 0; j < this._cubeDataArray.Length; j++)
            {
                for (int k = 0; k < this._cubeDataArray[j].Length; k++)
                {
                    int height = this._cubeDataArray[j][k].Count(d => d.Value == 0x00000001);

                    if (height < Globals.Instance.WaterLevel)
                    {
                        for (int l = height; l < Globals.Instance.WaterLevel; l++)
                        {
                            if (this._cubeDataArray[j][k][l].Value == 0x00000000) 
                                this._cubeDataArray[j][k][l] = this.CreateNewCubeData(0x00000001, true, true);
                        }
                    }
                }
            }
        }

        #endregion

        private List<Chunk> GenerateChunks(Effect cubeShader, int[,] heightMap)
        {
            List<Chunk> chunks = new();

            if(this.SceneMode == SceneMode.Normal) 
            {
                this.CreateAndFillCubesDataArray(heightMap);
            }
            else if (this.SceneMode == SceneMode.Layered)
            {
                for (int i = 0; i < 10; i++)
                {
                    Array.Clear(heightMap);
                    heightMap = this._noiseGenerator.GenerateNoise();
                    
                    if(i == 0)
                        this.CreateAndFillCubesDataArray(heightMap);
                    else
                        this.AddExtraLayerToCubesDataArray(heightMap);

                    Globals.Instance.Frequency += 0.01f;
                    Globals.Instance.MaxChunkHeight -= 10;
                    this._noiseGenerator.UpdateGeneratorValuesBasedOnGlobals();
                }
            }

            this.AddWaterDataToCubesDataArray();

            int iterSize = Globals.Instance.HeightMapSize / Globals.Instance.ChunkSize;
            for (int i = 0; i < iterSize; i++)
            {
                for (int l = 0; l < iterSize; l++)
                {
                    int startX = i * this._chunkWidth;
                    int endX = (i + 1) * this._chunkWidth;

                    int startZ = l * this._chunkDepth;
                    int endZ = (l + 1) * this._chunkDepth;

                    var chunk = new Chunk(cubeShader, new List<object>() { this._cubeDataArray, startX, startZ, endX, endZ });
                    chunks.Add(chunk);
                    this._vertexCount += chunk.Vertecies.Length;
                    this._indexCount += chunk.Indicies.Length;
                }
            }

            Array.Clear(this._cubeDataArray);
            this._cubeDataArray = null;

            GC.Collect();
            GC.WaitForPendingFinalizers();

            return chunks;
        }


        public void DrawHeightMap() 
        {
            Main.SpriteBatch.Draw(this._heightMapTexture, new Rectangle(0, 0, Globals.Instance.HeightMapSize, Globals.Instance.HeightMapSize), Color.White);
        }

        public void Update(MouseHandler mouseHandler, KeyboardHandler keyboardHandler) 
        {
            mouseHandler.HandleMouse(this.Camera, keyboardHandler.ZeroKeyBool);
            keyboardHandler.HandleKey(this.Camera);
            this.Camera.UpdateViewMatrix();
        }

        public void Render(float timeElapsed) 
        {
            /*
            this._water.Render(
                this._camera.WorldMatrix, 
                this._camera.ViewMatrix, 
                this._camera.ProjectionMatrix, timeElapsed);
            */

            this._chunks.ForEach(chunk => 
                    chunk.Render(
                        this._camera.WorldMatrix, 
                        this._camera.ViewMatrix, 
                        this._camera.ProjectionMatrix, 
                        timeElapsed));
            
            //this._chunkBatcher.Render(this._camera.WorldMatrix, this._camera.ViewMatrix, this._camera.ProjectionMatrix, this._cubeShader);
        }
    }
}
