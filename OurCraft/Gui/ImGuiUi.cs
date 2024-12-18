using ImGuiNET;
using Microsoft.Xna.Framework;
using MonoGame.ImGui.Standard;
using OurCraft.Graphics;
using OurCraft.Graphics.DataTypes;
using OurCraft.Tools;
using NUM = System.Numerics;

namespace OurCraft.Gui
{
    public class ImGuiUi
    {
        private ImGUIRenderer _guiRenderer;
        
        private int _chunkCount = Globals.Instance.ChunkCount;
        private int _maxChunkHeight = Globals.Instance.MaxChunkHeight;
        private float _perlinFrequency = Globals.Instance.Frequency;
        private SceneMode _sceneMode = SceneMode.Normal;
        private float _fractalGain = Globals.Instance.FractalGain;
        private int _octaves = Globals.Instance.Octaves;
        private float _fractalLacunarity = Globals.Instance.FractalLacunarity;
        private int _waterLevel = Globals.Instance.WaterLevel;

        private bool _isInfoWindowShowing = false;
        private bool _isValueGuideWindowShowing = false;
        private bool _isControlPanelShowing = false;

        private NUM.Vector2 _infoWindowLoc = new NUM.Vector2(0f, 0f);
        private NUM.Vector2 _infoWindowSizeIfVisible = new NUM.Vector2(450f, 400f);
        private NUM.Vector2 _infoWindowSizeIfNotVisible = new NUM.Vector2(450f, 45f);

        private NUM.Vector2 _valueGuideWindowLocIfInfoVisible = new NUM.Vector2(0f, 400f);
        private NUM.Vector2 _valueGuideWindowLocIfInfoNotVisible = new NUM.Vector2(0f, 40f);
        private NUM.Vector2 _valueGuideWindowSizeIfVisible = new NUM.Vector2(450f, 250f);
        private NUM.Vector2 _valueGuideWindowSizeIfNotVisible = new NUM.Vector2(450f, 40f);

        private NUM.Vector2 _controlPanelWindowLoc = new NUM.Vector2(Main.Graphics.PreferredBackBufferWidth - 300, 0f);
        private NUM.Vector2 _controlPanelSizeIfVisible = new NUM.Vector2(300, 0f);
        private NUM.Vector2 _controlPanelSizeIfNotVisible = new NUM.Vector2(300, 40f);


        public ImGuiUi(Game main) 
        {
            this._guiRenderer = new ImGUIRenderer(main).Initialize().RebuildFontAtlas();
        }

        private ResetSceneData CreateSceneData() 
        {
            return new ResetSceneData()
            {
                ChunkCount = this._chunkCount,
                MaxChunkHeight = this._maxChunkHeight,
                PerlinFrequency = this._perlinFrequency,
                SceneMode = this._sceneMode,
                Octaves = this._octaves,
                FractalGain = this._fractalGain,
                FractalLacunarity = this._fractalLacunarity,
                WaterLevel = this._waterLevel
            };
        }

        private void DrawInfoWindow(Main main, GameTime gameTime) 
        {
            ImGuiIOPtr io = ImGui.GetIO();
            ImGuiWindowFlags windowFlags = ImGuiWindowFlags.NoDecoration | ImGuiWindowFlags.NoSavedSettings | ImGuiWindowFlags.NoFocusOnAppearing | ImGuiWindowFlags.NoMove;


            ImGui.SetNextWindowBgAlpha(0.35f);
            if (ImGui.Begin("InfoWindow", windowFlags))
            {
                ImGui.SetWindowPos(this._infoWindowLoc);
                ImGui.SetWindowFontScale(2.0f);

                if (ImGui.CollapsingHeader("InfoWindow"))
                {
                    this._isInfoWindowShowing = true;
                    ImGui.SetWindowSize(this._infoWindowSizeIfVisible, ImGuiCond.None);

                    ImGui.Text("Infos:");
                    ImGui.Separator();

                    ImGui.Text(main.GetFrameRates(gameTime));
                    ImGui.Separator();

                    ImGui.Text(string.Format("Mouse Position: ({0},{1})", io.MousePos.X, io.MousePos.Y));
                    ImGui.Separator();

                    int chunkCount = Globals.Instance.ChunkCount * Globals.Instance.ChunkCount;
                    ImGui.Text($"Number of Chunks: {chunkCount.ToString()}");
                    ImGui.Text($"Chunk size: {Globals.Instance.ChunkSize} * {Globals.Instance.ChunkSize}");
                    ImGui.Text($"Max chunk height: {Globals.Instance.MaxChunkHeight}");
                    ImGui.Text($"Number of cubes rendered: \n{Globals.Instance.CubeCount}");

                    ImGui.Separator();
                    ImGui.Text($"VertexCount: {main.VertexCount}");
                    ImGui.Text($"IndexCount: {main.IndexCount}");

                    ImGui.Separator();
                    string sceneMode = main.Scene.SceneMode == SceneMode.Normal ? "Normal" : "Layered";
                    ImGui.Text($"Scene Mode: {sceneMode}");
                }
                else 
                {
                    this._isInfoWindowShowing = false;
                    ImGui.SetWindowSize(this._infoWindowSizeIfNotVisible, ImGuiCond.None);
                }
            }

            ImGui.End();
        }

        private void DrawNoiseValueGuide(Main main) 
        {
            ImGuiIOPtr io = ImGui.GetIO();
            ImGuiWindowFlags windowFlags = ImGuiWindowFlags.NoDecoration | ImGuiWindowFlags.NoSavedSettings | ImGuiWindowFlags.NoFocusOnAppearing | ImGuiWindowFlags.NoMove;


            ImGui.SetNextWindowBgAlpha(0.35f);
            if (ImGui.Begin("Noise Values Guide", windowFlags))
            {
                if (this._isInfoWindowShowing)
                    ImGui.SetWindowPos(this._valueGuideWindowLocIfInfoVisible);
                else
                    ImGui.SetWindowPos(this._valueGuideWindowLocIfInfoNotVisible);

                ImGui.SetWindowFontScale(1.5f);

                if (ImGui.CollapsingHeader("Noise Values Guide"))
                {
                    this._isValueGuideWindowShowing = true;
                    ImGui.SetWindowSize(this._valueGuideWindowSizeIfVisible, ImGuiCond.None);

                    ImGui.Text("Noise Values Guide:");
                    ImGui.Separator();

                    ImGui.Text($"ChunkCount -> min: {Globals.Instance.ChunkCountMinMax.Item1}, max: {Globals.Instance.ChunkCountMinMax.Item2}");
                    ImGui.Text($"MaxChunkHeight -> min: {Globals.Instance.MaxChunkHeightMinMax.Item1}, max: {Globals.Instance.MaxChunkHeightMinMax.Item2}");
                    ImGui.Text($"WaterLevel -> min: {Globals.Instance.WaterLevelMinMax.Item1}, max: {Globals.Instance.WaterLevelMinMax.Item2}");
                    ImGui.Text($"Frequency -> min: {Globals.Instance.FrequencyMinMax.Item1}, max: {Globals.Instance.FrequencyMinMax.Item2} ");
                    ImGui.Text($"FractalGain -> min: {Globals.Instance.FractalGainMinMax.Item1}, max: {Globals.Instance.FractalGainMinMax.Item2}");
                    ImGui.Text($"Octaves -> min: {Globals.Instance.OctavesMinMax.Item1}, max: {Globals.Instance.OctavesMinMax.Item2}");
                    ImGui.Text($"FractalLacunarity -> min: {Globals.Instance.FractalLacunarityMinMax.Item1}, max: {Globals.Instance.FractalLacunarityMinMax.Item2}");
                }
                else
                {
                    this._isValueGuideWindowShowing = false;
                    ImGui.SetWindowSize(this._valueGuideWindowSizeIfNotVisible, ImGuiCond.None);
                }
            }

            ImGui.End();
        }

        private void DrawControlPanel(Main main)
        {
            ImGuiWindowFlags windowFlags = ImGuiWindowFlags.NoDecoration | ImGuiWindowFlags.NoSavedSettings | ImGuiWindowFlags.NoMove;
            ImGui.SetNextWindowBgAlpha(0.5f);

            if (ImGui.Begin("Control Panel", windowFlags))
            {

                ImGui.SetWindowPos(this._controlPanelWindowLoc);
                ImGui.SetWindowFontScale(1.5f);

                if (ImGui.CollapsingHeader("ControlPanel")) 
                {
                    this._isControlPanelShowing = true;
                    ImGui.SetWindowSize(this._controlPanelSizeIfVisible, ImGuiCond.None);

                    ImGui.Text("Control Panel");
                    ImGui.Separator();


                    //Set chunk count
                    ImGui.Text("Chunk Count:");
                    ImGui.InputInt("##ChunkCount", ref this._chunkCount);

                    //Set max chunk height
                    ImGui.Separator();
                    ImGui.Text("Max chunk height:");
                    ImGui.InputInt("##MaxChunkHeight", ref this._maxChunkHeight, 1);

                    //Set water level
                    ImGui.Separator();
                    ImGui.Text("Water Level:");
                    ImGui.InputInt("##WaterLevel", ref this._waterLevel, 1);

                    //Set perlin noise frequency
                    ImGui.Separator();
                    ImGui.Text("Perlin noise frequency");
                    ImGui.InputFloat("##Frequency", ref this._perlinFrequency, 0.001f);

                    //Set perlin noise octaves
                    ImGui.Separator();
                    ImGui.Text("Perlin Noise Octaves");
                    ImGui.InputInt("##Octaves", ref this._octaves, 1);

                    //Set fractal gain
                    ImGui.Separator();
                    ImGui.Text("Fractal Gain");
                    ImGui.InputFloat("##FractalGain", ref this._fractalGain, 0.1f);

                    //Set fractal lacunarity
                    ImGui.Separator();
                    ImGui.Text("Fractal Lacunarity");
                    ImGui.InputFloat("##FractalLacunarity", ref this._fractalLacunarity, 0.1f);


                    ImGui.Separator();
                    if (ImGui.Button("Change Scene Mode"))
                    {
                        this._sceneMode = this._sceneMode == SceneMode.Normal ? SceneMode.Layered : SceneMode.Normal;
                        main.ResetScene(this.CreateSceneData());
                    }

                    //Reset Scene
                    ImGui.Separator();
                    if (ImGui.Button("Reset Scene"))
                        main.ResetScene(this.CreateSceneData());
                }
                else 
                {
                    this._isControlPanelShowing = false;
                    ImGui.SetWindowSize(this._controlPanelSizeIfNotVisible, ImGuiCond.None);
                }
            }
            ImGui.End();
        }

        public void UpdatePrivateValuesBasedOnGlobals()
        {
            this._chunkCount = Globals.Instance.ChunkCount;
            this._maxChunkHeight = Globals.Instance.MaxChunkHeight;
            this._perlinFrequency = Globals.Instance.Frequency;
            this._fractalGain = Globals.Instance.FractalGain;
            this._octaves = Globals.Instance.Octaves;
            this._fractalLacunarity = Globals.Instance.FractalLacunarity;
            this._waterLevel = Globals.Instance.WaterLevel;
        }

        public void RenderUi(GameTime gameTime, Main main) 
        {
            this._guiRenderer.BeginLayout(gameTime);

            this.DrawInfoWindow(main, gameTime);
            this.DrawNoiseValueGuide(main);
            this.DrawControlPanel(main);

            this._guiRenderer.EndLayout();
        }
    }
}
