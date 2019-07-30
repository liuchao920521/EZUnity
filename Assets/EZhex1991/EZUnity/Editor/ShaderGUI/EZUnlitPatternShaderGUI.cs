/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-02-25 19:52:09
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using EZhex1991.EZUnity;
using UnityEditor;

public class EZUnlitPatternShaderGUI : EZShaderGUI
{
    public enum PatternType
    {
        Chessboard,
        Diamond,
        Frame,
        Spot,
        Stripe,
        Triangle,
        Wave,
        Diagonal,
    }
    public enum DistortionType
    {
        None,
        Swirl,
    }

    private MaterialProperty _PatternType;
    private MaterialProperty _CoordMode;
    private MaterialProperty _SecondColor;
    private MaterialProperty _ScaleOffset;
    private MaterialProperty _PatternCenter;
    private MaterialProperty _FillRatio;
    private MaterialProperty _DistortionType;
    private MaterialProperty _Swirl;

    public override void OnEZShaderGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
    {
        MainTextureWithColorGUI(materialEditor, properties);

        _PatternType = FindProperty("_PatternType", properties);
        _CoordMode = FindProperty("_CoordMode", properties);
        _SecondColor = FindProperty("_SecondColor", properties);
        _ScaleOffset = FindProperty("_ScaleOffset", properties);
        _PatternCenter = FindProperty("_PatternCenter", properties);
        _FillRatio = FindProperty("_FillRatio", properties);
        _DistortionType = FindProperty("_DistortionType", properties);
        _Swirl = FindProperty("_Swirl", properties);

        materialEditor.ShaderProperty(_PatternType);
        materialEditor.ShaderProperty(_CoordMode);
        materialEditor.ShaderProperty(_SecondColor);
        materialEditor.ShaderProperty(_ScaleOffset);
        switch ((PatternType)_PatternType.floatValue)
        {
            case PatternType.Chessboard:
                materialEditor.ShaderProperty(_PatternCenter);
                break;
            case PatternType.Diamond:
                materialEditor.ShaderProperty(_PatternCenter);
                materialEditor.ShaderProperty(_FillRatio);
                break;
            case PatternType.Frame:
                materialEditor.ShaderProperty(_PatternCenter);
                materialEditor.ShaderProperty(_FillRatio);
                break;
            case PatternType.Spot:
                materialEditor.ShaderProperty(_PatternCenter);
                materialEditor.ShaderProperty(_FillRatio);
                break;
            case PatternType.Stripe:
                materialEditor.ShaderProperty(_FillRatio);
                break;
            case PatternType.Triangle:
                materialEditor.ShaderProperty(_PatternCenter);
                materialEditor.ShaderProperty(_FillRatio);
                break;
            case PatternType.Wave:
                materialEditor.ShaderProperty(_PatternCenter);
                materialEditor.ShaderProperty(_FillRatio);
                break;
            default:
                materialEditor.ShaderProperty(_PatternCenter);
                materialEditor.ShaderProperty(_FillRatio);
                break;
        }

        materialEditor.ShaderProperty(_DistortionType);
        switch ((DistortionType)_DistortionType.floatValue)
        {
            case DistortionType.None:
                break;
            case DistortionType.Swirl:
                materialEditor.ShaderProperty(_Swirl);
                break;
            default:
                materialEditor.ShaderProperty(_Swirl);
                break;
        }

        AdvancedOptionsGUI(materialEditor);
    }
}
