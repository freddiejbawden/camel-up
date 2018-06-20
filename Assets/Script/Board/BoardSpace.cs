using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UnityEngine {
	public class BoardSpace  {
		private int bonusForLanding;
		private List<string> camelsOnSpace = new List<string> ();
		private Vector3 worldLocation;
        private int id;
		public BoardSpace(Vector3 v, int id) {
			this.bonusForLanding = 0;
			this.camelsOnSpace = new List<string> ();
			this.worldLocation = v;
            this.id = id;
		}
		public int getBonus() {
			return this.bonusForLanding;
		}
		public List<string> getCamelOnSpaces() {
			return this.camelsOnSpace;
		}
		public void removeCamel(string camel) {
			this.camelsOnSpace.Remove (camel);
		}
		public void addCamel(string camel) {
			this.camelsOnSpace.Add (camel);
		}
		public void setSpaceBonus(int bonus) {
			this.bonusForLanding = bonus;
		}
		public void removeRange(int index, int count) {
			this.camelsOnSpace.RemoveRange (index, count);
		}
		public void resetBonus() {
			this.bonusForLanding = 0;
		}
        public int getID()
        {
            return this.id;
        }
		public Vector3 getPosition() {
			return this.worldLocation;
		}
		public void setPosition(Vector3 v) {
			this.worldLocation = v;
		}
	}

}
