Texture2D Texture : register(t0);
SamplerState Sampler : register(s0);

matrix World;
matrix View;
matrix Projection;

//float amplitude = 0.1f;
//float frequency = 2.0f;
//float speed = 1.0f;
float time;


struct VS_INPUT
{
    float4 Position : POSITION;
    float2 TexCoord : TEXCOORD0;
};

struct PS_INPUT
{
    float4 Position : SV_POSITION;
    float2 TexCoord : TEXCOORD0;
};

PS_INPUT VS(VS_INPUT input)
{
    PS_INPUT output;
    
    output.TexCoord = input.TexCoord;
    
    float4 worldPosition = mul(input.Position, World);
    output.Position = mul(worldPosition, View);
    output.Position = mul(output.Position, Projection);
    output.Position.y += sin(time + output.Position.x * 8) / 8;
    
    return output;
};

float4 PS(PS_INPUT input) : COLOR
{
    return Texture.Sample(Sampler, input.TexCoord);
};

technique BasicCubemap
{
    pass P0
    {
        VertexShader = compile vs_3_0 VS();
        PixelShader = compile ps_3_0 PS();
    }
};