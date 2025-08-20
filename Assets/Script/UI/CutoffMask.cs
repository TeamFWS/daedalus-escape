using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class CutoffMask : Image
{
    private Animator _animator;

    public override Material materialForRendering
    {
        get
        {
            var rendering = new Material(base.materialForRendering);
            rendering.SetInt("_StencilComp", (int)CompareFunction.NotEqual);
            return rendering;
        }
    }

    protected override void Start()
    {
        base.Start();
        _animator = transform.parent.gameObject.GetComponent<Animator>();
    }

    public void StartTransition()
    {
        _animator.Play("CircleTransition");
    }
}