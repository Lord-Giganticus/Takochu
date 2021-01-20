﻿using GL_EditorFramework;
using GL_EditorFramework.EditorDrawables;
using GL_EditorFramework.GL_Core;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Takochu.fmt;

namespace Takochu.smg.obj
{
    public class StartObj : AbstractObj
    {
        public StartObj(BCSV.Entry entry, Zone parentZone, string path) : base(entry)
        {
            mParentZone = parentZone;
            string[] content = path.Split('/');
            mDirectory = content[0];
            mLayer = content[1];
            mFile = content[2];

            mType = "StartObj";
            Position = new Vector3(Get<float>("pos_x") / 100, Get<float>("pos_y") / 100, Get<float>("pos_z") / 100);
            Rotation = new Vector3(Get<float>("dir_x"), Get<float>("dir_y"), Get<float>("dir_z"));
            Scale = new Vector3(Get<float>("scale_x"), Get<float>("scale_y"), Get<float>("scale_z"));
        }

        public override string ToString()
        {
            return $"[{Get<uint>("MarioNo")}] {mName} [{mLayer}] [{mParentZone.mZoneName}]";
        }

        public override uint Select(int index, GL_ControlBase control)
        {

            if (!Selected)
            {
                Selected = true;
                control.AttachPickingRedrawer();
            }
            return 0;
        }

        public override uint SelectDefault(GL_ControlBase control)
        {

            if (!Selected)
            {
                Selected = true;
                control.AttachPickingRedrawer();
            }
            return 0;
        }

        public override uint SelectAll(GL_ControlBase control)
        {

            if (!Selected)
            {
                Selected = true;
                control.AttachPickingRedrawer();
            }
            return 0;
        }

        public override uint Deselect(int index, GL_ControlBase control)
        {

            if (Selected)
            {
                Selected = false;
                control.DetachPickingRedrawer();
            }
            return 0;
        }

        public override uint DeselectAll(GL_ControlBase control)
        {

            if (Selected)
            {
                Selected = false;
                control.DetachPickingRedrawer();
            }
            return 0;
        }

        public override bool TrySetupObjectUIControl(EditorSceneBase scene, ObjectUIControl objectUIControl)
        {
            if (!Selected)
                return false;

            objectUIControl.AddObjectUIContainer(new PropertyProvider(this, scene), "Transform");
            objectUIControl.AddObjectUIContainer(new StartObjUI(this, scene), "Starting Point Settings");
            return true;
        }

        public class StartObjUI : IObjectUIContainer
        {
            AbstractObj obj;
            EditorSceneBase scene;

            string text = "";
            uint marioNo = 0;
            int arg_0 = 0;
            uint cameraID = 0;
            string zone = "";

            static List<string> zones;

            public StartObjUI(AbstractObj obj, EditorSceneBase scene)
            {
                this.obj = obj;
                this.scene = scene;

                zones = new List<string>();
                zones.AddRange(obj.mParentZone.mGalaxy.GetZones().Keys);
            }

            public void DoUI(IObjectUIControl control)
            {
                text = control.TextInput(obj.Get<string>("name"), "Name");
                zone = control.DropDownTextInput("Zone", obj.mParentZone.mZoneName, zones.ToArray(), false);

                uint val = obj.Get<uint>("Obj_arg0");

                arg_0 = val == 0xFFFFFFFF ? -1 : (int)val;

                marioNo = (uint)control.NumberInput(obj.Get<uint>("MarioNo"), "Mario No");
                arg_0 =  (int)control.NumberInput(arg_0, "Obj_arg0");
                cameraID = (uint)control.NumberInput(obj.Get<uint>("Camera_id"), "Camera ID");
            }

            public void OnValueChangeStart()
            {

            }

            public void OnValueChanged()
            {
                scene.Refresh();
            }

            public void OnValueSet()
            {

            }

            public void UpdateProperties()
            {

            }
        }
    }
}
