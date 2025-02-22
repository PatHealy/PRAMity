﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pram.Data {
    [System.Serializable]
    public class Group {
        //public Dictionary<string, string> attributes;
        public string[] attributeKeys;
        public string[] attributeValues;
        public string[] relationKeys;
        public string[] relationValues;
        public string site = "";
        public double n;

        public Group() {
            //this.attributes = new Dictionary<string, string>();
            this.attributeKeys = new string[0];
            this.attributeValues = new string[0];
            this.relationKeys = new string[0];
            this.relationValues = new string[0];
            this.site = null;
            this.n = 0;
        }

        public Group(double mass) {
            //this.attributes = new Dictionary<string, string>();
            this.attributeKeys = new string[0];
            this.attributeValues = new string[0];
            this.relationKeys = new string[0];
            this.relationValues = new string[0];
            this.site = null;
            this.n = mass;
        }

        public Group(string[] attributeKeys, string[] attributeValues, string site, double mass) {
            //this.attributes = attributes;
            this.attributeKeys = attributeKeys;
            this.attributeValues = attributeValues;
            this.relationKeys = new string[0];
            this.relationValues = new string[0];
            this.site = site;
            this.n = mass;
        }

        public Group(string[] attributeKeys, string[] attributeValues, string[] relationKeys, string[] relationValues, string site, double mass) {
            //this.attributes = attributes;
            this.attributeKeys = attributeKeys;
            this.attributeValues = attributeValues;
            this.relationKeys = relationKeys;
            this.relationValues = relationValues;
            this.site = site;
            this.n = mass;
        }

        public Group(Dictionary<string, string> attributes, string site, double mass) {
            this.attributeKeys = new string[attributes.Count];
            this.attributeValues = new string[attributes.Count];

            int i = 0;
            foreach (KeyValuePair<string, string> p in attributes) {
                attributeKeys[i] = p.Key;
                attributeValues[i++] = p.Value;
            }

            this.relationKeys = new string[0];
            this.relationValues = new string[0];

            this.site = site;
            this.n = mass;
        }

        public Group(Dictionary<string, string> attributes, Dictionary<string,string> relations, string site, double mass) {
            this.attributeKeys = new string[attributes.Count];
            this.attributeValues = new string[attributes.Count];

            int i = 0;
            foreach (KeyValuePair<string, string> p in attributes) {
                attributeKeys[i] = p.Key;
                attributeValues[i++] = p.Value;
            }

            this.relationKeys = new string[relations.Count];
            this.relationValues = new string[relations.Count];

            i = 0;
            foreach (KeyValuePair<string, string> p in relations) {
                relationKeys[i] = p.Key;
                relationValues[i++] = p.Value;
            }

            this.site = site;
            this.n = mass;
        }

        public Group(Group g) : this(g.attributes(), g.relations(), g.site, g.n){}

        public string ToString() {
            string attributesString = "{ ";
            for (int i = 0; i < attributeKeys.Length; i++) {
                attributesString += attributeKeys[i] + ": " + attributeValues[i] + ", ";
            }
            attributesString += " }";

            string relationsString = "{ ";
            for (int i = 0; i < relationKeys.Length; i++) {
                relationsString += relationKeys[i] + ": " + relationValues[i] + ", ";
            }
            relationsString += " }";

            return "{ attributes: " + attributesString + "relations: " + relationsString + ", site: " + site + ", mass: " + n + " }";
        }

        public void SetSite(string s) {
            site = s;
        }

        /// <summary>
        /// Returns true if this group is equivalent to the other (it has the same attributes/relations).
        /// </summary>
        /// <param name="other">The other group to compare this one to.</param>
        /// <returns></returns>
        public bool Equivalent(Group other) {
            if (other == null) {
                return false;
            }

            if (!this.site.Equals(other.site)) {
                return false;
            }

            return this.EquivalentAttributes(other) && this.EquivalentRelations(other);
        }

        public Dictionary<string, string> attributes() {
            Dictionary<string, string> d = new Dictionary<string, string>();
            for (int i = 0; i < attributeKeys.Length; i++) {
                d.Add(attributeKeys[i], attributeValues[i]);
            }
            return d;
        }

        public Dictionary<string, string> relations() {
            Dictionary<string, string> d = new Dictionary<string, string>();
            for (int i = 0; i < relationKeys.Length; i++) {
                d.Add(relationKeys[i], relationValues[i]);
            }
            return d;
        }

        public bool EquivalentIgnoringRelations(Group other) {
            return this.EquivalentAttributes(other) && this.site.Equals(other.site);
        }

        public bool EquivalentAttributesAndRelations(Group other) {
            return this.EquivalentAttributes(other) && this.EquivalentRelations(other);
        }

        public bool EquivalentAttributes(Group other) {
            Dictionary<string, string> a1 = this.attributes();
            Dictionary<string, string> a2 = other.attributes();

            foreach (string k in a1.Keys) {
                if (!a2.ContainsKey(k) || !a1[k].Equals(a2[k])) {
                    return false;
                }
            }

            foreach (string k in a2.Keys) {
                if (!a1.ContainsKey(k) || !a1[k].Equals(a2[k])) {
                    return false;
                }
            }

            return true;
        }

        public bool EquivalentRelations(Group other) {
            Dictionary<string, string> a1 = this.relations();
            Dictionary<string, string> a2 = other.relations();

            foreach (string k in a1.Keys) {
                if (!a2.ContainsKey(k) || !a1[k].Equals(a2[k])) {
                    return false;
                }
            }

            foreach (string k in a2.Keys) {
                if (!a1.ContainsKey(k) || !a1[k].Equals(a2[k])) {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Returns a deep copy of this group
        /// </summary>
        /// <returns>A Group object identical to this one.</returns>
        public Group GetCopy() {
            Dictionary<string, string> attributesCopy = new Dictionary<string, string>();
            Dictionary<string, string> relationsCopy = new Dictionary<string, string>();
            foreach (KeyValuePair<string, string> kvp in this.attributes()) { attributesCopy.Add(kvp.Key, kvp.Value); }
            foreach (KeyValuePair<string, string> kvp in this.relations()) { relationsCopy.Add(kvp.Key, kvp.Value); }
            return new Group(attributesCopy, relationsCopy, this.site, this.n);
        }

        public bool IsPlayable() {
            int playableIndex = -1;
            for (int i = 0; i < attributeKeys.Length; i++) {
                if (attributeKeys[i].Equals("playable")) {
                    playableIndex = i;
                }
            }
            if (playableIndex == -1) { return false; }
            return attributeValues[playableIndex].Equals("yes");
        }

        public void MakePlayable() {
            Dictionary<string, string> tmp = attributes();
            tmp.Add("playable", "yes");

            List<string> k = new List<string>(tmp.Keys);
            List<string> v = new List<string>(tmp.Values);

            attributeKeys = k.ToArray();
            attributeValues = v.ToArray();
        }
    }

}
