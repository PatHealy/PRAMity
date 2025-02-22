﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Pram.Data;
using Pram.Entities;

namespace Pram.Managers {
    public class SimpleFluManager : PramManager {

        int step = 0;
        public Text counter;
        BoxSite s;

        private new void Start() {
            base.Start();
            s = gameObject.GetComponent<BoxSite>();
        }

        override public void DefineGroups() {
            Dictionary<string, string> g1Attributes = new Dictionary<string, string>();
            Dictionary<string, string> g2Attributes = new Dictionary<string, string>();
            Dictionary<string, string> g3Attributes = new Dictionary<string, string>();

            g1Attributes.Add("flu-status", "s");
            g2Attributes.Add("flu-status", "i");
            g3Attributes.Add("flu-status", "r");

            Group g1 = new Group(g1Attributes, "", 1000);
            Group g2 = new Group(g2Attributes, "", 0);
            Group g3 = new Group(g3Attributes, "", 0);

            this.groups = new Group[] { g1, g2, g3 };
        }

        override public void DefineRules() {
            this.rules = new string[] { "Simple Flu Progress Rule" };
        }

        public override void NotifyPlayableGroupChange(PlayableAgent a) {
            throw new System.NotImplementedException();
        }

        public override Vector3 GetPosition() {
            if (s == null) {
                s = gameObject.GetComponent<BoxSite>();
            }
            return s.GetPosition();
        }

        public void Update() {
            if (Input.GetKeyDown(KeyCode.N)) {
                this.SimStep();
                step++;
                counter.text = "Step: " + step;
            }
        }

    }
}
