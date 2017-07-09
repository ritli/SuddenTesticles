using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

static public class gameHandler {

	// Spawn point variables
	static GameObject m_spawnPointsObj;
	static List<Vector3> m_spawnPoints;

	// Score variables & getters
	static int[] m_pScore = new int[2];
	static public int P1Score {
		get { return m_pScore[0]; }
	}
	static public int P2Score {
		get { return m_pScore[1]; }
	}

	// UI Text objects
	static Text[] m_scoreText = new Text[2];

	// Map object
	static GameObject m_level;

	static int m_playerCount = 0;

	// Setup new game
	public static void instantiate(){

		// Load spawn points
		m_spawnPointsObj = GameObject.Instantiate (Resources.Load ("SpawnPoints")) as GameObject;
		m_spawnPoints = new List<Vector3> ();

		foreach (Transform child in m_spawnPointsObj.transform) {
			m_spawnPoints.Add (child.position);
		}

		// Load map
		m_level = GameObject.Instantiate(Resources.Load("level_holder1")) as GameObject;

		// Load and start music

		// Instantiate players, give IDs and give spawnpoints
		GameObject.Instantiate(Resources.Load("player"));
		
	}

	static public Vector3 playerDeath(int playerId, bool points = true){
		if (points) {
			m_pScore [playerId - 1]++;
			m_scoreText [playerId - 1].text = m_pScore [playerId - 1].ToString ();
		}

		return m_spawnPoints [Random.Range (0, m_spawnPoints.Count)];
	}

	static public KeyValuePair<int, Vector3> spawn(){
		return new KeyValuePair<int, Vector3> (++m_playerCount, m_spawnPoints [Random.Range (0, m_spawnPoints.Count)]);
	}

	static public void setScoreText(Text textObj, int playerId){
		m_scoreText [playerId - 1] = textObj;
		textObj.text = 0.ToString();
	}
}
