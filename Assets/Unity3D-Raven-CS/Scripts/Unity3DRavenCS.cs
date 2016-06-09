using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Unity3DRavenCS {
	using RavenOptionType = Dictionary<string, string>;

	public class Unity3DRavenCS {
		private DSN m_dsn;
		private bool m_valid;
		private RavenOptionType m_options;

		public Unity3DRavenCS(string dsnUri)
		{
			m_dsn = new DSN(dsnUri);
			if (!m_dsn.isValid) {
				m_valid = false;
				Debug.Log ("Unity3DRavenCS is disabled because the DSN is invalid.");
			} else {
				// m_options = options;
				m_valid = true;
			}
		}
	}
}
