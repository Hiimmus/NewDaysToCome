    Shader "MediumSolid/Custom/Wraper"
    {
        Properties
        {
            _MainTex("Main Tex", 2D) = "white"
            _Count("Count", Integer) = 1
        }
        SubShader
        {
            Tags { "RenderType"="Opaque" }
            LOD 100
     
            Pass
            {
                Cull Off
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
     
                #include "UnityCG.cginc"
     
                sampler2D _MainTex;
                float4 _MainTex_ST;
                uniform half _Count;
     
                struct appdata
                {
                    float4 vertex : POSITION;
                    float2 uv : TEXCOORD0;
                };
     
                struct v2f
                {
                    float2 uv : TEXCOORD0;
                    float4 vertex : SV_POSITION;
                };
     
     
                v2f vert (appdata v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = TRANSFORM_TEX(v.uv, _MainTex);
     
                    return o;
                }
     
                // digit rendering from https://www.shadertoy.com/view/4sBSWW
                float DigitBin( const int x )
                {
                    return x==0?480599.0:x==1?139810.0:x==2?476951.0:x==3?476999.0:x==4?350020.0:x==5?464711.0:x==6?464727.0:x==7?476228.0:x==8?481111.0:x==9?481095.0:0.0;
                }
     
                float PrintValue( float2 vStringCoords, float fValue, float fMaxDigits, float fDecimalPlaces )
                {      
                    if ((vStringCoords.y < 0.0) || (vStringCoords.y >= 1.0)) return 0.0;
                   
                    bool bNeg = ( fValue < 0.0 );
                    fValue = abs(fValue);
                   
                    float fLog10Value = log2(abs(fValue)) / log2(10.0);
                    float fBiggestIndex = max(floor(fLog10Value), 0.0);
                    float fDigitIndex = fMaxDigits - floor(vStringCoords.x);
                    float fCharBin = 0.0;
                    if(fDigitIndex > (-fDecimalPlaces - 1.01)) {
                        if(fDigitIndex > fBiggestIndex) {
                            if((bNeg) && (fDigitIndex < (fBiggestIndex+1.5))) fCharBin = 1792.0;
                        } else {      
                            if(fDigitIndex == -1.0) {
                                if(fDecimalPlaces > 0.0) fCharBin = 2.0;
                            } else {
                                float fReducedRangeValue = fValue;
                                if(fDigitIndex < 0.0) { fReducedRangeValue = frac( fValue ); fDigitIndex += 1.0; }
                                float fDigitValue = (abs(fReducedRangeValue / (pow(10.0, fDigitIndex))));
                                fCharBin = DigitBin(int(floor(fmod(fDigitValue, 10.0))));
                            }
                        }
                    }
                    return floor(fmod((fCharBin / pow(2.0, floor(frac(vStringCoords.x) * 4.0) + (floor(vStringCoords.y * 5.0) * 4.0))), 2.0));
                }
                 
                fixed4 frag (v2f i) : SV_Target
                {
                    int n = _Count; // doesn't work properly with some values (e.g. 30)
                    // int n = 30; // works fine with any value
                    float x_i = floor(i.uv.x*n);
                    float x_f = frac(i.uv.x*n);
                    float cell1 = x_i/n;
                    float cell2 = fmod((x_i+1.0), n)/n;
                    float val = lerp(cell1, cell2, x_f*x_f*(3.0-2.0*x_f));
     
                    float num = PrintValue(i.uv * float2(15.0, 5.0) + float2(8.0,-2.0), _Count, 10, 10);
                    val = lerp(val, 0.0, num);
     
                    return fixed4(val.xxx, 1.0);
                }
                ENDCG
            }
        }
    }
