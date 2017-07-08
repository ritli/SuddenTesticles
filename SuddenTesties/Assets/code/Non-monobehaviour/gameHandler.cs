using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

static public class gameHandler {

	static List<Vector2> m_spawnPoints;
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
		m_spawnPoints = new List<Vector2> ();
		// Load spawnpoints

		// Load map

		// Load and start music

		// Instantiate players, give IDs and give spawnpoints
		
	}

	static public Vector2 playerDeath(int playerId, bool points = true){
		if (points) {
			m_pScore [(playerId == 1) ? 0 : 1]++;
		}

		return m_spawnPoints [Random.Range (0, m_spawnPoints.Count)];
	}

	static public void setScoreText(Text textObj, int playerId){
		m_scoreText [playerId == 1 ? 0 : 1] = textObj;
		textObj.text = 0.ToString();
	}
}
