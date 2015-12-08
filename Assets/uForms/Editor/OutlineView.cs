﻿using System;
using System.Collections.Generic;
using uForms.Editor.Control;
using UnityEditor;
using UnityEngine;

namespace uForms.Editor.View
{
    public class OutlineView : EditorWindow
    {
        public static OutlineView OpenWindow()
        {
            var window = GetWindow<OutlineView>("Outline");
            window.minSize = new Vector2(300, 200);
            return window;
        }

        List<UFControl> drawList = null;

        GenericMenu menu = new GenericMenu();

        void Awake()
        {
            menu.AddItem(new GUIContent("Delete"), false, OnMenuDelete);
            menu.AddItem(new GUIContent("Add/Button"), false, OnMenuAdd, "Button");
            menu.AddItem(new GUIContent("Add/Label"), false, OnMenuAdd, "Label");
        }

        private void OnMenuDelete()
        {
            if(UFSelection.ActiveControl != null)
            {
                Debug.Log("delete : " + UFSelection.ActiveControl.Name);
            }
        }

        private void OnMenuAdd(object type)
        {
            var current = UFSelection.ActiveControl;
            if(current == null) { return; }
            string typeString = (string)type;
            switch(typeString)
            {
                case "Button":
                    var button = new UFButton(current);
                    button.Name = "HogeButton";
                    button.Text = "Hoge";
                    current.AddChild(button);
                    break;
                case "Label":
                    var label = new UFLabel();
                    label.Name = "HogeButton";
                    label.Text = "Hoge";
                    current.AddChild(label);
                    break;
            }
        }

        void OnEnable()
        {
        }
        
        void OnGUI()
        {
            if(Event.current.type == EventType.ContextClick)
            {
                this.menu.ShowAsContext();
            }

            GUILayout.BeginHorizontal();
            {
                GUILayout.Label(" ");
                if(GUILayout.Button(UFContent.VisibleSwitch, EditorStyles.label, GUILayout.Width(20), GUILayout.Height(16)))
                {
                    bool allHidden = !UFStudio.project.rootWindow.IsHidden;
                    UFStudio.project.rootWindow.ForTree(node => node.IsHidden = allHidden);
                }

                if(GUILayout.Button(UFContent.LockSwitch, EditorStyles.label, GUILayout.Width(20), GUILayout.Height(16)))
                {
                    bool allLock = !UFStudio.project.rootWindow.IsLocked;
                    UFStudio.project.rootWindow.ForTree(node => node.IsLocked = allLock);
                }
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(20);

            this.drawList = UFStudio.project.rootWindow.GetOutlineDrawList();

            BeginWindows();
            {
                bool anyControlSelected = false;
                for(int i = 0; i < this.drawList.Count; ++i)
                {
                    Rect rect = new Rect(0, 40 + (i * 16), this.position.width, 16);

                    string style = (UFSelection.ActiveControl == this.drawList[i]) ? "LODSliderRangeSelected" : "LODSliderText";
                    
                    GUI.Window(i, rect, WindowCallback, "", style);
                    if(Event.current.type == EventType.MouseUp && rect.Contains(Event.current.mousePosition))
                    {
                        UFSelection.ActiveControl = this.drawList[i];
                        anyControlSelected = true;
                    }
                }
                if(Event.current.type == EventType.MouseUp && !anyControlSelected)
                {
                    UFSelection.ActiveControl = null;
                }
            }
            EndWindows();
        }

        void WindowCallback(int id)
        {
            UFControl current = this.drawList[id];
            if(UFSelection.ActiveControl != current)
            {
                GUILayout.Space(-1);
            }

            GUILayout.BeginHorizontal();

            if(UFSelection.ActiveControl != current)
            {
                GUILayout.Space(-3);
            }

            GUILayout.Space(current.Nest * 10);

            if(current.HasChild)
            {
                string text = (current.Foldout ? "\u25BC" : "\u25BA");

                if(GUILayout.Button(text, EditorStyles.label, GUILayout.Width(13)))
                {
                    current.Foldout = !current.Foldout;
                }
            }
            else
            {
                GUILayout.Label(" ", GUILayout.Width(13));
            }

            GUILayout.Label(current.Name);

            GUIContent visibleContent = (current.IsHidden ? UFContent.Minus : UFContent.VisibleSwitch);
            if(GUILayout.Button(visibleContent, EditorStyles.label, GUILayout.Width(20)))
            {
                current.IsHidden = !current.IsHidden;
            }

            GUIContent lockContent = (current.IsLocked ? UFContent.LockSwitch : UFContent.Minus);
            if(GUILayout.Button(lockContent, EditorStyles.label, GUILayout.Width(20)))
            {
                current.IsLocked = !current.IsLocked;
            }

            GUILayout.EndHorizontal();
            //GUI.DragWindow();
            if(GUI.changed)
            {
                UFStudio.Repaint();
            }
        }
    }
}