TextureCube<float4> DirtBlockCubeMap;
TextureCube<float4> RockBlockCubeMap;
TextureCube<float4> WaterBlockCubeMap;

matrix World;
matrix View;
matrix Projection;

float3 LightDirection;
float3 LightColor;
float3 AmbientColor = float3(0.05, 0.05, 0.05);


samplerCUBE dirBlockSamplerSampler = sampler_state
{
    Texture = <DirtBlockCubeMap>;
    MAGFILTER = LINEAR;
    MINFILTER = ANISOTROPIC;
    MIPFILTER = LINEAR;
    AddressU = Wrap;
    AddressV = Wrap;
    AddressW = Wrap;
};


samplerCUBE rockBlockSampler = sampler_state
{
    Texture = <RockBlockCubeMap>;
    MAGFILTER = LINEAR;
    MINFILTER = ANISOTROPIC;
    MIPFILTER = LINEAR;
    AddressU = Wrap;
    AddressV = Wrap;
    AddressW = Wrap;
};

samplerCUBE waterBlockSampler = sampler_state
{
    Texture = <WaterBlockCubeMap>;
    MAGFILTER = LINEAR;
    MINFILTER = ANISOTROPIC;
    MIPFILTER = LINEAR;
    AddressU = Wrap;
    AddressV = Wrap;
    AddressW = Wrap;
};

struct VS_INPUT
{
    float4 Position : POSITION;
    float3 Normal : NORMAL;
    float3 TexCoord : TEXCOORD0;
    float CubeType : TEXCOORD1;
};

struct PS_INPUT
{
    float4 Position : SV_POSITION;
    float3 Normal : TEXCOORD1;
    float3 TexCoord : TEXCOORD0;
    float CubeType : TEXCOORD2;
};

PS_INPUT VS(VS_INPUT input)
{
    PS_INPUT output;
    
    float4 worldPosition = mul(input.Position, World);
    output.Position = mul(worldPosition, View);
    output.Position = mul(output.Position, Projection);
    //output.WorldPosition = worldPosition;
    
    output.TexCoord = input.TexCoord;
    output.Normal = input.Normal;
    output.CubeType = input.CubeType;
    
    return output;
};

float4 PS(PS_INPUT input) : COLOR
{
    float3 lightDir = normalize(LightDirection);
    
    float diffuseFactor = max(dot(input.Normal, -lightDir), 0);
    float3 diffuse = LightColor * diffuseFactor;
    
    float3 finalColor = diffuse + AmbientColor;

    //float3 direction = normalize(input.WorldPosition.xyz) - normalize(input.WorldPosition.xyz + input.Normal);
    float4 textureColor;
    if (input.CubeType == 0)
    {
        textureColor = texCUBE(dirBlockSamplerSampler, input.TexCoord);
    }
    else if (input.CubeType == 1)
    {
        textureColor = texCUBE(rockBlockSampler, input.TexCoord);
    }
    else
    {
        textureColor = texCUBE(waterBlockSampler, input.TexCoord);
    }
    
    return textureColor + float4(finalColor, 0);
};

technique BasicCubemap
{
    pass P0
    {
        VertexShader = compile vs_3_0 VS();
        PixelShader = compile ps_3_0 PS();
    }
};