float4x4 WorldViewProjection;
Texture theTexture;
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

//UI用
float4 PixelShaderFunction1(PixelShaderInput input) : COLOR0
{
	// 中央からの位置を-1 〜 1の範囲に正規化
	float2 pos = (input.textureCoordinates - float2(0.5, 0.5)) * 2.0;
	pos.y *= -1;

	//上記の位置から角度を算出（0-1の範囲）
	//また今回は上端から（つまり90度）のところから時計回りに徐々に透明にしたいので
	//90度だけ回転した状態に変換しておく
	float angle = (atan2(pos.y, pos.x) - atan2(1, 0)) / (3.1415926 * 2);

	//atan2では-180-180度の値が返されるので、0-360になるように正規化する
	//角度を0〜1にしているため、実質足すのは1。
	if (angle < 0) { angle += 1.0; }

	//指定されたcutoffの値がしきい値以上の場合のみ透明度を有効にする
	float cutoff = angle < Rate ? 0 : 1;
	cutoff *= Alpha;
	float4 color = tex2D(ColoredTextureSampler, input.textureCoordinates);
	color.r = color.r * color.a * cutoff;
	color.g = color.g * color.a * cutoff;
	color.b = color.b * color.a * cutoff;
	color.a = color.a * cutoff;	//Rate
	return color;
}
//UI用
technique Technique1
{
	pass Pass1
	{
		VertexShader = compile vs_2_0 VertexShaderFunction();
		PixelShader = compile ps_2_0 PixelShaderFunction1();
	}
}
