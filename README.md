# UnityShader
整理一些UnityShader效果  

##################################################################################    

#2017-09-08 更新   
## 顶点动画    
-顶点着色器阶段修改坐标，从而修改网格形状    
-Assets\Examples\VertexAnimate    
-常见用途，模拟动画，比如水波浪，树叶，草，头发，飘带，旗帜的风动    
-摘自 《Unity Shader入门精要》 冯乐乐    
 博主http://superzhan.cn/blog/2016/08/17/unity_surface_vertex/    

##################################################################################    

#2017-09-07 更新   
## 贴图动画    
-时间轴上更改采样UV，达到更改渲染结果的目的    
-Assets\Examples\TextureAnimation    
-常见用途，动画表情，滚动物体（背景，水，瀑布），原理简单，适用性强，性能压力也不大    
-摘自 《Unity Shader入门精要》 冯乐乐    

##################################################################################    

#2017-08-31 更新   
## Mirror Reflection 镜面反射效果（RenderTexture方法实现）    
-观察相机根据镜面计算出镜像相机（关于镜面对称），把镜像相机渲染出来的RenderTexture传参给镜面shader，采样结果跟镜面混合    
-Assets\Examples\MirrorReflection    
-PS：    
	-1，OnWillRenderObject的时候计算一次相机位置    
	-2，镜像相机culling mask需要忽略镜面          
-摘自博主http://www.gad.qq.com/article/detail/18544    