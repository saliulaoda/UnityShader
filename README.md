# UnityShader
整理一些UnityShader效果  

##################################################################################    

#2017-08-31 更新   
## Mirror Reflection 镜面反射效果（RenderTexture方法实现）    
-观察相机根据镜面计算出镜像相机（关于镜面对称），把镜像相机渲染出来的RenderTexture传参给镜面shader，采样结果跟镜面混合    
-Assets\Examples\MirrorReflection    
-PS：    
	-1，OnWillRenderObject的时候计算一次相机位置    
	-2，镜像相机culling mask需要忽略镜面          
-摘自博主http://www.gad.qq.com/article/detail/18544    