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
using System.Text;
using UnityEditor;


namespace TruGUI.Controls.Editor
{
    /// <summary>
    /// Defines an object field.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ObjectField<T> : TruGUIElement<T> where T : UnityEngine.Object
    {
        /// <summary>
        /// Gets or sets the current value in the Object Field.
        /// </summary>
        public T CurrentValue
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets whether scene objects are allowed to be put inside the field.
        /// </summary>
        public bool AllowSceneObjects
        {
            get;
            set;
        }

        public override void Draw()
        {
            base.StartSetOptions();
            if (IsVisible)
            {
                T oldValue = CurrentValue;
                //draw as normal GUI
                if (!IsLayout)
                {
                    CurrentValue = (T)EditorGUI.ObjectField(CurrentRect, Content, CurrentValue, typeof(T), AllowSceneObjects);
                }
                else
                {
                    CurrentValue = (T)EditorGUILayout.ObjectField(Content, CurrentValue, typeof(T), AllowSceneObjects, GUILayoutOptions);
                }
                if (oldValue != CurrentValue)
                {
                    SetActionProperties(this, CurrentValue);
                }
            }

            base.EndSetOptions();
        }
    }

    public class FloatField : TruGUIElement<float>
    {
        /// <summary>
        /// Gets or sets the current value in the Object Field.
        /// </summary>
        public float CurrentValue
        {
            get;
            set;
        }

        public override void Draw()
        {
            base.StartSetOptions();
            if (IsVisible)
            {
                float oldValue = CurrentValue;
                //draw as normal GUI
                if (!IsLayout)
                {
                    if (GUIStyle != null)
                    {
                        CurrentValue = EditorGUI.FloatField(CurrentRect, Content, CurrentValue, GUIStyle);
                    }
                    else
                    {
                        CurrentValue = EditorGUI.FloatField(CurrentRect, Content, CurrentValue);
                    }
                }
                else
                {
                    if (GUIStyle != null)
                    {
                        CurrentValue = EditorGUILayout.FloatField(Content, CurrentValue, GUIStyle, GUILayoutOptions);
                    }
                    else
                    {
                        CurrentValue = EditorGUILayout.FloatField(Content, CurrentValue, GUILayoutOptions);
                    }
                }
                if (oldValue != CurrentValue)
                {
                    SetActionProperties(this, CurrentValue);
                }
            }
            base.EndSetOptions();
        }
    }

    public class IntField : TruGUIElement<int>
    {
        /// <summary>
        /// Gets or sets the current value in the Object Field.
        /// </summary>
        public int CurrentValue
        {
            get;
            set;
        }

        public override void Draw()
        {
            base.StartSetOptions();
            if (IsVisible)
            {
                float oldValue = CurrentValue;
                //draw as normal GUI
                if (!IsLayout)
                {
                    if (GUIStyle != null)
                    {
                        CurrentValue = EditorGUI.IntField(CurrentRect, Content, CurrentValue, GUIStyle);
                    }
                    else
                    {
                        CurrentValue = EditorGUI.IntField(CurrentRect, Content, CurrentValue);
                    }
                }
                else
                {
                    if (GUIStyle != null)
                    {
                        CurrentValue = EditorGUILayout.IntField(Content, CurrentValue, GUIStyle, GUILayoutOptions);
                    }
                    else
                    {
                        CurrentValue = EditorGUILayout.IntField(Content, CurrentValue, GUILayoutOptions);
                    }
                }
                if (oldValue != CurrentValue)
                {
                    SetActionProperties(this, CurrentValue);
                }
            }
            base.EndSetOptions();
        }
    }
}
