using Microsoft.Xna.Framework.Graphics;
using OurCraft.Graphics.DataTypes;
using OurCraft.Graphics.Meshes;
using OurCraft.Graphics.Vertextypes;
using OurCraft.Tools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace OurCraft.Graphics.WorldObjects
{
    public class Chunk : WorldObjectBase<VertexPositionNormalCubeTexture>
    {
        private int _startXOffset;
        private int _startZOffset;
        private int _endXOffset;
        private int _endZOffset;
        private Stopwatch _stopwatch;

        private ByteCubeType[][][] _chunkDataArray;
        private List<List<List<Cube>>> _cubes;

        public Chunk(Effect shader, List<object> param) : base(shader, param)
        {
            
        }

        protected override void UniqeConstructorBeforeBase(object param)
        {
            this._chunkDataArray = (param as List<object>)[0] as ByteCubeType[][][];
            this._startXOffset = (int)(param as List<object>)[1];
            this._startZOffset = (int)(param as List<object>)[2];
            this._endXOffset = (int)(param as List<object>)[3]; 
            this._endZOffset = (int)(param as List<object>)[4];

            this._cubes = new();
            this._stopwatch = new Stopwatch();
        }

        private void GenerateCubes()
        {
            //this._stopwatch.Start();

            byte CountNeighbours(int i, int j, int k)
            {
                return (byte)(this._chunkDataArray[i + 1][j][k].Value & this._chunkDataArray[i - 1][j][k].Value
                    & this._chunkDataArray[i][j + 1][k].Value & this._chunkDataArray[i][j - 1][k].Value
                    & this._chunkDataArray[i][j][k + 1].Value & this._chunkDataArray[i][j][k - 1].Value);
                
            }

            List<Direction> allSides = new() { Direction.FORWARD, Direction.BACKWARD, Direction.LEFT, Direction.RIGHT, Direction.UP, Direction.DOWN };

            int cubesAborted = 0;
            int cubeFacesAborted = 0;

            for (int i = this._startXOffset; i < this._endXOffset; i++)
            {
                List<List<Cube>> cubeList2d = new();
                
                for (int j = this._startZOffset; j < this._endZOffset; j++)
                {
                    List<Cube> cubeList1d = new();
                    int height = this._chunkDataArray[i][j].Count(d => d.Value == 0x00000001);

                    for (int k = 0; k < height; k++)
                    {
                        List<Direction> sides = new();
                        byte currentCube = this._chunkDataArray[i][j][k].Value;

                        if (i != 0 && i != this._chunkDataArray.Length - 1 && 
                            j != 0 && j != this._chunkDataArray[i].Length - 1 && 
                            k != 0 && k != this._chunkDataArray[i][j].Length - 1)
                        {
                            byte neighbourCount = CountNeighbours(i, j, k);
                            if (neighbourCount == 0x00000000)
                            {
                                if ((currentCube & this._chunkDataArray[i][j - 1][k].Value) == 0x00000000) sides.Add(Direction.BACKWARD);
                                else cubeFacesAborted++;

                                if ((currentCube & this._chunkDataArray[i][j + 1][k].Value) == 0x00000000) sides.Add(Direction.FORWARD);
                                else cubeFacesAborted++;

                                if ((currentCube & this._chunkDataArray[i - 1][j][k].Value) == 0x00000000) sides.Add(Direction.LEFT);
                                else cubeFacesAborted++;

                                if ((currentCube & this._chunkDataArray[i + 1][j][k].Value) == 0x00000000) sides.Add(Direction.RIGHT);
                                else cubeFacesAborted++;

                                if ((currentCube & this._chunkDataArray[i][j][k - 1].Value) == 0x00000000) sides.Add(Direction.DOWN);
                                else cubeFacesAborted++;

                                if ((currentCube & this._chunkDataArray[i][j][k + 1].Value) == 0x00000000) sides.Add(Direction.UP);
                                else cubeFacesAborted++;
                            }

                            else cubesAborted++;
                        }
                        else 
                        {
                            if(i == 0) sides.Add(Direction.LEFT);
                            else if ((currentCube & this._chunkDataArray[i - 1][j][k].Value) == 0x00000000) sides.Add(Direction.LEFT);
                            else cubeFacesAborted++;

                            if (i == this._chunkDataArray.Length - 1) sides.Add(Direction.RIGHT);
                            else if ((currentCube & this._chunkDataArray[i + 1][j][k].Value) == 0x00000000) sides.Add(Direction.RIGHT);
                            else cubeFacesAborted++;

                            if (j == 0) sides.Add(Direction.BACKWARD);
                            else if ((currentCube & this._chunkDataArray[i][j - 1][k].Value) == 0x00000000) sides.Add(Direction.BACKWARD);
                            else cubeFacesAborted++;

                            if (j == this._chunkDataArray[i].Length - 1) sides.Add(Direction.FORWARD);
                            else if ((currentCube & this._chunkDataArray[i][j + 1][k].Value) == 0x00000000) sides.Add(Direction.FORWARD);
                            else cubeFacesAborted++;
                            
                            /*
                            if (k == 0) sides.Add(Direction.DOWN);
                            else if ((currentCube & this._chunkByteArray[i][j][k - 1]) == 0x00000000) sides.Add(Direction.DOWN);
                            else cubeFacesAborted++;
                            */

                            if (k == this._chunkDataArray[i][j].Length - 1) sides.Add(Direction.UP);
                            else if((currentCube & this._chunkDataArray[i][j][k + 1].Value) == 0x00000000) sides.Add(Direction.UP);
                            else cubeFacesAborted++;
                        }
                        cubeList1d.Add(new Cube(i, k, j, sides, (float)((int)this._chunkDataArray[i][j][k].Type)));
                    }
                    cubeList2d.Add(cubeList1d);
                }
                this._cubes.Add(cubeList2d);
            }
            //Debug.WriteLine($"Cubes aborted in chunk: {cubesAborted.ToString()}");
            //Debug.WriteLine($"Cube faces aborted in chunk: {cubeFacesAborted}");
            //Debug.WriteLine("---------------------------------------------");

            //this._stopwatch.Stop();
            //Debug.WriteLine($"Cube generating time: {this._stopwatch.ElapsedMilliseconds} milliseconds");
            //this._stopwatch.Reset();

            this._chunkDataArray = null;
        }

        protected override void GenerateVerteciesAndIndicies()
        {
            this.GenerateCubes();
            Dictionary<VertexPositionNormalCubeTexture, int> vertexIndexHashMap = new();

            //this._stopwatch.Start();
            foreach (var list2d in this._cubes)
            {
                foreach (var list1d in list2d)
                {
                    foreach (var cube in list1d)
                    {
                        foreach (var v in cube.Vertecies)
                        {
                            if (!vertexIndexHashMap.ContainsKey(v))
                            {
                                this._vertecies.Add(v);
                                vertexIndexHashMap[v] = this._vertecies.Count - 1;
                            }
                        }

                        foreach (var ind in cube.Indicies)
                        {
                            var vertexValue = cube.Vertecies[ind];
                            this._indicies.Add((uint)vertexIndexHashMap[vertexValue]);
                        }
                        Globals.Instance.CubeCount++;
                    }
                }
            }
            //this._stopwatch.Stop( );
            //Debug.WriteLine($"Vertex generating time: {this._stopwatch.ElapsedMilliseconds} milliseconds");
            //this._stopwatch.Reset( );
            //this._stopwatch = null;
        }
    }
}