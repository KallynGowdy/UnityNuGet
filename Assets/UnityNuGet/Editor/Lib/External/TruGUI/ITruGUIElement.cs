using System;
namespace TruGUI
{
    /// <summary>
    /// Defines an interface for a TruGUIElement.
    /// </summary>
    public interface ITruGUIElement : IActionable
    {
        TruGUI.Controls.TruGUIElement ActionElement
        {
            get;
            set;
        }
        object ActionResult
        {
            get;
        }
        void AddOption(TruGUI.ElementLayoutOption option);
        void AnimateTo(float width, float height, float animateRate);
        void AnimateTo(float width, float height, float widthRate, float heightRate);
        void AnimateTo(TruGUI.Controls.TruGUIElement.ColapseExpandOptions options, float length, float animateRate);
        UnityEngine.Color BackgroundColor
        {
            get;
            set;
        }
        void Colapse(TruGUI.Controls.TruGUIElement.ColapseExpandOptions options);
        void ColapseAnimate(TruGUI.Controls.TruGUIElement.ColapseExpandOptions options, float animateRate);
        void ColapseAnimate(TruGUI.Controls.TruGUIElement.ColapseExpandOptions options, float widthRate, float heightRate);
        UnityEngine.Color Color
        {
            set;
        }
        UnityEngine.GUIContent Content
        {
            get;
            set;
        }
        UnityEngine.Color ContentColor
        {
            get;
            set;
        }
        UnityEngine.Rect CurrentRect
        {
            get;
            set;
        }
        int Depth
        {
            get;
            set;
        }
        UnityEngine.Vector2 Dimentions
        {
            get;
            set;
        }
        void Draw();
        TruGUI.ElementLayoutOption[] ElementLayoutOptions
        {
            get;
            set;
        }
        bool Enabled
        {
            get;
            set;
        }
        void Expand(float width, float height);
        void Expand(TruGUI.Controls.TruGUIElement.ColapseExpandOptions options, float length);
        UnityEngine.GUILayoutOption[] GUILayoutOptions
        {
            get;
        }
        UnityEngine.GUIStyle GUIStyle
        {
            get;
            set;
        }
        bool HasActionTaken
        {
            get;
        }
        bool HasRectInfo
        {
            get;
        }
        bool HasStyle
        {
            get;
        }
        bool IsAnimating();
        bool IsFocused
        {
            get;
        }
        bool IsLayout
        {
            get;
            set;
        }
        bool IsVisible
        {
            get;
            set;
        }
        UnityEngine.Matrix4x4 Matrix
        {
            get;
            set;
        }
        string Name
        {
            get;
            set;
        }
        event TruGUI.Controls.TruGUIElement.AnimationStateChanged OnAnimationDone;
        event TruGUI.Controls.TruGUIElement.AnimationStateChanged OnAnimationRestarted;
        event TruGUI.Controls.TruGUIElement.AnimationStateChanged OnAnimationStarted;
        event TruGUI.Controls.TruGUIElement.OnChanged OnFieldChanged;
        event TruGUI.Controls.TruGUIElement.Focus OnMouseFocusChanged;
        event TruGUI.Controls.TruGUIElement.Focus OnMouseFocusSame;
        void PlusEqualsDimentions(float width, float height = 0);
        UnityEngine.Vector2 Position
        {
            get;
            set;
        }
        UnityEngine.Vector4 PositionDimentions
        {
            get;
            set;
        }
        TruGUI.Enums.ElementReturnStyle ReturnStyle
        {
            get;
        }
        void SetActionProperties(TruGUI.Controls.TruGUIElement element, object result);
        UnityEngine.GUISkin Skin
        {
            get;
            set;
        }
        void StartAnimate(UnityEngine.Rect futureRect, UnityEngine.Rect rateRect);
    }

    /// <summary>
    /// Defines an interface for a generic TruGUIElement
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ITruGUIElement<T> : ITruGUIElement
    {
        new T ActionResult
        {
            get;
        }
        new event TruGUI.Controls.TruGUIElement<T>.Action<T> OnActionTaken;
        void SetActionProperties(TruGUI.Controls.TruGUIElement element, T result);
    }
}
