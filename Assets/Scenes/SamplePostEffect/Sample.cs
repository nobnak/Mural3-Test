using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Sample : ScriptableRendererFeature {

    [SerializeField]
    protected Tuner tuner = new Tuner();

    class CustomRenderPass : ScriptableRenderPass {

        public const string RES_MATERIAL = "Unlit_Sample";
        public readonly static string KEY_COMMNAND_BUFFER = typeof(CustomRenderPass).FullName;

        public readonly static int P_TmpTex = Shader.PropertyToID("_TmpTex");
        public readonly static int P_Throttle = Shader.PropertyToID("_Throttle");

        protected Material mat;

        public CustomRenderPass() {
            mat = Resources.Load<Material>(RES_MATERIAL);
        }

        #region interface
        public Tuner CurrTuner { get; set; } = new Tuner();

        #region ScriptableRenderPass
        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData) {
            var cmd = CommandBufferPool.Get(KEY_COMMNAND_BUFFER);
            var dst = renderingData.cameraData.renderer.cameraColorTarget;
            var dstDesc = renderingData.cameraData.cameraTargetDescriptor;

            dstDesc.depthBufferBits = 0;
            cmd.GetTemporaryRT(P_TmpTex, dstDesc);

            cmd.SetGlobalFloat(P_Throttle, CurrTuner.throttle);

            cmd.Blit(dst, P_TmpTex);
            cmd.Blit(P_TmpTex, dst, mat);

            context.ExecuteCommandBuffer(cmd);
            cmd.ReleaseTemporaryRT(P_TmpTex);
            CommandBufferPool.Release(cmd);
        }
        #endregion

        #endregion
    }

    CustomRenderPass m_ScriptablePass;

    /// <inheritdoc/>
    public override void Create() {
        m_ScriptablePass = new CustomRenderPass();
        m_ScriptablePass.renderPassEvent = RenderPassEvent.AfterRendering;
        m_ScriptablePass.CurrTuner = tuner;
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData) {
        renderer.EnqueuePass(m_ScriptablePass);
    }

    #region definitions
    [System.Serializable]
    public class Tuner {
        [Range(0f, 1f)]
        public float throttle = 1f;
    }
    #endregion
}


