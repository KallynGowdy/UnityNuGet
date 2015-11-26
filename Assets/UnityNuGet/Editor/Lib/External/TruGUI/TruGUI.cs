//  Copyright 2013 Kallyn Gowdy
//
//     Licensed under the Apache License, Version 2.0 (the "License");
//     you may not use this file except in compliance with the License.
//     You may obtain a copy of the License at
//
//         http://www.apache.org/licenses/LICENSE-2.0
//
//     Unless required by applicable law or agreed to in writing, software
//     distributed under the License is distributed on an "AS IS" BASIS,
//     WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//     See the License for the specific language governing permissions and
//     limitations under the License.

using System;
using System.Collections.Generic;
using System.Linq;
using TruGUI.Controls;
using UnityEditor;
using UnityEngine;
using System.Reflection;


namespace TruGUI
{

    namespace Enums
    {

        public enum GUITypes
        {
            Button,
            RepeatButton,
            Label,
            TextField
        };

        public enum GroupSortingType
        {
            Horizontal,
            Vertical,
            Both
        };
        public enum ElementReturnStyle
        {
            Continuous = 1,
            Single = 0
        };
    }

    namespace Utilities
    {
        using TruGUI.Controls;

        /// <summary>
        /// A utillity class that provides different functions for TruGUI.
        /// </summary>
        public static class TruGUIUtility
        {
            /// <summary>
            /// Converts a given Rect object to an array of ElementLayoutOption objects based on the given variables
            /// </summary>
            /// <param name="rect">The Rect object to convert into ElementLayoutOptions</param>
            /// <param name="optionTypes">An Array of LayoutOptionType enums which specifiy which ElementLayoutOptions are to be produced</param>
            /// <returns>A one-dimentional Array of ElementLayoutOption objects</returns>
            public static ElementLayoutOption[] ToLayoutOptions(Rect rect, params LayoutOptionType[] optionTypes)
            {
                List<ElementLayoutOption> options = new List<ElementLayoutOption>();

                foreach (LayoutOptionType type in optionTypes)
                {
                    if (type == LayoutOptionType.Width)
                    {

                        options.Add(ElementLayoutOption.Width(rect.width));
                    }
                    else if (type == LayoutOptionType.ExpandWidth)
                    {
                        options.Add(ElementLayoutOption.ExpandWidth((rect.width > 0)));
                    }
                    else if (type == LayoutOptionType.MaxWidth)
                    {
                        options.Add(ElementLayoutOption.MaxWidth(rect.width));
                    }
                    else if (type == LayoutOptionType.MinWidth)
                    {
                        options.Add(ElementLayoutOption.MinWidth(rect.width));
                    }
                    else if (type == LayoutOptionType.Height)
                    {
                        options.Add(ElementLayoutOption.Height(rect.height));
                    }
                    else if (type == LayoutOptionType.ExpandHeight)
                    {
                        options.Add(ElementLayoutOption.ExpandHeight((rect.height > 0)));
                    }
                    else if (type == LayoutOptionType.MaxHeight)
                    {
                        options.Add(ElementLayoutOption.MaxHeight(rect.height));
                    }
                    else
                    {
                        options.Add(ElementLayoutOption.MinHeight(rect.height));
                    }
                }
                return options.Distinct().ToArray();
            }

            /// <summary>
            /// Returns a new Rect object that moves towards the given future rect using the max delta as a dampener.
            /// It's Mathf.MoveTowards for Rect objects.
            /// </summary>
            /// <param name="currentRect">The current Rect to move towards the target.</param>
            /// <param name="futureRect">The target Rect.</param>
            /// <param name="maxDelta">The maximum change(Per property) that should be applied to the current Rect.</param>
            /// <returns>A new Rect object that represents the current rect with the applied change.</returns>
            public static Rect RectMoveTowards(Rect currentRect, Rect futureRect, Rect maxDelta)
            {
                Rect nextRect = new Rect();
                nextRect.xMin = Mathf.MoveTowards(currentRect.xMin, futureRect.xMin, maxDelta.xMin);
                nextRect.yMin = Mathf.MoveTowards(currentRect.yMin, futureRect.yMin, maxDelta.yMin);
                nextRect.width = Mathf.MoveTowards(currentRect.width, futureRect.width, maxDelta.width);
                nextRect.height = Mathf.MoveTowards(currentRect.height, futureRect.height, maxDelta.height);
                return currentRect;
            }

            /// <summary>
            /// Applies Mathf.Abs to each property(xMin, yMin, width, height) in the given Rect object and returns a new representation of it.
            /// </summary>
            /// <param name="rect">The rect to get the absolute value of.</param>
            /// <returns>A new Rect object representing the original's absolute value.</returns>
            public static Rect RectAbs(Rect rect)
            {
                return new Rect(Mathf.Abs(rect.xMin), Mathf.Abs(rect.yMin), Mathf.Abs(rect.width), Mathf.Abs(rect.height));
            }

            /// <summary>
            /// Adds the two given Rect objects together and returns a new Rect object representing the result.
            /// </summary>
            /// <param name="first">The left-hand rect to add.</param>
            /// <param name="second">The right-hand rect to add.</param>
            /// <returns>A new Rect object representing the result of the addition.</returns>
            public static Rect AddRect(Rect first, Rect second)
            {
                return new Rect(first.xMin + second.xMin, first.yMin + second.yMin, first.width + second.width, first.height + second.height);
            }

            /// <summary>
            /// Subtracts the two given Rect objects from each other and returns a new Rect object representing the result.
            /// </summary>
            /// <param name="first">The left-hand rect to subtract from.</param>
            /// <param name="second">The right-hand rect to subtract with.</param>
            /// <returns>A new Rect object representing the result of the subtraction.</returns>
            public static Rect RectDifference(Rect first, Rect second)
            {
                return new Rect(first.xMin - second.xMin, first.yMin - second.yMin, first.width - second.width, first.height - second.height);
            }

            /// <summary>
            /// Determines if the given first Rect object's values is less-than the given second Rect object's values.
            /// </summary>
            /// <param name="first">The Rect to determine if it is smaller than the second Rect.</param>
            /// <param name="second">The Rect to determine if it is greater than the first Rect.</param>
            /// <returns>Whether the first Rect is smaller than the second Rect.</returns>
            public static bool RectLess(Rect first, Rect second)
            {
                if (first.xMin + first.yMin + first.width + first.height < second.xMin + second.yMin + second.width + second.height)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            /// <summary>
            /// Determines if the given first Rect object's values is greater-than the given second Rect object's values.
            /// </summary>
            /// <param name="first">The Rect to determine if it is greater than the second Rect.</param>
            /// <param name="second">The Rect to determine if it is smaller than the first Rect.</param>
            /// <returns>Whether the first Rect is greater than the second Rect.</returns>
            public static bool RectGreater(Rect first, Rect second)
            {
                return !RectLess(first, second);
            }

            /// <summary>
            /// Determines if the given first Rect object's values equals the given second Rect object's values.
            /// </summary>
            /// <param name="first">The Rect to determine if it is equal to the second Rect.</param>
            /// <param name="second">The Rect to determine if it is equal to the first Rect.</param>
            /// <returns>Whether the first Rect is equal to the second Rect.</returns>
            public static bool RectEquals(Rect first, Rect second)
            {
                if (first.xMin == second.xMin)
                {
                    if (first.yMin == second.yMin)
                    {
                        if (first.width == second.width)
                        {
                            if (first.height == second.height)
                            {
                                return true;
                            }
                        }
                    }
                }
                return false;
            }

        }

        /// <summary>
        /// A non-generic object that represents an action result from a TruGUI control.
        /// </summary>
        public class GUIControlResult
        {
            object result;

            /// <summary>
            /// Gets the value of the result.
            /// </summary>
            public object Result
            {
                get
                {
                    return result;
                }
            }

            TruGUIElement element;

            /// <summary>
            /// Gets the element that the result is from.
            /// </summary>
            public TruGUIElement Element
            {
                get
                {
                    return element;
                }
            }

            /// <summary>
            /// Creates a new GUIControlResult object given the result value and element.
            /// </summary>
            /// <param name="result">The value of the result that the action of the given element produced.</param>
            /// <param name="element">The element that the user interacted with to produce the given result.</param>
            public GUIControlResult(object result, TruGUIElement element)
            {
                this.result = result;
                this.element = element;
            }

            /// <summary>
            /// Gets a string representing this GUIControlResult.
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return string.Format("Result: {0} Element: {{ {1} }}", this.result, this.element);
            }
        }
    }

    /// <summary>
    /// Defines an enumeration that defines different layout options.
    /// </summary>
    public enum LayoutOptionType
    {
        MinWidth,
        MinHeight,
        MaxWidth,
        MaxHeight,
        Width,
        Height,
        ExpandWidth,
        ExpandHeight
    };

    /// <summary>
    /// Defines a class that provides layout options to TruGUI Controls.
    /// </summary>
    [Serializable]
    public class ElementLayoutOption
    {
        [SerializeField]
        private float lengthVar;

        /// <summary>
        /// Gets or sets the length value of the option.
        /// </summary>
        public float LengthVar
        {
            get
            {
                return lengthVar;
            }
            set
            {
                lengthVar = value;
            }
        }

        [SerializeField]
        private bool boolVar;

        /// <summary>
        /// Gets or sets the boolean value of the option.
        /// </summary>
        public bool BoolVar
        {
            get
            {
                return boolVar;
            }
            set
            {
                boolVar = value;
            }
        }

        [SerializeField]
        public LayoutOptionType type;

        /// <summary>
        /// Gets the LayoutOptionType of the LayoutOption.
        /// </summary>
        public LayoutOptionType Type
        {
            get
            {
                return type;
            }
        }

        private ElementLayoutOption(float floatVar, bool boolVar, LayoutOptionType type)
        {
            lengthVar = floatVar;
            this.boolVar = boolVar;
            this.type = type;
        }

        /// <summary>
        /// Creates a new ElementLayoutOption that specifies the exact width a control should be.
        /// </summary>
        /// <param name="width">The width that the control should be.</param>
        /// <returns>A new ElementLayoutOption object.</returns>
        public static ElementLayoutOption Width(float width)
        {
            return new ElementLayoutOption(width, false, LayoutOptionType.Width);
        }

        /// <summary>
        /// Creates a new ElementLayoutOption that specifies the exact height a control should be.
        /// </summary>
        /// <param name="height">The height that the control should be.</param>
        /// <returns>A new ElementLayoutOption object.</returns>
        public static ElementLayoutOption Height(float height)
        {
            return new ElementLayoutOption(height, false, LayoutOptionType.Height);
        }

        /// <summary>
        /// Creates a new ElementLayoutOption that specifies the minimum height a control should be.
        /// </summary>
        /// <param name="minHeight">The minimum height that the control should be.</param>
        /// <returns>A new ElementLayoutOption object.</returns>
        public static ElementLayoutOption MinHeight(float minHeight)
        {
            return new ElementLayoutOption(minHeight, false, LayoutOptionType.MinHeight);
        }

        /// <summary>
        /// Creates a new ElementLayoutOption that specifies the minimum width a control should be.
        /// </summary>
        /// <param name="minWidth">The minimum width that the control should be.</param>
        /// <returns>A new ElementLayoutOption object.</returns>
        public static ElementLayoutOption MinWidth(float minWidth)
        {
            return new ElementLayoutOption(minWidth, false, LayoutOptionType.MinWidth);
        }

        /// <summary>
        /// Creates a new ElementLayoutOption that specifies the maximum height a control should be.
        /// </summary>
        /// <param name="maxHeight">The maximum height that the control should be.</param>
        /// <returns>A new ElementLayoutOption object.</returns>
        public static ElementLayoutOption MaxHeight(float MaxHeight)
        {
            return new ElementLayoutOption(MaxHeight, false, LayoutOptionType.MaxHeight);
        }

        /// <summary>
        /// Creates a new ElementLayoutOption that specifies the maximum width a control should be.
        /// </summary>
        /// <param name="maxWidth">The maximum width that the control should be.</param>
        /// <returns>A new ElementLayoutOption object.</returns>
        public static ElementLayoutOption MaxWidth(float maxWidth)
        {
            return new ElementLayoutOption(maxWidth, false, LayoutOptionType.MaxWidth);
        }

        public static ElementLayoutOption ExpandWidth(bool expandWidth)
        {
            return new ElementLayoutOption(0, expandWidth, LayoutOptionType.ExpandWidth);
        }

        public static ElementLayoutOption ExpandHeight(bool expandHeight)
        {
            return new ElementLayoutOption(0, expandHeight, LayoutOptionType.ExpandHeight);
        }

        public static implicit operator GUILayoutOption(ElementLayoutOption option)
        {
            if (option.type == LayoutOptionType.ExpandHeight)
            {
                return GUILayout.ExpandHeight(option.boolVar);
            }
            else if (option.type == LayoutOptionType.ExpandWidth)
            {
                return GUILayout.ExpandWidth(option.boolVar);
            }
            else if (option.type == LayoutOptionType.Height)
            {
                return GUILayout.Height(option.lengthVar);
            }
            else if (option.type == LayoutOptionType.MinHeight)
            {
                return GUILayout.MinHeight(option.lengthVar);
            }
            else if (option.type == LayoutOptionType.MaxHeight)
            {
                return GUILayout.MaxHeight(option.lengthVar);
            }
            else if (option.type == LayoutOptionType.Width)
            {
                return GUILayout.Width(option.lengthVar);
            }
            else if (option.type == LayoutOptionType.MinWidth)
            {
                return GUILayout.MinWidth(option.lengthVar);
            }
            else if (option.type == LayoutOptionType.MaxWidth)
            {
                return GUILayout.MaxWidth(option.lengthVar);
            }
            return null;
        }

        public override string ToString()
        {
            if (this.type != LayoutOptionType.ExpandHeight && this.type != LayoutOptionType.ExpandWidth)
            {
                return this.type.ToString() + ": " + this.lengthVar;
            }
            else
            {
                return this.type.ToString() + ": " + this.boolVar;
            }
        }
    }

    namespace Controls
    {
        using Enums;
        using Utilities;

        /// <summary>
        /// Defines a class that provides many different options that a control should be initialized with.
        /// </summary>
        [Serializable]
        public class GUIElementOptions
        {

            private bool isEnabled;

            /// <summary>
            /// Gets or sets whether the control is enabled.
            /// </summary>
            public bool IsEnabled
            {
                get
                {
                    return isEnabled;
                }
                set
                {
                    isEnabled = value;
                }
            }

            private bool isVisible;

            /// <summary>
            /// Gets or sets whether the control is visible.
            /// </summary>
            public bool IsVisible
            {
                get
                {
                    return isVisible;
                }
                set
                {
                    isVisible = value;
                }
            }

            private int depth;

            /// <summary>
            /// Gets or sets the depth of the control (lower values are drawn on top of higher values)/
            /// </summary>
            public int Depth
            {
                get
                {
                    return depth;
                }
                set
                {
                    depth = value;
                }
            }

            private string name;

            /// <summary>
            /// Gets or sets the name of the control.
            /// </summary>
            public string Name
            {
                get
                {
                    return name;
                }
                set
                {
                    name = value;
                }
            }

            private GUIContent content;

            /// <summary>
            /// Gets or sets the content that is drawn with the control.
            /// </summary>
            public GUIContent Content
            {
                get
                {
                    return content;
                }
                set
                {
                    content = value;
                }
            }

            private bool isLayout;

            /// <summary>
            /// Gets or sets whether the control is drawn using automatic layouts.
            /// </summary>
            public bool IsLayout
            {
                get
                {
                    return isLayout;
                }
                set
                {
                    isLayout = value;
                }
            }


            private bool isEditor;

            /// <summary>
            /// Gets or sets whether the control is an editor control.
            /// </summary>
            public bool IsEditor
            {
                get
                {
                    return isEditor;
                }
                set
                {
                    isEditor = value;
                }
            }

            private Rect rectangle;

            /// <summary>
            /// Gets or sets the layout Rectangle that the control is drawn in.
            /// </summary>
            public Rect Rectangle
            {
                get
                {
                    return rectangle;
                }
                set
                {
                    rectangle = value;
                }
            }

            private GUIStyle guiStyle;

            /// <summary>
            /// Gets or sets the GUI Style used to style the control.
            /// </summary>
            public GUIStyle GUIStyle
            {
                get
                {
                    return guiStyle;
                }
                set
                {
                    guiStyle = value;
                }
            }

            private ElementLayoutOption[] layoutOptions;

            /// <summary>
            /// Gets or sets the Layout Options used while in layout mode.
            /// </summary>
            public ElementLayoutOption[] LayoutOptions
            {
                get
                {
                    return layoutOptions;
                }
                set
                {
                    layoutOptions = value;
                }
            }

            /// <summary>
            /// Creates a new GUIElementOptions object given the desired area, name of the control (optional), content (optional), style (optional), visibility (optional), enabled (optional), depth (optional),
            /// isEditor (optional)
            /// </summary>
            /// <param name="area">The area to draw the control in.</param>
            /// <param name="name">The name of the control.</param>
            /// <param name="content">The content that the control should draw.</param>
            /// <param name="style">The style of the control.</param>
            /// <param name="isVisible">Whether the control is visible.</param>
            /// <param name="isEnabled">Whether the control is enabled.</param>
            /// <param name="depth">The depth to draw the control at.</param>
            /// <param name="isEditor">Whether the control is used for Unity Editors.</param>
            public GUIElementOptions(Rect area, string name = "", GUIContent content = null, GUIStyle style = null, bool isVisible = true, bool isEnabled = true, int depth = 0, bool isEditor = false)
            {
                IsLayout = false;
                IsEditor = isEditor;
                IsVisible = isVisible;
                IsEnabled = isEnabled;
                Rectangle = area;
                LayoutOptions = new ElementLayoutOption[] { };
                Name = name;
                Depth = depth;
                if (content != null)
                    Content = content;
                else
                    Content = new GUIContent();
                GUIStyle = style;
            }

            public GUIElementOptions(string name, GUIContent content, GUIStyle style, bool isVisible = true, bool isEnabled = true, int depth = 0, bool isEditor = false, params ElementLayoutOption[] options)
            {
                IsLayout = true;
                IsEditor = isEditor;
                IsVisible = isVisible;
                IsEnabled = isEnabled;
                Rectangle = new Rect();
                LayoutOptions = options;
                Name = name;
                Depth = depth;
                if (content != null)
                    Content = content;
                else
                    Content = new GUIContent();
                GUIStyle = style;
            }
            public GUIElementOptions(string name = "", GUIContent content = null, GUIStyle style = null, params ElementLayoutOption[] options)
            {
                IsLayout = true;
                IsEditor = false;
                IsVisible = true;
                IsEnabled = true;
                Rectangle = new Rect();
                LayoutOptions = options;
                Name = name;
                Depth = 0;
                if (content != null)
                    Content = content;
                else
                    Content = new GUIContent();
                GUIStyle = style;
            }

            /// <summary>
            /// Sets all of the options contained in this object to the given element.
            /// </summary>
            /// <param name="element">The element to set these options to.</param>
            /// <returns></returns>
            public TruGUIElement SetOptionsToElement(TruGUIElement element)
            {
                return SetOptionsToElement(element, this);
            }

            /// <summary>
            /// Sets all of the options contained in the given GUIElementOptions object to the given TruGUIElement.
            /// </summary>
            /// <param name="element">The element to set the given options to.</param>
            /// <param name="options">The options to set to the given element.</param>
            /// <returns></returns>
            public static TruGUIElement SetOptionsToElement(TruGUIElement element, GUIElementOptions options)
            {
                element.Enabled = options.IsEnabled;
                element.Content = options.Content;
                element.Name = options.Name;
                element.IsVisible = options.IsVisible;
                element.CurrentRect = options.Rectangle;
                element.Depth = options.Depth;
                element.IsLayout = options.IsLayout;
                element.GUIStyle = options.GUIStyle;
                element.ElementLayoutOptions = options.LayoutOptions;
                return element;
            }

            //public static TruGUI.Controls.TruGUIElement operator +(TruGUI.Controls.TruGUIElement element, GUIElementOptions options)
            //{
            //    element.Enabled = options.IsEnabled;
            //    element.Content = options.Content;
            //    element.Name = options.Name;
            //    element.IsVisible = options.IsVisible;
            //    element.CurrentRect = options.Rectangle;
            //    element.Depth = options.Depth;
            //    element.IsLayout = options.IsLayout;
            //    element.GUIStyle = options.GUIStyle;
            //    element.ElementLayoutOptions = options.LayoutOptions;
            //    return element;
            //}
        }

        /// <summary>
        /// Defines a non-generic TruGUIElement that acts as a base class for other GUI elements.
        /// </summary>
        [Serializable]
        public abstract class TruGUIElement : TruGUI.ITruGUIElement
        {
            private void setThisToOptions(GUIElementOptions options)
            {
                this.Enabled = options.IsEnabled;
                this.content = options.Content;
                this.Name = options.Name;
                this.IsVisible = options.IsVisible;
                this.CurrentRect = options.Rectangle;
                this.Depth = options.Depth;
                this.isLayout = options.IsLayout;
                this.layoutOptions = options.LayoutOptions;
                this.futureArea = CurrentRect;
            }

            public ElementLayoutOption[] ElementLayoutOptions
            {
                get
                {
                    return this.layoutOptions;
                }
                set
                {
                    if (value == null)
                    {
                        throw new ArgumentNullException();
                    }
                    this.layoutOptions = value;
                }
            }

            public GUILayoutOption[] GUILayoutOptions
            {
                get
                {
                    if (this.layoutOptions != null && this.layoutOptions.Length > 0)
                    {
                        return this.layoutOptions.Select(a => (GUILayoutOption)a).ToArray();
                    }
                    else
                    {
                        return null;
                    }
                }
            }

            private ElementReturnStyle returnStyle;

            public ElementReturnStyle ReturnStyle
            {
                get
                {
                    return returnStyle;
                }
                protected set
                {
                    returnStyle = value;
                }
            }

            private Color startingContentColor;

            private Color startingBackgroundColor;


            [SerializeField]
            protected Color contentColor;

            public Color ContentColor
            {
                get
                {
                    return contentColor;
                }
                set
                {
                    contentColor = value;
                    HandleChange(ContentColorProp, value);
                }
            }

            [SerializeField]
            protected Color backgroundColor;

            public Color BackgroundColor
            {
                get
                {
                    return backgroundColor;
                }
                set
                {
                    backgroundColor = value;
                    HandleChange(BackgroundColorProp, value);
                }
            }

            protected PropertyInfo ContentColorProp
            {
                get
                {
                    return typeof(TruGUIElement).GetProperty("ContentColor");
                }
            }

            protected PropertyInfo BackgroundColorProp
            {
                get
                {
                    return typeof(TruGUIElement).GetProperty("BackgroundColor");
                }
            }

            private Matrix4x4 matrix;

            public Matrix4x4 Matrix
            {
                get
                {
                    return matrix;
                }
                set
                {
                    matrix = value;
                }
            }

            protected Matrix4x4 beforeMatrix;

            protected void SetElementLayoutDimentions(Vector2 dim)
            {
                if (isLayout && HasRectInfo)
                {
                    ElementLayoutOption[] options = ElementLayoutOptions.Where(a => a.type == LayoutOptionType.Height || a.type == LayoutOptionType.Width).ToArray();

                    if (options.Length == 0)
                    {
                        AddOption(ElementLayoutOption.Width(dim.x));
                        AddOption(ElementLayoutOption.Height(dim.y));
                        options = ElementLayoutOptions.Where(a => a.type == LayoutOptionType.Height || a.type == LayoutOptionType.Width).ToArray();
                    }

                    foreach (ElementLayoutOption option in options)
                    {
                        if (option.type == LayoutOptionType.Width)
                        {
                            option.LengthVar = dim.x;
                        }
                        else if (option.type == LayoutOptionType.Height)
                        {
                            option.LengthVar = dim.y;
                        }
                    }
                }
            }

            protected Vector2 GetElementLayoutDimentions()
            {
                Vector2 dim = new Vector2();
                if (isLayout && HasRectInfo)
                {
                    if (ElementLayoutOptions.Any(a => a.type == LayoutOptionType.Height) && ElementLayoutOptions.Any(a => a.type == LayoutOptionType.Width))
                    {
                        ElementLayoutOption[] options = ElementLayoutOptions.Where(a => a.type == LayoutOptionType.Height || a.type == LayoutOptionType.Width).ToArray();


                        foreach (ElementLayoutOption option in options)
                        {
                            if (option.type == LayoutOptionType.Width)
                            {
                                dim.x = option.LengthVar;
                            }
                            else
                            {
                                dim.y = option.LengthVar;
                            }
                        }
                        return dim;
                    }
                    else
                    {
                        return this.Dimentions;
                    }
                }
                else
                {
                    return this.Dimentions;
                }
            }

            [HideInInspector]
            [SerializeField]
            protected Rect oldRect;

            protected void FixCurrentRect()
            {
                if (isLayout && oldRect != default(Rect))
                {
                    if (HasRectInfo)
                    {
                        Vector2 dim = GetElementLayoutDimentions();
                        if (currentRect != new Rect(currentRect.xMin, currentRect.yMin, dim.x, dim.y))
                        {
                            if (oldRect == currentRect)
                            {
                                currentRect = new Rect(currentRect.xMin, currentRect.yMin, dim.x, dim.y);
                            }
                            else
                            {
                                SetElementLayoutDimentions(Dimentions);
                            }
                        }
                    }
                }
                oldRect = currentRect;
            }

            protected virtual void StartSetOptions()
            {
                if (oldRect == default(Rect))
                {
                    oldRect = currentRect;
                }
                FixCurrentRect();
                if (matrix == default(Matrix4x4))
                {
                    matrix = GUI.matrix;
                }
                this.AnimateNext();
                GUI.enabled = this.enabled;
                beforeMatrix = GUI.matrix;
                GUI.matrix = this.matrix;
                this.startingContentColor = GUI.contentColor;
                this.startingBackgroundColor = GUI.backgroundColor;
                if (this.contentColor != default(Color))
                    GUI.contentColor = this.ContentColor;
                if (this.backgroundColor != default(Color))
                    GUI.backgroundColor = this.BackgroundColor;
                if (isVisible)
                {
                    SetName();
                    SetDepth();
                }

            }

            protected virtual void EndSetOptions()
            {

                GUI.enabled = true;
                GUI.matrix = this.beforeMatrix;
                GUI.contentColor = this.startingContentColor;
                GUI.backgroundColor = this.startingBackgroundColor;
                FindRect();
                this.handleEvents();
            }

            public Color Color
            {
                set
                {
                    BackgroundColor = value;
                    ContentColor = value;
                    HandleChange(ColorProp, value);
                }
            }

            public static bool Changed
            {
                get
                {
                    return GUI.changed;
                }
            }

            public GUISkin Skin
            {
                get
                {
                    return GUI.skin;
                }
                set
                {
                    GUI.skin = value;
                }
            }

            protected PropertyInfo ColorProp
            {
                get
                {
                    return typeof(TruGUIElement).GetProperty("Color");
                }

            }

            private int depth;

            public int Depth
            {
                get
                {
                    return depth;
                }
                set
                {
                    depth = value;
                    HandleChange(DepthProp, value);
                }
            }

            private PropertyInfo DepthProp
            {
                get
                {
                    return typeof(TruGUIElement).GetProperty("Depth");
                }
            }

            [SerializeField]
            protected string name;
            public string Name
            {
                get
                {
                    return name;
                }
                set
                {
                    name = value;
                    HandleChange(NameProp, value);
                }
            }

            private PropertyInfo NameProp
            {
                get
                {
                    return typeof(TruGUIElement).GetProperty("Name");
                }
            }

            [SerializeField]
            protected bool enabled = true;

            public bool Enabled
            {
                get
                {
                    return enabled;
                }
                set
                {
                    enabled = value;
                    HandleChange(EnabledProp, value);
                }
            }

            private PropertyInfo EnabledProp
            {
                get
                {
                    return typeof(TruGUIElement).GetProperty("Enabled");
                }
            }


            private PropertyInfo IsEditorProp
            {
                get
                {
                    return typeof(TruGUIElement).GetProperty("IsEditor");
                }
            }

            [SerializeField]
            public ElementLayoutOption[] layoutOptions;

            [SerializeField]
            protected bool isLayout = false;
            public bool IsLayout
            {
                get
                {
                    return isLayout;
                }
                set
                {
                    isLayout = value;
                    HandleChange(IsLayoutProp, value);
                }
            }

            private PropertyInfo IsLayoutProp
            {
                get
                {
                    return typeof(TruGUIElement).GetProperty("IsLayout");
                }
            }

            protected Rect startAnimationRect;

            [SerializeField]
            protected Rect currentRect;

            /// <summary>
            /// The current area that the element occupies
            /// </summary>
            public Rect CurrentRect
            {
                get
                {
                    return currentRect;
                }
                set
                {
                    if (!isLayout)
                    {
                        currentRect = value;
                    }
                    else
                    {
                        if (value.width != CurrentRect.width && this.layoutOptions.All(a => a.Type != LayoutOptionType.Width) && HasRectInfo)
                        {
                            AddOption(ElementLayoutOption.Width(CurrentRect.width));
                        }
                        if (value.height != CurrentRect.height && this.layoutOptions.All(a => a.Type != LayoutOptionType.Height) && HasRectInfo)
                        {
                            AddOption(ElementLayoutOption.Height(CurrentRect.height));
                        }
                        foreach (ElementLayoutOption option in layoutOptions)
                        {
                            if (option.Type == LayoutOptionType.Width)
                            {
                                option.LengthVar = value.width;
                            }
                            else
                            {
                                option.LengthVar = value.height;
                            }
                        }
                        //SetElementLayoutDimentions(new Vector2(value.width, value.height));
                        currentRect = value;
                        Debug.Log(value);
                    }
                }
            }

            public void AddOption(ElementLayoutOption option)
            {
                List<ElementLayoutOption> thisOptions = this.layoutOptions.ToList();
                thisOptions.Add(option);
                this.layoutOptions = thisOptions.ToArray();
            }

            public delegate void Focus(TruGUIElement sender, bool hasFocus);
            public delegate void OnChanged(TruGUIElement sender, PropertyInfo field, object newValue);
            public delegate void AnimationStateChanged(TruGUIElement sender);

            private event AnimationStateChanged onAnimationDone;

            public event AnimationStateChanged OnAnimationDone
            {
                add
                {
                    onAnimationDone += value;
                }
                remove
                {
                    onAnimationDone -= value;
                }
            }

            private event AnimationStateChanged onAnimationStarted;

            public event AnimationStateChanged OnAnimationStarted
            {
                add
                {
                    onAnimationStarted += value;
                }
                remove
                {
                    onAnimationStarted -= value;
                }
            }

            private event AnimationStateChanged onAnimationRestarted;

            public event AnimationStateChanged OnAnimationRestarted
            {
                add
                {
                    onAnimationRestarted += value;
                }
                remove
                {
                    onAnimationRestarted -= value;
                }
            }

            private event Action onActionTaken;
            public event Action OnActionTaken
            {
                add
                {
                    onActionTaken += value;
                }
                remove
                {
                    onActionTaken -= value;
                }
            }

            private event OnChanged onFieldChanged;
            public event OnChanged OnFieldChanged
            {
                add
                {
                    onFieldChanged += value;
                }
                remove
                {
                    onFieldChanged -= value;
                }
            }

            private event Focus onMouseFocusChanged;

            public event Focus OnMouseFocusChanged
            {
                add
                {
                    onMouseFocusChanged += value;
                }
                remove
                {
                    onMouseFocusChanged -= value;
                }
            }

            private event Focus onMouseFocusSame;

            public event Focus OnMouseFocusSame
            {
                add
                {
                    onMouseFocusSame += value;
                }
                remove
                {
                    onMouseFocusSame -= value;
                }
            }


            protected void HandleChange(PropertyInfo info, object newVal)
            {
                OnChanged temp = onFieldChanged;
                if (temp != null)
                {
                    temp(this, info, newVal);
                }
            }

            private bool isFocused;

            public bool IsFocused
            {
                get
                {
                    return isFocused;
                }
                protected set
                {
                    isFocused = value;
                }
            }


            public Vector4 PositionDimentions
            {
                get
                {
                    return new Vector4(Position.x, Position.y, Dimentions.x, Dimentions.y);
                }
                set
                {
                    Dimentions = new Vector2(value.z, value.w);
                    Position = new Vector2(value.x, value.y);
                    HandleChange(PositionDimentionsProp, value);
                }
            }

            private PropertyInfo PositionDimentionsProp
            {
                get
                {
                    return typeof(TruGUIElement).GetProperty("PositionDimentions");
                }
            }

            /// <summary>
            /// The content that the element will display
            /// </summary>
            protected GUIContent content = new GUIContent();

            public GUIContent Content
            {
                get
                {
                    return content;
                }
                set
                {
                    content = value;
                    HandleChange(ContentProp, value);
                }
            }

            public string Text
            {
                get { return Content.text; }
                set { Content = new GUIContent(value) { image = Content.image, tooltip = Content.tooltip }; }
            }

            private PropertyInfo ContentProp
            {
                get
                {
                    return typeof(TruGUIElement).GetProperty("Content");
                }
            }

            protected bool animate = false;

            /// <summary>
            /// The future area the the element will occupy
            /// </summary>
            [SerializeField]
            protected Rect futureArea;

            /// <summary>
            /// The rate at which the element will move to occupy the future area
            /// The individual elements of the rect object will determine the rate at which each value will move
            /// </summary>
            [SerializeField]
            protected Rect futureAreaRate;

            /// <summary>
            /// The GUIStyle object that will be used to render the element
            /// </summary>
            [SerializeField]
            protected GUIStyle guiStyle;
            public GUIStyle GUIStyle
            {
                get
                {
                    if (hasStyle)
                        return guiStyle;
                    else
                        return null;
                }
                set
                {
                    guiStyle = value;
                    if (value != null)
                        hasStyle = true;
                    else
                        hasStyle = false;
                    HandleChange(GUIStyleProp, value);
                }
            }

            private bool hasStyle = false;

            public bool HasStyle
            {
                get
                {
                    return hasStyle;
                }
                protected set
                {
                    hasStyle = value;
                }
            }


            private PropertyInfo GUIStyleProp
            {
                get
                {
                    return typeof(TruGUIElement).GetProperty("GUIStyle");
                }
            }

            /// <summary>
            /// The x and y coordinates of the center of the current rectangle(aka. CurrentRect)
            /// </summary>
            //[SerializeField]
            //protected Vector2 position = new Vector2(-1, -1);

            public Vector2 Position
            {
                get
                {
                    return new Vector2(CurrentRect.xMin, CurrentRect.yMin);
                }
                set
                {
                    CurrentRect = new Rect(value.x, value.y, CurrentRect.width, CurrentRect.height);
                    HandleChange(PositionProp, value);
                }
            }

            private PropertyInfo PositionProp
            {
                get
                {
                    return typeof(TruGUIElement).GetProperty("Position");
                }
            }

            [HideInInspector]
            [SerializeField]
            private bool hasLayoutRectInfo = false;

            public bool HasRectInfo
            {
                get
                {
                    if (isLayout && hasLayoutRectInfo)
                        return true;
                    else if (isLayout && !hasLayoutRectInfo)
                        return false;
                    else
                        return true;
                }
            }

            /// <summary>
            /// The width(x) and height(y) of the current rectangle(aka. CurrentRect) 
            /// </summary>
            //[HideInInspector]
            //[SerializeField]
            //protected Vector2 dimentions = new Vector2(-1, -1);

            public Vector2 Dimentions
            {
                get
                {
                    return new Vector2(CurrentRect.width, CurrentRect.height);
                }
                set
                {
                    if (this.ElementLayoutOptions.All(a => a.type != LayoutOptionType.Width) && isLayout)
                    {
                        this.AddOption(ElementLayoutOption.Width(value.x));
                    }
                    if (this.ElementLayoutOptions.All(a => a.type != LayoutOptionType.Height) && isLayout)
                    {
                        this.AddOption(ElementLayoutOption.Height(value.y));
                    }
                    CurrentRect = new Rect(CurrentRect.xMin, CurrentRect.yMin, value.x, value.y);
                    HandleChange(DimentionsProp, value);
                }
            }

            public void PlusEqualsDimentions(float width, float height = 0)
            {
                Dimentions += new Vector2(width, height);
                Debug.Log(Dimentions);
            }

            private PropertyInfo DimentionsProp
            {
                get
                {
                    return typeof(TruGUIElement).GetProperty("Dimentions");
                }
            }

            [SerializeField]
            protected bool isVisible = true;

            public bool IsVisible
            {
                get
                {
                    return isVisible;
                }
                set
                {
                    isVisible = value;
                    HandleChange(IsVisibleProp, value);
                }
            }

            private PropertyInfo IsVisibleProp
            {
                get
                {
                    return typeof(TruGUIElement).GetProperty("IsVisible");
                }
            }

            /// <summary>
            /// changes the value of the current rectangle(aka. CurrentRect) to the
            /// value of the second rectangle(aka. futureRect) by the rate(aka. rateRect), which 
            /// is defined as a rectangle object where each coresponding value determines the rate of change for that current value
            /// </summary>
            /// <param name="futureRect">The second rectangle</param>
            /// <param name="rateRect">The rate. Determines the rate of change for each value contained inside the rectangle object.</param>
            public virtual void StartAnimate(Rect futureRect, Rect rateRect)
            {
                if (futureArea != default(Rect))
                {
                    if (IsAnimating())
                    {
                        throwOnAnimationRestarted();
                    }
                }

                animate = true;
                futureArea = futureRect;
                futureAreaRate = TruGUIUtility.RectAbs(rateRect);
                startAnimationRect = CurrentRect;
                //make sure that we have the layout options to animate
                //if (this.isLayout)
                //{
                //    if (!ElementLayoutOptions.Any(a => a.Type == LayoutOptionType.Height && a.Type == LayoutOptionType.Width))
                //    {
                //        List<ElementLayoutOption> options = new List<ElementLayoutOption>(this.layoutOptions);
                //        options.Add(ElementLayoutOption.Width(CurrentRect.width));
                //        options.Add(ElementLayoutOption.Height(CurrentRect.height));
                //    }
                //    else if (!ElementLayoutOptions.Any(a => a.Type == LayoutOptionType.Height && a.Type != LayoutOptionType.Width))
                //    {
                //        List<ElementLayoutOption> options = new List<ElementLayoutOption>(this.layoutOptions);

                //        options.Add(ElementLayoutOption.Height(CurrentRect.height));
                //    }
                //    else if (!ElementLayoutOptions.Any(a => a.Type != LayoutOptionType.Height && a.Type == LayoutOptionType.Width))
                //    {
                //        List<ElementLayoutOption> options = new List<ElementLayoutOption>(this.layoutOptions);
                //        options.Add(ElementLayoutOption.Width(CurrentRect.width));
                //    }
                //}

                wasAnimating = false;
            }

            protected void Start()
            {
                oldRect = currentRect;
            }

            protected TruGUIElement(GUIElementOptions options)
            {
                this.setThisToOptions(options);
                Start();
            }

            protected TruGUIElement(Texture image, params ElementLayoutOption[] layoutOptions)
            {

                isLayout = true;

                this.content = new GUIContent(image);
                this.layoutOptions = layoutOptions;
                Start();
            }

            protected TruGUIElement(Texture image, GUIStyle style, params ElementLayoutOption[] layoutOptions)
            {

                isLayout = true;

                this.content = new GUIContent(image);
                this.GUIStyle = style;
                this.layoutOptions = layoutOptions;
                Start();
            }


            protected TruGUIElement(string text, params ElementLayoutOption[] layoutOptions)
            {

                isLayout = true;

                this.content = new GUIContent(text);
                this.layoutOptions = layoutOptions;
                Start();
            }

            protected TruGUIElement(string text, string tooltip, params ElementLayoutOption[] layoutOptions)
            {

                isLayout = true;
                this.content = new GUIContent(text, tooltip);

                this.layoutOptions = layoutOptions;
                Start();
            }

            protected TruGUIElement(string text, string tooltip, GUIStyle style, params ElementLayoutOption[] layoutOptions)
            {
                isLayout = true;

                this.content = new GUIContent(text, tooltip);
                this.GUIStyle = style;
                this.layoutOptions = layoutOptions;
                Start();
            }

            protected TruGUIElement(GUIContent content, params ElementLayoutOption[] layoutOptions)
            {
                isLayout = true;

                this.content = content;
                this.layoutOptions = layoutOptions;
                Start();
            }

            protected TruGUIElement(GUIContent content, GUIStyle style, params ElementLayoutOption[] layoutOptions)
            {
                isLayout = true;

                this.content = content;
                this.GUIStyle = style;
                this.layoutOptions = layoutOptions;
                Start();
            }

            public TruGUIElement(params ElementLayoutOption[] layoutOptions)
            {
                isLayout = true;
                this.layoutOptions = layoutOptions;
                Start();
            }

            protected TruGUIElement(Rect area, Texture image)
            {
                if (area.xMin < 0 || area.yMin < 0 || area.width < 0 || area.height < 0)
                {
                    this.Position = new Vector2(area.xMin, area.yMin);
                    Rect absRect = TruGUIUtility.RectAbs(area);
                    this.Dimentions = new Vector2(absRect.width, absRect.height);
                }
                else
                    this.CurrentRect = area;
                this.futureArea = area;
                this.content = new GUIContent(image);
                Start();
            }

            protected TruGUIElement(Rect area, Texture image, GUIStyle style)
            {
                if (area.xMin < 0 || area.yMin < 0 || area.width < 0 || area.height < 0)
                {
                    this.Position = new Vector2(area.xMin, area.yMin);
                    Rect absRect = TruGUIUtility.RectAbs(area);
                    this.Dimentions = new Vector2(absRect.width, absRect.height);
                }
                else
                    this.CurrentRect = area;
                this.futureArea = area;
                this.content = new GUIContent(image);
                this.GUIStyle = style;
                Start();
            }


            protected TruGUIElement(Rect area, string text)
            {
                if (area.xMin < 0 || area.yMin < 0 || area.width < 0 || area.height < 0)
                {
                    this.Position = new Vector2(area.xMin, area.yMin);
                    Rect absRect = TruGUIUtility.RectAbs(area);
                    this.Dimentions = new Vector2(absRect.width, absRect.height);
                }
                else
                    this.CurrentRect = area;
                this.futureArea = area;
                this.content = new GUIContent(text);
                Start();
            }

            protected TruGUIElement(Rect area, string text, string tooltip)
            {
                if (area.xMin < 0 || area.yMin < 0 || area.width < 0 || area.height < 0)
                {
                    this.Position = new Vector2(area.xMin, area.yMin);
                    Rect absRect = TruGUIUtility.RectAbs(area);
                    this.Dimentions = new Vector2(absRect.width, absRect.height);
                }
                else
                    this.CurrentRect = area;
                this.futureArea = area;
                this.content = new GUIContent(text, tooltip);
                Start();
            }

            protected TruGUIElement(Rect area, string text, string tooltip, GUIStyle style)
            {
                if (area.xMin < 0 || area.yMin < 0 || area.width < 0 || area.height < 0)
                {
                    this.Position = new Vector2(area.xMin, area.yMin);
                    Rect absRect = TruGUIUtility.RectAbs(area);
                    this.Dimentions = new Vector2(absRect.width, absRect.height);
                }
                else
                    this.CurrentRect = area;
                this.futureArea = area;
                this.content = new GUIContent(text, tooltip);
                this.GUIStyle = style;
                Start();
            }

            protected TruGUIElement(Rect area, GUIContent content)
            {
                if (area.xMin < 0 || area.yMin < 0 || area.width < 0 || area.height < 0)
                {
                    this.Position = new Vector2(area.xMin, area.yMin);
                    Rect absRect = TruGUIUtility.RectAbs(area);
                    this.Dimentions = new Vector2(absRect.width, absRect.height);
                }
                else
                    this.CurrentRect = area;
                this.futureArea = area;
                this.content = content;
                Start();
            }

            protected TruGUIElement(Rect area, GUIContent content, GUIStyle style)
            {
                if (area.xMin < 0 || area.yMin < 0 || area.width < 0 || area.height < 0)
                {
                    this.Position = new Vector2(area.xMin, area.yMin);
                    Rect absRect = TruGUIUtility.RectAbs(area);
                    this.Dimentions = new Vector2(absRect.width, absRect.height);
                }
                else
                    this.CurrentRect = area;
                this.futureArea = area;
                this.content = content;
                this.GUIStyle = style;
                Start();
            }

            public TruGUIElement(Rect area)
            {
                if (area.xMin < 0 || area.yMin < 0 || area.width < 0 || area.height < 0)
                {
                    this.Position = new Vector2(area.xMin, area.yMin);
                    Rect absRect = TruGUIUtility.RectAbs(area);
                    this.Dimentions = new Vector2(absRect.width, absRect.height);
                }
                else
                    this.CurrentRect = area;
                this.futureArea = area;
                Start();
            }

            public TruGUIElement(Rect area, GUIStyle style)
            {
                if (area.xMin < 0 || area.yMin < 0 || area.width < 0 || area.height < 0)
                {
                    this.Position = new Vector2(area.xMin, area.yMin);
                    Rect absRect = TruGUIUtility.RectAbs(area);
                    this.Dimentions = new Vector2(absRect.width, absRect.height);
                }
                else
                    this.CurrentRect = area;
                this.futureArea = area;
                this.GUIStyle = style;
                Start();
            }

            private void SetDepth()
            {
                GUI.depth = this.depth;
            }

            private void SetName()
            {
                GUI.SetNextControlName(this.name);
            }

            private void ToAccurateRect(Rect newRect)
            {

            }

            public override string ToString()
            {
                return "Name: " + this.name + " Area: " + this.CurrentRect + " Content: " + this.content.ToString() + " Type: " + this.GetType() + " Style: " + GUIStyle;
            }

            protected virtual void FindRect()
            {
                if (isLayout && Event.current.type == EventType.Repaint)
                {
                    Rect theRect = GUILayoutUtility.GetLastRect();
                    if (theRect != CurrentRect)
                    {
                        //position = new Vector2(theRect.xMin, theRect.yMin);
                        currentRect = theRect;
                        //this.CurrentRect =new Rect(theRect.xMin, theRect.yMin, theRect.width, theRect.height);
                        //foreach (ElementLayoutOption option in ElementLayoutOptions)
                        //{
                        //    if (option.type == LayoutOptionType.Height)
                        //    {
                        //        theRect.height = option.lengthVar;
                        //    }
                        //    else if (option.type == LayoutOptionType.MaxHeight && theRect.height > option.lengthVar)
                        //    {
                        //        theRect.height = option.lengthVar;
                        //    }
                        //    else if (option.type == LayoutOptionType.MinHeight && theRect.height < option.lengthVar)
                        //    {
                        //        theRect.height = option.lengthVar;
                        //    }
                        //    else if (option.type == LayoutOptionType.Width)
                        //    {
                        //        theRect.width = option.lengthVar;
                        //    }
                        //    else if (option.type == LayoutOptionType.MinWidth && theRect.width < option.lengthVar)
                        //    {
                        //        theRect.width = option.lengthVar;
                        //    }
                        //    else if (option.type == LayoutOptionType.MaxWidth && theRect.width > option.lengthVar)
                        //    {
                        //        theRect.width = option.lengthVar;
                        //    }
                        //}
                        //dimentions = new Vector2(theRect.width, theRect.height);

                    }
                    hasLayoutRectInfo = true;
                }
            }

            public virtual bool IsAnimating()
            {
                if (animate && !TruGUIUtility.RectEquals(CurrentRect, futureArea))
                {
                    return true;
                }
                return false;
            }

            bool wasAnimating = false;

            protected virtual void AnimateNext()
            {
                if (HasRectInfo)
                {
                    if (IsAnimating())
                    {
                        //if (!isLayout)
                        //{

                        CurrentRect = TruGUIUtility.RectMoveTowards(CurrentRect, futureArea, futureAreaRate);
                        wasAnimating = true;

                        //}
                        //else
                        //{
                        //    if (layoutOptions != null && layoutOptions.Length > 1)
                        //    {
                        //        foreach (ElementLayoutOption option in layoutOptions)
                        //        {
                        //            //Debug.Log(option.Type + " " + option.LengthVar);
                        //            if (option.Type == LayoutOptionType.MaxWidth || option.Type == LayoutOptionType.MinWidth || option.Type == LayoutOptionType.Width)
                        //            {
                        //                option.LengthVar = Mathf.MoveTowards(option.LengthVar, futureArea.width, futureAreaRate.width);
                        //            }
                        //            else if (option.Type != LayoutOptionType.ExpandWidth && option.Type != LayoutOptionType.ExpandHeight)
                        //            {
                        //                option.LengthVar = Mathf.MoveTowards(option.LengthVar, futureArea.height, futureAreaRate.height);
                        //            }
                        //        }
                        //    }
                        //    else
                        //    {

                        //        layoutOptions = new ElementLayoutOption[] { ElementLayoutOption.Width(this.dimentions.x), ElementLayoutOption.Height(this.dimentions.y) };
                        //        foreach (ElementLayoutOption option in layoutOptions)
                        //        {
                        //            //Debug.Log(option.Type + " " + option.LengthVar);
                        //            if (option.Type == LayoutOptionType.MaxWidth || option.Type == LayoutOptionType.MinWidth || option.Type == LayoutOptionType.Width)
                        //            {
                        //                option.LengthVar = Mathf.MoveTowards(option.LengthVar, futureArea.width, futureAreaRate.width);
                        //            }
                        //            else if (option.Type != LayoutOptionType.ExpandWidth && option.Type != LayoutOptionType.ExpandHeight)
                        //            {
                        //                option.LengthVar = Mathf.MoveTowards(option.LengthVar, futureArea.height, futureAreaRate.height);
                        //            }
                        //        }
                        //    }
                        //}
                    }
                    else
                    {
                        //Handle on animation done event parameters
                        if (wasAnimating)
                        {
                            wasAnimating = true;
                            animate = false;
                        }
                    }
                    hasLayoutRectInfo = false;
                }
            }

            protected bool WasAnimating()
            {
                if (wasAnimating && !IsAnimating())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            protected virtual void handleEvents()
            {
                handleFocus();
                handleActions();
                handleAnimations();
            }

            private void throwOnAnimationDone()
            {
                AnimationStateChanged temp = onAnimationDone;
                if (temp != null)
                {
                    temp(this);
                }
            }


            private void throwOnAnimationStarted()
            {
                AnimationStateChanged temp = this.onAnimationStarted;
                if (temp != null)
                {
                    temp(this);
                }
            }

            private void throwOnAnimationRestarted()
            {
                AnimationStateChanged temp = this.onAnimationRestarted;
                if (temp != null)
                {
                    temp(this);
                }
            }

            private void handleAnimations()
            {
                if (WasAnimating())
                {
                    throwOnAnimationDone();
                    wasAnimating = false;
                    animate = false;
                }
                else if (!wasAnimating && IsAnimating())
                {
                    throwOnAnimationStarted();
                    wasAnimating = true;
                    animate = true;
                }
            }


            protected virtual void handleActions()
            {
                if (actionResult != null)
                {
                    if (hasActionTaken)
                    {
                        Action temp = onActionTaken;
                        if (temp != null)
                        {
                            temp();
                        }
                        hasActionTaken = false;
                        actionResult = null;
                    }
                }
            }

            protected virtual void handleFocus()
            {
                if (isVisible)
                {
                    if (!isFocused)
                    {
                        if (this.CurrentRect.Contains(Event.current.mousePosition))
                        {
                            isFocused = true;
                            Focus temp = onMouseFocusChanged;
                            if (temp != null)
                            {
                                temp(this, isFocused);
                            }
                        }
                        else
                        {
                            Focus temp = onMouseFocusSame;
                            if (temp != null)
                            {
                                temp(this, isFocused);
                            }
                        }
                    }
                    else
                    {
                        if (!this.CurrentRect.Contains(Event.current.mousePosition))
                        {
                            isFocused = false;
                            Focus temp = onMouseFocusChanged;
                            if (temp != null)
                            {
                                temp(this, isFocused);
                            }
                        }
                        else
                        {
                            Focus temp = onMouseFocusSame;
                            if (temp != null)
                            {
                                temp(this, isFocused);
                            }
                        }
                    }
                }
            }

            public enum ColapseExpandOptions
            {
                Horizontal,
                Vertical,
                Both
            };


            public virtual void Colapse(ColapseExpandOptions options)
            {

                if (options == ColapseExpandOptions.Both)
                {
                    CurrentRect = new Rect(CurrentRect.xMin, CurrentRect.yMin, .1f, .1f);
                }
                else if (options == ColapseExpandOptions.Horizontal)
                {
                    CurrentRect = new Rect(CurrentRect.xMin, CurrentRect.yMin, .1f, CurrentRect.height);
                }
                else if (options == ColapseExpandOptions.Vertical)
                {
                    CurrentRect = new Rect(CurrentRect.xMin, CurrentRect.yMin, CurrentRect.width, .1f);
                }

            }

            public virtual void Expand(ColapseExpandOptions options, float length)
            {
                length = Mathf.Abs(length);

                if (options == ColapseExpandOptions.Both)
                {
                    CurrentRect = new Rect(CurrentRect.xMin, CurrentRect.yMin, length, length);
                }
                else if (options == ColapseExpandOptions.Horizontal)
                {
                    CurrentRect = new Rect(CurrentRect.xMin, CurrentRect.yMin, length, CurrentRect.height);
                }
                else if (options == ColapseExpandOptions.Vertical)
                {
                    CurrentRect = new Rect(CurrentRect.xMin, CurrentRect.yMin, CurrentRect.width, length);
                }
            }

            public virtual void Expand(float width, float height)
            {
                width = Mathf.Abs(width);
                height = Mathf.Abs(height);
                CurrentRect = new Rect(CurrentRect.xMin, CurrentRect.yMin, width, height);
            }

            public virtual void AnimateTo(ColapseExpandOptions options, float length, float animateRate)
            {
                length = Mathf.Abs(length);
                animateRate = Mathf.Abs(animateRate);

                if (options == ColapseExpandOptions.Both)
                {
                    StartAnimate(new Rect(CurrentRect.xMin, CurrentRect.yMin, length, length), new Rect(animateRate, animateRate, animateRate, animateRate));
                }
                else if (options == ColapseExpandOptions.Horizontal)
                {
                    StartAnimate(new Rect(CurrentRect.xMin, CurrentRect.yMin, length, CurrentRect.height), new Rect(animateRate, animateRate, animateRate, animateRate));
                }
                else if (options == ColapseExpandOptions.Vertical)
                {
                    StartAnimate(new Rect(CurrentRect.xMin, CurrentRect.yMin, CurrentRect.width, length), new Rect(animateRate, animateRate, animateRate, animateRate));
                }
            }

            public virtual void AnimateTo(float width, float height, float animateRate)
            {
                width = Mathf.Abs(width);
                animateRate = Mathf.Abs(animateRate);
                height = Mathf.Abs(height);
                StartAnimate(new Rect(CurrentRect.xMin, CurrentRect.yMin, width, height), new Rect(animateRate, animateRate, animateRate, animateRate));
            }

            public virtual void AnimateTo(float width, float height, float widthRate, float heightRate)
            {
                width = Mathf.Abs(width);
                widthRate = Mathf.Abs(widthRate);
                height = Mathf.Abs(height);
                heightRate = Mathf.Abs(heightRate);
                StartAnimate(new Rect(CurrentRect.xMin, CurrentRect.yMin, width, height), new Rect(widthRate, widthRate, widthRate, heightRate));
            }

            public virtual void ColapseAnimate(ColapseExpandOptions options, float animateRate)
            {
                //animateRate = Mathf.Abs(animateRate);

                if (options == ColapseExpandOptions.Both)
                {
                    StartAnimate(new Rect(CurrentRect.xMin, CurrentRect.yMin, .1f, .1f), new Rect(animateRate, animateRate, animateRate, animateRate));
                }
                else if (options == ColapseExpandOptions.Horizontal)
                {
                    StartAnimate(new Rect(CurrentRect.xMin, CurrentRect.yMin, .1f, CurrentRect.height), new Rect(animateRate, animateRate, animateRate, animateRate));
                }
                else if (options == ColapseExpandOptions.Vertical)
                {
                    StartAnimate(new Rect(CurrentRect.xMin, CurrentRect.yMin, CurrentRect.width, .1f), new Rect(animateRate, animateRate, animateRate, animateRate));
                }
            }

            public virtual void ColapseAnimate(ColapseExpandOptions options, float widthRate, float heightRate)
            {
                widthRate = Mathf.Abs(widthRate);
                heightRate = Mathf.Abs(heightRate);

                if (options == ColapseExpandOptions.Both)
                {
                    StartAnimate(new Rect(CurrentRect.xMin, CurrentRect.yMin, .1f, .1f), new Rect(widthRate, widthRate, widthRate, heightRate));
                }
                else if (options == ColapseExpandOptions.Horizontal)
                {
                    StartAnimate(new Rect(CurrentRect.xMin, CurrentRect.yMin, .1f, CurrentRect.height), new Rect(widthRate, widthRate, widthRate, heightRate));
                }
                else if (options == ColapseExpandOptions.Vertical)
                {
                    StartAnimate(new Rect(CurrentRect.xMin, CurrentRect.yMin, CurrentRect.width, .1f), new Rect(widthRate, widthRate, widthRate, heightRate));
                }
            }

            public abstract void Draw();

            private bool hasActionTaken;
            public bool HasActionTaken
            {
                get
                {
                    return hasActionTaken;
                }
                protected set
                {
                    hasActionTaken = value;
                }
            }
            private object actionResult;
            public object ActionResult
            {
                get
                {
                    return actionResult;
                }
                protected set
                {
                    actionResult = value;
                }
            }

            private TruGUIElement actionElement;

            public TruGUIElement ActionElement
            {
                get
                {
                    return actionElement;
                }
                set
                {
                    actionElement = value;
                }
            }


            public virtual void SetActionProperties(TruGUIElement element, object result)
            {
                actionElement = element;
                actionResult = result;
                hasActionTaken = true;
            }

        }

        [Serializable]
        public abstract class TruGUIElement<T> : TruGUIElement, ITruGUIElement<T>
        {

            protected T actionResult;
            public new T ActionResult
            {
                get
                {
                    return actionResult;
                }
                protected set
                {
                    actionResult = value;
                }
            }

            public delegate void Action<Type>(TruGUIElement<Type> sender, Type result);
            public new event Action<T> OnActionTaken;

            protected override void handleActions()
            {
                try
                {
                    if ((typeof(T).IsClass && !actionResult.Equals(default(T))) || (typeof(T).IsValueType))
                    {
                        if (HasActionTaken)
                        {
                            Action<T> temp = OnActionTaken;
                            if (temp != null)
                            {
                                temp(this, actionResult);
                            }
                            base.handleActions();

                            HasActionTaken = false;
                            actionResult = default(T);
                        }
                    }
                }
                catch (NullReferenceException)
                {
                    actionResult = default(T);
                    HasActionTaken = false;
                }
            }

            public void SetActionProperties(TruGUIElement element, T result)
            {
                actionResult = result;

                base.SetActionProperties(element, result);
                HasActionTaken = true;
            }


            protected TruGUIElement(GUIElementOptions options)
                : base(options)
            {
            }

            protected TruGUIElement(params ElementLayoutOption[] layoutOptions)
                : base(layoutOptions)
            {
            }

            protected TruGUIElement(Texture image, params ElementLayoutOption[] layoutOptions)
                : base(image, layoutOptions)
            {
            }

            protected TruGUIElement(Texture image, GUIStyle style, params ElementLayoutOption[] layoutOptions)
                : base(image, style, layoutOptions)
            {

            }


            protected TruGUIElement(string text, params ElementLayoutOption[] layoutOptions)
                : base(text, layoutOptions)
            {

            }

            protected TruGUIElement(string text, string tooltip, params ElementLayoutOption[] layoutOptions)
                : base(text, tooltip, layoutOptions)
            {
            }

            protected TruGUIElement(string text, string tooltip, GUIStyle style, params ElementLayoutOption[] layoutOptions)
                : base(text, tooltip, style, layoutOptions)
            {

            }

            protected TruGUIElement(GUIContent content, params ElementLayoutOption[] layoutOptions)
                : base(content, layoutOptions)
            {

            }

            protected TruGUIElement(GUIContent content, GUIStyle style, params ElementLayoutOption[] layoutOptions)
                : base(content, style, layoutOptions)
            {
            }


            protected TruGUIElement(Rect area)
                : base(area)
            {
            }

            protected TruGUIElement(Rect area, Texture image)
                : base(area, image)
            {
            }

            protected TruGUIElement(Rect area, Texture image, GUIStyle style)
                : base(area, image, style)
            {

            }


            protected TruGUIElement(Rect area, string text)
                : base(area, text)
            {

            }

            protected TruGUIElement(Rect area, string text, string tooltip)
                : base(area, text, tooltip)
            {
            }

            protected TruGUIElement(Rect area, string text, string tooltip, GUIStyle style)
                : base(area, text, tooltip, style)
            {

            }

            protected TruGUIElement(Rect area, GUIContent content)
                : base(area, content)
            {

            }

            protected TruGUIElement(Rect area, GUIContent content, GUIStyle style)
                : base(area, content, style)
            {
            }

            protected TruGUIElement(Rect area, GUIStyle style)
                : base(area, style)
            {
            }

        }


        [Serializable]
        [TruGUIElementIdentifier("Toolbar")]
        ///A classic Toolbar
        public class Toolbar : TruGUIElement<int>
        {
            public Toolbar(GUIElementOptions options, params GUIContent[] contents)
                : base(options)
            {
                this.contents = contents;
            }

            public Toolbar(GUIElementOptions options, string[] texts)
                : base(options)
            {
                this.contents = texts.Select(a => new GUIContent(a)).ToArray();
            }

            public Toolbar(GUIElementOptions options, Texture[] images)
                : base(options)
            {
                this.contents = images.Select(a => new GUIContent(a)).ToArray();
            }

            protected int currentIndex;

            private GUIContent[] contents;

            public GUIContent[] Contents
            {
                get
                {
                    return contents;
                }
                set
                {
                    contents = value;
                }
            }

            public override void Draw()
            {
                this.StartSetOptions();
                if (this.isVisible)
                {
                    int oldIndex = currentIndex;
                    if (this.isLayout)
                    {
                        currentIndex = GUI.Toolbar(this.CurrentRect, currentIndex, this.contents);
                    }
                    else
                    {
                        currentIndex = GUILayout.Toolbar(currentIndex, contents, GUILayoutOptions);
                    }
                    if (oldIndex != currentIndex)
                    {
                        this.SetActionProperties(this, currentIndex);
                    }
                }
                this.EndSetOptions();
            }
        }

        [Serializable]
        [TruGUIElementIdentifier("DropDownGenericBox")]
        public class DropDownGenericBox : TruGUIElement<GUIControlResult[]>
        {
            private bool animateOpen = false;

            public bool AnimateOpen
            {
                get
                {
                    return animateOpen;
                }
                set
                {
                    animateOpen = value;
                }
            }

            private bool animateClose = false;

            public bool AnimateClose
            {
                get
                {
                    return animateClose;
                }
                set
                {
                    animateClose = value;
                }
            }

            public void ClearElements()
            {
                this.containingGroup.ClearElements();
            }

            public enum OpenStyle
            {
                On_Element_Return_True,
                On_Mouse_Focus
            };

            OpenStyle boxOpenStyle = OpenStyle.On_Element_Return_True;
            public OpenStyle BoxOpenStyle
            {
                get
                {
                    return boxOpenStyle;
                }
                set
                {
                    boxOpenStyle = value;
                    HandleChange(BoxOpenStyleProp, value);
                }
            }

            PropertyInfo BoxOpenStyleProp
            {
                get
                {
                    return typeof(DropDownGenericBox).GetProperty("BoxOpenStyle");
                }
            }

            public TruGUIElement[] Elements
            {
                get
                {
                    if (containingGroup != null)
                        return containingGroup.Elements;
                    else
                        return null;
                }
            }

            private GUIStyle backgroundBoxStyle;
            public GUIStyle BackgroundBoxStyle
            {
                get
                {
                    return backgroundBoxStyle;
                }
                set
                {
                    backgroundBoxStyle = value;
                    HandleChange(BBSProp, value);
                }
            }

            PropertyInfo BBSProp
            {
                get
                {
                    return typeof(DropDownGenericBox).GetProperty("BackgroundBoxStyle");
                }
            }

            private GUIStyle scrollBarStyle;
            public GUIStyle ScrollBarStyle
            {
                get
                {
                    return scrollBarStyle;
                }
                set
                {
                    scrollBarStyle = value;
                    HandleChange(SBSProp, value);
                }
            }

            PropertyInfo SBSProp
            {
                get
                {
                    return typeof(DropDownGenericBox).GetProperty("ScrollBarStyle");
                }
            }

            [SerializeField]
            private bool isOpen;
            public bool IsOpen
            {
                get
                {
                    return isOpen;
                }
                set
                {
                    isOpen = value;
                    containingGroup.IsVisible = value;
                    HandleChange(IsOpenProp, value);
                }
            }

            public void Close()
            {
                isOpen = false;
            }

            public void Open()
            {
                isOpen = true;
            }

            PropertyInfo IsOpenProp
            {
                get
                {
                    return typeof(DropDownGenericBox).GetProperty("IsOpen");
                }
            }

            public DropDownGenericBox(Rect area, params TruGUIElement[] elements)
                : base(area)
            {
                this.AssignControls(elements.ToList());
            }
            public DropDownGenericBox(Rect area, GUIStyle mainStyle, GUIStyle backgroundBoxStyle, GUIStyle ScrollBarStyle, params TruGUIElement[] elements)
                : base(area, mainStyle)
            {
                this.BackgroundBoxStyle = backgroundBoxStyle;
                this.ScrollBarStyle = ScrollBarStyle;
                this.AssignControls(elements.ToList());
            }
            public DropDownGenericBox(Rect area, GUIContent content, params TruGUIElement[] elements)
                : base(area, content)
            {
                this.AssignControls(elements.ToList());
            }
            public DropDownGenericBox(Rect area, string text, params TruGUIElement[] elements)
                : base(area, text)
            {
                this.AssignControls(elements.ToList());
            }
            public DropDownGenericBox(params TruGUIElement[] elements)
                : base()
            {
                this.AssignControls(elements.ToList());
            }
            public DropDownGenericBox(ElementLayoutOption[] layoutOptions, TruGUIElement[] elements)
                : base(layoutOptions)
            {
                this.AssignControls(elements.ToList());
            }
            public DropDownGenericBox(TruGUIElement[] elements, params ElementLayoutOption[] layoutOptions)
                : base(layoutOptions)
            {
                this.AssignControls(elements.ToList());
            }
            public DropDownGenericBox(string text, TruGUIElement[] elements, params ElementLayoutOption[] layoutOptions)
                : base(text, layoutOptions)
            {
                this.AssignControls(elements.ToList());
            }

            private void AssignControls(List<TruGUIElement> elements)
            {

                if (this.Elements != null)
                {
                    if (this.Elements.Length > 0)
                    {
                        if (this.Elements[0] != null)
                        {
                            if (!isLayout)
                            {
                                fullGroup = new Group(this.CurrentRect, true);
                                if (triggerElement == null)
                                {
                                    triggerElement = new Toggle(new GUIElementOptions(this.name + "trggr", this.Elements[0].Content));
                                }
                                containingGroup = new Group(GroupSortingType.Both);

                            }
                            else
                            {
                                fullGroup = new Group(GroupSortingType.Both);
                                if (triggerElement == null)
                                {
                                    triggerElement = new Toggle(this.content, layoutOptions);
                                }
                                containingGroup = new Group(GroupSortingType.Both);

                            }
                        }
                        else
                        {
                            if (!isLayout)
                            {
                                fullGroup = new Group(this.CurrentRect, true);
                                if (triggerElement == null)
                                {
                                    triggerElement = new Toggle(new GUIElementOptions(this.name + "trggr", new GUIContent()));
                                }
                                containingGroup = new Group(GroupSortingType.Both);
                            }
                            else
                            {
                                fullGroup = new Group(GroupSortingType.Both);
                                if (triggerElement == null)
                                {
                                    triggerElement = new Toggle(this.content, layoutOptions);
                                }
                                containingGroup = new Group(GroupSortingType.Both);

                            }
                        }
                    }
                    else
                    {
                        if (!isLayout)
                        {
                            fullGroup = new Group(this.CurrentRect, true);
                            if (triggerElement == null)
                            {
                                triggerElement = new Toggle(new GUIElementOptions(this.name + "trggr", new GUIContent()));
                            }
                            containingGroup = new Group(GroupSortingType.Both);
                        }
                        else
                        {
                            fullGroup = new Group(GroupSortingType.Both);
                            if (triggerElement == null)
                            {
                                triggerElement = new Toggle(this.content, layoutOptions);
                            }
                            containingGroup = new Group(GroupSortingType.Both);

                        }
                    }
                }
                else
                {
                    if (!isLayout)
                    {
                        fullGroup = new Group(this.CurrentRect, true);
                        if (triggerElement == null)
                        {
                            triggerElement = new Toggle(new GUIElementOptions(this.name + "trggr", new GUIContent()));
                        }
                        containingGroup = new Group(GroupSortingType.Both);
                    }
                    else
                    {
                        fullGroup = new Group(GroupSortingType.Both);
                        if (triggerElement == null)
                        {
                            triggerElement = new Toggle(this.content, layoutOptions);
                        }
                        containingGroup = new Group(GroupSortingType.Both);

                    }
                }
                if (this.GUIStyle != null)
                {
                    //triggerElement.GUIStyle = this.GUIStyle;
                }

                //triggerElement.Name = this.name + "trggr";
                triggerElement.OnActionTaken += new TruGUIElement<bool>.Action<bool>(triggerElement_OnActionTaken);
                triggerElement.OnMouseFocusChanged += new Focus(triggerElement_OnMouseFocusChanged);
                containingGroup.IsVisible = false;

                fullGroup.AddElements(triggerElement, containingGroup);
                fullGroup.OnMouseFocusChanged += new Focus(fullGroup_OnMouseFocusChanged);
                this.OnFieldChanged += new OnChanged(DropDownGenericBox_OnFieldChanged);

            }

            void DropDownGenericBox_OnFieldChanged(TruGUIElement sender, PropertyInfo field, object newValue)
            {
                if (sender == this)
                {
                    if (field == this.IsOpenProp)
                    {
                        bool value = (bool)newValue;
                        if (value && animateOpen)
                        {
                            containingGroup.ElementLayoutOptions = new ElementLayoutOption[] { ElementLayoutOption.Height(1) };
                            containingGroup.StartAnimate(new Rect(0, 0, containingGroup.CurrentRect.width, GetContainingGroupHeight()), new Rect(0, 0, 0, .5f));

                        }
                        else if (!value && animateClose)
                        {
                            containingGroup.StartAnimate(new Rect(0, 0, containingGroup.CurrentRect.width, .1f), new Rect(0, 0, 0, .5f));
                        }
                    }
                }
            }

            void triggerElement_OnActionTaken(TruGUIElement<bool> sender, bool result)
            {
                if (sender.ReturnStyle == ElementReturnStyle.Single)
                {
                    if (boxOpenStyle == OpenStyle.On_Element_Return_True)
                    {
                        if (result)
                        {
                            IsOpen = !IsOpen;
                            containingGroup.IsVisible = IsOpen;
                        }
                    }
                }
                else
                {
                    if (boxOpenStyle == OpenStyle.On_Element_Return_True)
                    {

                        IsOpen = result;
                        containingGroup.IsVisible = isOpen;

                    }
                }
            }


            public float GetContainingGroupHeight()
            {
                float height = 0;
                foreach (TruGUIElement element in containingGroup.Elements)
                {
                    if (element.IsLayout)
                    {
                        if (element.ElementLayoutOptions.Any(a => a.Type == LayoutOptionType.Height))
                        {
                            height += element.ElementLayoutOptions.First(a => a.Type == LayoutOptionType.Height).LengthVar;
                        }
                        else if (element.ElementLayoutOptions.Any(a => a.Type == LayoutOptionType.MinHeight))
                        {
                            height += element.ElementLayoutOptions.First(a => a.Type == LayoutOptionType.MinHeight).LengthVar;
                        }
                        else
                        {
                            height += 20;
                        }
                    }
                }
                return height;
            }

            void fullGroup_OnMouseFocusChanged(object sender, bool hasFocus)
            {
                if (boxOpenStyle == OpenStyle.On_Mouse_Focus)
                {
                    if (!hasFocus)
                    {
                        isOpen = false;
                        containingGroup.IsVisible = false;
                    }
                }
            }


            void triggerElement_OnMouseFocusChanged(object sender, bool hasFocus)
            {
                if (boxOpenStyle == OpenStyle.On_Mouse_Focus)
                {
                    if (hasFocus)
                    {
                        IsOpen = true;

                        containingGroup.IsVisible = true;

                    }
                }
            }

            public void AddElements(params TruGUIElement[] elements)
            {

                containingGroup.AddElements(elements);
                foreach (TruGUIElement theElement in this.Elements)
                {
                    theElement.OnActionTaken += new Action(theElement_OnActionTaken);
                }

            }

            void theElement_OnActionTaken()
            {
                //if (results.Count > 0)
                //{
                //    results = results.Where(a => a.Element != resultOfAction.Element).ToList();
                //}
                //results.Add(resultOfAction);
                //SetActionProperties(resultOfAction.Element, resultOfAction.Result);
            }


            List<GUIControlResult> results = new List<GUIControlResult>();

            [SerializeField]
            TruGUIElement<bool> triggerElement;
            public TruGUIElement<bool> TriggerElement
            {
                get
                {
                    return triggerElement;
                }
                set
                {
                    triggerElement = value;
                }
            }

            [SerializeField]
            private bool drawFencingGroup = true;

            public bool DrawFullContainingGroup
            {
                get
                {
                    return drawFencingGroup;
                }
                set
                {
                    drawFencingGroup = value;
                    HandleChange(DrawFullContainingGroupProp, value);
                }
            }

            protected PropertyInfo DrawFullContainingGroupProp
            {
                get
                {
                    return typeof(DropDownGenericBox).GetProperty("DrawFullContainingGroup");
                }
            }

            [SerializeField]
            private bool drawContainer = true;

            public bool DrawContainingGroup
            {
                get
                {
                    return drawContainer = true;
                }
                set
                {
                    drawContainer = value;
                    HandleChange(DrawContainingGroupProp, value);
                    if (value)
                    {
                        if (!fullGroup.Elements.Contains(this.containingGroup))
                        {
                            fullGroup.AddElements(this.containingGroup);
                        }
                    }
                    else
                    {
                        fullGroup.RemoveElement(containingGroup);
                    }
                }
            }

            //[SerializeField]
            //private bool animate;

            public bool Animate
            {
                get
                {
                    return animate;
                }
                set
                {
                    animate = value;
                    HandleChange(AnimateProp, value);
                }
            }

            protected PropertyInfo AnimateProp
            {
                get
                {
                    return typeof(DropDownGenericBox).GetProperty("Animate");
                }
            }

            protected PropertyInfo DrawContainingGroupProp
            {
                get
                {
                    return typeof(DropDownGenericBox).GetProperty("DrawContainingGroup");
                }
            }

            [SerializeField]
            Group containingGroup;
            public Group ContainingGroup
            {
                get
                {
                    return containingGroup;
                }
            }

            [SerializeField]
            Group fullGroup;


            private void SetTriggerBtnProps()
            {
                if (triggerElement.GUIStyle == null)
                {
                    triggerElement.GUIStyle = "button";
                }
            }

            private void SetContainingGroupProps()
            {

                if (containingGroup.GUIStyle == null)
                {
                    containingGroup.GUIStyle = "box";
                }
            }

            public override void Draw()
            {
                //GUIControlResult[] results = new GUIControlResult[]{};
                this.StartSetOptions();
                SetTriggerBtnProps();
                SetContainingGroupProps();
                if (this.isVisible)
                {
                    if (drawFencingGroup && DrawContainingGroup)
                    {
                        fullGroup.Draw();
                    }
                    else
                    {
                        triggerElement.Draw();
                        if (DrawContainingGroup)
                        {
                            containingGroup.Draw();
                        }
                    }
                }

                this.EndSetOptions();
                containingGroup.IsVisible = isOpen;
                if (triggerElement is Toggle)
                {
                    (triggerElement as Toggle).Value = isOpen;
                }
            }

        }

        [Serializable]
        [TruGUIElementIdentifier("Group")]
        public class Group : TruGUIElement<GUIControlResult[]>
        {
            public void ClearElements()
            {
                this.elements.Clear();
            }

            public bool IsDrawing
            {
                get;
                protected set;
            }

            public bool LayoutContents
            {
                get;
                protected set;
            }

            protected List<TruGUIElement> elements;
            public TruGUIElement[] Elements
            {
                get
                {
                    return elements.ToArray();
                }
                set
                {
                    if (!IsDrawing)
                        elements = value.ToList();
                }
            }

            protected GroupSortingType sortingType = GroupSortingType.Both;
            public GroupSortingType SortingType
            {
                get
                {
                    return sortingType;
                }
            }

            public Group(Rect area, bool isForLayout)
                : base(area)
            {
                LayoutContents = isForLayout;
                elements = new List<TruGUIElement>();
            }

            public Group(GroupSortingType sortingType)
                : base()
            {
                isLayout = true;
                LayoutContents = true;
                this.sortingType = sortingType;
                elements = new List<TruGUIElement>();
            }

            public void AddElements(params TruGUIElement[] elements)
            {
                this.elements.AddRange(elements.Where(a => a.IsLayout));
                foreach (TruGUIElement theElement in this.elements)
                {
                    theElement.OnActionTaken += new Action(theElement_OnActionTaken);
                }
            }

            public bool RemoveElement(TruGUIElement element)
            {
                element.OnActionTaken -= this.theElement_OnActionTaken;
                return this.elements.Remove(element);
            }

            void theElement_OnActionTaken()
            {
                //if (results.Count > 0)
                //{
                //    results = results.Where(a => a.Element == resultOfAction.Element).ToList();
                //}
                //results.Add(resultOfAction);
                //SetActionProperties(resultOfAction.Element, resultOfAction.Result);
            }

            [SerializeField]
            List<GUIControlResult> results = new List<GUIControlResult>();

            public void StartDraw()
            {

                IsDrawing = true;
                GUI.enabled = this.enabled;
                if (isVisible)
                {

                    if (!isLayout)
                    {
                        if (!LayoutContents)
                        {
                            if (GUIStyle != null)
                            {
                                GUI.BeginGroup(this.CurrentRect, this.GUIStyle);
                            }
                            else
                            {
                                GUI.BeginGroup(this.CurrentRect);

                            }
                        }
                        else
                        {
                            if (GUIStyle != null)
                            {
                                GUILayout.BeginArea(this.CurrentRect, this.GUIStyle);
                                if (sortingType == GroupSortingType.Horizontal || sortingType == GroupSortingType.Both)
                                {
                                    GUILayout.BeginHorizontal();
                                }
                                if (sortingType == GroupSortingType.Vertical || sortingType == GroupSortingType.Both)
                                {
                                    GUILayout.BeginVertical();
                                }
                            }
                            else
                            {
                                GUILayout.BeginArea(this.CurrentRect);
                                if (sortingType == GroupSortingType.Horizontal || sortingType == GroupSortingType.Both)
                                {
                                    GUILayout.BeginHorizontal();
                                }
                                if (sortingType == GroupSortingType.Vertical || sortingType == GroupSortingType.Both)
                                {
                                    GUILayout.BeginVertical();
                                }
                            }
                        }
                    }
                    else
                    {
                        if (GUIStyle != null)
                        {
                            if (sortingType == GroupSortingType.Horizontal || sortingType == GroupSortingType.Both)
                            {
                                GUILayout.BeginHorizontal(GUIStyle);
                            }
                            if (sortingType == GroupSortingType.Vertical)
                            {
                                GUILayout.BeginVertical(GUIStyle);
                            }
                            else if (sortingType == GroupSortingType.Both)
                            {
                                GUILayout.BeginVertical();
                            }
                        }
                        else
                        {
                            if (sortingType == GroupSortingType.Horizontal || sortingType == GroupSortingType.Both)
                            {
                                GUILayout.BeginHorizontal();
                            }
                            if (sortingType == GroupSortingType.Vertical || sortingType == GroupSortingType.Both)
                            {
                                GUILayout.BeginVertical();
                            }
                        }
                    }
                }
            }

            public void EndDraw()
            {
                if (isVisible)
                {
                    if (!isLayout)
                    {
                        if (!LayoutContents)
                        {
                            if (GUIStyle != null)
                            {
                                GUI.EndGroup();
                            }
                            else
                            {
                                GUI.EndGroup();

                            }
                        }
                        else
                        {
                            if (GUIStyle != null)
                            {

                                if (sortingType == GroupSortingType.Horizontal || sortingType == GroupSortingType.Both)
                                {
                                    GUILayout.EndHorizontal();
                                }
                                if (sortingType == GroupSortingType.Vertical || sortingType == GroupSortingType.Both)
                                {
                                    GUILayout.EndHorizontal();
                                }
                                GUILayout.EndArea();
                            }
                            else
                            {

                                if (sortingType == GroupSortingType.Horizontal || sortingType == GroupSortingType.Both)
                                {
                                    GUILayout.EndHorizontal();
                                }
                                if (sortingType == GroupSortingType.Vertical || sortingType == GroupSortingType.Both)
                                {
                                    GUILayout.EndHorizontal();
                                }
                                GUILayout.EndArea();
                            }
                        }
                    }
                    else
                    {
                        if (GUIStyle != null)
                        {
                            if (sortingType == GroupSortingType.Horizontal || sortingType == GroupSortingType.Both)
                            {
                                GUILayout.EndHorizontal();
                            }
                            if (sortingType == GroupSortingType.Vertical)
                            {
                                GUILayout.EndVertical();
                            }
                            else if (sortingType == GroupSortingType.Both)
                            {
                                GUILayout.EndVertical();
                            }
                        }
                        else
                        {
                            if (sortingType == GroupSortingType.Horizontal || sortingType == GroupSortingType.Both)
                            {
                                GUILayout.EndHorizontal();
                            }
                            if (sortingType == GroupSortingType.Vertical || sortingType == GroupSortingType.Both)
                            {
                                GUILayout.EndVertical();
                            }
                        }
                    }
                }
                GUI.enabled = true;
                IsDrawing = false;
            }


            public override void Draw()
            {
                StartSetOptions();
                List<GUIControlResult> results = new List<GUIControlResult>();
                if (isVisible)
                {

                    this.StartDraw();
                    foreach (TruGUIElement element in elements)
                    {
                        element.Draw();
                    }
                    this.EndDraw();


                }
                EndSetOptions();
            }
        }

        [Serializable]
        [TruGUIElementIdentifier("TextArea")]
        public class TextArea : TruGUIElement<string>
        {
            [SerializeField]
            string currentText;
            public string CurrentText
            {
                get
                {
                    return currentText;
                }
            }

            [SerializeField]
            string oldText;

            public TextArea(Rect area, string currentText = "")
                : base(area)
            {
                this.currentText = currentText;
                oldText = currentText;
            }

            public TextArea(string currentText, params ElementLayoutOption[] layoutOptions)
                : base(layoutOptions)
            {
                this.currentText = currentText;
                oldText = currentText;
            }

            public TextArea(GUIContent labelContent, GUIStyle style, string currentText, params ElementLayoutOption[] layoutOptions)
                : base(labelContent, style, layoutOptions)
            {
                this.currentText = currentText;
                oldText = currentText;
            }

            public override void Draw()
            {
                this.StartSetOptions();
                if (IsVisible)
                {
                    if (!isLayout)
                    {
                        if (this.GUIStyle != null)
                        {
                            this.currentText = GUI.TextArea(CurrentRect, currentText, this.GUIStyle);
                        }
                        else
                        {
                            this.currentText = GUI.TextArea(CurrentRect, currentText);
                        }
                    }
                    else
                    {
                        if (this.GUIStyle != null)
                        {
                            this.currentText = GUILayout.TextArea(this.currentText, this.GUIStyle, this.GUILayoutOptions);
                        }
                        else
                        {
                            this.currentText = GUILayout.TextArea(this.currentText, this.GUILayoutOptions);
                        }
                    }
                }

                if (oldText != currentText)
                {
                    Debug.Log("true");
                    SetActionProperties(this, currentText);
                }
                oldText = currentText;
                this.EndSetOptions();

            }
        }

        [Serializable]
        [TruGUIElementIdentifier("DrawTexture")]
        public class DrawTexture : TruGUIElement
        {

            public DrawTexture(GUIElementOptions options, Texture image, ScaleMode scaleMode = UnityEngine.ScaleMode.StretchToFill, bool alphaBlend = true, float aspectRatio = 0)
                : base(options)
            {
                this.image = image;
                this.scaleMode = scaleMode;
                this.alphaBlend = alphaBlend;
                this.aspectRatio = aspectRatio;
            }

            [SerializeField]
            private Texture image;

            public Texture Image
            {
                get
                {
                    return image;
                }
                set
                {
                    image = value;
                    HandleChange(imagePropInfo, value);
                }
            }

            protected PropertyInfo imagePropInfo
            {
                get
                {
                    return typeof(DrawTexture).GetProperty("Image");
                }
            }

            [SerializeField]
            private ScaleMode scaleMode = ScaleMode.StretchToFill;

            public ScaleMode ScaleMode
            {
                get
                {
                    return scaleMode;
                }
                set
                {
                    scaleMode = value;
                    HandleChange(scaleModePropInfo, value);
                }
            }

            protected PropertyInfo scaleModePropInfo
            {
                get
                {
                    return typeof(DrawTexture).GetProperty("ScaleMode");
                }
            }

            [SerializeField]
            private bool alphaBlend = true;

            public bool AlphaBlend
            {
                get
                {
                    return alphaBlend;
                }
                set
                {
                    alphaBlend = value;
                    HandleChange(alphaBlendPropInfo, value);
                }
            }

            protected PropertyInfo alphaBlendPropInfo
            {
                get
                {
                    return typeof(DrawTexture).GetProperty("AlphaBlend");
                }
            }

            [SerializeField]
            private float aspectRatio = 0;

            public float AspectRatio
            {
                get
                {
                    return aspectRatio;
                }
                set
                {
                    aspectRatio = value;
                    HandleChange(aspectRatioPropInfo, value);
                }
            }

            protected PropertyInfo aspectRatioPropInfo
            {
                get
                {
                    return typeof(DrawTexture).GetProperty("AspectRatio");
                }
            }

            public override void Draw()
            {
                StartSetOptions();
                if (isVisible)
                {
                    if (image != null)
                    {
                        if (!isLayout)
                        {
                            GUI.DrawTexture(this.CurrentRect, this.image, this.scaleMode, this.alphaBlend, this.aspectRatio);
                        }
                        else
                        {

                            GUI.DrawTexture(GUILayoutUtility.GetRect(image.width, image.height, this.GUILayoutOptions), this.image, this.scaleMode, this.alphaBlend, this.aspectRatio);

                        }
                    }
                }
                EndSetOptions();
            }
        }


        [Serializable]
        [TruGUIElementIdentifier("Button")]
        public class Button : TruGUIElement<bool>
        {

            public Button(GUIElementOptions options)
                : base(options)
            {
            }

            public Button(Rect area, string text, string tooltip = "")
                : base(area, text, tooltip)
            {
            }
            public Button(Rect area, Texture image)
                : base(area, image)
            {
            }
            public Button(Rect area, Texture image, GUIStyle style)
                : base(area, image, style)
            {
            }
            public Button(Rect area, string text, GUIStyle style)
                : base(area, new GUIContent(text), style)
            {
            }
            public Button(Rect area, GUIContent content)
                : base(area, content)
            {
            }


            public Button(string text, string tooltip = "", params ElementLayoutOption[] options)
                : base(text, tooltip, options)
            {
            }
            public Button(Texture image, params ElementLayoutOption[] options)
                : base(image, options)
            {
            }
            public Button(Texture image, GUIStyle style, params ElementLayoutOption[] options)
                : base(image, style, options)
            {
            }
            public Button(string text, GUIStyle style, params ElementLayoutOption[] options)
                : base(new GUIContent(text), style, options)
            {
            }
            public Button(GUIContent content, params ElementLayoutOption[] options)
                : base(content, options)
            {
            }

            public override void Draw()
            {
                this.StartSetOptions();
                if (this.IsVisible)
                {
                    if (!isLayout)
                    {
                        if (this.GUIStyle != null)
                        {
                            if (GUI.Button(this.CurrentRect, this.content, this.GUIStyle))
                            {
                                HasActionTaken = true;
                                ActionResult = true;

                            }

                        }
                        else
                        {
                            if (GUI.Button(this.CurrentRect, this.content))
                            {
                                HasActionTaken = true;
                                ActionResult = true;

                            }
                        }
                    }
                    else
                    {
                        if (this.GUIStyle != null)
                        {
                            if (GUILayout.Button(this.content, this.GUIStyle, GUILayoutOptions))
                            {
                                HasActionTaken = true;
                                ActionResult = true;

                            }
                        }
                        else
                        {
                            if (GUILayout.Button(this.content, GUILayoutOptions))
                            {
                                HasActionTaken = true;
                                ActionResult = true;

                            }
                        }
                    }

                }
                EndSetOptions();
            }
        }

        [Serializable]
        [TruGUIElementIdentifier("Label")]
        public class Label : TruGUIElement
        {
            public Label(Rect area, string text)
                : base(area, new GUIContent(text))
            {
            }
            public Label(Rect area, string text, GUIStyle style)
                : base(area, text, "", style)
            {
            }

            public Label(GUIElementOptions options)
                : base(options)
            {
            }


            public override void Draw()
            {
                this.StartSetOptions();
                if (this.IsVisible)
                {
                    if (!isLayout)
                    {
                        if (this.GUIStyle != null)
                        {
                            GUI.Label(this.CurrentRect, this.content, this.GUIStyle);
                        }
                        else
                        {
                            GUI.Label(this.CurrentRect, this.content);
                        }
                    }
                    else
                    {
                        if (this.GUIStyle != null)
                        {
                            GUILayout.Label(this.content, this.GUIStyle, this.GUILayoutOptions);
                        }
                        else
                        {
                            GUILayout.Label(this.content, this.GUILayoutOptions);
                        }
                    }
                }
                EndSetOptions();
            }
        }

        /// <summary>
        /// A Button class that throws events every draw frame. result is true when the button is pressed, false if the button is not pressed
        /// </summary>
        [Serializable]
        [TruGUIElementIdentifier("RepeatButton")]
        public class RepeatButton : TruGUIElement<bool>
        {

            public RepeatButton(Rect area, string text, string tooltip = "")
                : base(area, text, tooltip)
            {
                ReturnStyle = ElementReturnStyle.Continuous;
            }
            public RepeatButton(Rect area, Texture image)
                : base(area, new GUIContent(image))
            {
                ReturnStyle = ElementReturnStyle.Continuous;
            }
            public RepeatButton(Rect area, GUIContent content, GUIStyle style)
                : base(area, content, style)
            {
                ReturnStyle = ElementReturnStyle.Continuous;
            }

            public RepeatButton(string text, string tooltip = "", params ElementLayoutOption[] layoutOptions)
                : base(text, tooltip, layoutOptions)
            {
                ReturnStyle = ElementReturnStyle.Continuous;
            }
            public RepeatButton(Texture image, params ElementLayoutOption[] layoutOptions)
                : base(new GUIContent(image), layoutOptions)
            {
                ReturnStyle = ElementReturnStyle.Continuous;
            }
            public RepeatButton(GUIContent content, GUIStyle style, params ElementLayoutOption[] layoutOptions)
                : base(content, style, layoutOptions)
            {
                ReturnStyle = ElementReturnStyle.Continuous;
            }

            public override void Draw()
            {
                this.StartSetOptions();
                if (IsVisible)
                {
                    if (!isLayout)
                    {
                        if (this.GUIStyle != null)
                        {
                            if (GUI.RepeatButton(CurrentRect, this.content, this.GUIStyle))
                            {
                                SetActionProperties(this, true);

                            }
                        }
                        else
                        {
                            if (GUI.RepeatButton(CurrentRect, this.content))
                            {
                                SetActionProperties(this, true);

                            }
                        }
                    }
                    else
                    {
                        if (this.GUIStyle != null)
                        {
                            if (GUILayout.RepeatButton(this.content, this.GUIStyle, this.GUILayoutOptions))
                            {
                                SetActionProperties(this, true);

                            }
                        }
                        else
                        {
                            if (GUILayout.RepeatButton(this.content, this.GUILayoutOptions))
                            {
                                SetActionProperties(this, true);

                            }
                        }
                    }
                }
                EndSetOptions();

            }
        }

        [Serializable]
        [TruGUIElementIdentifier("MouseOver")]
        public class MouseOver : TruGUIElement<bool>
        {
            public MouseOver(GUIElementOptions options)
                : base(options)
            {
            }
            public MouseOver(Rect position)
                : base(position)
            {
            }

            private bool isOver;

            public bool IsOver
            {
                get
                {
                    return isOver;
                }
            }


            public override void Draw()
            {
                //set the options for this element
                StartSetOptions();
                //if is visible and enabled
                if (isVisible && enabled)
                {
                    //if the element should be layouted using GUILayout
                    if (isLayout)
                    {
                        //get the next layout rectangle that will fit this element
                        //if the mouse is within the rectangle
                        if (GUILayoutUtility.GetRect(this.CurrentRect.width, this.CurrentRect.height).Contains(Event.current.mousePosition))
                        {
                            //if the mouse is not over this element
                            if (!isOver)
                            {
                                //set the action properties which will allow us to handle OnActionTaken events for this element
                                //because the mouse was not over this element but is now
                                SetActionProperties(this, true);
                            }
                            //set isOver to true since the mouse is over this element
                            isOver = true;
                        }
                        else
                        {
                            if (isOver)
                            {
                                SetActionProperties(this, false);
                            }
                            isOver = false;
                        }
                    }
                    else
                    {
                        if (this.CurrentRect.Contains(Event.current.mousePosition))
                        {
                            if (!isOver)
                            {
                                SetActionProperties(this, true);
                            }
                            isOver = true;
                        }
                        else
                        {
                            if (isOver)
                            {
                                SetActionProperties(this, false);
                            }
                            isOver = false;
                        }
                    }
                }
                else
                {
                    isOver = false;
                }
                EndSetOptions();
            }
        }

        [Serializable]
        [TruGUIElementIdentifier("TextField")]
        public class TextField : TruGUIElement<string>
        {
            [SerializeField]
            string currentText;
            public string CurrentText
            {
                get
                {
                    return currentText;
                }
            }

            [SerializeField]
            string oldText;

            public TextField(Rect area, string currentText = "", string label = "", string tooltip = "")
                : base(area, label, tooltip)
            {
                this.currentText = currentText;
                oldText = this.currentText;
            }

            public TextField(Rect area, GUIContent labelContent, GUIStyle style, string currentText)
                : base(area, labelContent, style)
            {
                this.currentText = currentText;
                oldText = this.currentText;
            }


            public TextField(string currentText = "", string label = "", string tooltip = "", params ElementLayoutOption[] layoutOptions)
                : base(label, tooltip, layoutOptions)
            {
                this.currentText = currentText;
                oldText = this.currentText;
            }

            public TextField(GUIContent labelContent, GUIStyle style, string currentText, params ElementLayoutOption[] layoutOptions)
                : base(labelContent, style, layoutOptions)
            {
                this.currentText = currentText;
                oldText = this.currentText;
            }

            public override void Draw()
            {
                this.StartSetOptions();
                if (IsVisible)
                {
                    if (!isLayout)
                    {
                        if (this.GUIStyle != null)
                        {
                            if (string.IsNullOrEmpty(this.content.text))
                            {
                                this.currentText = GUI.TextField(CurrentRect, currentText, this.GUIStyle);
                            }
                            else
                            {
                                GUI.Label(new Rect(this.CurrentRect.xMin, CurrentRect.yMin, content.text.Length * 5, this.CurrentRect.height), this.content, this.GUIStyle);
                                this.currentText = GUI.TextField(new Rect(this.CurrentRect.xMin + content.text.Length * 5, CurrentRect.yMin, this.CurrentRect.width - (this.CurrentRect.xMin + content.text.Length * 5), CurrentRect.height), currentText, this.GUIStyle);
                            }
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(this.content.text))
                            {
                                this.currentText = GUI.TextField(CurrentRect, currentText);
                            }
                            else
                            {
                                GUI.Label(new Rect(this.CurrentRect.xMin, CurrentRect.yMin, content.text.Length * 5, this.CurrentRect.height), this.content);
                                this.currentText = GUI.TextField(new Rect(this.CurrentRect.xMin + content.text.Length * 5, CurrentRect.yMin, this.CurrentRect.width - (this.CurrentRect.xMin + content.text.Length * 5), CurrentRect.height), currentText);
                            }
                        }
                    }
                    else
                    {
                        if (this.GUIStyle != null)
                        {
                            if (string.IsNullOrEmpty(this.content.text))
                            {
                                this.currentText = GUILayout.TextField(this.currentText, this.GUIStyle, this.GUILayoutOptions);
                            }
                            else
                            {
                                GUILayout.EndHorizontal();
                                GUILayout.Label(this.content);
                                this.currentText = GUILayout.TextField(this.currentText, this.GUIStyle, this.GUILayoutOptions);

                                GUILayout.EndHorizontal();
                            }
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(this.content.text))
                            {
                                this.currentText = GUILayout.TextField(this.currentText, this.GUILayoutOptions);
                            }
                            else
                            {
                                GUILayout.BeginHorizontal();//this.layoutOptions);
                                GUILayout.Label(this.content);
                                this.currentText = GUILayout.TextField(currentText, this.GUILayoutOptions);
                                GUILayout.EndHorizontal();
                            }
                        }
                    }
                }

                if (oldText != currentText)
                {

                    SetActionProperties(this, currentText);
                }
                oldText = currentText;
                EndSetOptions();
            }
        }

        [Serializable]
        [TruGUIElementIdentifier("PasswordField")]
        public class PasswordField : TruGUIElement<string>
        {
            [SerializeField]
            string currentText;
            public string CurrentText
            {
                get
                {
                    return currentText;
                }
            }

            char passwordChar = '*';
            public char PasswordChar
            {
                get
                {
                    return passwordChar;
                }
            }

            [SerializeField]
            string oldText;

            public PasswordField(Rect area, string currentText = "", char passwordChar = '*', string label = "", string tooltip = "")
                : base(area, label, tooltip)
            {
                this.currentText = currentText;
                this.passwordChar = passwordChar;
                oldText = currentText;
            }

            public PasswordField(Rect area, GUIContent labelContent, GUIStyle style, string currentText)
                : base(area, labelContent, style)
            {
                this.currentText = currentText;
                oldText = currentText;
            }


            public PasswordField(string currentText = "", char passwordChar = '*', string label = "", string tooltip = "", params ElementLayoutOption[] layoutOptions)
                : base(label, tooltip, null, layoutOptions)
            {
                this.currentText = currentText;
                this.passwordChar = passwordChar;
                oldText = currentText;
            }

            public PasswordField(GUIContent labelContent, GUIStyle style, string currentText, params ElementLayoutOption[] layoutOptions)
                : base(labelContent, style, layoutOptions)
            {
                this.currentText = currentText;
                oldText = currentText;
            }

            public override void Draw()
            {
                this.StartSetOptions();
                if (IsVisible)
                {
                    if (!isLayout)
                    {
                        if (this.GUIStyle != null)
                        {
                            if (string.IsNullOrEmpty(this.content.text))
                            {
                                this.currentText = GUI.PasswordField(CurrentRect, currentText, passwordChar, this.GUIStyle);
                            }
                            else
                            {
                                GUI.Label(new Rect(this.CurrentRect.xMin, CurrentRect.yMin, content.text.Length * 7, this.CurrentRect.height), this.content, this.GUIStyle);
                                this.currentText = GUI.PasswordField(new Rect(this.CurrentRect.xMin + content.text.Length * 7, CurrentRect.yMin, this.CurrentRect.width - (this.CurrentRect.xMin + content.text.Length * 5), CurrentRect.height), currentText, passwordChar, this.GUIStyle);
                            }
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(this.content.text))
                            {
                                this.currentText = GUI.PasswordField(CurrentRect, currentText, passwordChar);
                            }
                            else
                            {
                                GUI.Label(new Rect(this.CurrentRect.xMin, CurrentRect.yMin, content.text.Length * 7, this.CurrentRect.height), this.content);
                                this.currentText = GUI.PasswordField(new Rect(this.CurrentRect.xMin + content.text.Length * 7, CurrentRect.yMin, this.CurrentRect.width - (this.CurrentRect.xMin + content.text.Length * 5), CurrentRect.height), currentText, passwordChar);
                            }
                        }
                    }
                    else
                    {
                        if (this.GUIStyle != null)
                        {
                            if (string.IsNullOrEmpty(this.content.text))
                            {
                                this.currentText = GUILayout.PasswordField(currentText, passwordChar, this.GUIStyle, this.GUILayoutOptions);
                            }
                            else
                            {
                                GUILayout.BeginHorizontal(this.GUILayoutOptions);
                                GUILayout.Label(this.content, this.GUIStyle);
                                this.currentText = GUILayout.PasswordField(currentText, passwordChar, this.GUIStyle);
                                GUILayout.EndHorizontal();
                            }
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(this.content.text))
                            {
                                this.currentText = GUILayout.PasswordField(currentText, passwordChar, this.GUILayoutOptions);
                            }
                            else
                            {
                                GUILayout.BeginHorizontal(this.GUILayoutOptions);
                                GUILayout.Label(this.content);
                                this.currentText = GUILayout.PasswordField(currentText, passwordChar);
                                GUILayout.EndHorizontal();
                            }
                        }
                    }
                }

                if (oldText != currentText)
                {
                    SetActionProperties(this, currentText);
                }
                oldText = currentText;
                EndSetOptions();
            }
        }

        [Serializable]
        [TruGUIElementIdentifier("Toggle")]
        public class Toggle : TruGUIElement<bool>
        {
            [SerializeField]
            bool value;
            public bool Value
            {
                get
                {
                    return value;
                }
                set
                {
                    this.value = value;
                    HandleChange(ValueProp, value);
                }
            }

            protected PropertyInfo ValueProp
            {
                get
                {
                    return typeof(Toggle).GetProperty("Value");
                }
            }

            bool oldValue;


            public Toggle(GUIElementOptions options)
                : base(options)
            {
                oldValue = value;
                this.ReturnStyle = ElementReturnStyle.Continuous;
            }

            public Toggle(GUIContent gUIContent, ElementLayoutOption[] layoutOptions)
                : base(gUIContent, layoutOptions)
            {
                oldValue = value;
                this.ReturnStyle = ElementReturnStyle.Continuous;
            }


            public override void Draw()
            {
                GUI.enabled = enabled;
                if (isVisible)
                {
                    if (!isLayout)
                    {
                        if (GUIStyle != null)
                        {
                            value = GUI.Toggle(this.CurrentRect, value, this.Content, this.GUIStyle);
                        }
                        else
                        {
                            value = GUI.Toggle(this.CurrentRect, value, this.Content);
                        }
                    }
                    else
                    {
                        if (GUIStyle != null)
                        {
                            value = GUILayout.Toggle(value, this.Content, this.GUIStyle);
                        }
                        else
                        {
                            value = GUILayout.Toggle(value, this.Content);
                        }
                    }
                }
                FindRect();
                GUI.enabled = true;
                if (oldValue != value)
                {
                    SetActionProperties(this, value);
                }
                oldValue = value;
                handleEvents();
            }
        }
    }
}

