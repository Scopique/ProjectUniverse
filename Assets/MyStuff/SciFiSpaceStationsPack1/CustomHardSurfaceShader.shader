Shader "Custom/CustomHardSurfaceShader" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_MainColor("Color", Color) = (0,0,0,0)
		_SpecularMap ("Specular map", 2D) = "white" {}
		

		_Shininess ("Shininess", Range(0,2)) = 0
		_SpecIntensity ("Specular intensity", float) = 0

		_BumpMap ("Bumpmap", 2D) = "bump" {}
		_EmissionMap ("EmissionMap", 2D) = "bump" {}
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Specular
		
	    half4 LightingSpecular (SurfaceOutput s, half3 lightDir, half3 viewDir, half atten) {
	        half3 h = normalize (lightDir + viewDir);

	        half diff = max (0, dot (s.Normal, lightDir));

	        float nh = max (0, dot (s.Normal, h));
	        float spec = pow (nh, 48.0 * s.Gloss);

	        half4 c;
	        //c.rgb = (s.Albedo * _LightColor0.rgb * diff + _LightColor0.rgb * spec * s.Specular) * (atten * 2);
	        c.rgb = (s.Albedo  * _LightColor0.rgb * diff + _LightColor0.rgb  * spec * s.Specular) * (atten * 2);
	        c.a = s.Alpha;
	        return c;
	    }

		sampler2D _MainTex;
		sampler2D _SpecularMap;
	
		sampler2D _BumpMap;
		sampler2D _EmissionMap;
		
		float _Shininess;
		float _SpecIntensity;
	
		
		half4 _MainColor;
		
		struct Input {
			float2 uv_MainTex;			
		
		};
		
	

		void surf (Input IN, inout SurfaceOutput o) {
			half4 diff = tex2D (_MainTex, IN.uv_MainTex);
			
			half4 spec = tex2D (_SpecularMap, IN.uv_MainTex);
			half4 lights = tex2D(_EmissionMap, IN.uv_MainTex);
	
			
			o.Albedo = diff.rgb * _MainColor.rgb;
			
			o.Normal = UnpackNormal (tex2D (_BumpMap, IN.uv_MainTex));
			o.Alpha = diff.a;
			o.Specular = spec.r * _SpecIntensity;
			o.Gloss = spec.b * _Shininess;
			o.Emission = lights;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
