namespace OurCraft.Graphics.DataTypes
{
    public enum CubeType
    {
        DIRT = 0, STONE = 1, WATER = 2
    }

    public struct ByteCubeType
    {
        public byte Value { get; set; }
        public CubeType Type { get; set; }
    }

    public struct ResetSceneData
    {
        public int ChunkCount { get; set; }
        public int MaxChunkHeight { get; set; }
        public int WaterLevel { get; set; }
        public float PerlinFrequency { get; set; }
        public SceneMode SceneMode { get; set; }
        public int Octaves { get; set; }
        public float FractalGain { get; set; }
        public float FractalLacunarity { get; set; }
    }
}
