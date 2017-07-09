using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

static public class gameHandler {

	static GameObject m_spawnPointsObj;
	static List<Vector3> m_spawnPoints;
	static int[] m_pScore = new int[2];
	static public int P1Score {
		get { return m_pScore[0]; }
	}
	static public int P2Score {
		get { return m_pScore[1]; }
	}

	static Text[] m_scoreText = new Text[2];

	// Setup new game
	public static void instantiate(){
		m_spawnPointsObj = GameObject.Instantiate (Resources.Load ("SpawnPoints")) as GameObject;
		m_spawnPoints = new List<Vector3> ();

		foreach (Transform child in m_spawnPointsObj.transform) {
			m_spawnPoints.Add (child.position);
		}

		// Load map

		// Load and start music

		// Instantiate players, give IDs and give spawnpoints
		
	}

	static public Vector3 playerDeath(int playerId, bool points = true){
		if (points) {
			m_pScore [playerId - 1]++;
			m_scoreText [playerId - 1].text = m_pScore [playerId - 1].ToString ();
		}

		return m_spawnPoints [Random.Range (0, m_spawnPoints.Count)];
	}

	static public void setScoreText(Text textObj, int playerId){
		m_scoreText [playerId - 1] = textObj;
		textObj.text = 0.ToString();
	}
}
