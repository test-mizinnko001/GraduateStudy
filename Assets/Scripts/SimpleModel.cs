using UnityEngine;
using live2d;

[ExecuteInEditMode]
public class SimpleModel: MonoBehaviour 
{
	private Live2DModelUnity live2DModel;
    private Live2DMotion motion;
    private MotionQueueManager motionMgr;

    private Matrix4x4 live2DCanvasPos;

    public TextAsset mocFile ;
    public Texture2D[] textureFiles ;
    public TextAsset motionFile;

	void Start () 
	{
        Live2D.init();

        live2DModel = Live2DModelUnity.loadModel(mocFile.bytes);

        for (int i = 0; i < textureFiles.Length; i++)
        {
            live2DModel.setTexture(i, textureFiles[i]);
        }

        float modelWidth = live2DModel.getCanvasWidth();
        live2DCanvasPos = Matrix4x4.Ortho(0, modelWidth, modelWidth, 0, -50.0f, 50.0f);

        motionMgr = new MotionQueueManager();
        motion = Live2DMotion.loadMotion(motionFile.bytes);
	}
	

    void Update()
    {
        if (live2DModel == null) return;
        live2DModel.setMatrix(transform.localToWorldMatrix * live2DCanvasPos);

        if (!Application.isPlaying)
        {
            live2DModel.update();
            return;
        }

        if (motionMgr.isFinished()==true)
        {
			motionMgr.startMotion(motion);
        }

        motionMgr.updateParam(live2DModel);

        live2DModel.update();
    }

	
	void OnRenderObject()
	{
		//背景との描画順調整
		if (SystemMgr.loadBackBoradUsabale == false) {
			if (live2DModel == null)
				return;
			/*// MainCamera以外で描画する
			if (Camera.current.tag == "MainCamera")
				return;
			*/
			live2DModel.draw ();
		}
	}
}