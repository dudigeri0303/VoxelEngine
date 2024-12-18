using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using OurCraft.InputDevices;
using OurCraft.Graphics;
using OurCraft.Gui;
using OurCraft.Tools;
using System;
using OurCraft.Graphics.DataTypes;

namespace OurCraft
{
    public class Main : Game
    {
        public static GraphicsDeviceManager Graphics { get; private set; }
        public static SpriteBatch SpriteBatch { get; private set; }

        private RasterizerState _rasterizerState;

        private Camera _camera;
        private Scene _scene;

        private KeyboardHandler _keyboardHandler;
        private MouseHandler _mouseHandler;

        private ImGuiUi _gui;

        private float _timeElapsed = 0.0f;

        //Contents
        private TextureCube _debugCubaMap;
        private TextureCube _dirtBlockCubeMap;
        private TextureCube _rockBlockCubeMap;
        private TextureCube _waterBlockCubeMap;

        private Texture2D _waterTexture;

        private Effect _cubeShader;
        private Effect _waterShader;

        public Scene Scene => _scene;

        public Main()
        {
            Graphics = new GraphicsDeviceManager(this);

            Graphics.PreferredBackBufferWidth = Globals.Instance.WindowWidth;
            Graphics.PreferredBackBufferHeight = Globals.Instance.WindowHeight;
            Graphics.GraphicsProfile = GraphicsProfile.HiDef;
            Graphics.PreferMultiSampling = true;
            Graphics.ToggleFullScreen();

            Mouse.SetPosition(Main.Graphics.PreferredBackBufferWidth / 2, Main.Graphics.PreferredBackBufferHeight / 2);

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        { 
            base.Initialize();
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            this._rasterizerState = new RasterizerState
            {
                CullMode = CullMode.CullCounterClockwiseFace,
                FillMode = FillMode.Solid,
                DepthClipEnable = true,
                ScissorTestEnable = false,
                MultiSampleAntiAlias = true
            };

            this._camera = new Camera();
            this._keyboardHandler = new KeyboardHandler();
            this._mouseHandler = new MouseHandler();

            this._gui = new ImGuiUi(this);
            
            //Loading Contents
            this._debugCubaMap = Content.Load<TextureCube>("debug_cubemap");
            this._waterTexture = Content.Load<Texture2D>("wave2");
            this._dirtBlockCubeMap = Content.Load<TextureCube>("dirt_block_cubemap");
            this._rockBlockCubeMap = Content.Load<TextureCube>("rock_block_cubemap");
            this._waterBlockCubeMap = Content.Load<TextureCube>("water_block_cubemap");

            this._cubeShader = Content.Load<Effect>("CubeShader");
            this._waterShader = Content.Load<Effect>("WaterShader");

            //Set shader parameters
            this._cubeShader.Parameters["DirtBlockCubeMap"].SetValue(this._dirtBlockCubeMap);
            this._cubeShader.Parameters["RockBlockCubeMap"].SetValue(this._rockBlockCubeMap);
            this._cubeShader.Parameters["WaterBlockCubeMap"].SetValue(this._waterBlockCubeMap);
            this._cubeShader.Parameters["LightDirection"].SetValue(new Vector3(-1f, 1f, 1f));
            this._cubeShader.Parameters["LightColor"].SetValue(new Vector3(0.5f, 0.42f, 0.4f));

            this._waterShader.Parameters["Sampler+Texture"].SetValue(this._waterTexture);

            this._scene = new Scene(this._cubeShader, this._waterShader, this._camera, SceneMode.Normal);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            this._timeElapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            this._scene.Update(this._mouseHandler, this._keyboardHandler);
            
            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            GraphicsDevice.RasterizerState = this._rasterizerState;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.BlendState = BlendState.Opaque;

            this._scene.Render(this._timeElapsed);

            GraphicsDevice.DepthStencilState = DepthStencilState.None;
            GraphicsDevice.BlendState = BlendState.AlphaBlend;

            base.Draw(gameTime);

            this._gui.RenderUi(gameTime, this);
        }

        #region Unique methods for the Gui (and other stuff)
        public string GetFrameRates(GameTime gameTime)
        {
            var fps = (1 / gameTime.ElapsedGameTime.TotalSeconds);
            return $"Current FPS: {Math.Round(fps, 4).ToString()}";
        }

        //public void ResetScene(int chunkCount, int maxChunkHeight, float perlinFrequency, SceneMode sceneMode, int octaves, float fractalGain, float fractalLacunarity)
        public void ResetScene(ResetSceneData sceneData)
        {
            bool isNormalScene = this._scene.SceneMode == SceneMode.Normal;

            this._scene.Dispose();

            //If scene mode is changed from layered to normal, we need to reset the values
            if (sceneData.SceneMode == SceneMode.Normal && !isNormalScene)
            {
                sceneData.MaxChunkHeight = Globals.Instance.StartingMaxChunkHeightForNormalScene;
                sceneData.ChunkCount = Globals.Instance.StartingChunkCountForNormalScene;
                sceneData.PerlinFrequency = Globals.Instance.StartingFrequencyForNormalScene;
                sceneData.WaterLevel = Globals.Instance.StartingWaterLevelForNormalScene;
            }

            //If scene mode is changed from normal to layered, we need to reset the values
            if (sceneData.SceneMode == SceneMode.Layered && isNormalScene) 
            {
                sceneData.MaxChunkHeight = Globals.Instance.StartingMaxChunkHeightForLayeredScene;
                sceneData.ChunkCount = Globals.Instance.StartingChunkCountForLayeredScene;
                sceneData.PerlinFrequency = Globals.Instance.StartingFrequencyForLayeredScene;
                sceneData.WaterLevel = Globals.Instance.StartingWaterLevelForLayeredScene;
            }

            this.UpdateGlobalAndPerlinValuesBasedOnGui(sceneData);
            this.UpdateNoiseValues();
            this._gui.UpdatePrivateValuesBasedOnGlobals();

            this._scene = new Scene(this._cubeShader, this._waterShader, this._camera, sceneData.SceneMode);
        }

        private void UpdateGlobalAndPerlinValuesBasedOnGui(ResetSceneData sceneData) 
        {

            Globals.Instance.CubeCount = 0;
            Globals.Instance.ChunkCount = sceneData.ChunkCount;
            Globals.Instance.MaxChunkHeight = sceneData.MaxChunkHeight;
            Globals.Instance.Frequency = sceneData.PerlinFrequency;
            Globals.Instance.Octaves = sceneData.Octaves;
            Globals.Instance.FractalGain = sceneData.FractalGain;
            Globals.Instance.FractalLacunarity = sceneData.FractalLacunarity;
            Globals.Instance.WaterLevel = sceneData.WaterLevel;
        }

        private void UpdateNoiseValues() 
        {
            this._scene.NoiseGenerator.UpdateGeneratorValuesBasedOnGlobals();
        }

        public int VertexCount => this._scene.VertexCount;
        public int IndexCount => this._scene.IndexCount;
        #endregion
    }
}
