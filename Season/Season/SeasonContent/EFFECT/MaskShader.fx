float4x4 WorldViewProjection;
Texture theTexture;
Texture theMask;
float4 Color;
float Alpha;
float Rate;

sampler ColoredTextureSampler = sampler_state {
	texture = <theTexture>;
	magfilter = LINEAR;
	minfilter = LINEAR;
	mipfilter = POINT;
	AddressU = Clamp;
	AddressV = Clamp;
};

sampler ColoredTextureMask = sampler_state {
	texture = <theMask>;
	magfilter = LINEAR;
	minfilter = LINEAR;
	mipfilter = POINT;
	AddressU = Clamp;
	AddressV = Clamp;
};

struct VertexShaderInput
{
	float4 Position : POSITION0;
	float2 textureCoordinates : TEXCOORD0;
};

struct VertexShaderOutput
{
	float4 Position : POSITION0;
	float2 textureCoordinates : TEXCOORD0;
};


VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
	VertexShaderOutput output;

	output.Position = mul(input.Position, WorldViewProjection);
	output.textureCoordinates = input.textureCoordinates;

	return output;
}


struct PixelShaderInput {
	float2 textureCoordinates : TEXCOORD0;
};


//Maskあり（出現）
float4 PixelShaderFunction1(PixelShaderInput input) : COLOR0
{
	float4 color = tex2D(ColoredTextureSampler, input.textureCoordinates);
	float4 colorMask = tex2D(ColoredTextureMask, input.textureCoordinates);
	color.r = color.r * colorMask.r * (1 - Rate) * 3;
	color.g = color.g * colorMask.g * (1 - Rate) * 3;
	color.b = color.b * colorMask.b * (1 - Rate) * 3;
	color.a = color.a * colorMask.a * (1 - Rate);
	return color;
}

//Maskあり（消える）
float4 PixelShaderFunction2(PixelShaderInput input) : COLOR0
{
	float4 color = tex2D(ColoredTextureSampler, input.textureCoordinates);
	float4 colorMask = tex2D(ColoredTextureMask, input.textureCoordinates);
	color.r = color.r * colorMask.r * Rate;
	color.g = color.g * colorMask.g * Rate;
	color.b = color.b * colorMask.b * Rate;
	color.a = color.a * Rate;
	return color;
}

//Maskあり（出現）
technique Technique1
{
	pass Pass1
	{
		VertexShader = compile vs_2_0 VertexShaderFunction();
		PixelShader = compile ps_2_0 PixelShaderFunction1();
	}
}

//Maskあり（消える）
technique Technique2
{
	pass Pass1
	{
		VertexShader = compile vs_2_0 VertexShaderFunction();
		PixelShader = compile ps_2_0 PixelShaderFunction2();
	}
}

