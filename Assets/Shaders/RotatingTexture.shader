Shader "Custom/RotatingTexture" 
{
		Properties
		{
			_Color("Tint color",Color) = (1,1,1,1)
			_MainTex("Texture (RGBA)", 2D) = "white" {}
			_RotationSpeed("Rotation Speed", Float) = 2.0
			_Metallic("Metallic intensity",Range(0,1)) = 1.0
			_Smoothness("Smoothness",Range(0,1)) = 1.0
		}
		SubShader
		{
			Tags{ "RenderType" = "Opaque" }
			LOD 200

			CGPROGRAM

			#pragma surface surf Standard vertex:vert alpha:fade fullforwardshadows addshadow

			#pragma target 3.0
			
			sampler2D _MainTex;

			float _Cutoff;
			float _RotationSpeed;

			half _Metallic;
			half _Smoothness;

			fixed4 _Color;

			struct Input {
				float2 uv_MainTex;
			};

			void vert(inout appdata_full v)
			{
				v.texcoord.xy -= 0.5;

				float s = sin(_RotationSpeed * _Time);
				float c = cos(_RotationSpeed * _Time);

				float2x2 rotationMatrix = float2x2(c, -s, s, c);

				rotationMatrix *= 0.5;
				rotationMatrix += 0.5;

				rotationMatrix = rotationMatrix * 2 - 1;

				v.texcoord.xy = mul(v.texcoord.xy, rotationMatrix);
				v.texcoord.xy += 0.5;
			}

			void surf(Input IN, inout SurfaceOutputStandard o)
			{
				fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;

				o.Albedo = c.rgb;


				if (c.a > 0) 
				{
					o.Alpha = c.a;
					o.Metallic = _Metallic;
					o.Smoothness = _Smoothness;
				}
				else
					o.Alpha = 0;



			}

			ENDCG

	}

	FallBack "Diffuse"
}
