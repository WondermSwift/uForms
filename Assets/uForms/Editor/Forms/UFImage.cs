﻿using System.Xml.Serialization;
using UnityEditor;
using UnityEngine;

namespace uForms
{
    public class UFImage : UFControl
    {
        public static readonly Vector2 DefaultOffset = new Vector2(10, 10);
        public static readonly Vector2 DefaultSize = new Vector2(100, 100);

        private GUIContent image = new GUIContent();

        public string GUID = "";

        [XmlIgnore]
        public Texture Image
        {
            get
            {
                return this.image.image;
            }
            set
            {
                this.image.image = value;
            }
        }

        public override void DrawByRect()
        {
            GUI.Label(this.DrawRect, this.image);
        }

        public override void DrawDesignByRect()
        {
            if(IsHidden) { return; }
            GUI.Label(this.DrawRect, this.image);
        }

        public UFImage() { }

        public override void WriteCode(CodeBuilder builder)
        {
            builder.WriteLine("this." + this.Name + " = new UFImage();");
            base.WriteCode(builder);
            builder.WriteLine("this." + this.Name + ".GUID = \"" + this.GUID + "\";");
            builder.WriteLine("this." + this.Name + ".Image = UFUtility.LoadAssetFromGUID<Texture>(\"" + this.GUID + "\");");
        }

        public override void WriteDefinitionCode(CodeBuilder builder)
        {
            builder.WriteLine("private UFImage " + this.Name + ";");
        }

        public override void RefleshHierarchy()
        {
            base.RefleshHierarchy();
            this.Image = UFUtility.LoadAssetFromGUID<Texture>(this.GUID);
        }

        protected override void DrawPropertyOriginal()
        {
            DrawPropertyItem("Image", () =>
            {
                var tex = EditorGUILayout.ObjectField(this.Image, typeof(Texture), false) as Texture;
                if(tex != this.Image)
                {
                    this.Image = tex;
                    string path = AssetDatabase.GetAssetPath(this.Image);
                    this.GUID = AssetDatabase.AssetPathToGUID(path);
                }
            });
        }
    }
}
